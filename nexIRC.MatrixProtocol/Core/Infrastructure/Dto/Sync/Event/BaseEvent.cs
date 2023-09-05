namespace nexIRC.MatrixProtocol.Core.Infrastructure.Dto.Sync.Event {
    using Newtonsoft.Json.Linq;
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
                Constants.EventType.Encrypted => EventType.Encrypted,
                Constants.EventType.Create => EventType.Create,
                Constants.EventType.Member => EventType.Member,
                Constants.EventType.Message => EventType.Message,
                _ => EventType.Unknown
            };
        }
    }
}