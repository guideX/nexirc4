/*
using nexIRC.Model;
using System.Collections.ObjectModel;
namespace nexIRC.IrcProtocol {
    /// <summary>
    /// Query
    /// </summary>
    public class Query {
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
        public Query(UserModel user) {
            User = user;
            Messages = new ObservableCollection<QueryMessageModel>();
        }
    }
}*/