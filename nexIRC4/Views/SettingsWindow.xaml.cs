using System;
using System.Linq;
using System.Windows.Controls;
using nexIRC.Business.Helper;
using nexIRC.Business.Objects;
using nexIRC.Messages;
using nexIRC.Model;
using nexIRC.Models;
using nexIRC.Properties;
using MahApps.Metro.Controls;

namespace nexIRC.Views {
    /// <summary>
    /// Settings Window
    /// </summary>
    public partial class SettingsWindow : MetroWindow {
        /// <summary>
        /// Autojoin
        /// </summary>
        private AutoJoinObject _autojoin;
        
        /// <summary>
        /// Available networks
        /// </summary>
        private readonly System.Collections.Generic.List<IrcNetwork> _networks;
        
        /// <summary>
        /// Constructor
        /// </summary>
        private SettingsWindow() {
            InitializeComponent();
            cmdAdd.Click += cmdAdd_Click;
            cmdDelete.Click += cmdDelete_Click;
            _autojoin = new AutoJoinObject();
            
            // Initialize networks
            _networks = PredefinedNetworks.GetDefaultNetworks();
            NetworkComboBox.ItemsSource = _networks;
            
            // Select default network or Custom
            var defaultNetwork = _networks.FirstOrDefault(n => n.Name == "Libera.Chat") ?? _networks.Last();
            NetworkComboBox.SelectedItem = defaultNetwork;
            
            FillAutoJoin();
        }
        
        /// <summary>
        /// Network selection changed
        /// </summary>
        private void NetworkComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            try {
                if (NetworkComboBox.SelectedItem is IrcNetwork network) {
                    ServerComboBox.ItemsSource = network.Servers;
                    if (network.Servers.Count > 0) {
                        ServerComboBox.SelectedIndex = 0;
                        
                        // Auto-fill server details
                        var server = network.Servers[0];
                        ServerAddress.Text = server.Address;
                        ServerPort.Text = server.Port.ToString();
                        
                        // Fill default channel if network has one
                        if (!string.IsNullOrEmpty(network.DefaultChannel)) {
                            DefaultChannel.Text = network.DefaultChannel;
                        }
                    }
                }
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.Views.SettingsWindow.NetworkComboBox_SelectionChanged");
            }
        }
        
        /// <summary>
        /// Fill Auto Join
        /// </summary>
        private void FillAutoJoin() {
            txtIRCChannel.Text = "";
            txtMatrixChannelID.Text = "";
            lvwAutoJoin.Items.Clear();
            foreach (var aj in _autojoin.Autojoin)
                lvwAutoJoin.Items.Add(aj);
        }
        
        /// <summary>
        /// Delete
        /// </summary>
        private void cmdDelete_Click(object sender, System.Windows.RoutedEventArgs e) {
            try {
                if (lvwAutoJoin.SelectedItem is AutojoinModel item) {
                    _autojoin.Delete(item.IRCChannel, item.MatrixChannelID);
                    FillAutoJoin();
                }
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.Views.SettingsWindow.cmdDelete_Click");
            }
        }
        
        /// <summary>
        /// Add
        /// </summary>
        private void cmdAdd_Click(object sender, System.Windows.RoutedEventArgs e) {
            try {
                _autojoin.Create(txtIRCChannel.Text, txtMatrixChannelID.Text);
                FillAutoJoin();
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.Views.SettingsWindow.cmdAdd_Click");
            }
        }
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parent"></param>
        public SettingsWindow(MainWindow parent) : this() {
            try {
                Owner = parent;
                
                // Load current settings
                LoadSettings();
                
                ConnectButton.Click += async (s, e) => {
                    Save();
                    Close();
                    await App.EventAggregator.PublishOnUIThreadAsync(new ConnectMessage());
                };
                OkButton.Click += (s, e) => {
                    Save();
                    Close();
                };
                CancelButton.Click += (s, e) => Close();
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.Views.SettingsWindow.Constructor");
            }
        }
        
        /// <summary>
        /// Load Settings
        /// </summary>
        private void LoadSettings() {
            try {
                // Try to match existing server to a network
                var existingAddress = Settings.Default.ServerAddress;
                var matchedNetwork = _networks.FirstOrDefault(n => 
                    n.Servers.Any(s => s.Address.Equals(existingAddress, StringComparison.OrdinalIgnoreCase)));
                
                if (matchedNetwork != null) {
                    NetworkComboBox.SelectedItem = matchedNetwork;
                    var matchedServer = matchedNetwork.Servers.FirstOrDefault(s => 
                        s.Address.Equals(existingAddress, StringComparison.OrdinalIgnoreCase));
                    if (matchedServer != null) {
                        ServerComboBox.SelectedItem = matchedServer;
                    }
                } else {
                    // Select Custom network
                    NetworkComboBox.SelectedItem = _networks.Last();
                }
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.Views.SettingsWindow.LoadSettings");
            }
        }
        
        /// <summary>
        /// Save
        /// </summary>
        private void Save() {
            try {
                Settings.Default.Nick = Nickname.Text;
                Settings.Default.Alternative = Alternative.Text;
                Settings.Default.RealName = RealName.Text;
                Settings.Default.DefaultChannel = DefaultChannel.Text;
                Settings.Default.ServerName = ServerName.Text;
                Settings.Default.ServerAddress = ServerAddress.Text;
                Settings.Default.ServerPort = ServerPort.Text;
                Settings.Default.ServerPassword = ServerPassword.Password;
                Settings.Default.MatrixChannel = MatrixChannel.Text;
                Settings.Default.MatrixMachineID = MatrixMachineID.Text;
                Settings.Default.MatrixNodeAddress = MatrixNodeAddress.Text;
                Settings.Default.MatrixPassword = MatrixPassword.Password;
                Settings.Default.MatrixUserName = MatrixUsername.Text;
                Settings.Default.UseMultipleNicknames = chkUseMultipleNicknames.IsChecked ?? false;
                Settings.Default.AutoReconnect = chkAutoReconnect.IsChecked ?? false;
                Settings.Default.IdentUsername = IdentUserName.Text;
                Settings.Default.UseMatrix = chkUseMatrix.IsChecked ?? false;
                Settings.Default.Save();
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.Views.SettingsWindow.Save");
            }
        }
    }
}