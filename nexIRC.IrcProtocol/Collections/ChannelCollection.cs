using nexIRC.Business.Helper;
using System.Collections.ObjectModel;
namespace nexIRC.IrcProtocol {
    /// <summary>
    /// Channel Collection
    /// </summary>
    public class ChannelCollection : ObservableCollection<Channel> {
        /// <summary>
        /// AppPath
        /// </summary>
        private string _appPath;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="appPath"></param>
        public ChannelCollection(string appPath) {
            _appPath = appPath;
        }
        /// <summary>
        /// Get Channel
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Channel? GetChannel(string name) {
            try {
                var channel = Items.FirstOrDefault(c => c.Name == name);
                if (channel is null) {
                    channel = new Channel(name);
                    Client.DispatcherInvoker?.Invoke(() => Add(channel));
                }
                return channel;
            } catch(Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.IrcProtocol.ChannelCollection", _appPath);
                return null;
            }
        }
    }
}