using nexIRC.Business.Helper;

namespace nexIRC.IrcProtocol.Messages.Handlers {
    /// <summary>
    /// Join Handler
    /// </summary>
    public class JoinHandler : MessageHandler<JoinMessage> {
        /// <summary>
        /// App Path
        /// </summary>
        private string _appPath;
        /// <summary>
        /// Join Handler
        /// </summary>
        /// <param name="appPath"></param>
        public JoinHandler(string appPath) {
            _appPath = appPath;
        }
        /// <summary>
        /// Handle Async
        /// </summary>
        /// <param name="serverMessage"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public override Task HandleAsync(JoinMessage serverMessage, Client client) {
            try {
                var channel = client.Channels.GetChannel(serverMessage.Channel);
                if (serverMessage.Nick != client.User.Nick && channel != null) {
                    var user = client.Peers.GetUser(serverMessage.Nick);
                    if (user != null) channel.AddUser(user);
                }
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.IrcProtocol.Messages.Handlers.JoinHandler", _appPath);
            }
            return Task.CompletedTask;
        }
    }
}