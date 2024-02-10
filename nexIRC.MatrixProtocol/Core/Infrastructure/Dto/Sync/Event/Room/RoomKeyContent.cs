namespace nexIRC.MatrixProtocol.Core.Infrastructure.Dto.Sync.Event.Room {
    using Newtonsoft.Json;
    /// <summary>
    /// Room Key Content
    /// </summary>
    public record RoomKeyContent {
        /// <summary>
        /// Algorithm
        /// </summary>
        [JsonProperty("algorithm")]
        public string Algorithm { get; set; }
        /// <summary>
        /// Room ID
        /// </summary>
        [JsonProperty("room_id")]
        public string Room_id { get; set; }
        /// <summary>
        /// Session Key
        /// </summary>
        [JsonProperty("session_key")]
        public string Session_key { get; set; }
        /// <summary>
        /// Session ID
        /// </summary>
        [JsonProperty("session_id")]
        public string Session_id { get; set; }
    }
}