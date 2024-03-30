using nexIRC.Business.Helper;
using nexIRC.IrcProtocol.Ctcp;
using nexIRC.Model;
namespace nexIRC.IrcProtocol.Messages {
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
            try {
                var channel = client.Channels.GetChannel(serverMessage.Channel);
                if (serverMessage.Nick != client.User.Nick && channel != null) {
                    var user = client.Peers.GetUser(serverMessage.Nick);
                    if (user != null) channel.AddUser(user);
                }
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "HandleAsync");
            }
            return Task.CompletedTask;
        }
    }
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
            try {
                var channel = client.Channels.GetChannel(serverMessage.Channel);
                if (channel != null) {
                    if (string.Equals(serverMessage.Nick, client.User.Nick, StringComparison.InvariantCultureIgnoreCase)) {
                        client.Channels.Remove(channel);
                    } else {
                        channel.RemoveUser(serverMessage.Nick);
                    }
                }
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "HandleAsync");
            }
            return Task.CompletedTask;
        }
    }
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
            try {
                var user = client.Peers.GetUser(serverMessage.OldNick);
                if (user != null) user.Nick = serverMessage.NewNick;
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.IrcProtocol.Messages.Handlers.HandleAsync");
            }
            return Task.CompletedTask;
        }
    }
    /// <summary>
    /// Part Handler
    /// </summary>
    public class PartHandler : MessageHandler<PartMessage> {
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
                ExceptionHelper.HandleException(ex, "nexIRC.IrcProtocol.Messages.Handlers.HandleAsync");
            }
            return Task.CompletedTask;
        }
    }
    /// <summary>
    /// Ping Handler
    /// </summary>
    public class PingHandler : MessageHandler<PingMessage> {
        /// <summary>
        /// Constructor
        /// </summary>
        public PingHandler() {
        }
        /// <summary>
        /// Handler Async
        /// </summary>
        /// <param name="serverMessage"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public override Task HandleAsync(PingMessage serverMessage, Client client) {
            return client.SendAsync(new PongMessage(serverMessage.Target));
        }
    }
    /// <summary>
    /// Priv Msg Handler
    /// </summary>
    public class PrivMsgHandler : MessageHandler<PrivMsgMessage> {
        /// <summary>
        /// Handle Async
        /// </summary>
        /// <param name="serverMessage"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public override Task HandleAsync(PrivMsgMessage serverMessage, Client client) {
            if (serverMessage.IsCtcp) {
                client.OnCtcpReceived(new CtcpEventArgs(serverMessage));
                return Task.CompletedTask;
            }
            var user = client.Peers.GetUser(serverMessage.From);
            if (user != null) {
                if (serverMessage.IsChannelMessage) {
                    var channel = client.Channels.GetChannel(serverMessage.To);
                    if (channel != null) {
                        var message = new ChannelMessage(user, channel, serverMessage.Message);
                        Client.DispatcherInvoker?.Invoke(() => channel.Messages.Add(message));
                    }
                } else {
                    var query = client.Queries.GetQuery(user);
                    if (query != null) {
                        var message = new QueryMessageModel(user, serverMessage.Message);
                        Client.DispatcherInvoker?.Invoke(() => query.Messages.Add(message));
                    }
                }
            }
            return Task.CompletedTask;
        }
    }
    /// <summary>
    /// Quit Handler
    /// </summary>
    public class QuitHandler : MessageHandler<QuitMessage> {
        /// <summary>
        /// Constructor
        /// </summary>
        public QuitHandler() {
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
                ExceptionHelper.HandleException(ex, "nexIRC.IrcProtocol.Messages.Handlers.HandleAsync");
            }
            return Task.CompletedTask;
        }
    }
    /// <summary>
    /// Rpl Nam Reply Handler
    /// </summary>
    [Command("353")]
    public class RplNamReplyHandler : MessageHandler<RplNamReplyMessage> {
        /// <summary>
        /// Constructor
        /// </summary>
        public RplNamReplyHandler() {
        }
        /// <summary>
        /// Handle Async
        /// </summary>
        /// <param name="serverMessage"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public override Task HandleAsync(RplNamReplyMessage serverMessage, Client client) {
            try {
                var channel = client.Channels.GetChannel(serverMessage.Channel);
                if (channel != null) {
                    foreach (var nick in serverMessage.Nicks) {
                        var user = client.Peers.GetUser(nick.Key);
                        if (user != null && channel.GetUser(nick.Key) is null)
                            channel.AddUser(user, nick.Value);
                    }
                }
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.IrcProtocol.Messages.Handlers.HandleAsync");
            }
            return Task.CompletedTask;
        }
    }
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