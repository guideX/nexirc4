using MvvmHelpers.Commands;
using nexIRC.Business.Helper;
using System;
using System.Diagnostics;
using System.Windows.Input;
namespace nexIRC.ViewModels {
    /// <summary>
    /// About Window View Model
    /// </summary>
    public class AboutWindowViewModel {
        /// <summary>
        /// Close Command
        /// </summary>
        public ICommand CloseCommand { get; }
        /// <summary>
        /// Open Link Command
        /// </summary>
        public ICommand OpenLinkCommand { get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="closeAction"></param>
        public AboutWindowViewModel(Action closeAction) {
            try {
                CloseCommand = new Command(closeAction);
                OpenLinkCommand = new Command(OpenLink);
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.ViewModels.AboutWindowViewModel");
            }
        }
        /// <summary>
        /// Open Link
        /// </summary>
        /// <param name="link"></param>
        private void OpenLink(object link) {
            try {
                if (!(link is Uri uri) || string.IsNullOrWhiteSpace(uri.OriginalString)) {
                    return;
                }
                Process.Start(new ProcessStartInfo(uri.AbsoluteUri) { UseShellExecute = true });
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.ViewModels.AboutWindowViewModel.OpenLink");
            }
        }
    }
}