using nexIRC.Business.Helper;
using nexIRC.Model;
using System.Collections.ObjectModel;
namespace nexIRC.IrcProtocol {
    /// <summary>
    /// User Collection
    /// </summary>
    public class UserCollection : ObservableCollection<UserModel> {
        /// <summary>
        /// Constructor
        /// </summary>
        public UserCollection() {
        }
        /// <summary>
        /// Get User
        /// </summary>
        /// <param name="nick"></param>
        /// <returns></returns>
        public UserModel GetUser(string nick) {
            try {
                var user = Items.FirstOrDefault(u => string.Equals(u.Nick, nick, StringComparison.InvariantCultureIgnoreCase));
                if (user is null) {
                    user = new UserModel(nick);
                    Client.DispatcherInvoker?.Invoke(() => Add(user));
                }
                return user;
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.IrcProtocol.GetUser");
                throw;
            }
        }
    }
}