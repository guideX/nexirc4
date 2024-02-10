namespace nexIRC.Model.IrcProtocol {
    /// <summary>
    /// Client Builder Model
    /// </summary>
    public class ClientBuilderModel {
        /// <summary>
        /// Password
        /// </summary>
        private string? _password;
        /// <summary>
        /// Password
        /// </summary>
        public string Password { 
            get {
                return _password != null ? _password : "";
            }
            set { 
                _password = value;
            }
        }
        /// <summary>
        /// Client Builder Model
        /// </summary>
        /// <param name="password"></param>
        public ClientBuilderModel() {
            
        }
    }
}