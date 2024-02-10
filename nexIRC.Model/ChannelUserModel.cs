namespace nexIRC.Model {
    /// <summary>
    /// Represents a user in a specific channel
    /// </summary>
    public class ChannelUserModel {
        /// <summary>
        /// User
        /// </summary>
        public UserModel User { get; }
        /// <summary>
        /// Status
        /// </summary>
        public string Status { get; }
        /// <summary>
        /// Nick
        /// </summary>
        public string Nick => User.Nick;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="user"></param>
        /// <param name="status"></param>
        public ChannelUserModel(UserModel user, string status) {
            User = user;
            Status = status;
        }
        /// <summary>
        /// Costructor
        /// </summary>
        /// <param name="user"></param>
        public ChannelUserModel(UserModel user) : this(user, string.Empty) {
        }

        public override string ToString() => Status + User.Nick;
    }
}