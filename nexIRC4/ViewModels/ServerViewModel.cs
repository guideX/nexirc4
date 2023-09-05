using MvvmHelpers.Commands;
using nexIRC.IrcProtocol;
using nexIRC.Business.Business;
using nexIRC.Properties;
using System.Threading.Tasks;
using nexIRC.MatrixProtocol.Wrapper;
namespace nexIRC.ViewModels {
    /// <summary>
    /// Server View Model
    /// </summary>
    public class ServerViewModel : TabItemViewModel {
        /// <summary>
        /// Client
        /// </summary>
        private Client _ircClient;
        /// <summary>
        /// Matrix Client
        /// </summary>
        private MatrixWrapper _matrixClient;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client"></param>
        /// <param name="matrixClient"></param>
        public ServerViewModel(Client client, MatrixWrapper matrixClient) {
            _ircClient = client;
            _matrixClient = matrixClient;
            _matrixClient.MatrixRoomEvent += _matrixClient_MatrixRoomEvent;
            _matrixClient.MatrixConnected += _matrixClient_MatrixConnected;
            _ircClient.ServerMessages.CollectionChanged += ServerMessages_CollectionChanged;
            SendMessageCommand = new AsyncCommand(SendServerMessage);
        }
        /// <summary>
        /// Matrix Connected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _matrixClient_MatrixConnected(object sender, System.EventArgs e) {
            ShowText("Matrix Connected");
        }
        /// <summary>
        /// Matrix Room Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void _matrixClient_MatrixRoomEvent(object sender, MatrixRoomEventArgs e) {
            switch (e.EventType) {
                case MatrixProtocol.Core.Infrastructure.Dto.Sync.Event.EventType.Encrypted:
                    ShowText("[" + e.RoomId + e.SenderUserId + "] " + e.Message);
                    break;
                case MatrixProtocol.Core.Infrastructure.Dto.Sync.Event.EventType.Message:
                    ShowText("[" + e.RoomId + e.SenderUserId + "] " + e.Message);
                    break;
                case MatrixProtocol.Core.Infrastructure.Dto.Sync.Event.EventType.Create:
                    //ShowText(e.RoomId + " was created by " + e.SenderUserId + );
                    break;
            }
        }
        /// <summary>
        /// Send Server Message
        /// </summary>
        /// <returns></returns>
        private async Task SendServerMessage() {
            if (string.IsNullOrWhiteSpace(Message)) return;
            if (Message.StartsWith("/")) {
                var tb = new TextBoxOutputBusiness(Message);
                tb.Process();
                //_matrixClient.LastException = null;
                switch (tb.LastOutputEvent) {
                    case Business.Enum.OutputEventEnum.GetJoinedChannels:
                        _matrixClient?.GetJoinedChannels();
                        break;
                    case Business.Enum.OutputEventEnum.PartChannelMatrix:
                        if (tb.MatrixJoinChannel != null && !string.IsNullOrWhiteSpace(tb.MatrixJoinChannel.ChannelName))
                            _matrixClient?.PartChannel(
                                tb.MatrixJoinChannel.ChannelName
                            );
                        break;
                    case Business.Enum.OutputEventEnum.SendMessageMatrix:
                        _matrixClient?.SendMessage(
                            tb.MatrixSendMessage.ChannelName,
                            tb.MatrixSendMessage.Message
                        );
                        break;
                    case Business.Enum.OutputEventEnum.JoinChannelMatrix:
                        if (tb.MatrixJoinChannel != null && !string.IsNullOrWhiteSpace(tb.MatrixJoinChannel.ChannelName))
                            _matrixClient?.JoinChannel(tb.MatrixJoinChannel.ChannelName);
                        break;
                    case Business.Enum.OutputEventEnum.PrivMsg:
                        if (tb.PrivMsg.Success) {
                            ShowText("PRIVMSG " + tb.PrivMsg.Nickname + " :" + tb.PrivMsg.MessageToSend);
                            await App.Client.SendRaw("PRIVMSG " + tb.PrivMsg.Nickname + " :" + tb.PrivMsg.MessageToSend + ((char)13).ToString());
                        } else {
                            ShowText("Error: " + tb.PrivMsg.Message);
                        }
                        break;
                    case Business.Enum.OutputEventEnum.ConnectMatrix:
                        _matrixClient?.Login();
                        /*
                        tb.ConnectMatrix?.Invoke(this, new MatrixRoomEventArgs() {
                            RoomId = roomId,
                            SenderUserId = senderUserId,
                            Message = message
                        });
                        */
                        break;
                    case Business.Enum.OutputEventEnum.AutoJoinMatrix:
                        _matrixClient.JoinChannel(Settings.Default.MatrixChannel);
                        break;
                }
                return;
            }
            Message = string.Empty;
        }
        /// <summary>
        /// Server Messages Collection Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ServerMessages_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
            foreach (ServerMessage message in e.NewItems) {
                ShowText(message);
            }
        }
        /// <summary>
        /// To String
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Settings.Default.ServerName ?? "Server";
        /// <summary>
        /// Show Text
        /// </summary>
        /// <param name="message"></param>
        public void ShowText(string message) {
            ShowText(new ServerMessage(message));
        }
        /// <summary>
        /// Show Text
        /// </summary>
        /// <param name="message"></param>
        public void ShowText(ServerMessage message) {
            App.Dispatcher.Invoke(() => Messages.Add(Models.Message.Received(message)));
        }
    }
}