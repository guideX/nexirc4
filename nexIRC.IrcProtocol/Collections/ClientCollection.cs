using nexIRC.Business.Helper;
using nexIRC.IrcProtocol.Wrappers;
namespace nexIRC.IrcProtocol.Collections {
    /// <summary>
    /// Client Collection
    /// </summary>
    public class ClientCollection {
        #region "private variables"
        /// <summary>
        /// Ident Protocol
        /// </summary>
        private nexIRC.IrcProtocol.Ident _ident;
        /// <summary>
        /// Clients
        /// </summary>
        private List<ClientWrapper> _clients;
        /// <summary>
        /// Server
        /// </summary>
        private string _server;
        /// <summary>
        /// Port
        /// </summary>
        private string _port;
        #endregion
        #region "public methods"
        /// <summary>
        /// Constructor
        /// </summary>
        public ClientCollection(string server, string port, nexIRC.IrcProtocol.Ident ident) {
            _ident = ident;
            _server = server;
            _port = port;
            _clients = new List<ClientWrapper>();
        }
        /// <summary>
        /// Add Client
        /// </summary>
        /// <summary>
        /// Send Message
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        public bool SendMessageAsUser(string channel, string user, string message) {
            try {
                var clientsFound = _clients.Where(c => c.User == user);
                var clientMessage = new ClientMessageToSend(channel, message);
                if (clientsFound.Any()) {
                    var client = clientsFound.FirstOrDefault();
                    if (client != null) {
                        client.Send(clientMessage);
                        return true;
                    }
                } else {
                    _ident.ChangeSettings(113, "UNIX", user);
                    var client = new ClientWrapper(_server, _port, user, user, "", channel);
                    client.Send(clientMessage);
                    client.Connect();
                    _clients.Add(client);
                    return true;
                }
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.IrcProtocol.ClientCollection.SendMessageAsUser");
            }
            return false;
        }
        /// <summary>
        /// Is User In Collection
        /// </summary>
        /// <returns></returns>
        public bool IsUserInCollection(string channel, string user) {
            try {
                var clientsFound = _clients.Where(c => c.User == user);
                if (clientsFound.Any()) {
                    return true;
                } else {
                    return false;
                }
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.IrcProtocol.ClientCollection.IsUserInCollection");
            }
            return false;
        }
        #endregion
    }
}