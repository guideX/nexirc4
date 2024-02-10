using MvvmHelpers.Commands;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using nexIRC.Business.Helper;
using nexIRC.IrcProtocol;
using nexIRC.IrcProtocol.Collections;
using nexIRC.IrcProtocol.Messages;
using nexIRC.MatrixProtocol.Wrapper;
using nexIRC.Messages;
using nexIRC.Model;
using nexIRC.Properties;
namespace nexIRC.ViewModels {
    /// <summary>
    /// Main View Model
    /// </summary>
    public class MainViewModel : BaseViewModel, IHandle<ConnectMessage>, IHandle<OpenQueryMessage>, IHandle<ClientDisconnectedMessage> {
        /// <summary>
        /// Matrix Client
        /// </summary>
        private readonly MatrixWrapper _matrixClient;
        /// <summary>
        /// Irc Client
        /// </summary>
        private readonly Client _ircClient;
        /// <summary>
        /// Ident
        /// </summary>
        //private readonly Ident _ident;
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
        /// <summary>
        /// Matrix Delay Value
        /// </summary>
        private int _matrixDelayValue = 0;
        /// <summary>
        /// Send Matrix Messages
        /// </summary>
        private bool _sendMatrixMessages = false;
        /// <summary>
        /// Client Collection
        /// </summary>
        private ClientCollection _clientCollection;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="showSettingsAction"></param>
        /// <param name="showAboutAction"></param>
        public MainViewModel(Action showSettingsAction, Action showAboutAction) {
            try {
                //_ident = new Ident("guideX");
                ShowSettingsWindow = new Command(showSettingsAction);
                ShowAboutWindow = new Command(showAboutAction);
                App.EventAggregator.SubscribeOnPublishedThread(this);
                _matrixClient = new MatrixProtocol.Wrapper.MatrixWrapper(Settings.Default.MatrixNodeAddress, Settings.Default.MatrixUserName, Settings.Default.MatrixPassword, Settings.Default.MatrixMachineID, Settings.Default.MatrixChannel, Settings.Default.DefaultChannel, Settings.Default.Nick, Settings.Default.MatrixUserName);
                _matrixClient.MatrixRoomEvent += _matrixClient_MatrixRoomEvent;
                _matrixClient.MatrixConnected += _matrixClient_MatrixConnected;
                _ircClient = App.CreateClient();
                _ircClient.RegistrationCompleted += Client_RegistrationCompleted;
                _ircClient.Queries.CollectionChanged += Queries_CollectionChanged;
                _ircClient.Channels.CollectionChanged += Channels_CollectionChanged;
                if (Settings.Default.UseMultipleNicknames) {
                    _clientCollection = new ClientCollection(Settings.Default.ServerAddress, Settings.Default.ServerPort);
                }
                _matrixDelay = new DispatcherTimer();
                _matrixDelay.Tick += _matrixDelay_Tick;
                _matrixDelay.Interval = new TimeSpan(0, 0, 10);
                _matrixDelay.Start();
                if (Settings.Default.AutoReconnect && _ircClient != null)
                    Connect();
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.ViewModels.MainViewModel");
            }

        }
        /// <summary>
        /// Connect
        /// </summary>
        private async void Connect() {
            try {
                await App.EventAggregator.PublishOnUIThreadAsync(new ConnectMessage());
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.ViewModels.Connect");
            }
        }
        /// <summary>
        /// Matrix Connection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void _matrixClient_MatrixConnected(object sender, EventArgs e) {
            try {
                _matrixClient.JoinChannel(Settings.Default.MatrixChannel);
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.ViewModels._matrixClient_MatrixConnected");
            }
        }
        /// <summary>
        /// Matrix Delay Before Linking
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _matrixDelay_Tick(object sender, EventArgs e) {
            try {
                _matrixDelayValue++;
                if (_matrixDelayValue == 3) {
                    _matrixDelay.IsEnabled = false;
                    _sendMatrixMessages = true;
                }
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.ViewModels._matrixDelay_Tick");
            }
        }
        /// <summary>
        /// Matrix Client Matrix Room Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _matrixClient_MatrixRoomEvent(object sender, MatrixRoomEventArgs e) {
            try {
                switch (e.EventType) {
                    case MatrixProtocol.Core.Infrastructure.Dto.Sync.Event.EventType.Encrypted:
                        switch (e.Algorithm) {
                            // THIS DOESN'T WORK YET !!!
                            case "m.megolm.v1.aes-sha2":
                                e.Message = "Warning: Decryption Failure";
                                /*
                                var decryptionResult = Olm.OlmHelper.GroupDecrypt(e.SenderSessionID, e.Message);
                                if (decryptionResult.Success && decryptionResult.Bytes != null) {
                                    e.Message = System.Text.Encoding.UTF8.GetString(decryptionResult.Bytes, 0, decryptionResult.Bytes.Length - 1);
                                } else {
                                    e.Message = "Warning: Decryption Failure";
                                }
                                */
                                /*
                                var n = EncryptionDecryptionHelper.Decrypt(e.SenderKey, e.Message);
                                */
                                break;
                        }
                        break;
                    case MatrixProtocol.Core.Infrastructure.Dto.Sync.Event.EventType.Message:
                        if (_sendMatrixMessages && !e.Details.DoubleRelayDetected && e.Details.SendMessage)
                            if (Settings.Default.UseMultipleNicknames) {
                                _clientCollection.SendMessageAsUser(e.Details.IrcChannel, e.Details.SenderUserID, e.Details.RawMessage);
                            } else {
                                if (e.Details.IrcChannel == "##running" && e.Details.Message.Contains("!strava speed")) {
                                    _ircClient.SendRaw("PRIVMSG " + e.Details.IrcChannel + " :!strava speed");
                                    System.Threading.Thread.Sleep(500);
                                    _ircClient.SendRaw("PRIVMSG " + e.Details.IrcChannel + " :" + e.Details.SenderUserID + " requested !strava speed");
                                } else if (e.Details.IrcChannel == "##running" && e.Details.Message.Contains("!strava elev")) {
                                    _ircClient.SendRaw("PRIVMSG " + e.Details.IrcChannel + " :!strava elev");
                                    System.Threading.Thread.Sleep(500);
                                    _ircClient.SendRaw("PRIVMSG " + e.Details.IrcChannel + " :" + e.Details.SenderUserID + " requested !strava elev");
                                } else if (e.Details.IrcChannel == "##running" && e.Details.Message.Contains("!strava slope")) {
                                    _ircClient.SendRaw("PRIVMSG " + e.Details.IrcChannel + " :!strava slope");
                                    System.Threading.Thread.Sleep(500);
                                    _ircClient.SendRaw("PRIVMSG " + e.Details.IrcChannel + " :" + e.Details.SenderUserID + " requested !strava slope");
                                } else if (e.Details.IrcChannel == "##running" && e.Details.Message.Contains("!strava")) {
                                    _ircClient.SendRaw("PRIVMSG " + e.Details.IrcChannel + " :!strava");
                                    System.Threading.Thread.Sleep(500);
                                    _ircClient.SendRaw("PRIVMSG " + e.Details.IrcChannel + " :" + e.Details.SenderUserID + " requested !strava");
                                } else if (e.Details.IrcChannel == "##running" && e.Details.Message.Contains("!help")) {
                                    _ircClient.SendRaw("PRIVMSG " + e.Details.IrcChannel + " :!help");
                                    System.Threading.Thread.Sleep(500);
                                    _ircClient.SendRaw("PRIVMSG " + e.Details.IrcChannel + " :" + e.Details.SenderUserID + " requested !help");
                                } else {
                                    _ircClient.SendRaw("PRIVMSG " + e.Details.IrcChannel + " :" + e.Details.Message);
                                }
                            }
                        break;
                }
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.ViewModels._matrixClient_MatrixRoomEvent");
            }
        }
        /// <summary>
        /// Handle Async
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task HandleAsync(ConnectMessage message, CancellationToken cancellationToken) {
            try {
                if (App.IsConnected) {
                    MessageBox.Show("Client is already connected.");
                    return;
                }
                var serverTab = new ServerViewModel(_ircClient, _matrixClient, this);
                Tabs.Add(serverTab);
                SelectedTab = serverTab;
                await _ircClient.ConnectAsync();
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.ViewModels.HandleAsync");
            }
        }
        /// <summary>
        /// Handle Async
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task HandleAsync(OpenQueryMessage message, CancellationToken cancellationToken) {
            try {
                App.Client.Queries.GetQuery(message.User);
                var tab = FindQueryTab(message.User);
                if (tab != null)
                    SelectedTab = tab;
                return Task.CompletedTask;
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.ViewModels.HandleAsync");
            }
            return new Task(null);
        }
        /// <summary>
        /// Handle Async
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task HandleAsync(ClientDisconnectedMessage message, CancellationToken cancellationToken) {
            try {
                return Task.CompletedTask;
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.ViewModels.HandleAsync");
            }
            return new(null);
        }
        /// <summary>
        /// Client Registration Completed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Client_RegistrationCompleted(object sender, EventArgs e) {
            try {
                var channel = Settings.Default.DefaultChannel;
                if (string.IsNullOrWhiteSpace(channel)) return;
                await App.Client.SendAsync(new JoinMessage(channel));
                _matrixClient.Login();
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.ViewModels.Client_RegistrationCompleted");
            }
        }
        /// <summary>
        /// Queries Collection Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Queries_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            try {
                foreach (QueryModel query in e.NewItems) App.Dispatcher.Invoke(() => Tabs.Add(new QueryViewModel(query)));
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.ViewModels.Queries_CollectionChanged");
            }
        }
        /// <summary>
        /// Channels Collection Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Channels_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            try {
                foreach (Channel channel in e.NewItems) App.Dispatcher.Invoke(() => Tabs.Add(new ChannelViewModel(channel, _matrixClient)));
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.ViewModels.Channels_CollectionChanged");
            }
        }
        /// <summary>
        /// Fnd Query Tab
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private TabItemViewModel FindQueryTab(UserModel user) {
            try {
                return Tabs.OfType<QueryViewModel>().FirstOrDefault(q => q.Query.User == user);
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.ViewModels.FindQueryTab");
                return null;
            }
        }
        /// <summary>
        /// Find Channel Tab
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public TabItemViewModel FindChannelTab(string channel) {
            try {
                return Tabs.OfType<ChannelViewModel>().FirstOrDefault(q => q.Channel.Name == channel);
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.ViewModels.FindChannelTab");
            }
            return null;
        }
    }
}