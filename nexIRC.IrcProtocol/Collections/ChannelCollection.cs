using System.Collections.ObjectModel;
namespace nexIRC.IrcProtocol {
    /// <summary>
    /// Channel Collection
    /// </summary>
    public class ChannelCollection : ObservableCollection<Channel> {
        /// <summary>
        /// Get Channel
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Channel GetChannel(string name) {
            var channel = Items.FirstOrDefault(c => c.Name == name);
            if (channel is null) {
                channel = new Channel(name);
                Client.DispatcherInvoker?.Invoke(() => Add(channel));
            }
            return channel;
        }
    }
}