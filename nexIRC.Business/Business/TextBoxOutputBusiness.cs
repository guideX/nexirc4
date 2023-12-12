using nexIRC.Business.Enum;
using nexIRC.Business.Helper;
using nexIRC.Model;
namespace nexIRC.Business.Business {
    /// <summary>
    /// Text Box Output Business
    /// </summary>
    public class TextBoxOutputBusiness {
        /// <summary>
        /// Matrix Send Message
        /// </summary>
        private MatrixSendMessageModel? _matrixSendMessage;
        /// <summary>
        /// Matrix Join Channel
        /// </summary>
        private MatrixJoinChannelModel? _matrixJoinChannel;
        /// <summary>
        /// Input
        /// </summary>
        private readonly string _input;
        /// <summary>
        /// Last PrivMsg
        /// </summary>
        private PrivMsgParamModel? _lastPrivMsg;
        /// <summary>
        /// Last Output Enum
        /// </summary>
        private OutputEventEnum? _lastOutputEvent;
        /// <summary>
        /// Constructor
        /// </summary>
        public TextBoxOutputBusiness(string input) {
            _input = input;
        }
        /// <summary>
        /// Process
        /// </summary>
        public void Process() {
            try {
                if (_input.Contains(" ")) {
                    var splt = _input.Split(' ');
                    switch (splt[0].ToLower()) {
                        case "/matrix":
                            switch (splt[1].ToLower()) {
                                case "joinedchannels":
                                    _lastOutputEvent = OutputEventEnum.GetJoinedChannels;
                                    break;
                                case "join":
                                    _lastOutputEvent = OutputEventEnum.JoinChannelMatrix;
                                    _matrixJoinChannel = new MatrixJoinChannelModel {
                                        ChannelName = splt[2]
                                    };
                                    break;
                                case "autojoin":
                                    _lastOutputEvent = OutputEventEnum.AutoJoinMatrix;
                                    break;
                                case "leave":
                                    _lastOutputEvent = OutputEventEnum.PartChannelMatrix;
                                    _matrixJoinChannel = new() {
                                        ChannelName = splt[2]
                                    };
                                    break;
                                case "connect":
                                    _lastOutputEvent = OutputEventEnum.ConnectMatrix;
                                    break;
                                case "disconnect":
                                    _lastOutputEvent = OutputEventEnum.DisconnectMatrix;
                                    break;
                                case "msg":
                                    _lastOutputEvent = OutputEventEnum.SendMessageMatrix;
                                    _matrixSendMessage = new();
                                    _matrixSendMessage.ChannelName = splt[2];
                                    _matrixSendMessage.Message = _input.Substring(12 + _matrixSendMessage.ChannelName.Length);
                                    break;
                            }
                            break;
                        case "/msg":
                            _lastOutputEvent = OutputEventEnum.PrivMsg;
                            var privMsg = new PrivMsgParamModel();
                            var msg = _input.Substring(4, _input.Length - 4);
                            var splt2 = msg.Split(' ');
                            privMsg.Nickname = splt2[1];
                            var substringLen1 = privMsg.Nickname.Length + 1;
                            var substringLen2 = msg.Length - 1;
                            privMsg.MessageToSend = msg.Substring(privMsg.Nickname.Length + 1).Trim();
                            privMsg.Success = true;
                            _lastPrivMsg = privMsg;
                            break;
                    }
                }
            } catch (Exception ex) {
                _lastPrivMsg = new PrivMsgParamModel() { 
                    Message = ex.Message
                };
                ExceptionHelper.HandleException(ex, "nexIRC.Business.Business.TextBoxOutputBusiness.Process");
            }
        }
        /// <summary>
        /// Get Last PrivMsg
        /// </summary>
        /// <returns></returns>
        public PrivMsgParamModel? PrivMsg {
            get {
                return _lastPrivMsg;
            }
        }
        /// <summary>
        /// Matrix Join Channel
        /// </summary>
        public MatrixJoinChannelModel? MatrixJoinChannel {
            get {
                return _matrixJoinChannel;
            }
        }
        /// <summary>
        /// Last Output Event
        /// </summary>
        public OutputEventEnum? LastOutputEvent {
            get {
                return _lastOutputEvent;
            }
        }
        /// <summary>
        /// Matrix Send Message
        /// </summary>
        public MatrixSendMessageModel? MatrixSendMessage {
            get {
                return _matrixSendMessage;
            }
        }
    }
}