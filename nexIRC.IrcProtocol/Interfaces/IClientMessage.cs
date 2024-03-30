namespace nexIRC.IrcProtocol.Interfaces {
    /// <summary>
    /// Client Message
    /// </summary>
    public interface IClientMessage {
        /// <summary>
        /// Tokens
        /// </summary>
        IEnumerable<string> Tokens { get; }
    }
}