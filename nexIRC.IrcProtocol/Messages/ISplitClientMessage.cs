using System.Collections.Generic;

namespace nexIRC.IrcProtocol.Messages
{
    public interface ISplitClientMessage
    {
        IEnumerable<string[]> LineSplitTokens { get; }
    }
}
