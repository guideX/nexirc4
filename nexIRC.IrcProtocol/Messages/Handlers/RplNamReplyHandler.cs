using nexIRC.Business.Helper;
namespace nexIRC.IrcProtocol.Messages.Handlers {
    /// <summary>
    /// Rpl Nam Reply Handler
    /// </summary>
    [Command("353")]
    public class RplNamReplyHandler : MessageHandler<RplNamReplyMessage> {
        /// <summary>
        /// Constructor
        /// </summary>
        public RplNamReplyHandler() {
        }
        /// <summary>
        /// Handle Async
        /// </summary>
        /// <param name="serverMessage"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public override Task HandleAsync(RplNamReplyMessage serverMessage, Client client) {
            try {
                var channel = client.Channels.GetChannel(serverMessage.Channel);
                if (channel != null) {
                    foreach (var nick in serverMessage.Nicks) {
                        var user = client.Peers.GetUser(nick.Key);
                        if (user != null && channel.GetUser(nick.Key) is null)
                            channel.AddUser(user, nick.Value);
                    }
                }
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.IrcProtocol.Messages.Handlers.HandleAsync");
            }
            return Task.CompletedTask;
        }
    }
}