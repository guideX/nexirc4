using MahApps.Metro.Controls;
using nexIRC.Business.Helper;
using nexIRC.ViewModels;
using System;
using System.Windows;
namespace nexIRC.Views {
    /// <summary>
    /// About Window
    /// </summary>
    public sealed partial class AboutWindow : MetroWindow {
        /// <summary>
        /// Constructor
        /// </summary>
        private AboutWindow() {
            InitializeComponent();
            DataContext = new AboutWindowViewModel(Close);
            try {
                var msg = "a";
                var i = int.Parse(msg);
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "AboutWindow");
            }
            
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