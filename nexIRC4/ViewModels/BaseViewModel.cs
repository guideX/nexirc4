using nexIRC.Business.Helper;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
namespace nexIRC.ViewModels {
    /// <summary>
    /// Base View Model
    /// </summary>
    public abstract class BaseViewModel : INotifyPropertyChanged {
        /// <summary>
        /// App Path
        /// </summary>
        public string AppPath;
        /// <summary>
        /// Base View Model
        /// </summary>
        public BaseViewModel() {
            AppPath = System.AppDomain.CurrentDomain.BaseDirectory;
        }
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
            try {
                if (!Equals(field, newValue)) {
                    field = newValue;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                    return true;
                }
            } catch (Exception ex) { 
                ExceptionHelper.HandleException(ex, "nexIRC.ViewModels.SetProperty");
            } 
            return false;
        }
        /// <summary>
        /// Property Changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}