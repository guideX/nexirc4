using nexIRC.Business.Helper;
using nexIRC.IrcProtocol.Messages;
using nexIRC.Model;
namespace nexIRC.IrcProtocol.Wrappers {
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
        /// Auto Joined Channel
        /// </summary>
        private bool _autoJoinedChannel;
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
        /// Channel
        /// </summary>
        private string _channel;
        #endregion
        #region "private methods"
        /// <summary>
        /// Registration Completed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _client_RegistrationCompleted(object? sender, EventArgs e) {
            if (!_autoJoinedChannel) 
                AutoJoinChannel(); 
            System.Threading.Thread.Sleep(1000);
        }
        /// <summary>
        /// Auto Join Channel
        /// </summary>
        private async void AutoJoinChannel() {
            if (string.IsNullOrWhiteSpace(_channel)) return;
            _autoJoinedChannel = true;
            await _client.SendAsync(new JoinMessage(_channel));
        }
        /// <summary>
        /// Connected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _connection_Connected(object? sender, EventArgs e) {
            _connected = true;
            if (!_autoJoinedChannel) AutoJoinChannel(); System.Threading.Thread.Sleep(1000);
        }
        /// <summary>
        /// Disconnected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _connection_Disconnected(object? sender, EventArgs e) {
            try {
                _autoJoinedChannel = false;
                _user = "";
                _messagesToSend = new List<ClientMessageToSend>();
                _connected = false;
                _client.Dispose();
                _connection.Dispose();
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "_connection_Disconnected");
            }
        }
        /// <summary>
        /// Send Message As User
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="message"></param>
        private void SendMessages() {
            if (!_autoJoinedChannel) AutoJoinChannel(); System.Threading.Thread.Sleep(1000);
            if (_connected && _autoJoinedChannel) {
                foreach (var item in _messagesToSend.Where(i => !i.Sent).ToList()) {
                    _client.SendRaw("PRIVMSG " + item.Channel + " :" + item.Message);
                    item.Sent = true;
                }
            }
        }
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
            _messagesToSend = new List<ClientMessageToSend>();
            try {
                _connection = new IrcProtocol.Connection.TcpClientConnection(host, Convert.ToInt32(port));
                _connection.DataReceived += _connection_DataReceived;
                _connection.Disconnected += _connection_Disconnected;
                _connection.Connected += _connection_Connected;
                _client = new Client(new UserModel(user, realName), _connection);
                _client.RegistrationCompleted += _client_RegistrationCompleted;
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.IrcProtocol.ClientWrapper.Constructor");
                throw;
            }
        }
        /// <summary>
        /// Data Received
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _connection_DataReceived(object? sender, Connection.DataReceivedEventArgs e) {
            var msg = System.AppDomain.CurrentDomain.BaseDirectory + "matrixirclog.txt";
            System.IO.File.AppendAllText(msg, e.Data + Environment.NewLine );
        }
        /// <summary>
        /// Connect
        /// </summary>
        public async void Connect() {
            try {
                await _client.ConnectAsync();
                _connected = true;
                System.Threading.Thread.Sleep(2000);
                if (!_autoJoinedChannel) AutoJoinChannel(); System.Threading.Thread.Sleep(1000);
                SendMessages();
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.IrcProtocol.Connect");
            }
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
            try {
                _messagesToSend.Add(msg);
                SendMessages();
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.IrcProtocol.ClientWrapper.Send");
            }
        }
        #endregion
    }
    /// <summary>
    /// Client Message To Send
    /// </summary>
    public class ClientMessageToSend {
        #region "public variables"
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
        #endregion
        #region "public methods"
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="message"></param>
        public ClientMessageToSend(string channel, string message) {
            Channel = channel;
            Message = message;
        }
        #endregion
    }
}