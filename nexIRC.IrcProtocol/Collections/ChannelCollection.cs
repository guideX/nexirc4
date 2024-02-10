using nexIRC.Business.Helper;
using System.Collections.ObjectModel;
namespace nexIRC.IrcProtocol {
    /// <summary>
    /// Channel Collection
    /// </summary>
    public class ChannelCollection : ObservableCollection<Channel> {
        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelCollection() {
        }
        /// <summary>
        /// Get Channel
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Channel GetChannel(string name) {
            try {
                var channel = Items.FirstOrDefault(c => c.Name == name);
                if (channel is null) {
                    channel = new Channel(name);
                    Client.DispatcherInvoker?.Invoke(() => Add(channel));
                }
                return channel;
            } catch(Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.IrcProtocol.ChannelCollection");
                throw;
            }
        }
    }
}