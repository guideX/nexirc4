namespace nexIRC.Model {
    /// <summary>
    /// Matrix Send Message
    /// </summary>
    public class MatrixSendMessageModel {
        /// <summary>
        /// Channel
        /// </summary>
        public string ChannelName { get; set; } = "";
        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; } = "";
    }
}