using System.Collections.Generic;

namespace nexIRC.IrcProtocol.Messages
{
    public class TopicMessage : IRCMessage, IServerMessage, IClientMessage
    {
        public string Channel { get; }
        public string Topic { get; }

        public TopicMessage(ParsedIRCMessage parsedMessage)
        {
            Channel = parsedMessage.Parameters[0];
            Topic = parsedMessage.Trailing;
        }

        public TopicMessage(string channel, string topic)
        {
            Channel = channel;
            Topic = topic;
        }

        public IEnumerable<string> Tokens => new[] { "TOPIC", Channel, Topic };
    }
}
