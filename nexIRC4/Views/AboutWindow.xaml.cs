using MahApps.Metro.Controls;
using nexIRC.ViewModels;
using System.Windows;
namespace nexIRC.Views {
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public sealed partial class AboutWindow : MetroWindow {
        private AboutWindow() {
            InitializeComponent();
            DataContext = new AboutWindowViewModel(Close);
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parent"></param>
        public AboutWindow(Window parent) : this() {
            Owner = parent;
        }
    }
}