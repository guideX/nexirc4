using nexIRC.Business.Helper;
using System.Net;
using System.Net.Sockets;
using System.Text;
using team_nexgen.core.Helpers;
namespace nexIRC.IrcProtocol {
    /// <summary>
    /// Identification Protocol
    /// A server listens for TCP connections on TCP port 113 (decimal).  Once a connection is established, 
    /// the server reads a line of data which specifies the connection of interest. If it exists, the 
    /// system dependent user identifier of the connection of interest is sent as the reply. The server 
    /// may then either shut the connection down or it may continue to read/respond to multiple queries.
    /// The server should close the connection down after a configurable amount of time with no 
    /// queries - a 60-180 second idle timeout is recommended.  The client may close the connection down 
    /// at any time; however to allow for network delays the client should wait at least 30 seconds (or 
    /// longer) after a query before abandoning the query and closing the connection.
    /// </summary>
    public class Ident {
        #region "variables"
        /// <summary>
        /// Listener
        /// </summary>
        private Socket? _listener;
        /// <summary>
        /// IP Address
        /// </summary>
        private uint _ipAddress;
        /// <summary>
        /// Port
        /// </summary>
        private int _port;
        /// <summary>
        /// System
        /// </summary>
        private string _system;
        /// <summary>
        /// User Name
        /// </summary>
        private string _userName;
        #endregion
        #region "methods"
        /// <summary>
        /// Constructor
        /// </summary>
        public Ident(int port, string system, string userName) {
            _ipAddress = SocketHelper.GetAnyIpForListening();
            _port = port;
            _system = system;
            _userName = userName;
        }
        /// <summary>
        /// Change Settings
        /// </summary>
        /// <param name="port"></param>
        /// <param name="system"></param>
        /// <param name="userName"></param>
        public void ChangeSettings(int port, string system, string userName) {
            Close();
            _port = port;
            _system = system;
            _userName = userName;
        }
        /// <summary>
        /// Listen
        /// </summary>
        /// <param name="port"></param>
        /// <param name="ipAddress"></param>
        public async void Listen() {
            var err = false;
            try {
                IPEndPoint ep = new(_ipAddress, _port);
                _listener = new(
                    ep.AddressFamily,
                    SocketType.Stream,
                    ProtocolType.Tcp
                );
                _listener.Bind(ep);
                _listener.Listen(100);
                var handler = await _listener.AcceptAsync();
                var b = true;
                while (b) {
                    var buffer = new byte[1_024];
                    int received = 0;
                    received = await handler.ReceiveAsync(buffer, SocketFlags.None);
                    var response = Encoding.UTF8.GetString(buffer, 0, received);
                    if (!string.IsNullOrEmpty(response)) {
                        var splt = response.Split(',');
                        if (splt.Length > 0) {
                            var message = splt[0].Replace("\r\n", "").Trim() + ", " + splt[1].Replace("\r\n", "").Trim() + " : USERID : " + _system + " : " + _userName + "\r\n";
                            await handler.SendAsync(Encoding.UTF8.GetBytes(message), 0);
                            b = false;
                        }
                    }
                }
            } catch (Exception ex) {
                err = true;
                ExceptionHelper.HandleException(ex, "nexIRC.IrcProtocol.Listen");
            }
            if (err) 
                Listen(); // Keep listening again
            else
                throw new Exception("Ident Server failed to listen"); // Fail and die
        }
        /// <summary>
        /// Close
        /// </summary>
        public void Close() {
            if (_listener != null) {
                _listener?.Shutdown(SocketShutdown.Both);
                _listener?.Close();
            }
        }
        #endregion
    }
}