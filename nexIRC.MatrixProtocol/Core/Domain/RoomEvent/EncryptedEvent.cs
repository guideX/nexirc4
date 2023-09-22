namespace nexIRC.MatrixProtocol.Core.Domain.RoomEvent {
    using Infrastructure.Dto.Sync.Event;
    using Infrastructure.Dto.Sync.Event.Room;
    using Infrastructure.Dto.Sync.Event.Room.Messaging;
    using nexIRC.MatrixProtocol.Core.Domain.Models;
    /// <summary>
    /// Encrypted Event
    /// </summary>
    /// <param name="RoomId"></param>
    /// <param name="SenderUserId"></param>
    /// <param name="CipherText"></param>
    /// <param name="Algorithm"></param>
    public record EncryptedEvent(string RoomId, string SenderUserId, string CipherText, string Algorithm, string SenderKey, string SenderSessionID) : BaseRoomEvent(RoomId, SenderUserId) {
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
                    encryptedEvent = new EncryptedEvent(roomId, roomEvent.SenderUserId, encryptedModel.ciphertext, encryptedModel.algorithm, encryptedModel.sender_key, encryptedModel.session_id);
                    return true;
                }
                encryptedEvent = new EncryptedEvent(string.Empty, string.Empty, "", "", "", "");
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
                    encryptedEvent = new EncryptedEvent(roomId, roomStrippedState.SenderUserId, encryptedModel.ciphertext, encryptedModel.algorithm, encryptedModel.sender_key, encryptedModel.session_id);
                    return true;
                }
                encryptedEvent = new EncryptedEvent(string.Empty, string.Empty, "", "", "", "");
                return false;
            }
        }
    }
}