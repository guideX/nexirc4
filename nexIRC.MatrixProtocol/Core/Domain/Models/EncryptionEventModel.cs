namespace nexIRC.MatrixProtocol.Core.Domain.Models {
    /// <summary>
    /// Encryption Event Model
    /// </summary>
    public class EncryptionEventModel {
        /// <summary>
        /// Algorithm
        /// </summary>
        public string? algorithm { get; set; }
        /// <summary>
        /// Rotation Period Ms
        /// </summary>
        public int rotation_period_ms { get; set; }
        /// <summary>
        /// Rotation Period Msgs
        /// </summary>
        public int rotation_period_msgs { get; set; }
        /*
        /// <summary>
        /// EventID
        /// </summary>
        public string? event_id { get; set; }
        /// <summary>
        /// Origin Server TS
        /// </summary>
        public long origin_server_ts { get; set; }
        /// <summary>
        /// RoomID
        /// </summary>
        public string? room_id { get; set; }
        /// <summary>
        /// Sender
        /// </summary>
        public string? sender { get; set; }
        /// <summary>
        /// State Key
        /// </summary>
        public string? state_key { get; set; }
        */
    }
}