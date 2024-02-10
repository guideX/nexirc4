using ControlzEx.Theming;
using MahApps.Metro.Controls;
using nexIRC.Business.Helper;
using nexIRC.Properties;
using nexIRC.ViewModels;
using System;
using System.Windows;
namespace nexIRC.Views {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow {
        /// <summary>
        /// Main View Model
        /// </summary>
        private readonly MainViewModel _mainViewModel;
        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindow() {
            InitializeComponent();
            _mainViewModel = new MainViewModel(ShowSettingsWindowDialog, ShowAboutWindowDialog);
            DataContext = _mainViewModel;
            ContentRendered += MainWindow_ContentRendered;
            ThemeManager.Current.ChangeTheme(Application.Current, $"{Settings.Default.Theme}.Blue");
        }
        /// <summary>
        /// Content Rendered
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_ContentRendered(object sender, EventArgs e) {
            try {
                if (!Settings.Default.AutoReconnect) ShowSettingsWindowDialog();
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.Views.MainWindow_ContentRendered");
            }
        }
        /// <summary>
        /// Show Settings Window Dialog
        /// </summary>
        private void ShowSettingsWindowDialog() {
            try {
                new SettingsWindow(this).ShowDialog();
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.Views.ShowSettingsWindowDialog");
            }
        }
        /// <summary>
        /// Show About Window Dialog
        /// </summary>
        private void ShowAboutWindowDialog() {
            try {
                new AboutWindow(this).ShowDialog();
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.Views.ShowAboutWindowDialog");
            }
        }
    }
}