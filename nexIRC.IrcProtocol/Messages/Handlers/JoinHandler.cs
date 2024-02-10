namespace nexIRC.IrcProtocol.Messages.Handlers {
    /// <summary>
    /// Join Handler
    /// </summary>
    public class JoinHandler : MessageHandler<JoinMessage> {
        /// <summary>
        /// Join Handler
        /// </summary>
        public JoinHandler() {
        }
        /// <summary>
        /// Handle Async
        /// </summary>
        /// <param name="serverMessage"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public override Task HandleAsync(JoinMessage serverMessage, Client client) {
            var channel = client.Channels.GetChannel(serverMessage.Channel);
            if (serverMessage.Nick != client.User.Nick && channel != null) {
                var user = client.Peers.GetUser(serverMessage.Nick);
                if (user != null) channel.AddUser(user);
            }
            return Task.CompletedTask;
        }
    }
}