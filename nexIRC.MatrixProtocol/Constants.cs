namespace nexIRC.MatrixProtocol {
    /// <summary>
    /// Constants
    /// </summary>
    public static class Constants {
        /// <summary>
        /// Matrix
        /// </summary>
        public const string Matrix = nameof(Matrix);
        /// <summary>
        /// First Sync Timeout
        /// </summary>
        public const int FirstSyncTimout = 0;
        /// <summary>
        /// Later Sync Timeout
        /// </summary>
        public const int LaterSyncTimout = 30000;
        /// <summary>
        /// Event Type
        /// </summary>
        public class EventType {
            /// <summary>
            /// Create
            /// </summary>
            public const string Create = "m.room.create";
            /// <summary>
            /// Member
            /// </summary>
            public const string Member = "m.room.member";
            /// <summary>
            /// Message
            /// </summary>
            public const string Message = "m.room.message";
            /// <summary>
            /// Encrypted
            /// </summary>
            public const string Encrypted = "m.room.encrypted";
            /// <summary>
            /// Encryption
            /// </summary>
            public const string Encryption = "m.room.encryption";
            /// <summary>
            /// Room Key
            /// </summary>
            public const string RoomKey = "m.room.key";
        }
        /// <summary>
        /// Message Type
        /// </summary>
        public class MessageType {
            /// <summary>
            /// Text
            /// </summary>
            public const string Text = "m.text";
        }
    }
}