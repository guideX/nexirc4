namespace nexIRC.MatrixProtocol.Core.Domain.Services {
    using System;
    using MatrixRoom;
    /// <summary>
    /// Poling Service
    /// </summary>
    public interface IPollingService : IDisposable {
        /// <summary>
        /// Invited Rooms
        /// </summary>
        MatrixRoom[] InvitedRooms { get; }
        /// <summary>
        /// Joined Rooms
        /// </summary>
        MatrixRoom[] JoinedRooms { get; }
        /// <summary>
        /// Left Rooms
        /// </summary>
        MatrixRoom[] LeftRooms { get; }
        /// <summary>
        /// Is Syncing
        /// </summary>
        public bool IsSyncing { get; }
        /// <summary>
        /// On Sync Batch Received
        /// </summary>
        public event EventHandler<SyncBatchEventArgs> OnSyncBatchReceived;
        /// <summary>
        /// Init
        /// </summary>
        /// <param name="nodeAddress"></param>
        /// <param name="accessToken"></param>
        void Init(Uri nodeAddress, string accessToken);
        /// <summary>
        /// Start
        /// </summary>
        /// <param name="nextBatch"></param>
        void Start(string? nextBatch = null);
        /// <summary>
        /// Stop
        /// </summary>
        void Stop();
        /// <summary>
        /// Get Matrix Room
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        MatrixRoom? GetMatrixRoom(string roomId);
    }
}