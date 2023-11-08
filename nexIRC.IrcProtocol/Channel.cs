using nexIRC.Model;
using System.Collections.ObjectModel;
namespace nexIRC.IrcProtocol {
    /// <summary>
    /// Channel
    /// </summary>
    public class Channel {
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Topic
        /// </summary>
        public string Topic { get; private set; }
        /// <summary>
        /// Users
        /// </summary>
        public ObservableCollection<ChannelUserModel> Users { get; }
        /// <summary>
        /// Messages
        /// </summary>
        public ObservableCollection<ChannelMessage> Messages { get; }
        /// <summary>
        /// User Statuses
        /// </summary>
        internal static char[] UserStatuses = new[] { '~', '&', '@', '%', '+' };
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        public Channel(string name) {
            Name = name;
            Users = new ObservableCollection<ChannelUserModel>();
            Messages = new ObservableCollection<ChannelMessage>();
            Topic = "";
        }

        internal void AddUser(UserModel user) {
            AddUser(user, string.Empty);
        }

        internal void AddUser(UserModel user, string status) {
            Client.DispatcherInvoker.Invoke(() => Users.Add(new ChannelUserModel(user, status)));
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

        public ChannelUserModel GetUser(string nick)
            => Users.FirstOrDefault(u => string.Equals(u.Nick, nick, StringComparison.InvariantCultureIgnoreCase))!;
    }
}