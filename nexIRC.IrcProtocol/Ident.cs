using System;
using System.Net;
using System.Net.Sockets;
namespace nexIRC.IrcProtocol {
    /// <summary>
    /// Ident
    /// </summary>
    public class Ident {
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
        /// Local Port
        /// </summary>
        private int _localPort;
        /// <summary>
        /// Remote Port
        /// </summary>
        private int _remotePort;
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
        public Ident(int localPort, int remotePort, string username, string system) { 
            _localPort = localPort;
            _remotePort = remotePort;
            _username = username;
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
            Thread socketThread = new Thread(new ThreadStart(listen));
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
                _networkStream = null;
                _clientSocket = null;
            }
        }
        /// <summary>
        /// Listen
        /// </summary>
        private void listen() {
            _clientSocket.Send(System.Text.Encoding.ASCII.GetBytes(" : USERID : UNIX : " + _username + Environment.NewLine));
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



