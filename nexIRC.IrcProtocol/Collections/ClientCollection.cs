using nexIRC.IrcProtocol.Messages;
namespace nexIRC.IrcProtocol.Collections {
    /// <summary>
    /// Client Collection
    /// </summary>
    public class ClientCollection {
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
        /// <summary>
        /// Constructor
        /// </summary>
        public ClientCollection(string server, string port) {
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
            var clientsFound = _clients.Where(c => c.User == user);
            if (clientsFound.Any()) {
                var client = clientsFound.FirstOrDefault();
                if (client != null) 
                    client.Send(new ClientMessageToSend(channel, message));
            } else {
                var client = new ClientWrapper(_server, _port, user, user, "", channel);
                client.Send(new ClientMessageToSend(channel, message));
                client.Connect();
                _clients.Add(client);
            }
            return false;
        }
    }
    /// <summary>
    /// Client Wrapper
    /// </summary>
    public class ClientWrapper {
        #region "private variables"
        /// <summary>
        /// Messages To Send
        /// </summary>
        private List<ClientMessageToSend> _messagesToSend;
        /// <summary>
        /// Connected
        /// </summary>
        private bool _connected;
        /// <summary>
        /// Client
        /// </summary>
        private Client _client;
        /// <summary>
        /// Connection
        /// </summary>
        private Connection.TcpClientConnection _connection;
        /// <summary>
        /// User
        /// </summary>
        private string _user;
        /// <summary>
        /// Joined Channel
        /// </summary>
        private bool _joinedChannel;
        /// <summary>
        /// Channel
        /// </summary>
        private string _channel;
        #endregion
        #region "public methods"
        /// <summary>
        /// Client Wrapper
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="channel"></param>
        public ClientWrapper(string host, string port, string user, string realName, string password, string channel) {
            _channel = channel;
            _user = user;
            _connection = new IrcProtocol.Connection.TcpClientConnection(host, Convert.ToInt32(port));
            _connection.DataReceived += _connection_DataReceived;
            _connection.Disconnected += _connection_Disconnected;
            _connection.Connected += _connection_Connected;
            _client = new Client(new User(user, realName), _connection);
            _client.CtcpReceived += _client_CtcpReceived;
            _client.IRCMessageParsed += _client_IRCMessageParsed;
            _client.RegistrationCompleted += _client_RegistrationCompleted;
            _client.RawDataReceived += _client_RawDataReceived;
            _messagesToSend = new List<ClientMessageToSend>();
        }
        /// <summary>
        /// Connect
        /// </summary>
        public async void Connect() {
            await _client.ConnectAsync();
        }
        /// <summary>
        /// Connected
        /// </summary>
        public bool Connected {
            get {
                return _connected;
            }
        }
        /// <summary>
        /// User
        /// </summary>
        public string User {
            get {
                return _user;
            }
        }
        /// <summary>
        /// Send Message As User
        /// </summary>
        /// <param name="msg"></param>
        public void Send(ClientMessageToSend msg) {
            _messagesToSend.Add(msg);
            if (Connected) SendMessages();
        }
        #endregion
        #region "private methods"
        /// <summary>
        /// Raw Data Received
        /// </summary>
        /// <param name="client"></param>
        /// <param name="rawData"></param>
        private void _client_RawDataReceived(Client client, string rawData) {
        }
        /// <summary>
        /// Registration Completed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _client_RegistrationCompleted(object? sender, EventArgs e) {
            AutoJoinChannel();
        }
        /// <summary>
        /// Auto Join Channel
        /// </summary>
        private async void AutoJoinChannel() {
            if (string.IsNullOrWhiteSpace(_channel)) return;
            await _client.SendAsync(new JoinMessage(_channel));
            Thread.Sleep(2000);
            if (_messagesToSend.Count > 0) SendMessages();
        }
        /// <summary>
        /// Irc Message Parsed
        /// </summary>
        /// <param name="client"></param>
        /// <param name="ircMessage"></param>
        private void _client_IRCMessageParsed(Client client, ParsedIRCMessage ircMessage) {
            System.IO.File.AppendAllText(@"C:\bkup\irclog.txt", ircMessage.Raw);
        }
        /// <summary>
        /// Ctcp Received
        /// </summary>
        /// <param name="client"></param>
        /// <param name="ctcpEventArgs"></param>
        private void _client_CtcpReceived(Client client, Ctcp.CtcpEventArgs ctcpEventArgs) {
        }
        /// <summary>
        /// Connected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _connection_Connected(object? sender, EventArgs e) {
            _connected = true;
        }
        /// <summary>
        /// Disconnected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _connection_Disconnected(object? sender, EventArgs e) {
            _user = "";
            _messagesToSend = new List<ClientMessageToSend>();
            _connected = false;
        }
        /// <summary>
        /// Data Received
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _connection_DataReceived(object? sender, Connection.DataReceivedEventArgs e) {
        }
        /// <summary>
        /// Send Message As User
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="message"></param>
        private void SendMessages() {
            if (_connected) {
                foreach (var item in _messagesToSend.Where(i => i.Sent == false).ToList()) {
                    _client.SendRaw("PRIVMSG " + item.Channel + " :" + item.Message);
                    item.Sent = true;
                }
            }
        }
        #endregion
    }
    /// <summary>
    /// Client Message To Send
    /// </summary>
    public class ClientMessageToSend { 
        /// <summary>
        /// Channel
        /// </summary>
        public string Channel { get; set; }
        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Sent
        /// </summary>
        public bool Sent { get; set; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="message"></param>
        public ClientMessageToSend(string channel, string message) {
            Channel = channel;
            Message = message;
        }

    }
}