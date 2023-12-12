namespace nexIRC.IrcProtocol.Messages.Handlers {
    /// <summary>
    /// Ping Handler
    /// </summary>
    public class PingHandler : MessageHandler<PingMessage> {
        /// <summary>
        /// Constructor
        /// </summary>
        public PingHandler() {
        }
        /// <summary>
        /// Handler Async
        /// </summary>
        /// <param name="serverMessage"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public override Task HandleAsync(PingMessage serverMessage, Client client) {
            return client.SendAsync(new PongMessage(serverMessage.Target));
        }
    }
}