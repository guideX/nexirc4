using nexIRC.Business.Helper;
using nexIRC.MatrixProtocol.Core.Domain.Models;
namespace nexIRC.MatrixProtocol.Core.Domain.RoomEvent {
    using Infrastructure.Dto.Sync.Event;
    using Infrastructure.Dto.Sync.Event.Room;
    /// <summary>
    /// Encryption Event
    /// </summary>
    /// <param name="roomID"></param>
    /// <param name="sender"></param>
    /// <param name="stateKey"></param>
    public record EncryptionEvent(string roomID, string sender, string stateKey) : BaseRoomEvent(roomID, sender) {
        /// <summary>
        /// Factory
        /// </summary>
        public static class Factory {
            /// <summary>
            /// Try Create From
            /// </summary>
            /// <param name="roomEvent"></param>
            /// <param name="roomID"></param>
            /// <param name="encryptionEvent"></param>
            /// <returns></returns>
            public static bool TryCreateFrom(RoomEvent roomEvent, string roomID, string stateKey, out EncryptionEvent encryptionEvent) {
                try {
                    var encryptionModel = 
                        Newtonsoft.Json.JsonConvert.DeserializeObject<EncryptionEventModel>(
                            roomEvent.Content.ToString());
                    if (roomEvent.EventType == EventType.Encryption) {
                        encryptionEvent = new EncryptionEvent(roomID, roomEvent.SenderUserId, stateKey);
                        return true;
                    }
                } catch (Exception ex) {
                    ExceptionHelper.HandleException(ex, "nexIRC.MatrixProtocol.Core.Domain.Factory.TryCreateFrom");
                }
                encryptionEvent = new EncryptionEvent("", "", "");
                return false;
            }
            /// <summary>
            /// Try Create From Stripped State
            /// </summary>
            /// <param name="roomStrippedState"></param>
            /// <param name="roomId"></param>
            /// <param name="encryptionEvent"></param>
            /// <returns></returns>
            public static bool TryCreateFromStrippedState(RoomStrippedState roomStrippedState, string roomID, string stateKey, out EncryptionEvent encryptionEvent) {
                try {
                    var encryptionModel = Newtonsoft.Json.JsonConvert.DeserializeObject<EncryptionEventModel>(roomStrippedState.Content.ToString());
                    if (roomStrippedState.EventType == EventType.Encryption) {
                        encryptionEvent = new EncryptionEvent(roomID, roomStrippedState.SenderUserId, stateKey);
                        return true;
                    }
                } catch (Exception ex) {
                    ExceptionHelper.HandleException(ex, "nexIRC.MatrixProtocol.Core.Domain.Factory.TryCreateFromStrippedState");
                }
                encryptionEvent = new EncryptionEvent("", "", "");
                return false;
            }
        }
    }
}