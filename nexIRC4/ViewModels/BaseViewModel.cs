using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
namespace nexIRC.ViewModels {
    /// <summary>
    /// Base View Model
    /// </summary>
    public abstract class BaseViewModel : INotifyPropertyChanged {
        /// <summary>
        /// App
        /// </summary>
        public App App => (App)Application.Current;
        /// <summary>
        /// Set Property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="newValue"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null) {
            if (!Equals(field, newValue)) {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }
            return false;
        }
        /// <summary>
        /// Property Changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}