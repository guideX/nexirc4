namespace nexIRC.MatrixProtocol.Core.Infrastructure.Dto.Event {
    /// <summary>
    /// Event Response
    /// </summary>
    /// <param name="EventId"></param>
    public record EventResponse(string? EventId) {
        /// <summary>
        /// Event ID
        /// </summary>
        public string? EventId { get; } = EventId;
    }
}