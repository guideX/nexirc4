using nexIRC.Model;
namespace nexIRC.Messages {
    /// <summary>
    /// Open Query Message
    /// </summary>
    public class OpenQueryMessage {
        /// <summary>
        /// User
        /// </summary>
        public UserModel User { get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="user"></param>
        public OpenQueryMessage(UserModel user) => User = user;
    }
}