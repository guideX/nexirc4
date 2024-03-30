using nexIRC.IrcProtocol.Ctcp;
using nexIRC.IrcProtocol.Interfaces;
namespace nexIRC.IrcProtocol.Messages
{
    /// <summary>
    /// Ctcp Reply Message
    /// </summary>
    public class CtcpReplyMessage : IClientMessage {
        /// <summary>
        /// Target
        /// </summary>
        public string Target { get; }
        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="target"></param>
        /// <param name="text"></param>
        public CtcpReplyMessage(string target, string text) {
            Target = target;
            Message = $":{CtcpCommands.CtcpDelimiter}{text}{CtcpCommands.CtcpDelimiter}";
        }
        /// <summary>
        /// Tokens
        /// </summary>
        public IEnumerable<string> Tokens => new[] { "NOTICE", Target, Message };
        /// <summary>
        /// To String
        /// </summary>
        /// <returns></returns>
        public override string ToString() => string.Join(" ", Tokens);
    }
}