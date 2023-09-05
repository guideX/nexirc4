using System.Collections.Generic;

namespace nexIRC.IrcProtocol.Messages
{
    public class PassMessage : IRCMessage, IClientMessage
    {
        private readonly string password;

        public PassMessage(string password)
        {
            this.password = password;
        }

        public IEnumerable<string> Tokens => new[] { "PASS", password };
    }
}
