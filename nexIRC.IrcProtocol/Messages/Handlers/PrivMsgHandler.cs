using nexIRC.IrcProtocol.Ctcp;
using nexIRC.Model;
namespace nexIRC.IrcProtocol.Messages.Handlers {
    /// <summary>
    /// Priv Msg Handler
    /// </summary>
    public class PrivMsgHandler : MessageHandler<PrivMsgMessage> {
        /// <summary>
        /// Handle Async
        /// </summary>
        /// <param name="serverMessage"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public override Task HandleAsync(PrivMsgMessage serverMessage, Client client) {
            if (serverMessage.IsCtcp) {
                client.OnCtcpReceived(new CtcpEventArgs(serverMessage));
                return Task.CompletedTask;
            }
            var user = client.Peers.GetUser(serverMessage.From);
            if (user != null) {
                if (serverMessage.IsChannelMessage) {
                    var channel = client.Channels.GetChannel(serverMessage.To);
                    if (channel != null) {
                        var message = new ChannelMessage(user, channel, serverMessage.Message);
                        Client.DispatcherInvoker?.Invoke(() => channel.Messages.Add(message));
                    }
                } else {
                    var query = client.Queries.GetQuery(user);
                    if (query != null) {
                        var message = new QueryMessageModel(user, serverMessage.Message);
                        Client.DispatcherInvoker?.Invoke(() => query.Messages.Add(message));
                    }
                }
            }
            return Task.CompletedTask;
        }
    }
}