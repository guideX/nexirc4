using nexIRC.Enum;
namespace nexIRC.MatrixProtocol {
    using Core.Domain;
    using Core.Domain.MatrixRoom;
    using Core.Domain.Services;
    using Core.Infrastructure.Dto.Event;
    using Core.Infrastructure.Dto.Login;
    using Core.Infrastructure.Dto.Room.Create;
    using Core.Infrastructure.Dto.Room.Join;
    using Core.Infrastructure.Dto.Room.Joined;
    using Core.Infrastructure.Services;
    using nexIRC.Business.Helper;
    using nexIRC.MatrixProtocol.Interfaces;
    /// <summary>
    /// Matrix Client
    /// </summary>
    public class MatrixClient : IMatrixClient {
        #region "private variables"
        /// <summary>
        /// Cts
        /// </summary>
        private readonly CancellationTokenSource _cts = new();
        /// <summary>
        /// Polling Service
        /// </summary>
        private readonly IPollingService _pollingService;
        /// <summary>
        /// User Service
        /// </summary>
        private readonly UserService _userService;
        /// <summary>
        /// Room Service
        /// </summary>
        private readonly RoomService _roomService;
        /// <summary>
        /// Event Service
        /// </summary>
        private readonly EventService _eventService;
        /// <summary>
        /// Access Token
        /// </summary>
        private string? _accessToken;
        /// <summary>
        /// Transaction Number
        /// </summary>
        private ulong _transactionNumber;
        #endregion
        #region "public variables"
        /// <summary>
        /// UserID
        /// </summary>
        public string UserId { get; private set; } = "";
        /// <summary>
        /// Base Addresss
        /// </summary>
        public Uri? BaseAddress { get; private set; }
        /// <summary>
        /// Is logged In
        /// </summary>
        public bool IsLoggedIn { get; private set; }
        /// <summary>
        /// Is Syncing
        /// </summary>
        public bool IsSyncing { get; private set; }
        /// <summary>
        /// Invited Rooms
        /// </summary>
        public MatrixRoom[] InvitedRooms => _pollingService.InvitedRooms;
        /// <summary>
        /// Joined Rooms
        /// </summary>
        public MatrixRoom[] JoinedRooms => _pollingService.JoinedRooms;
        /// <summary>
        /// Left Rooms
        /// </summary>
        public MatrixRoom[] LeftRooms => _pollingService.LeftRooms;
        /// <summary>
        /// On Matrix Room Events Recieved
        /// </summary>
        public event EventHandler<MatrixRoomEventsEventArgs>? OnMatrixRoomEventsReceived;
        #endregion
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pollingService"></param>
        /// <param name="userService"></param>
        /// <param name="roomService"></param>
        /// <param name="eventService"></param>
        public MatrixClient(IPollingService pollingService, UserService userService, RoomService roomService, EventService eventService) {
            _pollingService = pollingService;
            _userService = userService;
            _roomService = roomService;
            _eventService = eventService;
        }
        /// <summary>
        /// Login Async
        /// </summary>
        /// <param name="baseAddress"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public async Task LoginAsync(Uri baseAddress, string user, string password, string deviceId) {
            try {
                _userService.BaseAddress = baseAddress;
                _roomService.BaseAddress = baseAddress;
                _eventService.BaseAddress = baseAddress;
                BaseAddress = baseAddress;
                LoginResponse response = await _userService.LoginAsync(user, password, deviceId, _cts.Token);
                UserId = response.UserId;
                _accessToken = response.AccessToken;
                _pollingService.Init(baseAddress, _accessToken);
                IsLoggedIn = true;
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.MatrixProtocol.MatrixClient.LoginAsync");
            }
        }
        /// <summary>
        /// Start
        /// </summary>
        /// <param name="nextBatch"></param>
        /// <exception cref="Exception"></exception>
        public void Start(string? nextBatch = null) {
            try {
                if (!IsLoggedIn)
                    throw new Exception("Call LoginAsync first");
                _pollingService.OnSyncBatchReceived += OnSyncBatchReceived;
                _pollingService.Start(nextBatch);
                IsSyncing = _pollingService.IsSyncing;
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.MatrixProtocol.MatrixClient.Start");
            }
        }
        /// <summary>
        /// Stop
        /// </summary>
        public void Stop() {
            try {
                _pollingService.Stop();
                _pollingService.OnSyncBatchReceived -= OnSyncBatchReceived;
                IsSyncing = _pollingService.IsSyncing;
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.MatrixProtocol.MatrixClient.Stop");
            }
        }
        /// <summary>
        /// Create Trusted Private Room Async
        /// </summary>
        /// <param name="invitedUserIds"></param>
        /// <returns></returns>
        public async Task<CreateRoomResponse> CreateTrustedPrivateRoomAsync(string[] invitedUserIds) => await _roomService.CreateRoomAsync(_accessToken!, invitedUserIds, _cts.Token);
        /// <summary>
        /// Join Trusted Private Room Async
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        public async Task<JoinRoomResponse> JoinTrustedPrivateRoomAsync(string roomId) {
            try {
                MatrixRoom? matrixRoom = _pollingService.GetMatrixRoom(roomId);
                if (matrixRoom != null && matrixRoom.Status != MatrixRoomStatusEnum.Invited) return new JoinRoomResponse(matrixRoom.Id);
                return await _roomService.JoinRoomAsync(_accessToken!, roomId, _cts.Token);
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.MatrixProtocol.MatrixClient.JoinTrustedPrivateRoomAsync");
                throw;
            }
        }
        /// <summary>
        /// Send Message Async
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public async Task<string> SendMessageAsync(string roomId, string message) {
            try {
                string transactionId = CreateTransactionId();
                EventResponse eventResponse = await _eventService.SendMessageAsync(_accessToken!, roomId, transactionId, message, _cts.Token);
                if (eventResponse.EventId == null)
                    throw new NullReferenceException(nameof(eventResponse.EventId));
                return eventResponse.EventId;
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.MatrixProtocol.MatrixClient.SendMessageAsync");
                throw;
            }
        }
        /// <summary>
        /// Get Joined Rooms IDS Async
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> GetJoinedRoomsIdsAsync() {
            try {
                JoinedRoomsResponse response = await _roomService.GetJoinedRoomsAsync(_accessToken!, _cts.Token);
                return response.JoinedRoomIds;
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.MatrixProtocol.MatrixClient.GetJoinedRoomsIdsAsync");
                throw;
            }
        }
        /// <summary>
        /// Leave Room Async
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        public async Task LeaveRoomAsync(string roomId) =>
            await _roomService.LeaveRoomAsync(_accessToken!, roomId, _cts.Token);
        /// <summary>
        /// On Sync Batch Received
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="syncBatchEventArgs"></param>
        /// <exception cref="ArgumentException"></exception>
        private void OnSyncBatchReceived(object? sender, SyncBatchEventArgs syncBatchEventArgs) {
            try {
                if (sender is not IPollingService) throw new ArgumentException("sender is not polling service");
                SyncBatch batch = syncBatchEventArgs.SyncBatch;
                OnMatrixRoomEventsReceived?.Invoke(this, new MatrixRoomEventsEventArgs(batch.MatrixRoomEvents, batch.NextBatch));
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.MatrixProtocol.MatrixClient.OnSyncBatchReceived");
            }
        }
        /// <summary>
        /// Create TransactionID
        /// </summary>
        /// <returns></returns>
        private string CreateTransactionId() {
            try {
                long timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                ulong counter = _transactionNumber;
                _transactionNumber += 1;
                return $"m{timestamp}.{counter}";
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.MatrixProtocol.MatrixClient.CreateTransactionId");
                return string.Empty;
            }
            
        }
    }
}