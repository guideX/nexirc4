namespace nexIRC.IrcProtocol.Messages {
    /// <summary>
    /// Message Handler
    /// </summary>
    /// <typeparam name="TServerMessage"></typeparam>
    public abstract class MessageHandler<TServerMessage> : IMessageHandler<TServerMessage> where TServerMessage : IServerMessage {
        /// <summary>
        /// Message
        /// </summary>
        public TServerMessage Message { get; internal set; }
        /// <summary>
        /// Handle Async
        /// </summary>
        /// <param name="serverMessage"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public abstract Task HandleAsync(TServerMessage serverMessage, Client client);
    }
}