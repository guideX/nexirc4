using System.Text;
using nexIRC.Business.Helper;
using nexIRC.IrcProtocol.Interfaces;
namespace nexIRC.IrcProtocol.Messages
{
    /// <summary>
    /// IRC Message
    /// </summary>
    public abstract class IRCMessage {
        /// <summary>
        /// Constructor
        /// </summary>
        public IRCMessage() {
        }
        /// <summary>
        /// When this IRC message was created
        /// </summary>
        public DateTime CreatedDate { get; } = DateTime.Now;
        /// <summary>
        /// To String
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            return this switch {
                ISplitClientMessage clientMessage => BuildClientMessage(clientMessage),
                IClientMessage clientMessage => BuildClientMessage(clientMessage),
                _ => base.ToString()!,
            };
        }
        /// <summary>
        /// Build Client Message
        /// </summary>
        /// <param name="clientMessage"></param>
        /// <returns></returns>
        private string BuildClientMessage(ISplitClientMessage clientMessage) {
            var sb = new StringBuilder(1024);
            foreach (var tokens in clientMessage.LineSplitTokens) {
                if (tokens.Length == 0) continue;
                AppendTokens(sb, tokens);
                sb.Append(ConstantsHelper.CrLf);
            }
            return sb.ToString().Trim();
        }
        /// <summary>
        /// Build Client Message
        /// </summary>
        /// <param name="clientMessage"></param>
        /// <returns></returns>
        private string BuildClientMessage(IClientMessage clientMessage) {
            try {
                var tokens = clientMessage.Tokens.ToArray();
                if (tokens.Length == 0) return string.Empty;
                var sb = new StringBuilder(256);
                AppendTokens(sb, tokens);
                return sb.ToString().Trim();
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.IrcProtocol.Messages.IRCMessage.BuildClientMessage");
            }
            return "";
        }
        /// <summary>
        /// Append Tokens
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="tokens"></param>
        private static void AppendTokens(StringBuilder sb, string[] tokens) {
            var lastIndex = tokens.Length - 1;
            try {
                for (int i = 0; i < tokens.Length; i++) {
                    if (i == lastIndex && tokens[i].Contains(' ')) sb.Append(':');
                    sb.Append(tokens[i]);
                    if (i < lastIndex) sb.Append(' ');
                    
                }
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.IrcProtocol.Messages.IRCMessage.AppendTokens");
            }
        }
    }
}