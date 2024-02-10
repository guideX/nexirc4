/*
using System.Net;
using System.Net.Sockets;
namespace team_nexgen.core.Socket {
    /// <summary>
    /// Server Socket
    /// </summary>
    public class ServerSocket {
        /// <summary>
        /// Server
        /// </summary>
        private TcpListener? _server;
        /// <summary>
        /// Constructor
        /// </summary>
        public ServerSocket() {
        }
        /// <summary>
        /// Start
        /// </summary>
        /// <param name="port"></param>
        public async void Start(string ipAdress, int port) {
            try {
                var localAddr = IPAddress.Parse(ipAdress);
                _server = new TcpListener(localAddr, port);
                _server.Start();
            } catch { 
            }
            
        }
    }
}*/