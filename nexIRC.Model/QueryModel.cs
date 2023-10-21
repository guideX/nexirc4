using System.Collections.ObjectModel;
namespace nexIRC.Model {
    /// <summary>
    /// Query
    /// </summary>
    public class QueryModel {
        /// <summary>
        /// User
        /// </summary>
        public UserModel User { get; }
        /// <summary>
        /// Nick
        /// </summary>
        public string Nick => User.Nick;
        /// <summary>
        /// Messages
        /// </summary>
        public ObservableCollection<QueryMessageModel> Messages { get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="user"></param>
        public QueryModel(UserModel user) {
            User = user;
            Messages = new ObservableCollection<QueryMessageModel>();
        }
    }
}
