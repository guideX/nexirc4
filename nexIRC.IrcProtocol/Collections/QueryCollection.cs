using nexIRC.Business.Helper;
using nexIRC.Model;
using System.Collections.ObjectModel;
namespace nexIRC.IrcProtocol {
    /// <summary>
    /// Query Collection
    /// </summary>
    public class QueryCollection : ObservableCollection<QueryModel> {
        /// <summary>
        /// App Path
        /// </summary>
        private string _appPath;
        /// <summary>
        /// Query Collection
        /// </summary>
        /// <param name="appPath"></param>
        public QueryCollection(string appPath) { 
            _appPath = appPath;
        }
        /// <summary>
        /// Get Query
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public QueryModel? GetQuery(UserModel user) {
            QueryModel? result = null;
            try {
                result = Items.FirstOrDefault(q => q.User.Nick == user.Nick);
                if (result is null) {
                    result = new QueryModel(user);
                    Client.DispatcherInvoker?.Invoke(() => Add(result));
                }
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.IrcProtocol.GetQuery", _appPath);
            }
            return result;
        }
    }
}