using System.Collections.Generic;

namespace nexIRC.IrcProtocol.Messages
{
    public interface IClientMessage
    {
        IEnumerable<string> Tokens { get; }
    }
}
