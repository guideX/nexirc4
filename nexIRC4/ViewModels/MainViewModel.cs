using MvvmHelpers.Commands;
using nexIRC.IrcProtocol;
using nexIRC.IrcProtocol.Messages;
using nexIRC.MatrixProtocol.Wrapper;
using nexIRC.Messages;
using nexIRC.Properties;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Threading;
namespace nexIRC.ViewModels {
    /// <summary>
    /// Main View Model
    /// </summary>
    public class MainViewModel : BaseViewModel, IHandle<ConnectMessage>, IHandle<OpenQueryMessage>, IHandle<ClientDisconnectedMessage> {
        /// <summary>
        /// Matrix Client
        /// </summary>
        private readonly nexIRC.MatrixProtocol.Wrapper.MatrixWrapper _matrixClient;
        /// <summary>
        /// Irc Client
        /// </summary>
        private readonly Client _ircClient;
        /// <summary>
        /// Tabs
        /// </summary>
        public ObservableCollection<TabItemViewModel> Tabs { get; } = new ObservableCollection<TabItemViewModel>();
        /// <summary>
        /// Selected Tab
        /// </summary>
        private TabItemViewModel selectedTab;
        /// <summary>
        /// Selected Tab
        /// </summary>
        public TabItemViewModel SelectedTab {
            get => selectedTab;
            set => SetProperty(ref selectedTab, value);
        }
        /// <summary>
        /// Show Settings Window
        /// </summary>
        public ICommand ShowSettingsWindow { get; }
        /// <summary>
        /// Show About Window
        /// </summary>
        public ICommand ShowAboutWindow { get; }
        /// <summary>
        /// Matrix Delay
        /// </summary>
        private DispatcherTimer _matrixDelay = new DispatcherTimer();
        private int _matrixDelayValue = 0;
        private bool _sendMatrixMessages = false;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="showSettingsAction"></param>
        /// <param name="showAboutAction"></param>
        public MainViewModel(Action showSettingsAction, Action showAboutAction) {
            ShowSettingsWindow = new Command(showSettingsAction);
            ShowAboutWindow = new Command(showAboutAction);
            App.EventAggregator.SubscribeOnPublishedThread(this);
            _matrixClient = new MatrixProtocol.Wrapper.MatrixWrapper(Settings.Default.MatrixNodeAddress, Settings.Default.MatrixUserName, Settings.Default.MatrixPassword, Settings.Default.MatrixMachineID, Settings.Default.MatrixChannel, Settings.Default.DefaultChannel);
            _matrixClient.MatrixRoomEvent += _matrixClient_MatrixRoomEvent;
            _matrixClient.MatrixConnected += _matrixClient_MatrixConnected;
            _ircClient = App.CreateClient();
            _ircClient.RegistrationCompleted += Client_RegistrationCompleted;
            _ircClient.Queries.CollectionChanged += Queries_CollectionChanged;
            _ircClient.Channels.CollectionChanged += Channels_CollectionChanged;
            _matrixDelay = new DispatcherTimer();
            _matrixDelay.Tick += _matrixDelay_Tick;
            _matrixDelay.Interval = new TimeSpan(0, 0, 10);
            _matrixDelay.Start();
        }
        /// <summary>
        /// Matrix Connection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void _matrixClient_MatrixConnected(object sender, EventArgs e) {
            _matrixClient.JoinChannel(Settings.Default.MatrixChannel);
        }
        /// <summary>
        /// Matrix Delay Before Linking
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _matrixDelay_Tick(object sender, EventArgs e) {
            _matrixDelayValue++;
            if (_matrixDelayValue == 3) {
                _matrixDelay.IsEnabled = false;
                _sendMatrixMessages = true;
            }
        }
        /// <summary>
        /// Matrix Client Matrix Room Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _matrixClient_MatrixRoomEvent(object sender, MatrixRoomEventArgs e) {
            switch (e.EventType) {
                case MatrixProtocol.Core.Infrastructure.Dto.Sync.Event.EventType.Message:
                    if (_sendMatrixMessages && !e.Details.DoubleRelayDetected && e.Details.SendMessage)
                        _ircClient.SendRaw("PRIVMSG " + e.Details.IrcChannel + " :" + e.Details.Message);
                    break;
            }
        }
        /// <summary>
        /// Handle Async
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task HandleAsync(ConnectMessage message, CancellationToken cancellationToken) {
            if (App.IsConnected) {
                MessageBox.Show("Client is already connected.");
                return;
            }
            var serverTab = new ServerViewModel(_ircClient, _matrixClient, this);
            Tabs.Add(serverTab);
            SelectedTab = serverTab;
            await _ircClient.ConnectAsync();
        }
        /// <summary>
        /// Handle Async
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task HandleAsync(OpenQueryMessage message, CancellationToken cancellationToken) {
            App.Client.Queries.GetQuery(message.User);
            var tab = FindQueryTab(message.User);
            if (tab != null) 
                SelectedTab = tab;
            return Task.CompletedTask;
        }
        /// <summary>
        /// Handle Async
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task HandleAsync(ClientDisconnectedMessage message, CancellationToken cancellationToken) {
            MessageBox.Show("Disconnected...");
            return Task.CompletedTask;
        }
        /// <summary>
        /// Client Registration Completed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Client_RegistrationCompleted(object sender, EventArgs e) {
            var channel = Settings.Default.DefaultChannel;
            if (string.IsNullOrWhiteSpace(channel)) return;
            await App.Client.SendAsync(new JoinMessage(channel));
            _matrixClient.Login();
        }
        /// <summary>
        /// Queries Collection Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Queries_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            foreach (Query query in e.NewItems)
                App.Dispatcher.Invoke(() => Tabs.Add(new QueryViewModel(query)));
        }
        /// <summary>
        /// Channels Collection Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Channels_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            foreach (Channel channel in e.NewItems)
                App.Dispatcher.Invoke(() => Tabs.Add(new ChannelViewModel(channel, _matrixClient)));
        }
        /// <summary>
        /// Fnd Query Tab
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private TabItemViewModel FindQueryTab(User user) {
            return Tabs.OfType<QueryViewModel>().FirstOrDefault(q => q.Query.User == user);
        
        }
        /// <summary>
        /// Find Channel Tab
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public TabItemViewModel FindChannelTab(string channel) {
            return Tabs.OfType<ChannelViewModel>().FirstOrDefault(q => q.Channel.Name == channel);
        }
    }
}