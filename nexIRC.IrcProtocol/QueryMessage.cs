/*
using nexIRC.Model;
using System;

namespace nexIRC.IrcProtocol
{
    /// <summary>
    /// Represents a query message (private message)
    /// </summary>
    public class QueryMessage : EventArgs
    {
        public UserModel User { get; }
        public string Text { get; }
        public DateTime Timestamp { get; }

        public QueryMessage(UserModel user, string text)
        {
            User = user;
            Text = text;
            Timestamp = DateTime.Now;
        }
    }
}
