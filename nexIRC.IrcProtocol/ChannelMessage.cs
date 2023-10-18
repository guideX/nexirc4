using nexIRC.Model;
using System;

namespace nexIRC.IrcProtocol
{
    /// <summary>
    /// Represents a channel message
    /// </summary>
    public class ChannelMessage : EventArgs
    {
        public UserModel User { get; }
        public Channel Channel { get; }
        public string Text { get; }
        public DateTime Timestamp { get; }

        public ChannelMessage(UserModel user, Channel channel, string text)
        {
            User = user;
            Channel = channel;
            Text = text;
            Timestamp = DateTime.Now;
        }
    }
}
