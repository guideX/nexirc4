namespace nexIRC.IrcProtocol.Messages {
    /// <summary>
    /// Quit Message
    /// </summary>
    public class QuitMessage : IRCMessage, IServerMessage, IClientMessage {
        #region "private variables"
        /// <summary>
        /// Nick
        /// </summary>
        private readonly string _nick;
        /// <summary>
        /// Message
        /// </summary>
        private readonly string _message;
        #endregion
        #region "public properties"
        /// <summary>
        /// Nick
        /// </summary>
        public string Nick { get { return _nick; } }
        /// <summary>
        /// Message
        /// </summary>
        public string Message { get { return _message; } }
        #endregion
        #region "public methods"
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parsedMessage"></param>
        public QuitMessage(ParsedIRCMessage parsedMessage) {
            _nick = parsedMessage.Prefix!.From;
            _message = parsedMessage.Trailing;
        }
        /// <summary>
        /// Quit Message
        /// </summary>
        /// <param name="message"></param>
        public QuitMessage(string message) {
            _message = message;
            _nick = "";
        }
        /// <summary>
        /// Tokens
        /// </summary>
        public IEnumerable<string> Tokens => new[] { "QUIT", Message };
        #endregion
    }
}