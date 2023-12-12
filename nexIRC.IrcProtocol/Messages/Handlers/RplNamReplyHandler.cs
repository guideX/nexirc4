using nexIRC.Business.Helper;
namespace nexIRC.IrcProtocol.Messages.Handlers {
    /// <summary>
    /// Rpl Nam Reply Handler
    /// </summary>
    [Command("353")]
    public class RplNamReplyHandler : MessageHandler<RplNamReplyMessage> {
        /// <summary>
        /// App Path
        /// </summary>
        //private string _appPath;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="appPath"></param>
        public RplNamReplyHandler(/*string appPath*/) { 
            //_appPath = appPath;
        }
        /// <summary>
        /// Handle Async
        /// </summary>
        /// <param name="serverMessage"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public override Task HandleAsync(RplNamReplyMessage serverMessage, Client client) {
            //try {
                var channel = client.Channels.GetChannel(serverMessage.Channel);
                if (channel != null) {
                    foreach (var nick in serverMessage.Nicks) {
                        var user = client.Peers.GetUser(nick.Key);
                        if (user != null && channel.GetUser(nick.Key) is null)
                            channel.AddUser(user, nick.Value);
                    }
                }
            //} catch (Exception ex) {
                //ExceptionHelper.HandleException(ex, "nexIRC.IrcProtocol.Messages.Handlers.HandleAsync", _appPath);
            //}
            return Task.CompletedTask;
        }
    }
}