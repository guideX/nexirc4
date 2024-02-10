using nexIRC.Business.Helper;
namespace nexIRC.IrcProtocol.Messages {
    /// <summary>
    /// Rpl Nam Reply Message
    /// </summary>
    public class RplNamReplyMessage : IRCMessage, IServerMessage {
        /// <summary>
        /// Channel
        /// </summary>
        public string Channel { get; }
        /// <summary>
        /// Nick
        /// </summary>
        public Dictionary<string, string> Nicks { get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parsedMessage"></param>
        public RplNamReplyMessage(ParsedIRCMessage parsedMessage) {
            Nicks = new Dictionary<string, string>();
            Channel = parsedMessage.Parameters![2];
            try {
                foreach (var nick in parsedMessage.Trailing.Split(' ')) {
                    if (!string.IsNullOrWhiteSpace(nick) && nexIRC.IrcProtocol.Channel.UserStatuses.Contains(nick[0]))
                        Nicks.Add(nick.Substring(1), nick.Substring(0, 1));
                    else
                        Nicks.Add(nick, string.Empty);
                }
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.IrcProtocol.Messages.RplNamReplyMessage.Constructor");
            }
        }
    }
}