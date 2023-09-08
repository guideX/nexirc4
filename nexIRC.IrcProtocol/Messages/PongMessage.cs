using System.Collections.Generic;

namespace nexIRC.IrcProtocol.Messages
{
    public class PongMessage : IRCMessage, IClientMessage
    {
        public string Target { get; }

        public PongMessage(string target)
        {
            Target = target;
        }

        public IEnumerable<string> Tokens => new[] { "PONG", Target };
    }
}
