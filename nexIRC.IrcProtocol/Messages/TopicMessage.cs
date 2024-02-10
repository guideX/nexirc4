namespace nexIRC.IrcProtocol.Messages {
    /// <summary>
    /// Topic Message
    /// </summary>
    public class TopicMessage : IRCMessage, IServerMessage, IClientMessage {
        /// <summary>
        /// Channel
        /// </summary>
        public string Channel { get; }
        /// <summary>
        /// Topic
        /// </summary>
        public string Topic { get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parsedMessage"></param>
        public TopicMessage(ParsedIRCMessage parsedMessage) {
            Channel = parsedMessage.Parameters![0];
            Topic = parsedMessage.Trailing;
        }
        /// <summary>
        /// Topic Message
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="topic"></param>
        public TopicMessage(string channel, string topic) {
            Channel = channel;
            Topic = topic;
        }
        /// <summary>
        /// Tokens
        /// </summary>
        public IEnumerable<string> Tokens => new[] { "TOPIC", Channel, Topic };
    }
}