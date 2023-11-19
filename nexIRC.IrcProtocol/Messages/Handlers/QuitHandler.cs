using nexIRC.Business.Helper;
namespace nexIRC.IrcProtocol.Messages.Handlers {
    /// <summary>
    /// Quit Handler
    /// </summary>
    public class QuitHandler : MessageHandler<QuitMessage> {
        /// <summary>
        /// App Path
        /// </summary>
        private string _appPath;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="appPath"></param>
        public QuitHandler(string appPath) { 
            _appPath = appPath;
        }
        /// <summary>
        /// Handle Async
        /// </summary>
        /// <param name="serverMessage"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public override Task HandleAsync(QuitMessage serverMessage, Client client) {
            try {
                foreach (var channel in client.Channels) channel.RemoveUser(serverMessage.Nick);
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.IrcProtocol.Messages.Handlers.HandleAsync", _appPath);
            }
            return Task.CompletedTask;
        }
    }
}