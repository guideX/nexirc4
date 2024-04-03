namespace nexIRC.Model {
    /// <summary>
    /// Autojoin Model
    /// </summary>
    public class AutojoinModel {
        /// <summary>
        /// IRC Channel
        /// </summary>
        public string? IRCChannel { get; set; }
        /// <summary>
        /// Matrix ChannelID
        /// </summary>
        public string? MatrixChannelID { get; set; }
    }
}