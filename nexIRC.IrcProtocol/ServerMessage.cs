using System;

namespace nexIRC.IrcProtocol
{
    public class ServerMessage
    {
        public string Text { get; }
        public DateTime Timestamp { get; }

        public ServerMessage(string text)
        {
            Text = text;
            Timestamp = DateTime.Now;
        }
    }
}
