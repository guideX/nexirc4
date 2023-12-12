namespace nexIRC.IrcProtocol.Messages {
    /// <summary>
    /// Join Message
    /// </summary>
    public class JoinMessage : IRCMessage, IServerMessage, IClientMessage {
        #region "private variables"
        /// <summary>
        /// Nick
        /// </summary>
        private readonly string _nick;
        /// <summary>
        /// Channels
        /// </summary>
        private readonly string _channels;
        /// <summary>
        /// Channel
        /// </summary>
        private readonly string _channel;
        /// <summary>
        /// Keys
        /// </summary>
        private readonly string _keys;
        #endregion
        /// <summary>
        /// Nick
        /// </summary>
        public string Nick {
            get {
                return _nick;
            }
        }
        /// <summary>
        /// Channel
        /// </summary>
        public string Channel {
            get {
                return _channel;
            }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parsedMessage"></param>
        public JoinMessage(ParsedIRCMessage parsedMessage) {
            _nick = parsedMessage.Prefix!.From;
            _channel = parsedMessage.Parameters![0];
            _channels = "";
            _keys = "";
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="channels"></param>
        /// <param name="keys"></param>
        public JoinMessage(string channels, string keys) {
            _channels = channels;
            _keys = keys;
            _nick = "";
            _channel = "";
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="channels"></param>
        public JoinMessage(params string[] channels)  {
            _channels = string.Join(",", channels);
            _nick = "";
            _channel = "";
            _keys = "";
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="channelsWithKeys"></param>
        public JoinMessage(Dictionary<string, string> channelsWithKeys) {
            _channels = string.Join(",", channelsWithKeys.Keys);
            _keys = string.Join(",", channelsWithKeys.Values);
            _nick = "";
            _channel = "";
        }
        /// <summary>
        /// Tokens
        /// </summary>
        public IEnumerable<string> Tokens => new[] { "JOIN", _channels, _keys };
    }
}