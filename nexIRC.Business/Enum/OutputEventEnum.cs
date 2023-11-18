namespace nexIRC.Business.Enum {
    /// <summary>
    /// Output Event Enum
    /// </summary>
    public enum OutputEventEnum {
        /// <summary>
        /// PrivMsg
        /// </summary>
        PrivMsg = 1,
        /// <summary>
        /// Connect Matrix
        /// </summary>
        ConnectMatrix = 2,
        /// <summary>
        /// Join Channel Matrix
        /// </summary>
        JoinChannelMatrix = 3,
        /// <summary>
        /// Disconnect Matrix
        /// </summary>
        DisconnectMatrix = 4,
        /// <summary>
        /// Part Channel Matrix
        /// </summary>
        PartChannelMatrix = 5,
        /// <summary>
        /// Get Joined Channels
        /// </summary>
        GetJoinedChannels = 6,
        /// <summary>
        /// Send Message Matrix
        /// </summary>
        SendMessageMatrix = 7,
        /// <summary>
        /// Auto Join Matrix
        /// </summary>
        AutoJoinMatrix = 8
    }
}