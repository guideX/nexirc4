namespace nexIRC.IrcProtocol.Messages {
    /// <summary>
    /// User Message
    /// </summary>
    public class UserMessage : IRCMessage, IClientMessage {
        /// <summary>
        /// User Name
        /// </summary>
        public string UserName { get; }
        /// <summary>
        /// Real Name
        /// </summary>
        public string RealName { get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="realName"></param>
        public UserMessage(string userName, string realName) {
            UserName = userName;
            RealName = realName;
        }
        /// <summary>
        /// Tokens
        /// </summary>
        public IEnumerable<string> Tokens => new[] { "USER", UserName, "0", "-", RealName };
    }
}