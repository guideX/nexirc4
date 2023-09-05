namespace nexIRC.MatrixProtocol.Core.Infrastructure.Dto.Sync.Event.Room.Messaging {
    using Newtonsoft.Json;
    /// <summary>
    /// Message Type
    /// </summary>
    public enum MessageType {
        /// <summary>
        /// Text
        /// </summary>
        Text,
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown
    }
    /// <summary>
    /// Message Content
    /// </summary>
    public record MessageContent {
        /// <summary>
        /// Body
        /// </summary>
        public string Body { get; init; }
        /// <summary>
        /// Message Type
        /// </summary>
        public MessageType MessageType { get; private set; }
        /// <summary>
        ///     <b>Required.</b> The type of message, e.g. m.image, m.text
        /// </summary>
        [JsonProperty("msgtype")]
        public string Type {
            set => MessageType = value switch {
                Constants.MessageType.Text => MessageType.Text,
                _ => MessageType.Unknown
            };
        }
        //[JsonProperty("ciphertext")]
        //public string? CipherText {
        //}
    }
}