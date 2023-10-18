namespace nexIRC.Model {
    /// <summary>
    /// Server Message Model
    /// </summary>
    public class ServerMessageModel {
        /// <summary>
        /// Text
        /// </summary>
        public string Text { get; }
        /// <summary>
        /// Time stamp
        /// </summary>
        public DateTime Timestamp { get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="text"></param>
        public ServerMessageModel(string text) {
            Text = text;
            Timestamp = DateTime.Now;
        }
    }
}