using nexIRC.Business.Helper;
using nexIRC.Model;
using System.Timers;
namespace nexIRC.IrcProtocol.Wrappers {
    /// <summary>
    /// Client Wrapper
    /// </summary>
    public class ClientWrapper {
        #region "private variables"
        /// <summary>
        /// Send Message Timer
        /// </summary>
        private static System.Timers.Timer? SendMessageTimer;
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
            if (!_autoJoinedChannel) AutoJoinChannel(); 
        }
        /// <summary>
        /// Set Timer
        /// </summary>
        private void SetTimer() {
            SendMessageTimer = new System.Timers.Timer(10000);
            SendMessageTimer.Elapsed += OnTimedEvent;
            SendMessageTimer.AutoReset = true;
            SendMessageTimer.Enabled = true;
        }
        /// <summary>
        /// On Timed Event
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnTimedEvent(Object? source, ElapsedEventArgs e) {
            SendMessages();
        }
        /// <summary>
        /// Auto Join Channel
        /// </summary>
        private async void AutoJoinChannel() {
            if (string.IsNullOrWhiteSpace(_channel)) return;
            _autoJoinedChannel = true;
            await _client.SendRaw("JOIN :" + _channel + Environment.NewLine);
            LogActivity("JOIN :" + _channel + Environment.NewLine);
        }
        /// <summary>
        /// Connected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _connection_Connected(object? sender, EventArgs e) {
            _connected = true;
            //if (!_autoJoinedChannel) AutoJoinChannel(); System.Threading.Thread.Sleep(1000);
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
            //if (!_autoJoinedChannel) AutoJoinChannel(); System.Threading.Thread.Sleep(1000);
            if (_autoJoinedChannel) {
                if (_connected && _autoJoinedChannel) {
                    foreach (var item in _messagesToSend.Where(i => !i.Sent).ToList()) {
                        SendRaw("PRIVMSG " + item.Channel + " :" + item.Message + Environment.NewLine);
                        item.Sent = true;
                    }
                }
            }
        }
        /// <summary>
        /// Send Raw
        /// </summary>
        /// <param name="raw"></param>
        private void SendRaw(string raw) {
            _client.SendRaw(raw);
            LogActivity("Sent: " + raw);
        }
        /// <summary>
        /// Send Raw
        /// </summary>
        /// <param name="raw"></param>
        private async void SendRawAsync(string raw) {
            await _client.SendRaw(raw);
            LogActivity("Sent: " + raw);
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
                SetTimer();
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
            LogActivity(e.Data + Environment.NewLine);
        }
        /// <summary>
        /// Connect
        /// </summary>
        public async void Connect() {
            try {
                await _client.ConnectAsync();
                _connected = true;
                System.Threading.Thread.Sleep(2000);
                //if (!_autoJoinedChannel) AutoJoinChannel(); System.Threading.Thread.Sleep(1000);
                SendMessages();
                LogActivity("Now Connected");
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.IrcProtocol.Connect");
            }
        }
        /// <summary>
        /// Log Activity
        /// </summary>
        /// <param name="activity"></param>
        private void LogActivity(string activity) {
            System.IO.File.AppendAllText(System.AppDomain.CurrentDomain.BaseDirectory + "matrixirclog.txt", activity + Environment.NewLine);
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
                LogActivity("Sent: " + msg.Message + ", to :" + msg.Channel);
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