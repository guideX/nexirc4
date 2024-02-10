namespace nexIRC.MatrixProtocol.Core.Infrastructure.Dto.Event {
    using Newtonsoft.Json;
    /// <summary>
    /// Message Event
    /// </summary>
    /// <param name="MessageType"></param>
    /// <param name="Message"></param>
    public record MessageEvent(MessageType MessageType, string Message) {
        [JsonProperty("msgtype")] public MessageType MessageType { get; } = MessageType;
        [JsonProperty("body")] public string Message { get; } = Message;
    }
    /// <summary>
    /// Message Event 2
    /// </summary>
    /// <param name="msgtype"></param>
    /// <param name="body"></param>
    public record MessageEvent2(string msgtype, string body) {
        public string msgtype { get; } = msgtype;
        public string body { get; } = body;
    }
}