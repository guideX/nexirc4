namespace nexIRC.IrcProtocol.Messages.Handlers {
    /// <summary>
    /// Nick Handler
    /// </summary>
    public class NickHandler : MessageHandler<NickMessage> {
        /// <summary>
        /// Handle Async
        /// </summary>
        /// <param name="serverMessage"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public override Task HandleAsync(NickMessage serverMessage, Client client) {
            var user = client.Peers.GetUser(serverMessage.OldNick);
            if (user != null) user.Nick = serverMessage.NewNick;
            return Task.CompletedTask;
        }
    }
}