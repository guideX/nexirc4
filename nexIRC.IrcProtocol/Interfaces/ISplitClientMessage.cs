namespace nexIRC.IrcProtocol.Interfaces {
    /// <summary>
    /// Split Client Message
    /// </summary>
    public interface ISplitClientMessage {
        /// <summary>
        /// Line Split Tokens
        /// </summary>
        IEnumerable<string[]> LineSplitTokens { get; }
    }
}