/*
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nexIRC.Model {
    /// <summary>
    /// Represents an IRC channel with its users and messages.
    /// </summary>
    public class ChannelModel {
        public string Name { get; }
        public string Topic { get; private set; }

        public ObservableCollection<ChannelUserModel> Users { get; }
        public ObservableCollection<ChannelMessageModel> Messages { get; }

        internal static char[] UserStatuses = new[] { '~', '&', '@', '%', '+' };

        public ChannelModel(string name) {
            Name = name;
            Users = new ObservableCollection<ChannelUser>();
            Messages = new ObservableCollection<ChannelMessage>();
        }

        internal void AddUser(UserModel user) {
            AddUser(user, string.Empty);
        }

        internal void AddUser(UserModel user, string status) {
            Client.DispatcherInvoker.Invoke(() => Users.Add(new ChannelUser(user, status)));
        }

        internal void RemoveUser(string nick) {
            var user = GetUser(nick);
            if (user != null) {
                Client.DispatcherInvoker.Invoke(() => Users.Remove(user));
            }
        }

        internal void SetTopic(string topic) {
            Topic = topic;
        }

        public ChannelUser GetUser(string nick)
            => Users.FirstOrDefault(u => string.Equals(u.Nick, nick, StringComparison.InvariantCultureIgnoreCase));
    }
}
*/