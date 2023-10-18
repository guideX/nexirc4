using nexIRC.Model;
using System.Collections.ObjectModel;
namespace nexIRC.IrcProtocol {
    /// <summary>
    /// Query Collection
    /// </summary>
    public class QueryCollection : ObservableCollection<Query> {
        /// <summary>
        /// Get Query
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Query GetQuery(UserModel user) {
            var query = Items.FirstOrDefault(q => q.User.Nick == user.Nick);
            if (query is null) {
                query = new Query(user);
                Client.DispatcherInvoker.Invoke(() => Add(query));
            }
            return query;
        }
    }
}