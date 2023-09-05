namespace nexIRC.MatrixProtocol.Core.Domain.RoomEvent {
    using Infrastructure.Dto.Sync.Event;
    using Infrastructure.Dto.Sync.Event.Room;
    using Infrastructure.Dto.Sync.Event.Room.Messaging;
    /// <summary>
    /// Encrypted Event Model
    /// </summary>
    public class EncryptedEventModel { 
        /// <summary>
        /// Algorythm
        /// </summary>
        public string algorithm { get; set; }
        /// <summary>
        /// Cipher Text
        /// </summary>
        public string ciphertext { get; set; }
        /// <summary>
        /// Device ID
        /// </summary>
        public string device_id { get; set; }
        /// <summary>
        /// Sender Key
        /// </summary>
        public string sender_key { get; set; }
        /// <summary>
        /// Session ID
        /// </summary>
        public string session_id { get; set; }
    }
    /// <summary>
    /// Encrypted Event
    /// </summary>
    /// <param name="RoomId"></param>
    /// <param name="SenderUserId"></param>
    /// <param name="Message"></param>
    public record EncryptedEvent(string RoomId, string SenderUserId, string CipherText) : BaseRoomEvent(RoomId, SenderUserId) {
        /// <summary>
        /// Factory
        /// </summary>
        public static class Factory {
            /// <summary>
            /// Try Create From
            /// </summary>
            /// <param name="roomEvent"></param>
            /// <param name="roomId"></param>
            /// <param name="encryptedEvent"></param>
            /// <returns></returns>
            public static bool TryCreateFrom(RoomEvent roomEvent, string roomId, out EncryptedEvent encryptedEvent) {
                if (roomEvent.EventType == EventType.Encrypted) {
                    var encryptedModel = Newtonsoft.Json.JsonConvert.DeserializeObject<EncryptedEventModel>(roomEvent.Content.ToString());
                    encryptedEvent = new EncryptedEvent(roomId, roomEvent.SenderUserId, encryptedModel.ciphertext);
                    return true;
                }
                encryptedEvent = new EncryptedEvent(string.Empty, string.Empty, "");
                return false;
            }
            /// <summary>
            /// Try Create From Stripped State
            /// </summary>
            /// <param name="roomStrippedState"></param>
            /// <param name="roomId"></param>
            /// <param name="encryptedEvent"></param>
            /// <returns></returns>
            public static bool TryCreateFromStrippedState(RoomStrippedState roomStrippedState, string roomId, out EncryptedEvent encryptedEvent) {
                if (roomStrippedState.EventType == EventType.Encrypted) {
                    var encryptedModel = Newtonsoft.Json.JsonConvert.DeserializeObject<EncryptedEventModel>(roomStrippedState.Content.ToString());
                    encryptedEvent = new EncryptedEvent(roomId, roomStrippedState.SenderUserId, encryptedModel.ciphertext);
                    return true;
                }
                encryptedEvent = new EncryptedEvent(string.Empty, string.Empty, "");
                return false;
            }
        }
    }
}