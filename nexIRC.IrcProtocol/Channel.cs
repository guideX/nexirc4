using nexIRC.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
namespace nexIRC.IrcProtocol
{
    /// <summary>
    /// Represents an IRC channel with its users and messages.
    /// </summary>
    public class Channel
    {
        public string Name { get; }
        public string Topic { get; private set; }

        public ObservableCollection<ChannelUserModel> Users { get; }
        public ObservableCollection<ChannelMessage> Messages { get; }

        internal static char[] UserStatuses = new[] { '~', '&', '@', '%', '+' };

        public Channel(string name)
        {
            Name = name;
            Users = new ObservableCollection<ChannelUserModel>();
            Messages = new ObservableCollection<ChannelMessage>();
        }

        internal void AddUser(UserModel user)
        {
            AddUser(user, string.Empty);
        }

        internal void AddUser(UserModel user, string status)
        {
            Client.DispatcherInvoker.Invoke(() => Users.Add(new ChannelUserModel(user, status)));
        }

        internal void RemoveUser(string nick)
        {
            var user = GetUser(nick);
            if (user != null)
            {
                Client.DispatcherInvoker.Invoke(() => Users.Remove(user));
            }
        }

        internal void SetTopic(string topic)
        {
            Topic = topic;
        }

        public ChannelUserModel GetUser(string nick)
            => Users.FirstOrDefault(u => string.Equals(u.Nick, nick, StringComparison.InvariantCultureIgnoreCase))!;
    }
}
