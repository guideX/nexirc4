using System.Net;
using System.Net.Sockets;
namespace nexIRC.IrcProtocol {
    /// <summary>
    /// The Ident Protocol (Identification Protocol, Ident), specified in RFC 1413, is an Internet protocol that helps 
    /// identify the user of a particular TCP connection. One popular daemon program for providing the ident service is identd
    /// </summary>
    public class Ident {
        /// <summary>
        /// Local Port - Generally 113
        /// </summary>
        private int _localPort;
        /// <summary>
        /// Remote Port
        /// </summary>
        private int _remotePort;
        /// <summary>
        /// Socket
        /// </summary>
        private Socket _clientSocket;
        /// <summary>
        /// Buffer
        /// </summary>
        private byte[] _buffer = new byte[256];
        /// <summary>
        /// Network Stream
        /// </summary>
        private NetworkStream _networkStream;
        /// <summary>
        /// Call Back Read
        /// </summary>
        private AsyncCallback _callbackRead;
        /// <summary>
        /// Call Back Write
        /// </summary>
        private AsyncCallback _callbackWrite;
        /// <summary>
        /// Username
        /// </summary>
        private string? _username;
        /// <summary>
        /// System
        /// </summary>
        private string? _system;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="port"></param>
        public Ident(string username, string system = "UNIX", int localPort = 113, int remotePort = 6191) {
            _username = username;
            _localPort = localPort;
            _remotePort = remotePort;
            _system = system;
            var ep = new IPEndPoint(IPAddress.Any, _localPort);
            _clientSocket = new Socket(ep.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _networkStream = new NetworkStream(_clientSocket);
            _callbackRead = new AsyncCallback(OnReadComplete);
            _callbackWrite = new AsyncCallback(OnWriteComplete);
        }
        /// <summary>
        /// Start
        /// </summary>
        public async void Start() {
            Thread socketThread = new(new ThreadStart(listen));
        }
        /// <summary>
        /// On Read Complete
        /// </summary>
        /// <param name="ar"></param>
        private void OnReadComplete(IAsyncResult ar) {
            int bytesRead = _networkStream.EndRead(ar);
            if (bytesRead > 0) {
                MemoryStream stream = new MemoryStream(_buffer);
                _networkStream.BeginRead(_buffer, 0, _buffer.Length, _callbackRead, null);
            } else {
                _networkStream.Close();
                _clientSocket.Close();
            }
        }
        /// <summary>
        /// Listen
        /// </summary>
        private void listen() {
            _clientSocket.Send(System.Text.Encoding.ASCII.GetBytes(" : USERID : " + _system + " : " + _username + Environment.NewLine));
            _clientSocket.Send(System.Text.Encoding.ASCII.GetBytes(_localPort.ToString() + ", " + _remotePort.ToString() + " : SYSTEM : " + _system + Environment.NewLine));
            _clientSocket.Close();
        }
        /// <summary>
        /// on Write Complete
        /// </summary>
        /// <param name="ar"></param>
        private void OnWriteComplete(IAsyncResult ar) {
            _networkStream.EndWrite(ar);
            _networkStream.BeginRead(_buffer, 0, _buffer.Length, _callbackRead, null);
        }
    }
}



