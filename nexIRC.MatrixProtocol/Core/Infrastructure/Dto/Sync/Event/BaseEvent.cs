namespace nexIRC.MatrixProtocol.Core.Infrastructure.Dto.Sync.Event {
    using Microsoft.VisualBasic;
    using Newtonsoft.Json.Linq;
    using nexIRC.MatrixProtocol;
    /// <summary>
    /// Base Event
    /// </summary>
    public record BaseEvent {
        /// <summary>
        /// Content
        /// </summary>
        public JObject Content { get; init; }
        /// <summary>
        /// Event Type
        /// </summary>
        public EventType EventType { get; private set; }
        /// <summary>
        /// Type
        /// </summary>
        public string Type {
            set => EventType = value switch {
                nexIRC.MatrixProtocol.Constants.EventType.RoomKey => EventType.RoomKey,
                nexIRC.MatrixProtocol.Constants.EventType.Encryption => EventType.Encryption,
                nexIRC.MatrixProtocol.Constants.EventType.Encrypted => EventType.Encrypted,
                nexIRC.MatrixProtocol.Constants.EventType.Create => EventType.Create,
                nexIRC.MatrixProtocol.Constants.EventType.Member => EventType.Member,
                nexIRC.MatrixProtocol.Constants.EventType.Message => EventType.Message,
                _ => EventType.Unknown
            };
        }
    }
}