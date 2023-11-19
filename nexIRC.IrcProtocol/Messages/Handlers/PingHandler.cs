namespace nexIRC.IrcProtocol.Messages.Handlers {
    /// <summary>
    /// Ping Handler
    /// </summary>
    public class PingHandler : MessageHandler<PingMessage> {
        /// <summary>
        /// App Path
        /// </summary>
        private string _appPath;
        /// <summary>
        /// Constructor
        /// </summary>
        public PingHandler(string appPath) {
            _appPath = appPath;
        }
        /// <summary>
        /// Handler Async
        /// </summary>
        /// <param name="serverMessage"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public override Task HandleAsync(PingMessage serverMessage, Client client) {
            return client.SendAsync(new PongMessage(serverMessage.Target, _appPath));
        }
    }
}