using nexIRC.Business.Helper;
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
        /// <summary>
        /// Add User
        /// </summary>
        /// <param name="user"></param>
        internal void AddUser(UserModel user) {
            try {
                AddUser(user, string.Empty);
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "AddUser");
            }
        }
        /// <summary>
        /// Add User
        /// </summary>
        /// <param name="user"></param>
        /// <param name="status"></param>
        internal void AddUser(UserModel user, string status) {
            try {
                Client.DispatcherInvoker?.Invoke(() => Users.Add(new ChannelUserModel(user, status)));
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "AddUser");
            }
        }
        /// <summary>
        /// Remove User
        /// </summary>
        /// <param name="nick"></param>
        internal void RemoveUser(string nick) {
            try {
                var user = GetUser(nick);
                if (user != null) {
                    Client.DispatcherInvoker?.Invoke(() => Users.Remove(user));
                }
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "RemoveUser");
            }
        }
        /// <summary>
        /// Set Topic
        /// </summary>
        /// <param name="topic"></param>
        internal void SetTopic(string topic) {
            Topic = topic;
        }
        /// <summary>
        /// Get User
        /// </summary>
        /// <param name="nick"></param>
        /// <returns></returns>
        public ChannelUserModel GetUser(string nick) =>
            Users.FirstOrDefault(u => string.Equals(u.Nick, nick, StringComparison.InvariantCultureIgnoreCase))!;
    }
}