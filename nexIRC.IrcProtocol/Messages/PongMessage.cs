namespace nexIRC.IrcProtocol.Messages {
    /// <summary>
    /// Pong Message
    /// </summary>
    public class PongMessage : IRCMessage, IClientMessage {
        /// <summary>
        /// Target
        /// </summary>
        public string Target { get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="target"></param>
        public PongMessage(string target) {
            Target = target;
        }
        /// <summary>
        /// Tokens
        /// </summary>
        public IEnumerable<string> Tokens => new[] { "PONG", Target };
    }
}