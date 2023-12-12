namespace nexIRC.IrcProtocol.Messages {
    public class KickMessage : IRCMessage, IServerMessage {
        /// <summary>
        /// Kicked By
        /// </summary>
        public string KickedBy { get; }
        /// <summary>
        /// Channel
        /// </summary>
        public string Channel { get; }
        /// <summary>
        /// Nick
        /// </summary>
        public string Nick { get; }
        /// <summary>
        /// Comment
        /// </summary>
        public string Comment { get; }
        /// <summary>
        /// kick message
        /// </summary>
        /// <param name="parsedMessage"></param>
        public KickMessage(ParsedIRCMessage parsedMessage) {
            KickedBy = parsedMessage.Prefix!.From;
            Channel = parsedMessage.Parameters![0];
            Nick = parsedMessage.Parameters[1];
            Comment = parsedMessage.Trailing;
        }
    }
}