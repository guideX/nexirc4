using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace nexIRC.Model {
    /// <summary>
    /// User Model
    /// </summary>
    public class UserModel : INotifyPropertyChanged {
        /// <summary>
        /// Property Changed Event Handler
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;
        /// <summary>
        /// Nick
        /// </summary>
        private string _nick;
        /// <summary>
        /// Real Name
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nick"></param>
        /// <exception cref="ArgumentException"></exception>
        public UserModel(string nick) {
            if (string.IsNullOrWhiteSpace(nick)) throw new ArgumentException("Nick should not be empty.", nameof(nick));
            _nick = nick;
            RealName = "";
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nick"></param>
        /// <param name="realName"></param>
        public UserModel(string nick, string realName) {
            _nick = nick;
            RealName = realName;
        }
        /// <summary>
        /// On Property Changed
        /// </summary>
        /// <param name="propertyName"></param>
        private void OnPropertyChanged([CallerMemberName] string propertyName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        /// <summary>
        /// Nick
        /// </summary>
        public string Nick {
            get {
                return _nick;
            }
            set {
                _nick = value;
                OnPropertyChanged();
            }
        }
    }
}