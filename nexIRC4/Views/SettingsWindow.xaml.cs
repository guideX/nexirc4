using MahApps.Metro.Controls;
using Microsoft.Extensions.Configuration;
using nexIRC.Business.Helper;
using nexIRC.Messages;
using nexIRC.Model;
using nexIRC.Properties;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
namespace nexIRC.Views {
    /// <summary>
    /// Settings Window
    /// </summary>
    public partial class SettingsWindow : MetroWindow {
        /// <summary>
        /// Autojoin
        /// </summary>
        private List<AutojoinModel> _autojoin;
        /// <summary>
        /// Themese
        /// </summary>
        private readonly string[] themes = new[] { "Light", "Dark" };
        /// <summary>
        /// Themes
        /// </summary>
        public string[] Themes => themes;
        /// <summary>
        /// Constructor
        /// </summary>
        private SettingsWindow() {
            InitializeComponent();
            cmdAdd.Click += cmdAdd_Click;
            cmdDelete.Click += cmdDelete_Click;
            IConfiguration autojoin = new ConfigurationBuilder().AddIniFile(System.AppDomain.CurrentDomain.BaseDirectory + @"autojoin.ini").Build();
            IConfigurationSection section = autojoin.GetSection("Settings");
            int.TryParse(section["Count"], out int count);
            _autojoin = new List<AutojoinModel>();
            lvwAutoJoin.Items.Clear();
            for (int i = 1; i < count + 1; i++) {
                IConfigurationSection n = autojoin.GetSection(i.ToString());
                var ajm = new AutojoinModel() {
                    IRCChannel = n["IRCChannel"],
                    MatrixChannelID = n["MatrixChannelID"]
                };
                _autojoin.Add(ajm);
                var lvItem = new ListViewItem();
                lvItem.Content = ajm;
                lvwAutoJoin.Items.Add(lvItem);
            }
        }
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdDelete_Click(object sender, System.Windows.RoutedEventArgs e) {

        }
        /// <summary>
        /// Add
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdAdd_Click(object sender, System.Windows.RoutedEventArgs e) {
            IConfiguration autojoin = new ConfigurationBuilder().AddIniFile(System.AppDomain.CurrentDomain.BaseDirectory + @"autojoin.ini").Build();
            IConfigurationSection section = autojoin.GetSection("Settings");
            //int.TryParse(section["Count"], out int count);
        }
        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="parent"></param>
        public SettingsWindow(MainWindow parent) : this() {
            try {
                Owner = parent;
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
                Settings.Default.ServerPassword = ServerPassword.Text;
                Settings.Default.MatrixChannel = MatrixChannel.Text;
                Settings.Default.MatrixMachineID = MatrixMachineID.Text;
                Settings.Default.MatrixNodeAddress = MatrixNodeAddress.Text;
                Settings.Default.MatrixPassword = MatrixPassword.Text;
                Settings.Default.MatrixUserName = MatrixUsername.Text;
                Settings.Default.UseMultipleNicknames = chkUseMultipleNicknames.IsChecked.Value;
                Settings.Default.AutoReconnect = chkAutoReconnect.IsChecked.Value;
                Settings.Default.IdentUsername = IdentUserName.Text;
                Settings.Default.UseMatrix = chkUseMatrix.IsChecked.Value;
                Settings.Default.Save();
                //ThemeManager.Current.ChangeTheme(Application.Current, $"{Theme.SelectedValue}.Blue");
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.Views.SettingsWindow.Save");
            }
        }
    }
}