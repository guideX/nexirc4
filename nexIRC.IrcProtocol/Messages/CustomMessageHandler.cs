namespace nexIRC.IrcProtocol.Messages {
    /// <summary>
    /// Custom Message Handler
    /// </summary>
    /// <typeparam name="TServerMessage"></typeparam>
    public abstract class CustomMessageHandler<TServerMessage> : MessageHandler<TServerMessage>, ICustomHandler where TServerMessage : IServerMessage {
        /// <summary>
        /// Handled
        /// </summary>
        public bool Handled { get; set; }
    }
}