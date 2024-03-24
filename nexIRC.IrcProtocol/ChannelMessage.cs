using nexIRC.Model;
namespace nexIRC.IrcProtocol {
    /// <summary>
    /// Represents a channel message
    /// </summary>
    public class ChannelMessage : EventArgs {
        /// <summary>
        /// User
        /// </summary>
        public UserModel User { get; }
        /// <summary>
        /// Channel
        /// </summary>
        public Channel Channel { get; }
        /// <summary>
        /// Text
        /// </summary>
        public string Text { get; }
        /// <summary>
        /// Timestamp
        /// </summary>
        public DateTime Timestamp { get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="user"></param>
        /// <param name="channel"></param>
        /// <param name="text"></param>
        public ChannelMessage(UserModel user, Channel channel, string text) {
            User = user;
            Channel = channel;
            Text = text;
            Timestamp = DateTime.Now;
        }
    }
}