namespace nexIRC.IrcProtocol.Messages {
    /// <summary>
    /// Pass Message
    /// </summary>
    public class PassMessage : IRCMessage, IClientMessage {
        /// <summary>
        /// Password
        /// </summary>
        private readonly string _password;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="password"></param>
        public PassMessage(string password/*, string appPath) : base(appPath*/) {
            _password = password;
        }
        /// <summary>
        /// Tokens
        /// </summary>
        public IEnumerable<string> Tokens => new[] { "PASS", _password };
    }
}