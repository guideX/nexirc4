using nexIRC.Business.Helper;
namespace nexIRC.IrcProtocol.Messages.Handlers {
    /// <summary>
    /// Part Handler
    /// </summary>
    public class PartHandler : MessageHandler<PartMessage> {
        /// <summary>
        /// App Path
        /// </summary>
        private string _appPath;
        /// <summary>
        /// Part Handler
        /// </summary>
        public PartHandler(string appPath) {
            _appPath = appPath;
        }
        /// <summary>
        /// Handle Async
        /// </summary>
        /// <param name="serverMessage"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public override Task HandleAsync(PartMessage serverMessage, Client client) {
            try {
                var channel = client.Channels.GetChannel(serverMessage.Channel);
                if (channel != null) channel.RemoveUser(serverMessage.Nick);
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.IrcProtocol.Messages.Handlers.PartHandler.HandleAsync", _appPath);
            }
            return Task.CompletedTask;
        }
    }
}