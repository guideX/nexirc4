namespace nexIRC.MatrixProtocol.Core.Domain.MatrixRoom {
    using nexIRC.Enum;
    using nexIRC.Model.Matrix.Room;
    using System.Collections.Generic;
    /// <summary>
    /// Matrix Room
    /// </summary>
    public record MatrixRoom {
        /// <summary>
        /// Input
        /// </summary>
        private MatrixRoomInputModel _input;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="joinedUserIds"></param>
        public MatrixRoom(MatrixRoomInputModel obj) {
            _input = obj;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="joinedUserIds"></param>
        public MatrixRoom(string id, MatrixRoomStatusEnum status, List<string> joinedUserIds) {
            _input = new MatrixRoomInputModel() {
                Id = id,
                Status = status,
                JoinedUserIds = joinedUserIds
            };
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        public MatrixRoom(string id, MatrixRoomStatusEnum status) {
            _input = new MatrixRoomInputModel() {
                Id = id,
                Status = status,
                JoinedUserIds = new List<string>()
            };
        }
        /// <summary>
        /// ID
        /// </summary>
        public string Id {
            get {
                return _input.Id!;
            }
        }
        /// <summary>
        /// Status
        /// </summary>
        public MatrixRoomStatusEnum Status {
            get {
                return _input.Status;
            }
        }
        /// <summary>
        /// Joined UserIDs
        /// </summary>
        public List<string> JoinedUserIds {
            get {
                return _input.JoinedUserIds!;
            }
        }
    }
}