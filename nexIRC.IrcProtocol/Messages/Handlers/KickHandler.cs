namespace nexIRC.IrcProtocol.Messages.Handlers {
    /// <summary>
    /// Kick Handler
    /// </summary>
    public class KickHandler : MessageHandler<KickMessage> {
        /// <summary>
        /// Handle Async
        /// </summary>
        /// <param name="serverMessage"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public override Task HandleAsync(KickMessage serverMessage, Client client) {
            var channel = client.Channels.GetChannel(serverMessage.Channel);
            if (channel != null) {
                if (string.Equals(serverMessage.Nick, client.User.Nick, StringComparison.InvariantCultureIgnoreCase)) {
                    client.Channels.Remove(channel);
                } else {
                    channel.RemoveUser(serverMessage.Nick);
                }
            }
            return Task.CompletedTask;
        }
    }
}