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
        #region "variables"
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
        private Ident _ident;
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
        /// Is Connected
        /// </summary>
        private bool _isConnected;
        public bool IsConnected {
            get => _isConnected;
            set {
                SetProperty(ref _isConnected, value);
                UpdateCommandStates();
            }
        }
        
        /// <summary>
        /// Connection Status Text
        /// </summary>
        private string _connectionStatus = "Disconnected";
        public string ConnectionStatus {
            get => _connectionStatus;
            set => SetProperty(ref _connectionStatus, value);
        }
        
        /// <summary>
        /// Connect Command
        /// </summary>
        public ICommand ConnectCommand { get; }
        
        /// <summary>
        /// Disconnect Command
        /// </summary>
        public ICommand DisconnectCommand { get; }
        
        /// <summary>
        /// Join Channel Command
        /// </summary>
        public ICommand JoinChannelCommand { get; }
        
        /// <summary>
        /// Part Channel Command
        /// </summary>
        public ICommand PartChannelCommand { get; }
        
        /// <summary>
        /// Change Nick Command
        /// </summary>
        public ICommand ChangeNickCommand { get; }
        
        /// <summary>
        /// Show Users Command
        /// </summary>
        public ICommand ShowUsersCommand { get; }
        
        /// <summary>
        /// Clear Messages Command
        /// </summary>
        public ICommand ClearMessagesCommand { get; }
        
        #endregion
        
        #region "methods"
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="showSettingsAction"></param>
        /// <param name="showAboutAction"></param>
        public MainViewModel(Action showSettingsAction, Action showAboutAction) {
            try {
                if (Settings.Default.UseMultipleNicknames) IdentListen(Settings.Default.Nick);
                
                // Initialize Commands
                ShowSettingsWindow = new Command(showSettingsAction);
                ShowAboutWindow = new Command(showAboutAction);
                ConnectCommand = new AsyncCommand(ConnectToServer);
                DisconnectCommand = new AsyncCommand(DisconnectFromServer);
                JoinChannelCommand = new AsyncCommand(JoinChannel);
                PartChannelCommand = new AsyncCommand(PartChannel);
                ChangeNickCommand = new AsyncCommand(ChangeNick);
                ShowUsersCommand = new Command(ShowUsers);
                ClearMessagesCommand = new Command(ClearMessages);
                
                App.EventAggregator.SubscribeOnPublishedThread(this);
                
                if (Settings.Default.UseMatrix) {
                    _matrixClient = new MatrixProtocol.Wrapper.MatrixWrapper(Settings.Default.MatrixNodeAddress, Settings.Default.MatrixUserName, Settings.Default.MatrixPassword, Settings.Default.MatrixMachineID, Settings.Default.MatrixChannel, Settings.Default.DefaultChannel, Settings.Default.Nick, Settings.Default.MatrixUserName);
                    _matrixClient.MatrixRoomEvent += _matrixClient_MatrixRoomEvent;
                    _matrixClient.MatrixConnected += _matrixClient_MatrixConnected;
                }
                
                _ircClient = App.CreateClient();
                _ircClient.RegistrationCompleted += Client_RegistrationCompleted;
                _ircClient.Queries.CollectionChanged += Queries_CollectionChanged;
                _ircClient.Channels.CollectionChanged += Channels_CollectionChanged;
                
                if (Settings.Default.UseMultipleNicknames)
                    _clientCollection = new ClientCollection(Settings.Default.ServerAddress, Settings.Default.ServerPort, _ident);
                
                _matrixDelay = new DispatcherTimer();
                _matrixDelay.Tick += _matrixDelay_Tick;
                _matrixDelay.Interval = new TimeSpan(0, 0, 10);
                _matrixDelay.Start();
                
                if (Settings.Default.AutoReconnect && _ircClient != null)
                    Connect();
                    
                UpdateConnectionStatus();
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.ViewModels.MainViewModel");
            }
        }
        
        /// <summary>
        /// Update Command States
        /// </summary>
        private void UpdateCommandStates() {
            // Commands will check CanExecute based on IsConnected property
        }
        
        /// <summary>
        /// Update Connection Status
        /// </summary>
        private void UpdateConnectionStatus() {
            IsConnected = ((App)Application.Current).IsConnected;
            ConnectionStatus = IsConnected ? $"Connected to {Settings.Default.ServerAddress}" : "Disconnected";
        }
        
        /// <summary>
        /// Connect to Server
        /// </summary>
        private async Task ConnectToServer() {
            try {
                if (((App)Application.Current).IsConnected) {
                    MessageBox.Show("Already connected to server.", "Connected", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                await App.EventAggregator.PublishOnUIThreadAsync(new ConnectMessage());
                UpdateConnectionStatus();
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.ViewModels.ConnectToServer");
            }
        }
        
        /// <summary>
        /// Disconnect from Server
        /// </summary>
        private async Task DisconnectFromServer() {
            try {
                if (!((App)Application.Current).IsConnected) {
                    MessageBox.Show("Not connected to any server.", "Not Connected", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                
                if (_ircClient != null) {
                    await _ircClient.SendAsync(new QuitMessage("User disconnected"));
                    _ircClient.Dispose();
                    
                    // Clear tabs except server tab
                    var serverTab = Tabs.OfType<ServerViewModel>().FirstOrDefault();
                    Tabs.Clear();
                    if (serverTab != null) {
                        Tabs.Add(serverTab);
                        SelectedTab = serverTab;
                    }
                    
                    UpdateConnectionStatus();
                    MessageBox.Show("Disconnected from server.", "Disconnected", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.ViewModels.DisconnectFromServer");
            }
        }
        
        /// <summary>
        /// Join Channel
        /// </summary>
        private async Task JoinChannel() {
            try {
                if (!((App)Application.Current).IsConnected) {
                    MessageBox.Show("Not connected to server.", "Not Connected", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                
                var channelName = Microsoft.VisualBasic.Interaction.InputBox(
                    "Enter channel name (e.g., #mychannel):",
                    "Join Channel",
                    "#");
                    
                if (!string.IsNullOrWhiteSpace(channelName)) {
                    if (!channelName.StartsWith("#")) {
                        channelName = "#" + channelName;
                    }
                    await _ircClient.SendAsync(new JoinMessage(channelName));
                }
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.ViewModels.JoinChannel");
            }
        }
        
        /// <summary>
        /// Part Channel
        /// </summary>
        private async Task PartChannel() {
            try {
                if (!((App)Application.Current).IsConnected) {
                    MessageBox.Show("Not connected to server.", "Not Connected", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                
                if (SelectedTab is ChannelViewModel channelTab) {
                    var result = MessageBox.Show(
                        $"Leave channel {channelTab.Channel.Name}?",
                        "Part Channel",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);
                        
                    if (result == MessageBoxResult.Yes) {
                        await _ircClient.SendAsync(new PartMessage(channelTab.Channel.Name));
                        Tabs.Remove(channelTab);
                    }
                } else {
                    MessageBox.Show("Please select a channel tab first.", "No Channel Selected", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.ViewModels.PartChannel");
            }
        }
        
        /// <summary>
        /// Change Nick
        /// </summary>
        private async Task ChangeNick() {
            try {
                if (!((App)Application.Current).IsConnected) {
                    MessageBox.Show("Not connected to server.", "Not Connected", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                
                var newNick = Microsoft.VisualBasic.Interaction.InputBox(
                    "Enter new nickname:",
                    "Change Nickname",
                    Settings.Default.Nick);
                    
                if (!string.IsNullOrWhiteSpace(newNick)) {
                    await _ircClient.SendAsync(new NickMessage(newNick));
                    Settings.Default.Nick = newNick;
                    Settings.Default.Save();
                }
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.ViewModels.ChangeNick");
            }
        }
        
        /// <summary>
        /// Show Users
        /// </summary>
        private void ShowUsers() {
            try {
                if (SelectedTab is ChannelViewModel channelTab) {
                    var users = string.Join(", ", channelTab.Channel.Users.Select(u => u.Nick));
                    MessageBox.Show(
                        $"Users in {channelTab.Channel.Name}:\n\n{users}",
                        "Channel Users",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                } else {
                    MessageBox.Show("Please select a channel tab first.", "No Channel Selected", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.ViewModels.ShowUsers");
            }
        }
        
        /// <summary>
        /// Clear Messages
        /// </summary>
        private void ClearMessages() {
            try {
                if (SelectedTab is ChannelViewModel channelTab) {
                    channelTab.Channel.Messages.Clear();
                } else if (SelectedTab is QueryViewModel queryTab) {
                    queryTab.Query.Messages.Clear();
                } else if (SelectedTab is ServerViewModel serverTab) {
                    serverTab.Messages.Clear();
                } else {
                    MessageBox.Show("Please select a tab first.", "No Tab Selected", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.ViewModels.ClearMessages");
            }
        }
        
        /// <summary>
        /// Is User In Client Collection
        /// </summary>
        /// <param name="user"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        public bool IsUserInClientCollection(string user, string channel) {
            if (_clientCollection != null) {
                return _clientCollection.IsUserInCollection(channel, user);
            }
            return false;
        }
        
        /// <summary>
        /// Ident Listen
        /// </summary>
        /// <param name="userName"></param>
        public void IdentListen(string userName) {
            if(_ident != null) _ident.Close();
            _ident = new Ident(113, "UNIX", userName);
            _ident.Listen();
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
                if (Settings.Default.UseMatrix) {
                    LogHelper.LogActivity("Matrix Connected");
                    _matrixClient.JoinChannel(Settings.Default.MatrixChannel);
                }
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
                if (Settings.Default.UseMatrix) {
                    switch (e.EventType) {
                        case MatrixProtocol.Core.Infrastructure.Dto.Sync.Event.EventType.Message:
                            if (_sendMatrixMessages && !e.Details.DoubleRelayDetected && e.Details.SendMessage) {
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
                            }
                            if (e.Details.IrcChannel.StartsWith("##running") && e.Details.Message != null && e.Details.Message.Contains("!pace ")) {
                                // Parameters:
                                //   --time:<time> // <time> is in minutes/seconds format, example: 50:52
                                //   --distance:<miles or km> // <miles> example: 4.52, <km> example: 3.1k
                                //   --method:<conversion method> // either eu or us (us is presumed)
                                //var splt = e.Details.Message.Split(' ')
                                var time = "";
                                double minutes = 0;
                                var distance = "";
                                var method = "us";
                                double result = 0;
                                if (e.Details.Message.ToLower().Contains("--time:")) {
                                    var time_splt1 = e.Details.Message.SplitStringByOtherString("--time");
                                    var time_splt2 = time_splt1[1].Split(' ');
                                    if (time_splt2[0].Contains(":")) {
                                        var time_split3 = time_splt2[0].Split(':');
                                        minutes = time_split3[1].ToIntNotNullable();
                                    } else if (time_splt2[0].IsNumeric()) {
                                        time = time_splt2[0];
                                    }
                                }
                                if (e.Details.Message.ToLower().Contains("--method:eu")) {
                                    method = "eu";
                                }
                                if (e.Details.Message.ToLower().Contains("--distance:")) {
                                    var distance_splt1 = e.Details.Message.SplitStringByOtherString("--distance");
                                    var distance_splt2 = distance_splt1[1].Split(' ');
                                    distance = distance_splt2[0].Replace(":", "");
                                }
                                if (minutes != 0 && !string.IsNullOrWhiteSpace(distance)) {
                                    result = (minutes / Convert.ToDouble(distance));
                                    switch (method) {
                                        case "us":
                                            _ircClient.SendRaw("PRIVMSG " + e.Details.IrcChannel + " :" + String.Format("{0:0.00}", result) + " minutes per mile");
                                            break;
                                        case "eu":
                                            result = (minutes / Convert.ToDouble(distance));
                                            _ircClient.SendRaw("PRIVMSG " + e.Details.IrcChannel + " :" + String.Format("{0:0.00}", result) + " minutes per km");
                                            break;
                                    }
                                }
                            }




                            break;
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
                    }
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
                if (((App)Application.Current).IsConnected) {
                    MessageBox.Show("Client is already connected.");
                    return;
                }
                var serverTab = new ServerViewModel(_ircClient, _matrixClient, this);
                Tabs.Add(serverTab);
                SelectedTab = serverTab;
                await _ircClient.ConnectAsync();
                
                // Update connection status after connecting
                UpdateConnectionStatus();
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
                // Update connection status when disconnected
                UpdateConnectionStatus();
                return Task.CompletedTask;
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.ViewModels.HandleAsync");
            }
            return Task.CompletedTask;
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
                if (Settings.Default.UseMatrix) {
                    _matrixClient.Login();
                }
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
                foreach (Channel channel in e.NewItems) 
                    App.Dispatcher.Invoke(() => 
                    Tabs.Add(new ChannelViewModel(channel, _matrixClient))
                );
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
        #endregion
    }
}