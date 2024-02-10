using nexIRC.Business.Helper;
using nexIRC.IrcProtocol.Messages;
namespace nexIRC.IrcProtocol.Ctcp {
    /// <summary>
    /// Ctcp Handler
    /// </summary>
    /// <param name="client"></param>
    /// <param name="ctcpEventArgs"></param>
    public delegate void CtcpHandler(Client client, CtcpEventArgs ctcpEventArgs);
    /// <summary>
    /// Ctcp Event Args
    /// </summary>
    public class CtcpEventArgs : EventArgs {
        /// <summary>
        /// From
        /// </summary>
        private readonly string _from;
        /// <summary>
        /// Prefix
        /// </summary>
        private readonly IRCPrefix _preFix;
        /// <summary>
        /// To
        /// </summary>
        private readonly string _to;
        /// <summary>
        /// Message
        /// </summary>
        private readonly string _message;
        /// <summary>
        /// Ctcp Command
        /// </summary>
        private readonly string? _ctcpCommand;
        /// <summary>
        /// Ctcp Message
        /// </summary>
        private readonly string? _ctcpMessage;
        /// <summary>
        /// Construcotr
        /// </summary>
        /// <param name="privMsgMessage"></param>
        public CtcpEventArgs(PrivMsgMessage privMsgMessage) {
            _from = privMsgMessage.From;
            _preFix = privMsgMessage.Prefix;
            _to = privMsgMessage.To;
            _message = privMsgMessage.Message;
            try {
                var ctcpMessage = _message.Replace(CtcpCommands.CtcpDelimiter, string.Empty);
                if (ctcpMessage.Contains(" ")) {
                    var startIndex = ctcpMessage.IndexOf(' ');
                    _ctcpCommand = ctcpMessage.Remove(startIndex);
                    _ctcpMessage = ctcpMessage.Substring(startIndex + 1);
                    return;
                }
                _ctcpCommand = ctcpMessage;
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.IrcProtocol.Ctcp.CtcpEventArgs.Constructor");
            }
        }
        /// <summary>
        /// Message
        /// </summary>
        public string Message {
            get { return _message; }
        }
        /// <summary>
        /// From
        /// </summary>
        public string From {
            get { return _from; }
        }
        /// <summary>
        /// Ctcp Command
        /// </summary>
        public string CtcpCommand { 
            get { return !string.IsNullOrWhiteSpace(_ctcpCommand) ? _ctcpCommand : ""; }
        }
        /// <summary>
        /// Ctcp Message
        /// </summary>
        public string CtcpMessage { 
            get { return !string.IsNullOrWhiteSpace(_ctcpMessage) ? _ctcpMessage : ""; }
        }
        /// <summary>
        /// To
        /// </summary>
        public string To { 
            get { return _to; }
        }
    }
}