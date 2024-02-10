namespace nexIRC.MatrixProtocol.Core.Infrastructure.Dto.Event {
    /// <summary>
    /// Event Type Enum
    /// </summary>
    public enum EventTypeEnum {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// Encryption
        /// </summary>
        Encryption = 1,
        /// <summary>
        /// Room Key
        /// </summary>
        RoomKey = 2,
        /// <summary>
        /// Join Room
        /// </summary>
        JoinRoom = 3,
        /// <summary>
        /// Invite To Room
        /// </summary>
        InviteToRoom = 4,
        /// <summary>
        /// Text Message
        /// </summary>
        TextMessage = 5,
        /// <summary>
        /// Create Room
        /// </summary>
        CreateRoom = 6,
        /// <summary>
        /// Encrypted
        /// </summary>
        Encrypted = 7
    }
}