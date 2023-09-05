using ControlzEx.Theming;
using MahApps.Metro.Controls;
using nexIRC.Messages;
using nexIRC.Properties;
using System.Windows;
namespace nexIRC.Views {
    /// <summary>
    /// Settings Window
    /// </summary>
    public partial class SettingsWindow : MetroWindow {
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
        }
        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="parent"></param>
        public SettingsWindow(MainWindow parent) : this() {
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
        }
        /// <summary>
        /// Save
        /// </summary>
        private void Save() {
            Settings.Default.Nick = Nickname.Text;
            Settings.Default.Alternative = Alternative.Text;
            Settings.Default.RealName = RealName.Text;
            Settings.Default.DefaultChannel = DefaultChannel.Text;
            Settings.Default.Theme = Theme.SelectedValue.ToString();
            Settings.Default.ServerName = ServerName.Text;
            Settings.Default.ServerAddress = ServerAddress.Text;
            Settings.Default.ServerPort = ServerPort.Text;
            Settings.Default.ServerPassword = ServerPassword.Text;
            Settings.Default.MatrixChannel = MatrixChannel.Text;
            Settings.Default.MatrixMachineID = MatrixMachineID.Text;
            Settings.Default.MatrixNodeAddress = MatrixNodeAddress.Text;
            Settings.Default.MatrixPassword = MatrixPassword.Text;
            Settings.Default.MatrixUserName = MatrixUsername.Text;
            Settings.Default.Save();
            ThemeManager.Current.ChangeTheme(Application.Current, $"{Theme.SelectedValue}.Blue");
        }
    }
}