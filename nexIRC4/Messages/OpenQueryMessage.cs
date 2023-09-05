using nexIRC.IrcProtocol;
namespace nexIRC.Messages {
    /// <summary>
    /// Open Query Message
    /// </summary>
    public class OpenQueryMessage {
        /// <summary>
        /// User
        /// </summary>
        public User User { get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="user"></param>
        public OpenQueryMessage(User user) => User = user;
    }
}