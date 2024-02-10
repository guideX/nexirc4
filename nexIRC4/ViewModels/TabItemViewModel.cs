using nexIRC.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
namespace nexIRC.ViewModels {
    /// <summary>
    /// Tab Item View Model
    /// </summary>
    public abstract class TabItemViewModel : BaseViewModel {
        /// <summary>
        /// Message
        /// </summary>
        public ObservableCollection<Message> Messages { 
            get; 
        } = new ObservableCollection<Message>();
        /// <summary>
        /// Mesage
        /// </summary>
        private string message;
        /// <summary>
        /// Message
        /// </summary>
        public string Message { 
            get => message; 
            set => SetProperty(ref message, value); 
        }
        /// <summary>
        /// Send Message Command
        /// </summary>
        public ICommand SendMessageCommand { get; protected set; }
    }
}