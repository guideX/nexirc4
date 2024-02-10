using nexIRC.Business.Helper;
using nexIRC.IrcProtocol.Messages;
using nexIRC.Model;
using System.Reflection;
namespace nexIRC.IrcProtocol.Ctcp {
    /// <summary>
    /// Ctcp Commands
    /// </summary>
    public static class CtcpCommands {
        /// <summary>
        /// Ctcp Delimiter
        /// </summary>
        internal const string CtcpDelimiter = "\x01";
        /// <summary>
        /// Action
        /// </summary>
        public const string ACTION = nameof(ACTION);
        /// <summary>
        /// Client Info
        /// </summary>
        public const string CLIENTINFO = nameof(CLIENTINFO);
        /// <summary>
        /// Err Msg
        /// </summary>
        public const string ERRMSG = nameof(ERRMSG);
        /// <summary>
        /// Ping
        /// </summary>
        public const string PING = nameof(PING);
        /// <summary>
        /// Time
        /// </summary>
        public const string TIME = nameof(TIME);
        /// <summary>
        /// Version
        /// </summary>
        public const string VERSION = nameof(VERSION);
        /// <summary>
        /// Handle Ctcp
        /// </summary>
        /// <param name="client"></param>
        /// <param name="ctcp"></param>
        /// <returns></returns>
        internal static Task HandleCtcp(Client client, CtcpEventArgs ctcp) {
            try {
                switch (ctcp.CtcpCommand.ToUpper()) {
                    case ACTION:
                        var channel = client.Channels.Where(c => c.Name == ctcp.To).FirstOrDefault();
                        if (channel != null) channel.Messages.Add(new ChannelMessage(new UserModel(ctcp.From), channel, ctcp.From + ctcp.Message.Remove(0, 7)));
                        break;
                    case ERRMSG:
                        break;
                    case CLIENTINFO:
                        return ClientInfoReply(client, ctcp.From);
                    case PING:
                        return PingReply(client, ctcp.From, ctcp.CtcpMessage);
                    case TIME:
                        return TimeReply(client, ctcp.From);
                    case VERSION:
                        return VersionReply(client, ctcp.From);
                }
                return Task.CompletedTask;
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "HandleCtcp");
                throw;
            }
        }
        /// <summary>
        /// Client Info Reply
        /// </summary>
        /// <param name="client"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        private static Task ClientInfoReply(Client client, string target) {
            return client.SendAsync(new CtcpReplyMessage(target, $"{CLIENTINFO} {ACTION} {CLIENTINFO} {PING} {TIME} {VERSION}"));
        }
        /// <summary>
        /// Ping Reply
        /// </summary>
        /// <param name="client"></param>
        /// <param name="target"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private static Task PingReply(Client client, string target, string message) {
            return client.SendAsync(new CtcpReplyMessage(target, $"{PING} {message}"));
        }
        /// <summary>
        /// Time Reply
        /// </summary>
        /// <param name="client"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        private static Task TimeReply(Client client, string target) {
            return client.SendAsync(new CtcpReplyMessage(target, $"{TIME} {DateTime.Now:F}"));
        }
        /// <summary>
        /// Version Reply
        /// </summary>
        /// <param name="client"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        private static Task VersionReply(Client client, string target) {
            var version = typeof(Client).Assembly
                .GetCustomAttribute<AssemblyFileVersionAttribute>()!
                .Version;
            return client.SendAsync(new CtcpReplyMessage(target, $"{VERSION} nexIRC v{version})"));
        }
    }
}