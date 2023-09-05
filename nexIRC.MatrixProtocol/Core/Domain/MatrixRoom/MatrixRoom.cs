namespace nexIRC.MatrixProtocol.Core.Domain.MatrixRoom {
    using System.Collections.Generic;
    /// <summary>
    /// Matrix Room
    /// </summary>
    public record MatrixRoom {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="joinedUserIds"></param>
        public MatrixRoom(string id, MatrixRoomStatus status, List<string> joinedUserIds) {
            Id = id;
            Status = status;
            JoinedUserIds = joinedUserIds;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        public MatrixRoom(string id, MatrixRoomStatus status) {
            Id = id;
            Status = status;
            JoinedUserIds = new List<string>();
        }
        /// <summary>
        /// ID
        /// </summary>
        public string Id { get; }
        /// <summary>
        /// Status
        /// </summary>
        public MatrixRoomStatus Status { get; }
        /// <summary>
        /// Joined UserIDs
        /// </summary>
        public List<string> JoinedUserIds { get; }
    }
}