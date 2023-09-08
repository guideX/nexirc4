namespace nexIRC.IrcProtocol.Connection {
    /// <summary>
    /// Represents an interface for a connection
    /// </summary>
    public interface IConnection : IDisposable {
        /// <summary>
        /// Connect Async
        /// </summary>
        /// <returns></returns>
        Task ConnectAsync();
        /// <summary>
        /// Send Async
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task SendAsync(string data);
        /// <summary>
        /// Data Received
        /// </summary>
        event EventHandler<DataReceivedEventArgs> DataReceived;
        /// <summary>
        /// Connected
        /// </summary>
        event EventHandler Connected;
        /// <summary>
        /// Disconnected
        /// </summary>
        event EventHandler Disconnected;
    }
}