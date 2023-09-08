namespace nexIRC.IrcProtocol.Messages
{
    public class PingMessage : IRCMessage, IServerMessage
    {
        public string Target { get; }

        public PingMessage(ParsedIRCMessage parsedMessage)
        {
            Target = parsedMessage.Trailing;
        }
    }
}
