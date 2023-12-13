using nexIRC.Business.Helper;
using nexIRC.IrcProtocol;
using nexIRC.Model;
using System;
namespace nexIRC.Models {
    /// <summary>
    /// Message
    /// </summary>
    public class Message {
        /// <summary>
        /// From
        /// </summary>
        public string From { get; }
        /// <summary>
        /// Text
        /// </summary>
        public string Text { get; }
        /// <summary>
        /// Timestamp
        /// </summary>
        public System.DateTime Timestamp { get; }
        /// <summary>
        /// Is Sent by Client
        /// </summary>
        public bool IsSentByClient { get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="from"></param>
        /// <param name="text"></param>
        /// <param name="timestamp"></param>
        /// <param name="isSentByClient"></param>
        private Message(string from, string text, System.DateTime timestamp, bool isSentByClient) {
            try {
                From = from;
                Text = text;
                Timestamp = timestamp;
                IsSentByClient = isSentByClient;
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.Messages.Message");
            }
        }
        /// <summary>
        /// Recieved
        /// </summary>
        /// <param name="queryMessage"></param>
        /// <returns></returns>
        public static Message Received(QueryMessageModel queryMessage) => new(queryMessage.User.Nick, queryMessage.Text, queryMessage.Timestamp, isSentByClient: false);
        /// <summary>
        /// Sent
        /// </summary>
        /// <param name="queryMessage"></param>
        /// <returns></returns>
        public static Message Sent(QueryMessageModel queryMessage) => new(queryMessage.User.Nick, queryMessage.Text, queryMessage.Timestamp, isSentByClient: true);
        /// <summary>
        /// Received
        /// </summary>
        /// <param name="channelMessage"></param>
        /// <returns></returns>
        public static Message Received(ChannelMessage channelMessage) => new(channelMessage.User.Nick, channelMessage.Text, channelMessage.Timestamp, isSentByClient: false);
        /// <summary>
        /// Sent
        /// </summary>
        /// <param name="channelMessage"></param>
        /// <returns></returns>
        public static Message Sent(ChannelMessage channelMessage) => new(channelMessage.User.Nick, channelMessage.Text, channelMessage.Timestamp, isSentByClient: true);
        /// <summary>
        /// Received
        /// </summary>
        /// <param name="serverMessage"></param>
        /// <returns></returns>
        public static Message Received(ServerMessageModel serverMessage) => new(string.Empty, serverMessage.Text, serverMessage.Timestamp, isSentByClient: false);
    }
}