namespace nexIRC.MatrixProtocol.Core.Domain.Services {
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using nexIRC.Enum;
    using Infrastructure.Dto.Sync;
    using Infrastructure.Services;
    using MatrixRoom;
    using nexIRC.Business.Helper;

    /// <summary>
    /// Polling Service
    /// </summary>
    public class PollingService : IPollingService {
        /// <summary>
        /// On Sync Batch Received
        /// </summary>
        public event EventHandler<SyncBatchEventArgs>? OnSyncBatchReceived;
        /// <summary>
        /// Event Service
        /// </summary>
        private readonly EventService _eventService;
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger<PollingService> _logger;
        /// <summary>
        /// Matrix Rooms
        /// </summary>
        private ConcurrentDictionary<string, MatrixRoom> _matrixRooms = new ConcurrentDictionary<string, MatrixRoom>();
        /// <summary>
        /// Cts
        /// </summary>
        private CancellationTokenSource? _cts;
        /// <summary>
        /// Access Token
        /// </summary>
        private string? _accessToken;
        /// <summary>
        /// Next Batch
        /// </summary>
        private string? _nextBatch;
        /// <summary>
        /// Polling Timer
        /// </summary>
        private Timer? _pollingTimer;
        /// <summary>
        /// Timeout
        /// </summary>
        private ulong _timeout;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="eventService"></param>
        /// <param name="logger"></param>
        public PollingService(EventService eventService, ILogger<PollingService> logger) {
            _eventService = eventService;
            _logger = logger;
            _timeout = Constants.FirstSyncTimout;
        }
        /// <summary>
        /// Is Syncing
        /// </summary>
        public bool IsSyncing { get; private set; }
        /// <summary>
        /// Invited Rooms
        /// </summary>
        public MatrixRoom[] InvitedRooms => _matrixRooms.Values.Where(x => x.Status == nexIRC.Enum.MatrixRoomStatusEnum.Invited).ToArray();
        /// <summary>
        /// Joined Rooms
        /// </summary>
        public MatrixRoom[] JoinedRooms => _matrixRooms.Values.Where(x => x.Status == MatrixRoomStatusEnum.Joined).ToArray();
        /// <summary>
        /// Left Rooms
        /// </summary>
        public MatrixRoom[] LeftRooms => _matrixRooms.Values.Where(x => x.Status == MatrixRoomStatusEnum.Left).ToArray();
        /// <summary>
        /// Init
        /// </summary>
        /// <param name="nodeAddress"></param>
        /// <param name="accessToken"></param>
        public void Init(Uri nodeAddress, string accessToken) {
            _eventService.BaseAddress = nodeAddress;
            _accessToken = accessToken;
            _cts = new CancellationTokenSource();
            _matrixRooms = new ConcurrentDictionary<string, MatrixRoom>();
            _pollingTimer = new Timer(async _ => await PollAsync());
        }
        /// <summary>
        /// Start
        /// </summary>
        /// <param name="nextBatch"></param>
        /// <exception cref="NullReferenceException"></exception>
        public void Start(string? nextBatch = null) {
            try {
                if (_pollingTimer == null)
                    throw new NullReferenceException("Call Init first.");
                if (nextBatch != null)
                    _nextBatch = nextBatch;
                _pollingTimer.Change(TimeSpan.Zero, TimeSpan.FromMilliseconds(-1));
                IsSyncing = true;
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "Start");
            }
        }
        /// <summary>
        /// Stop
        /// </summary>
        public void Stop() {
            try {
                _cts?.Cancel();
                if (_pollingTimer != null) _pollingTimer.Change(Timeout.Infinite, Timeout.Infinite);
                IsSyncing = false;
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "Stop");
            }
        }
        /// <summary>
        /// Get Matrix Room
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        public MatrixRoom? GetMatrixRoom(string roomId) => _matrixRooms.TryGetValue(roomId, out MatrixRoom? matrixRoom) ? matrixRoom : null;
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose() {
            _cts?.Dispose();
            _pollingTimer?.Dispose();
        }
        /// <summary>
        /// Poll Async
        /// </summary>
        /// <returns></returns>
        private async Task PollAsync() {
            try {
                _pollingTimer!.Change(Timeout.Infinite, Timeout.Infinite);
                IsSyncing = true;
                CancellationToken token;
                if (_cts != null) {
                    token = _cts.Token;
                    SyncResponse response = await _eventService.SyncAsync(_accessToken!, _cts.Token,
                        _timeout, _nextBatch);
                    SyncBatch syncBatch = SyncBatch.Factory.CreateFromSync(response.NextBatch, response.Rooms);
                    _nextBatch = syncBatch.NextBatch;
                    _timeout = Constants.LaterSyncTimout;
                    RefreshRooms(syncBatch.MatrixRooms);
                    if (OnSyncBatchReceived != null) OnSyncBatchReceived.Invoke(this, new SyncBatchEventArgs(syncBatch));
                    _pollingTimer?.Change(TimeSpan.Zero, TimeSpan.FromMilliseconds(-1));
                }
            } catch (TaskCanceledException ex) {
                if (_cts != null) {
                    if (_cts.IsCancellationRequested && _pollingTimer != null) _pollingTimer.Change(TimeSpan.FromMilliseconds(Constants.LaterSyncTimout), TimeSpan.FromMilliseconds(-1));
                    IsSyncing = false;
                    if (_logger != null) {
                        _logger.LogError(
                            "Polling cancelled, _cts.IsCancellationRequested {@IsCancellationRequested}:, {@Message}",
                            _cts.IsCancellationRequested, ex.Message);
                    }
                }
            } catch (Exception ex) {
                _pollingTimer?
                    .Change(TimeSpan.FromMilliseconds(Constants.LaterSyncTimout), TimeSpan.FromMilliseconds(-1));
                IsSyncing = false;
                if (_logger != null) _logger.LogError("Polling: exception occured. Message: {@Message}", ex.Message);
            }
        }
        /// <summary>
        /// Refresh Rooms
        /// </summary>
        /// <param name="matrixRooms"></param>
        private void RefreshRooms(List<MatrixRoom> matrixRooms) {
            try {
                foreach (MatrixRoom room in matrixRooms)
                    if (!_matrixRooms.TryGetValue(room.Id, out MatrixRoom? retrievedRoom)) {
                        if (!_matrixRooms.TryAdd(room.Id, room))
                            if (_logger != null) _logger.LogError("Can not add matrix room");
                    } else {
                        var updatedUserIds = retrievedRoom
                            .JoinedUserIds
                            .Concat(room.JoinedUserIds)
                            .Distinct()
                            .ToList();
                        var updatedRoom = new MatrixRoom(retrievedRoom.Id, room.Status, updatedUserIds);
                        _matrixRooms.TryUpdate(room.Id, updatedRoom, retrievedRoom);
                    }
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "RefreshRooms");
            }

        }
    }
}