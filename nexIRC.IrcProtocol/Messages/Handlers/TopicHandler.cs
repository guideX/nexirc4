namespace nexIRC.IrcProtocol.Messages.Handlers {
    /// <summary>
    /// Topic Handler
    /// </summary>
    public class TopicHandler : MessageHandler<TopicMessage> {
        /// <summary>
        /// Handle Async
        /// </summary>
        /// <param name="serverMessage"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public override Task HandleAsync(TopicMessage serverMessage, Client client) {
            var channel = client.Channels.GetChannel(serverMessage.Channel);
            channel?.SetTopic(serverMessage.Topic);
            return Task.CompletedTask;
        }
    }
}