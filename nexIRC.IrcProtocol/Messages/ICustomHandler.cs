namespace nexIRC.IrcProtocol.Messages {
    /// <summary>
    /// Custom Handler
    /// </summary>
    public interface ICustomHandler {
        /// <summary>
        /// Handled
        /// </summary>
        bool Handled { get; }
    }
}