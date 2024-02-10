/*
using System.Threading.Channels;

namespace nexIRC.Model {
    public class ChannelMessageModel : EventArgs {
        public UserModel User { get; }
        public Channel Channel { get; }
        public string Text { get; }
        public DateTime Timestamp { get; }

        public ChannelMessageModel(UserModel user, nexIRC.IrcProtocol.Channel channel, string text) {
            if (string.IsNullOrEmpty(text)) {
                throw new ArgumentException($"'{nameof(text)}' cannot be null or empty.", nameof(text));
            }

            User = user ?? throw new ArgumentNullException(nameof(user));
            Channel = channel ?? throw new ArgumentNullException(nameof(channel));
            Text = text;
            Timestamp = DateTime.Now;
        }
    }
}*/