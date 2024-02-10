namespace nexIRC.MatrixProtocol.Core.Domain.RoomEvent {
    using Infrastructure.Dto.Sync.Event;
    using Infrastructure.Dto.Sync.Event.Room;
    using Infrastructure.Dto.Sync.Event.Room.Messaging;
    /// <summary>
    /// Room Key Event
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="senderUserId"></param>
    /// <param name="sessionID"></param>
    /// <param name="sessionKey"></param>
    public record RoomKeyEvent(string roomId, string senderUserId, string sessionID, string sessionKey) : BaseRoomEvent(roomId, senderUserId) {
        /// <summary>
        /// Factory
        /// </summary>
        public static class Factory {
            /// <summary>
            /// Try Create From
            /// </summary>
            /// <param name="roomEvent"></param>
            /// <param name="roomId"></param>
            /// <param name="roomKeyEvent"></param>
            /// <returns></returns>
            public static bool TryCreateFrom(RoomEvent roomEvent, string roomId, out RoomKeyEvent roomKeyEvent) {
                var content = roomEvent.Content.ToObject<RoomKeyContent>();
                if (roomEvent.EventType == EventType.RoomKey) {
                    roomKeyEvent = new RoomKeyEvent(roomId, roomEvent.SenderUserId, content!.Session_id, content.Session_key);
                    return true;
                }
                roomKeyEvent = new RoomKeyEvent("", "", "", "");
                return false;
            }
            /// <summary>
            /// Try Create From Stripped State
            /// </summary>
            /// <param name="roomStrippedState"></param>
            /// <param name="roomId"></param>
            /// <param name="roomKeyEvent"></param>
            /// <returns></returns>
            public static bool TryCreateFromStrippedState(RoomStrippedState roomStrippedState, string roomId, out RoomKeyEvent roomKeyEvent) {
                var content = roomStrippedState.Content.ToObject<RoomKeyContent>();
                if (roomStrippedState.EventType == EventType.RoomKey) {
                    roomKeyEvent = new RoomKeyEvent(roomId, roomStrippedState.SenderUserId, content!.Session_id, content.Session_key);
                    return true;
                }
                roomKeyEvent = new RoomKeyEvent("", "", "", "");
                return false;
            }
        }
    }
}