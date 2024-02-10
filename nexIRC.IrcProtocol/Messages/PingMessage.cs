namespace nexIRC.IrcProtocol.Messages {
    /// <summary>
    /// Ping Message
    /// </summary>
    public class PingMessage : IRCMessage, IServerMessage {
        /// <summary>
        /// Target
        /// </summary>
        public string Target { get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parsedMessage"></param>
        public PingMessage(ParsedIRCMessage parsedMessage) {
            Target = parsedMessage.Trailing;
        }
    }
}