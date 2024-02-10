namespace nexIRC.MatrixProtocol.Core.Infrastructure.Dto.Room.Join {
    /// <summary>
    /// Join Room Response
    /// </summary>
    /// <param name="RoomId"></param>
    public record JoinRoomResponse(string RoomId) {
        /// <summary>
        ///     <b>Required.</b> The joined room ID.
        /// </summary>
        public string RoomId { get; } = RoomId;
    }
}