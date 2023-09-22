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
        public string algorithm { get; set; }
        /// <summary>
        /// Room ID
        /// </summary>
        [JsonProperty("room_id")]
        public string room_id { get; set; }
        /// <summary>
        /// Session Key
        /// </summary>
        [JsonProperty("session_key")]
        public string session_key { get; set; }
        /// <summary>
        /// Session ID
        /// </summary>
        [JsonProperty("session_id")]
        public string session_id { get; set; }
    }
}