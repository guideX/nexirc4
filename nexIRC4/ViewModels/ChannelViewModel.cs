using MvvmHelpers.Commands;
using nexIRC.Business.Helper;
using nexIRC.IrcProtocol;
using nexIRC.IrcProtocol.Messages;
using nexIRC.Messages;
using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
namespace nexIRC.ViewModels {
    /// <summary>
    /// Channel View Model
    /// </summary>
    public class ChannelViewModel : TabItemViewModel {
        /// <summary>
        /// Channel
        /// </summary>
        public Channel Channel { get; }
        /// <summary>
        /// Sort Users Command
        /// </summary>
        public ICommand SortUsersCommand { get; }
        /// <summary>
        /// Open Query Command
        /// </summary>
        public ICommand OpenQueryCommand { get; }
        /// <summary>
        /// Matrix Client
        /// </summary>
        private MatrixProtocol.Wrapper.MatrixWrapper _matrixClient;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="matrixClient"></param>
        public ChannelViewModel(Channel channel, MatrixProtocol.Wrapper.MatrixWrapper matrixClient) {
            try {
                _matrixClient = matrixClient;
                Channel = channel;
                channel.Messages.CollectionChanged += Messages_CollectionChanged;
                SendMessageCommand = new AsyncCommand(SendChannelMessage);
                SortUsersCommand = new Command(SortUsers);
                OpenQueryCommand = new AsyncCommand<ChannelUser>(OpenQuery);
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.ViewModels.ChannelViewModel", AppPath);
            }
        }
        /// <summary>
        /// Sort Users
        /// </summary>
        /// <param name="items"></param>
        private void SortUsers(object items) {
            try {
                var view = CollectionViewSource.GetDefaultView(items) as ListCollectionView;
                view.CustomSort = new ChannelUserComparer();
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.ViewModels.SortUsers", AppPath);
            }
        }
        /// <summary>
        /// Open Query
        /// </summary>
        /// <param name="channelUser"></param>
        /// <returns></returns>
        private async Task OpenQuery(ChannelUser channelUser) {
            try {
                await App.EventAggregator.PublishOnUIThreadAsync(new OpenQueryMessage(channelUser.User));
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.ViewModels.OpenQuery", AppPath);
            }
        }
        /// <summary>
        /// Send Channel Message
        /// </summary>
        /// <returns></returns>
        private async Task SendChannelMessage() {
            try {
                if (string.IsNullOrWhiteSpace(Message)) {
                    return;
                }
                Messages.Add(Models.Message.Sent(new ChannelMessage(App.Client.User, Channel, Message)));
                await App.Client.SendAsync(new PrivMsgMessage(Channel.Name, Message));
                Message = string.Empty;
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.ViewModels.SendChannelMessage", AppPath);
            }
        }
        /// <summary>
        /// Messages Collection Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Messages_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            try {
                foreach (ChannelMessage message in e.NewItems) {
                    App.Dispatcher.Invoke(() => Messages.Add(Models.Message.Received(message)));
                    _matrixClient.SendMessage(_matrixClient.CurrentChannelID, message.User.Nick + ": " + message.Text);
                }
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.ViewModels.Messages_CollectionChanged", AppPath);
            }
        }
        /// <summary>
        /// To String
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Channel.Name;
    }
}