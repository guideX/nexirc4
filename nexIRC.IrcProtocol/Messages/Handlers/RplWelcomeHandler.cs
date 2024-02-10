namespace nexIRC.IrcProtocol.Messages.Handlers {
    /// <summary>
    /// Rpl Welcome Handler
    /// </summary>
    [Command("001")]
    public class RplWelcomeHandler : MessageHandler<RplWelcomeMessage> {
        /// <summary>
        /// Handle Async
        /// </summary>
        /// <param name="serverMessage"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public override Task HandleAsync(RplWelcomeMessage serverMessage, Client client) {
            client.OnRegistrationCompleted();
            return Task.CompletedTask;
        }
    }
}