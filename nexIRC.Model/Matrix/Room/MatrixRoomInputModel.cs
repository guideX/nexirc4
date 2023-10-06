using nexIRC.Enum;
namespace nexIRC.Model.Matrix.Room {
    /// <summary>
    /// Matrix Room Input Model
    /// </summary>
    public class MatrixRoomInputModel {
        /// <summary>
        /// ID
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        public MatrixRoomStatusEnum Status { get; set; }
        /// <summary>
        /// Joined UserIDs
        /// </summary>
        public List<string>? JoinedUserIds { get; set; }
    }
}