/*
namespace nexIRC.Model.Message.Base {
    /// <summary>
    /// Message
    /// </summary>
    public class MessageModel {
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
        private MessageModel(string from, string text, System.DateTime timestamp, bool isSentByClient) {
            From = from;
            Text = text;
            Timestamp = timestamp;
            IsSentByClient = isSentByClient;
        }
        /// <summary>
        /// Recieved
        /// </summary>
        /// <param name="queryMessage"></param>
        /// <returns></returns>
        public static MessageModel Received(QueryMessageModel queryMessage) =>
            new MessageModel(queryMessage.User.Nick, queryMessage.Text, queryMessage.Timestamp, isSentByClient: false);
        /// <summary>
        /// Sent
        /// </summary>
        /// <param name="queryMessage"></param>
        /// <returns></returns>
        public static MessageModel Sent(QueryMessageModel queryMessage) =>
            new MessageModel(queryMessage.User.Nick, queryMessage.Text, queryMessage.Timestamp, isSentByClient: true);
        /// <summary>
        /// Received
        /// </summary>
        /// <param name="channelMessage"></param>
        /// <returns></returns>
        public static MessageModel Received(ChannelMessageModel channelMessage) =>
            new MessageModel(channelMessage.User.Nick, channelMessage.Text, channelMessage.Timestamp, isSentByClient: false);
        /// <summary>
        /// Sent
        /// </summary>
        /// <param name="channelMessage"></param>
        /// <returns></returns>
        public static MessageModel Sent(ChannelMessageModel channelMessage) =>
            new MessageModel(channelMessage.User.Nick, channelMessage.Text, channelMessage.Timestamp, isSentByClient: true);
        /// <summary>
        /// Received
        /// </summary>
        /// <param name="serverMessage"></param>
        /// <returns></returns>
        public static MessageModel Received(ServerMessageModel serverMessage) =>
            new MessageModel(string.Empty, serverMessage.Text, serverMessage.Timestamp, isSentByClient: false);
    }
}*/