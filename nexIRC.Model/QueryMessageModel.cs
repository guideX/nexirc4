namespace nexIRC.Model {
    /// <summary>
    /// Query Message Model
    /// </summary>
    public class QueryMessageModel : EventArgs {
        /// <summary>
        /// User
        /// </summary>
        public UserModel User { get; }
        /// <summary>
        /// Text
        /// </summary>
        public string Text { get; }
        /// <summary>
        /// Timestamp
        /// </summary>
        public DateTime Timestamp { get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="user"></param>
        /// <param name="text"></param>
        public QueryMessageModel(UserModel user, string text) {
            User = user;
            Text = text;
            Timestamp = DateTime.Now;
        }
    }
}