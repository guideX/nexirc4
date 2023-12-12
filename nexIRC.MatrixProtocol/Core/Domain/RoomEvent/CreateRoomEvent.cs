using nexIRC.Business.Helper;
namespace nexIRC.MatrixProtocol.Core.Domain.RoomEvent {
    using Infrastructure.Dto.Sync.Event;
    using Infrastructure.Dto.Sync.Event.Room;
    using Infrastructure.Dto.Sync.Event.Room.State;
    /// <summary>
    /// Create Room Event
    /// </summary>
    /// <param name="RoomId"></param>
    /// <param name="SenderUserId"></param>
    /// <param name="RoomCreatorUserId"></param>
    public record CreateRoomEvent(string RoomId, string SenderUserId, string RoomCreatorUserId) : BaseRoomEvent(RoomId, SenderUserId) {
        /// <summary>
        /// Factory
        /// </summary>
        public static class Factory {
            /// <summary>
            /// Try Create From
            /// </summary>
            /// <param name="roomEvent"></param>
            /// <param name="roomId"></param>
            /// <param name="createRoomEvent"></param>
            /// <returns></returns>
            public static bool TryCreateFrom(RoomEvent roomEvent, string roomId, out CreateRoomEvent createRoomEvent) {
                try {
                    var content = roomEvent.Content.ToObject<RoomCreateContent>();
                    if (roomEvent.EventType == EventType.Create && content != null) {
                        createRoomEvent = new CreateRoomEvent(roomId, roomEvent.SenderUserId, content.RoomCreatorUserId);
                        return true;
                    }
                } catch (Exception ex) {
                    ExceptionHelper.HandleException(ex, "nexIRC.MatrixProtocol.Core.Domain.RoomEvent.TryCreateFrom");
                }
                createRoomEvent = new CreateRoomEvent(string.Empty, string.Empty, string.Empty);
                return false;
            }
            /// <summary>
            /// Try Create From Stripped State
            /// </summary>
            /// <param name="roomStrippedState"></param>
            /// <param name="roomId"></param>
            /// <param name="createRoomEvent"></param>
            /// <returns></returns>
            public static bool TryCreateFromStrippedState(RoomStrippedState roomStrippedState, string roomId, out CreateRoomEvent createRoomEvent) {
                try {
                    var content = roomStrippedState.Content.ToObject<RoomCreateContent>();
                    if (roomStrippedState.EventType == EventType.Create && content != null) {
                        createRoomEvent = new CreateRoomEvent(roomId, roomStrippedState.SenderUserId, content.RoomCreatorUserId);
                        return true;
                    }
                } catch (Exception ex) {
                    ExceptionHelper.HandleException(ex, "nexIRC.MatrixProtocol.Core.Domain.RoomEvent.CreateRoomEvent");
                }
                createRoomEvent = new CreateRoomEvent(string.Empty, string.Empty, string.Empty);
                return false;
            }
        }
    }
}