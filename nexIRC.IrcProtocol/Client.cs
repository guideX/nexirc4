using nexIRC.IrcProtocol.Builder;
using nexIRC.IrcProtocol.Connection;
using nexIRC.IrcProtocol.Ctcp;
using nexIRC.IrcProtocol.Interfaces;
using nexIRC.IrcProtocol.Messages;
using nexIRC.Model;
using System.Reflection;
namespace nexIRC.IrcProtocol
{
    /// <summary>
    /// Client
    /// </summary>
    public class Client : IDisposable {
        /// <summary>
        /// Connection
        /// </summary>
        private readonly IConnection connection;
        /// <summary>
        /// Password
        /// </summary>
        private readonly string _password;
        /// <summary>
        /// Message Handler Container
        /// </summary>
        private readonly MessageHandlerContainer _messageHandlerContainer;
        /// <summary>
        /// Enables a custom dispatcher to be used if necessary. For example, WPF Dispatcher, to make sure collections are manipulated in the UI thread
        /// </summary>
        public static Action<Action>? DispatcherInvoker;
        /// <summary>
        /// User
        /// </summary>
        public UserModel User { get; }
        /// <summary>
        /// Server Messages
        /// </summary>
        public ServerMessageCollection ServerMessages { get; } = new ServerMessageCollection();
        /// <summary>
        /// Channels
        /// </summary>
        public ChannelCollection Channels { get; }
        /// <summary>
        /// Queries
        /// </summary>
        public QueryCollection Queries { get; }
        /// <summary>
        /// Peers
        /// </summary>
        public UserCollection Peers { get; }
        /// <summary>
        /// Raw Data Received
        /// </summary>
        public event IRCRawDataHandler? RawDataReceived;
        /// <summary>
        /// IRC Message Parsed
        /// </summary>
        public event ParsedIRCMessageHandler? IRCMessageParsed;
        /// <summary>
        /// Registration Completed
        /// </summary>
        public event EventHandler? RegistrationCompleted;
        /// <summary>
        /// On Registration Completed
        /// </summary>
        internal void OnRegistrationCompleted() {
            RegistrationCompleted?.Invoke(this, EventArgs.Empty);
        }
        /// <summary>
        /// Ctcp Received
        /// </summary>
        public event CtcpHandler? CtcpReceived;
        /// <summary>
        /// On Ctcp Received
        /// </summary>
        /// <param name="ctcp"></param>
        internal void OnCtcpReceived(CtcpEventArgs ctcp) {
            CtcpReceived?.Invoke(this, ctcp);
            CtcpCommands.HandleCtcp(this, ctcp);
        }
        /// <summary>
        /// Create Builder
        /// </summary>
        /// <returns></returns>
        public static ClientBuilder CreateBuilder() => new();
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="user"></param>
        /// <param name="connection"></param>
        public Client(UserModel user, IConnection connection) {
            _password = "";
            User = user;
            this.connection = connection;
            DispatcherInvoker = a => a.Invoke();
            _messageHandlerContainer = new MessageHandlerContainer(this);
            this.connection.DataReceived += Connection_DataReceived!;
            Channels = new ChannelCollection();
            Peers = new UserCollection();
            Queries = new QueryCollection();
        }
        /// <summary>
        /// Client
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="connection"></param>
        public Client(UserModel user, string password, IConnection connection) : this(user, connection) {
            _password = password;
            Channels = new ChannelCollection();
            Peers = new UserCollection();
            Queries = new QueryCollection();
        }
        /// <summary>
        /// Connection Data Received
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Connection_DataReceived(object sender, DataReceivedEventArgs e) {
            if (string.IsNullOrWhiteSpace(e.Data)) {
                return;
            }
            var rawData = e.Data;
            RawDataReceived?.Invoke(this, e.Data);
            var parsedIRCMessage = new ParsedIRCMessage(rawData);
            await HandleServerMessages(parsedIRCMessage);
            IRCMessageParsed?.Invoke(this, parsedIRCMessage);
            await _messageHandlerContainer.HandleAsync(parsedIRCMessage).ConfigureAwait(false);
        }
        /// <summary>
        /// Connect Async
        /// </summary>
        /// <returns></returns>
        public async Task ConnectAsync() {
            await connection.ConnectAsync() .ConfigureAwait(false);
            if (!string.IsNullOrWhiteSpace(_password)) {
                await SendAsync(new PassMessage(_password)).ConfigureAwait(false);
            }
            await SendAsync(new NickMessage(User.Nick)).ConfigureAwait(false);
            await SendAsync(new UserMessage(User.Nick, User.RealName)).ConfigureAwait(false);
        }
        /// <summary>
        /// Send Raw
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns></returns>
        public Task SendRaw(string rawData) {
            return connection.SendAsync(rawData);
        }
        /// <summary>
        /// Send Async
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task SendAsync(IClientMessage message) {
            return connection.SendAsync(message.ToString()!);
        }
        /// <summary>
        /// Disposes the connection
        /// </summary>
        public void Dispose() {
            connection.Dispose();
        }
        /// <summary>
        /// Register Custom Message Handler
        /// </summary>
        /// <typeparam name="TCustomMessageHandler"></typeparam>
        public void RegisterCustomMessageHandler<TCustomMessageHandler>()
            where TCustomMessageHandler : ICustomHandler {
            _messageHandlerContainer.RegisterCustomMessageHandler(typeof(TCustomMessageHandler));
        }
        /// <summary>
        /// Register Custom Message Handlers
        /// </summary>
        /// <param name="assembly"></param>
        public void RegisterCustomMessageHandlers(Assembly assembly) {
            _messageHandlerContainer.RegisterCustomMessageHandlers(assembly);
        }
        /// <summary>
        /// Set Dispatch Invoker
        /// </summary>
        /// <param name="dispatcherInvoke"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void SetDispatcherInvoker(Action<Action> dispatcherInvoke) {
            _ = dispatcherInvoke ?? throw new ArgumentNullException(nameof(dispatcherInvoke));
            DispatcherInvoker = dispatcherInvoke;
        }
        /// <summary>
        /// Handle Server Messaged
        /// </summary>
        /// <param name="parsedIRCMessage"></param>
        /// <returns></returns>
        private Task HandleServerMessages(ParsedIRCMessage parsedIRCMessage) {
            if (parsedIRCMessage.IsNumeric) {
                return HandleNumericReply(parsedIRCMessage);
            }
            return Task.CompletedTask;
        }
        /// <summary>
        /// Handle Numeric Reply
        /// </summary>
        /// <param name="parsedIRCMessage"></param>
        /// <returns></returns>
        private Task HandleNumericReply(ParsedIRCMessage parsedIRCMessage) {
            string text = string.Empty;
            switch (parsedIRCMessage.NumericReply) {
                case IrcNumericReplyEnum.RPL_MYINFO:
                case IrcNumericReplyEnum.RPL_ISUPPORT:
                    text = string.Join(" ", parsedIRCMessage.Parameters!.Skip(1));
                    break;
                case IrcNumericReplyEnum.RPL_LUSEROP:
                case IrcNumericReplyEnum.RPL_LUSERUNKNOWN:
                case IrcNumericReplyEnum.RPL_LUSERCHANNELS:
                    text = $"{parsedIRCMessage.Parameters![1]} {parsedIRCMessage.Trailing}";
                    break;
                case IrcNumericReplyEnum.RPL_NAMREPLY:
                case IrcNumericReplyEnum.RPL_ENDOFNAMES:
                    return Task.CompletedTask;
                default:
                    text = parsedIRCMessage.Trailing;
                    break;
            }
            if (!string.IsNullOrEmpty(text)) DispatcherInvoker?.Invoke(() => ServerMessages.Add(new ServerMessageModel(text)));
            return Task.CompletedTask;
        }
    }
}