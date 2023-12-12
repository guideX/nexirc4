namespace nexIRC.IrcProtocol.Messages {
    /// <summary>
    /// Rpl Welcome Message
    /// </summary>
    public class RplWelcomeMessage : IRCMessage, IServerMessage {
        /// <summary>
        /// Text
        /// </summary>
        public string Text { get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parsedMessage"></param>
        public RplWelcomeMessage(ParsedIRCMessage parsedMessage/*, string appPath) : base(appPath*/) {
            Text = parsedMessage.Trailing;
        }
    }
}