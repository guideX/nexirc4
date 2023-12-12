using nexIRC.Business.Helper;
namespace nexIRC.IrcProtocol.Messages.Handlers {
    /// <summary>
    /// Part Handler
    /// </summary>
    public class PartHandler : MessageHandler<PartMessage> {
        /// <summary>
        /// Part Handler
        /// </summary>
        public PartHandler() {
            
        }
        /// <summary>
        /// Handle Async
        /// </summary>
        /// <param name="serverMessage"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public override Task HandleAsync(PartMessage serverMessage, Client client) {
            var channel = client.Channels.GetChannel(serverMessage.Channel);
            if (channel != null) channel.RemoveUser(serverMessage.Nick);
            return Task.CompletedTask;
        }
    }
}