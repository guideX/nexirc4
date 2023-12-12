using nexIRC.Model.Matrix.Events;
namespace nexIRC.MatrixProtocol.Core.Domain.RoomEvent {
    using nexIRC.Business.Helper;
    using nexIRC.MatrixProtocol.Core.Infrastructure.Dto.Sync.Event;
    using nexIRC.MatrixProtocol.Core.Infrastructure.Dto.Sync.Event.Room;
    /// <summary>
    /// Encrypted Event
    /// </summary>
    /// <param name="RoomId"></param>
    /// <param name="SenderUserId"></param>
    /// <param name="CipherText"></param>
    /// <param name="Algorithm"></param>
    public record EncryptedEvent(string roomId, string senderUserId, string cipherText, string algorithm, string senderKey, string SenderSessionID) : BaseRoomEvent(roomId, senderUserId) {
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
                try {
                    var encryptedModel = Newtonsoft.Json.JsonConvert.DeserializeObject<EncryptedEventModel>(roomEvent.Content.ToString());
                    if (roomEvent.EventType == EventType.Encrypted && encryptedModel != null && encryptedModel.ciphertext != null && encryptedModel.algorithm != null && encryptedModel.sender_key != null && encryptedModel.session_id != null) {
                        encryptedEvent = new EncryptedEvent(roomId, roomEvent.SenderUserId, encryptedModel.ciphertext, encryptedModel.algorithm, encryptedModel.sender_key, encryptedModel.session_id);
                        return true;
                    }
                } catch (Exception ex) {
                    ExceptionHelper.HandleException(ex, "nexIRC.MatrixProtocol.Core.Domain.RoomEvent.EncryptedEvent");
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
                try {
                    var encryptedModel = Newtonsoft.Json.JsonConvert.DeserializeObject<EncryptedEventModel>(roomStrippedState.Content.ToString());
                    if (roomStrippedState.EventType == EventType.Encrypted && encryptedModel?.ciphertext != null && encryptedModel.algorithm != null && encryptedModel.sender_key != null && encryptedModel.session_id != null) {
                        encryptedEvent = new EncryptedEvent(roomId, roomStrippedState.SenderUserId, encryptedModel.ciphertext, encryptedModel.algorithm, encryptedModel.sender_key, encryptedModel.session_id);
                        return true;
                    }
                } catch (Exception ex) {
                    ExceptionHelper.HandleException(ex, "nexIRC.MatrixProtocol.Core.Domain.RoomEvent.TryCreateFromStrippedState");
                }
                encryptedEvent = new EncryptedEvent(string.Empty, string.Empty, "", "", "", "");
                return false;
            }
        }
    }
}