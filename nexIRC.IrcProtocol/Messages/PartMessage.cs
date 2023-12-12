namespace nexIRC.IrcProtocol.Messages {
    /// <summary>
    /// Part Message
    /// </summary>
    public class PartMessage : IRCMessage, IServerMessage, IClientMessage {
        #region "PRIVATE VARIABLES"
        /// <summary>
        /// Channels
        /// </summary>
        private readonly string _channels;
        /// <summary>
        /// Nick
        /// </summary>
        private readonly string _nick;
        /// <summary>
        /// Channel
        /// </summary>
        private readonly string _channel;
        #endregion
        #region "PUBLIC VARIABLES"
        /// <summary>
        /// Nick
        /// </summary>
        public string Nick { get { return _nick; } }
        /// <summary>
        /// Channel
        /// </summary>
        public string Channel { get { return _channel; } }
        #endregion
        #region "PUBLIC METHODS"
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parsedMessage"></param>
        public PartMessage(ParsedIRCMessage parsedMessage) {
            _channels = "";
            _nick = parsedMessage.Prefix!.From;
            _channel = parsedMessage.Parameters![0];
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="channels"></param>
        public PartMessage(string channels) {
            _nick = "";
            _channel = "";
            _channels = channels;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="channels"></param>
        public PartMessage(params string[] channels) : this(string.Join(",", channels)) {
        }
        /// <summary>
        /// Tokens
        /// </summary>
        public IEnumerable<string> Tokens => new[] { "PART", _channels };
        #endregion
    }
}