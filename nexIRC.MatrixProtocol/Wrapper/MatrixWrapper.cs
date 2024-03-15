using nexIRC.Business.Helper;
using nexIRC.MatrixProtocol.Core.Domain.RoomEvent;
using nexIRC.MatrixProtocol.Interfaces;
namespace nexIRC.MatrixProtocol.Wrapper
{
    /// <summary>
    /// Matrix
    /// </summary>
    public class MatrixWrapper {
        #region "public variables"
        /// <summary>
        /// Matrix Room Event Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void MatrixRoomEventHandler(object? sender, MatrixRoomEventArgs e);
        /// <summary>
        /// Matrix Room Event
        /// </summary>
        public event MatrixRoomEventHandler? MatrixRoomEvent;
        /// <summary>
        /// Connected
        /// </summary>
        public event EventHandler? MatrixConnected;
        #endregion
        #region "private variables"
        /// <summary>
        /// Connection Result
        /// </summary>
        private AjaxResultModel? _connectionResult;
        /// <summary>
        /// Connection Result
        /// </summary>
        public AjaxResultModel? ConnectionResult { get { return _connectionResult; } }
        /// <summary>
        /// Matrix Node Address
        /// </summary>
        private string? _matrixNodeAddress;
        /// <summary>
        /// User Name
        /// </summary>
        private string? _userName;
        /// <summary>
        /// Password
        /// </summary>
        private string? _password;
        /// <summary>
        /// DeviceID
        /// </summary>
        private string? _deviceID;
        /// <summary>
        /// Matrix Client Factory
        /// </summary>
        private MatrixClientFactory? _matrixClientFactory;
        /// <summary>
        /// Matrix Client
        /// </summary>
        private IMatrixClient _matrixClient;
        /// <summary>
        /// Joined Channels
        /// </summary>
        private List<string> _joinedChannels = new();
        /// <summary>
        /// Last Result
        /// </summary>
        //private AjaxResultModel? _lastResult;
        /// <summary>
        /// 
        /// </summary>
        private string? _currentChannelID;
        #endregion
        #region "public methods"
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="matrixNodeAddress"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="deviceID"></param>
        public MatrixWrapper(string matrixNodeAddress, string userName, string password, string deviceID, string currentChannelID, string ircChannel, string settings_IrcNickname, string settings_MatrixNick) {
            _currentChannelID = currentChannelID;
            _matrixClientFactory = new MatrixClientFactory();
            _matrixNodeAddress = matrixNodeAddress;
            _userName = userName;
            _password = password;
            _deviceID = deviceID;
            _matrixClient = _matrixClientFactory.Create();
            _matrixClient.OnMatrixRoomEventsReceived += (sender, eventArgs) => {
                foreach (BaseRoomEvent roomEvent in eventArgs.MatrixRoomEvents) {
                    if (roomEvent is TextMessageEvent) {
                        (string roomId, string senderUserId, string message) = (TextMessageEvent)roomEvent;
                        MatrixRoomEvent?.Invoke(this,
                            new MatrixRoomEventArgs() {
                                RoomId = roomId,
                                SenderUserId = senderUserId,
                                Message = message,
                                EventType = Core.Infrastructure.Dto.Sync.Event.EventType.Message,
                                Details = MessageHelper.GetMessageDetails(roomId, currentChannelID, ircChannel, message, senderUserId, false, settings_MatrixNick, settings_IrcNickname)
                            });
                    } else if (roomEvent is CreateRoomEvent) {
                        (string RoomId, string SenderUserId, string RoomCreatorUserId) = (CreateRoomEvent)roomEvent;
                        MatrixRoomEvent?.Invoke(this, new MatrixRoomEventArgs() {
                            RoomId = RoomId,
                            SenderUserId = SenderUserId,
                            RoomCreatorUserId = RoomCreatorUserId
                        });
                    } else if (roomEvent is InviteToRoomEvent) {
                        (string RoomId, string SenderUserId) = (InviteToRoomEvent)roomEvent;
                        MatrixRoomEvent?.Invoke(this, new MatrixRoomEventArgs() {
                            RoomId = RoomId,
                            SenderUserId = SenderUserId
                        });
                    } else if (roomEvent is JoinRoomEvent) {
                        (string RoomId, string SenderUserId) = (JoinRoomEvent)roomEvent;
                        MatrixRoomEvent?.Invoke(this, new MatrixRoomEventArgs() {
                            RoomId = RoomId,
                            SenderUserId = SenderUserId
                        });
                    } else if (roomEvent is EncryptedEvent) {
                        (string roomId, string senderUserId, string message, string algorithm, string senderKey, string SenderSessionID) = (EncryptedEvent)roomEvent;
                        MatrixRoomEvent?.Invoke(this, new MatrixRoomEventArgs() {
                            Details = MessageHelper.GetMessageDetails(roomId, currentChannelID, ircChannel, message, senderUserId, true, settings_MatrixNick, settings_IrcNickname),
                            RoomId = roomId,
                            SenderUserId = senderUserId,
                            Message = message,
                            EventType = Core.Infrastructure.Dto.Sync.Event.EventType.Encrypted,
                            Algorithm = algorithm,
                            SenderKey = senderKey,
                            SenderSessionID = SenderSessionID

                        });
                    }
                }
            };
        }
        /// <summary>
        /// Login
        /// </summary>
        public async void Login() {
            try {
                var result = new AjaxResultModel();
                if (!string.IsNullOrWhiteSpace(_matrixNodeAddress) && _matrixClient != null && !string.IsNullOrWhiteSpace(_userName) && !string.IsNullOrWhiteSpace(_password) && !string.IsNullOrWhiteSpace(_deviceID)) {
                    await _matrixClient.LoginAsync(new Uri(_matrixNodeAddress), _userName, _password, _deviceID);
                    if (MatrixConnected != null)
                        MatrixConnected?.Invoke(this, new EventArgs());
                    _matrixClient.Start();
                } else {
                    if (_matrixClient == null) result.Message = "Matrix Client is NULL.";
                    if (string.IsNullOrEmpty(_userName)) result.Message = "Matrix Username Empty.";
                    if (string.IsNullOrEmpty(_password)) result.Message = "Matrix Password Empty.";
                    if (string.IsNullOrEmpty(_matrixNodeAddress)) result.Message = "Matrix Node Address Empty.";
                }
                _connectionResult = result;
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.MatrixProtocol.Wrapper.Login");
            }
        }
        /// <summary>
        /// Part Channel
        /// </summary>
        /// <param name="channel"></param>
        public async void PartChannel(string channel) {
            try {
                if (_matrixClient != null) await _matrixClient.LeaveRoomAsync(channel);
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.MatrixProtocol.Wrapper.MatrixWrapper.PartChannel");
            }
        }
        /// <summary>
        /// Join Channel
        /// </summary>
        /// <param name="channel"></param>
        public async void JoinChannel(string channel) {
            try {
                if (_matrixClient != null) await _matrixClient.JoinTrustedPrivateRoomAsync(channel);
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.MatrixProtocol.Wrapper.MatrixWrapper.PartChannel");
            }
        }
        /// <summary>
        /// Get Joined Channels
        /// </summary>
        public async void GetJoinedChannels() {
            try {
                if (_matrixClient != null) _joinedChannels = await _matrixClient.GetJoinedRoomsIdsAsync();
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.MatrixProtocol.Wrapper.MatrixWrapper.GetJoinedChannels");
            }
        }
        /// <summary>
        /// Send Message
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="message"></param>
        public async void SendMessage(string channel, string message) {
            try {
                if (_matrixClient != null) await _matrixClient.SendMessageAsync(channel, message);
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.MatrixProtocol.Wrapper.MatrixWrapper.GetJoinedChannels");
            }
        }
        /// <summary>
        /// Current Channel ID
        /// </summary>
        public string? CurrentChannelID {
            get {
                return _currentChannelID;
            }
        }
        /// <summary>
        /// Joined Channels
        /// </summary>
        public List<string> JoinedChannels {
            get {
                return _joinedChannels;
            }
            set {
                _joinedChannels = value;
            }
        }
        #endregion
    }
}