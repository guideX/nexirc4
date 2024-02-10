namespace nexIRC.MatrixProtocol.Interfaces {
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Domain.MatrixRoom;
    using Core.Infrastructure.Dto.Room.Create;
    using Core.Infrastructure.Dto.Room.Join;
    /// <summary>
    /// Matrix Client
    /// </summary>
    public interface IMatrixClient {
        /// <summary>
        /// UserID
        /// </summary>
        string UserId { get; }
        /// <summary>
        /// Base Address
        /// </summary>
        Uri? BaseAddress { get; }
        /// <summary>
        /// Is Logged In
        /// </summary>
        bool IsLoggedIn { get; }
        /// <summary>
        /// Is Syncing
        /// </summary>
        bool IsSyncing { get; }
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
        /// On Matrix Room Events Received
        /// </summary>
        event EventHandler<MatrixRoomEventsEventArgs> OnMatrixRoomEventsReceived;
        /// <summary>
        /// Login Async
        /// </summary>
        /// <param name="baseAddress"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        Task LoginAsync(Uri baseAddress, string user, string password, string deviceId);
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
        /// Create trusted Private Room Async
        /// </summary>
        /// <param name="invitedUserIds"></param>
        /// <returns></returns>
        Task<CreateRoomResponse> CreateTrustedPrivateRoomAsync(string[] invitedUserIds);
        /// <summary>
        /// Join Trusted Private Room Async
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        Task<JoinRoomResponse> JoinTrustedPrivateRoomAsync(string roomId);
        /// <summary>
        /// Send Message Async
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        Task<string> SendMessageAsync(string roomId, string message);
        /// <summary>
        /// Get Joined Rooms Ids Async
        /// </summary>
        /// <returns></returns>
        Task<List<string>> GetJoinedRoomsIdsAsync();
        /// <summary>
        /// Leave Room Async
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        Task LeaveRoomAsync(string roomId);
    }
}