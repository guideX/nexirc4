namespace nexIRC.MatrixProtocol.Wrapper {
    /// <summary>
    /// Matrix Event Args
    /// </summary>
    public class MatrixRoomEventArgs : EventArgs {
        /// <summary>
        /// Event Type
        /// </summary>
        public nexIRC.MatrixProtocol.Core.Infrastructure.Dto.Sync.Event.EventType? EventType { get; set; }
        /// <summary>
        /// RoomId
        /// </summary>
        public string? RoomId { get; set; }
        /// <summary>
        /// Sender User ID
        /// </summary>
        public string? SenderUserId { get; set; }
        /// <summary>
        /// Message
        /// </summary>
        public string? Message { get; set; }
        /// <summary>
        /// Room Creator UserID
        /// </summary>
        public string? RoomCreatorUserId { get; set; }
    }
}