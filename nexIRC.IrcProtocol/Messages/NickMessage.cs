namespace nexIRC.IrcProtocol.Messages {
    /// <summary>
    /// Nick Message
    /// </summary>
    public class NickMessage : IRCMessage, IServerMessage, IClientMessage {
        /// <summary>
        /// Old Nick
        /// </summary>
        public string OldNick { get; }
        /// <summary>
        /// New Nick
        /// </summary>
        public string NewNick { get; }
        /// <summary>
        /// Nick Message
        /// </summary>
        /// <param name="parsedMessage"></param>
        public NickMessage(ParsedIRCMessage parsedMessage/*, string appPath) : base(appPath*/) {
            OldNick = parsedMessage.Prefix!.From;
            NewNick = parsedMessage.Parameters![0];
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="newNick"></param>
        public NickMessage(string newNick/*, string appPath) : base(appPath*/) {
            NewNick = newNick;
            OldNick = "";
        }
        /// <summary>
        /// Tokens
        /// </summary>
        public IEnumerable<string> Tokens => new[] { "NICK", NewNick };
    }
}