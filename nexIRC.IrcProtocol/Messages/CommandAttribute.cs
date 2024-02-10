namespace nexIRC.IrcProtocol.Messages {
    /// <summary>
    /// Command Attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CommandAttribute : Attribute {
        /// <summary>
        /// Command
        /// </summary>
        public string Command { get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="command"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public CommandAttribute(string command) {
            _ = command ?? throw new ArgumentNullException(nameof(command));
            Command = command.ToUpper();
        }
    }
}