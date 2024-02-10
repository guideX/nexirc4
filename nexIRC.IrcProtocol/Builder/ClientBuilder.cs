using nexIRC.Business.Helper;
using nexIRC.IrcProtocol.Connection;
using nexIRC.Model;
namespace nexIRC.IrcProtocol.Builder {
    /// <summary>
    /// Client Builder
    /// </summary>
    public sealed class ClientBuilder {
        /// <summary>
        /// User
        /// </summary>
        private UserModel? _user;
        /// <summary>
        /// Connection
        /// </summary>
        private IConnection? _connection;
        /// <summary>
        /// Password
        /// </summary>
        private string _password;
        /// <summary>
        /// Client Builder
        /// </summary>
        internal ClientBuilder() {
            _password = string.Empty;
        }
        /// <summary>
        /// With Nick
        /// </summary>
        /// <param name="nick"></param>
        /// <param name="realName"></param>
        /// <returns></returns>
        public ClientBuilder WithNick(string nick, string realName = "") {
            try {
                _user = new UserModel(nick, realName);
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.IrcProtocol.Builder.WithNick");
            }
            return this;
        }
        /// <summary>
        /// With Server
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public ClientBuilder WithServer(string host, int port, string password = "") {
            try {
                _connection = new TcpClientConnection(host, port);
                _password = password;
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.IrcProtocol.Builder.WithServer");
            }
            return this;
        }
        /// <summary>
        /// Build
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public Client Build() {
            try {
                _ = _user ?? throw new InvalidOperationException("Nick must be defined. Please use the WithNick method before building the client.");
                _ = _connection ?? throw new InvalidOperationException("Connection must be defined. Please use the WithServer method before building the client.");
                return new Client(_user, _password, _connection);
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.IrcProtocol.Builder.Build");
                throw;
            }
        }
    }
}