using nexIRC.Business.Helper;
using nexIRC.Model;
using nexIRC.Model.IrcProtocol;
using System.Collections.ObjectModel;
namespace nexIRC.IrcProtocol {
    /// <summary>
    /// Channel
    /// </summary>
    public class Channel {
        #region "private variables"
        /// <summary>
        /// Channel
        /// </summary>
        private ChannelModel _channel;
        #endregion
        #region "public variables"
        /// <summary>
        /// Messages
        /// </summary>
        public ObservableCollection<ChannelMessage> Messages { get; }
        #endregion
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        public Channel(string name) {
            _channel = new ChannelModel(name);
            Messages = new ObservableCollection<ChannelMessage>();
        }
        /// <summary>
        /// Name
        /// </summary>
        public string Name {
            get {
                return _channel.Name;
            }
        }
        /// <summary>
        /// Topic
        /// </summary>
        public string? Topic {
            get {
                return _channel.Topic!;
            }
            set {
                _channel.Topic = value;
            }
        }
        /// <summary>
        /// Add User
        /// </summary>
        /// <param name="user"></param>
        /// <param name="status"></param>
        internal void AddUser(UserModel user, string status) {
            try {
                Client.DispatcherInvoker?.Invoke(() => _channel.Users.Add(new ChannelUserModel(user, status)));
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "AddUser");
            }
        }
        /// <summary>
        /// Add User
        /// </summary>
        /// <param name="user"></param>
        internal void AddUser(UserModel user) {
            try {
                Client.DispatcherInvoker?.Invoke(() => _channel.Users.Add(new ChannelUserModel(user, string.Empty)));
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
                    Client.DispatcherInvoker?.Invoke(() => _channel.Users.Remove(user));
                }
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "RemoveUser");
            }
        }
        /// <summary>
        /// Get User
        /// </summary>
        /// <param name="nick"></param>
        /// <returns></returns>
        public ChannelUserModel GetUser(string nick) =>
            _channel.Users.FirstOrDefault(u => string.Equals(u.Nick, nick, StringComparison.InvariantCultureIgnoreCase))!;
    }
}