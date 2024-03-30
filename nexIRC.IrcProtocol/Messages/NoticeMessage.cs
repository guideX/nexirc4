using nexIRC.IrcProtocol.Interfaces;

namespace nexIRC.IrcProtocol.Messages
{
    /// <summary>
    /// Notice Message
    /// </summary>
    public class NoticeMessage : IRCMessage, IServerMessage, IClientMessage {
        /// <summary>
        /// From
        /// </summary>
        public string From { get; }
        /// <summary>
        /// Target
        /// </summary>
        public string Target { get; }
        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parsedMessage"></param>
        public NoticeMessage(ParsedIRCMessage parsedMessage) {
            From = parsedMessage.Prefix!.From;
            Target = parsedMessage.Parameters![0];
            Message = parsedMessage.Trailing;
        }
        /// <summary>
        /// Notice Message
        /// </summary>
        /// <param name="target"></param>
        /// <param name="text"></param>
        public NoticeMessage(string target, string text) {
            Target = target;
            Message = text;
            From = "";
        }
        /// <summary>
        /// Is Channel Message
        /// </summary>
        public bool IsChannelMessage => Target[0] == '#';
        /// <summary>
        /// Tokens
        /// </summary>
        public IEnumerable<string> Tokens => new[] { "NOTICE", Target, Message };
    }
}