using System.Collections.ObjectModel;
namespace nexIRC.IrcProtocol {
    /// <summary>
    /// Query
    /// </summary>
    public class Query {
        /// <summary>
        /// User
        /// </summary>
        public User User { get; }
        /// <summary>
        /// Nick
        /// </summary>
        public string Nick => User.Nick;
        /// <summary>
        /// Messages
        /// </summary>
        public ObservableCollection<QueryMessage> Messages { get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="user"></param>
        public Query(User user) {
            User = user;
            Messages = new ObservableCollection<QueryMessage>();
        }
    }
}