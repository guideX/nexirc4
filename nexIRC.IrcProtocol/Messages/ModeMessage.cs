namespace nexIRC.IrcProtocol.Messages {
    /// <summary>
    /// Mode Message
    /// </summary>
    public class ModeMessage : IRCMessage, IServerMessage, IClientMessage {
        /// <summary>
        /// Prefix
        /// </summary>
        public IRCPrefix Prefix { get; }
        /// <summary>
        /// Target
        /// </summary>
        public string Target { get; }
        /// <summary>
        /// Modes
        /// </summary>
        public string Modes { get; }
        /// <summary>
        /// Parameters
        /// </summary>
        public string[] Parameters { get; }
        /// <summary>
        /// Mode Message
        /// </summary>
        /// <param name="parsedMessage"></param>
        public ModeMessage(ParsedIRCMessage parsedMessage) {
            Parameters = new string[0];
            Prefix = parsedMessage.Prefix!;
            Target = parsedMessage.Parameters![0];
            Modes = parsedMessage.Parameters[1] ?? parsedMessage.Trailing;
            if (parsedMessage.Parameters.Length > 2) {
                Parameters = parsedMessage.Parameters
                    .Skip(2)
                    .ToArray();
            }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="target"></param>
        /// <param name="modes"></param>
        public ModeMessage(string target, string modes) {
            Target = target;
            Modes = modes;
            Parameters = new string[0];
            Prefix = new IRCPrefix("");
        }
        /// <summary>
        /// Tokens
        /// </summary>
        public IEnumerable<string> Tokens => new[] { "MODE", Target, Modes };
    }
}