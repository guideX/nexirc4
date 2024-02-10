namespace nexIRC.MatrixProtocol.Core.Infrastructure.Dto.Sync.Event {
    /// <summary>
    /// Event Type
    /// </summary>
    public enum EventType {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown,
        /// <summary>
        /// Create
        /// </summary>
        Create,
        /// <summary>
        /// Member
        /// </summary>
        Member,
        /// <summary>
        /// Message
        /// </summary>
        Message,
        /// <summary>
        /// Encryption
        /// </summary>
        Encrypted,
        /// <summary>
        /// Encryption
        /// </summary>
        Encryption,
        /// <summary>
        /// Room Key
        /// </summary>
        RoomKey
    }
}