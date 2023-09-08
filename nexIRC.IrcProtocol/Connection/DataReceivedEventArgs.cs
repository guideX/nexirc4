namespace nexIRC.IrcProtocol.Connection {
    /// <summary>
    /// Data Received Event Args
    /// </summary>
    public class DataReceivedEventArgs : EventArgs {
        /// <summary>
        /// Data
        /// </summary>
        public string Data { get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="data"></param>
        public DataReceivedEventArgs(string data) {
            Data = data;
        }
    }
}