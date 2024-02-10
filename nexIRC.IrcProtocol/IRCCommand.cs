namespace nexIRC.IrcProtocol {
    /// <summary>
    /// IRC Command
    /// </summary>
    public enum IRCCommand {
        /// <summary>
        /// Unknown
        /// </summary>
        UNKNOWN,
        /// <summary>
        /// Nick
        /// </summary>
        NICK,
        /// <summary>
        /// Mode
        /// </summary>
        MODE,
        /// <summary>
        /// Quit
        /// </summary>
        QUIT,
        /// <summary>
        /// Join
        /// </summary>
        JOIN,
        /// <summary>
        /// Part
        /// </summary>
        PART,
        /// <summary>
        /// Topic
        /// </summary>
        TOPIC,
        /// <summary>
        /// Invite
        /// </summary>
        INVITE,
        /// <summary>
        /// Kick
        /// </summary>
        KICK,
        /// <summary>
        /// PrivMsg
        /// </summary>
        PRIVMSG,
        /// <summary>
        /// Notice
        /// </summary>
        NOTICE,
        /// <summary>
        /// Ping
        /// </summary>
        PING,
        /// <summary>
        /// Error
        /// </summary>
        ERROR
    }
}