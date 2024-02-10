using nexIRC.Business.Helper;
using nexIRC.Model;
using System.Collections.ObjectModel;
namespace nexIRC.IrcProtocol {
    /// <summary>
    /// Query Collection
    /// </summary>
    public class QueryCollection : ObservableCollection<QueryModel> {
        /// <summary>
        /// Query Collection
        /// </summary>
        public QueryCollection() { 
        }
        /// <summary>
        /// Get Query
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public QueryModel GetQuery(UserModel user) {
            try {
                QueryModel? result = null;
                result = Items.FirstOrDefault(q => q.User.Nick == user.Nick);
                if (result is null) {
                    result = new QueryModel(user);
                    Client.DispatcherInvoker?.Invoke(() => Add(result));
                }
                return result;
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.IrcProtocol.GetQuery");
                throw;
            }
        }
    }
}