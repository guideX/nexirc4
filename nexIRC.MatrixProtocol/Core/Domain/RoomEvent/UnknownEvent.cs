namespace nexIRC.MatrixProtocol.Core.Domain.RoomEvent {
    using Infrastructure.Dto.Sync.Event;
    using Infrastructure.Dto.Sync.Event.Room;
    /// <summary>
    /// Unknown Event
    /// </summary>
    /// <param name="RoomId"></param>
    /// <param name="SenderUserId"></param>
    public record UnknownEvent(string RoomId, string SenderUserId) : BaseRoomEvent(RoomId, SenderUserId) {
        /// <summary>
        /// Factory
        /// </summary>
        public static class Factory {
            /// <summary>
            /// Try Create From
            /// </summary>
            /// <param name="roomEvent"></param>
            /// <param name="roomId"></param>
            /// <param name="unknownEvent"></param>
            /// <returns></returns>
            public static bool TryCreateFrom(RoomEvent roomEvent, string roomId, out UnknownEvent unknownEvent) {
                //var content = roomEvent.Content.ToObject<MessageContent>();
                if (roomEvent.EventType == EventType.Unknown) {
                    unknownEvent = new UnknownEvent(roomId, roomEvent.SenderUserId);
                    return true;
                }
                unknownEvent = new UnknownEvent(string.Empty, string.Empty);
                return false;
            }
            /// <summary>
            /// Try Create From Stripped State
            /// </summary>
            /// <param name="roomStrippedState"></param>
            /// <param name="roomId"></param>
            /// <param name="unknownEvent"></param>
            /// <returns></returns>
            public static bool TryCreateFromStrippedState(RoomStrippedState roomStrippedState, string roomId, out UnknownEvent unknownEvent) {
                //var content = roomStrippedState.Content.ToObject<MessageContent>();
                if (roomStrippedState.EventType == EventType.Unknown /*&& content?.MessageType == MessageType.Text*/) {
                    unknownEvent = new UnknownEvent(roomId, roomStrippedState.SenderUserId);
                    return true;
                }
                unknownEvent = new UnknownEvent(string.Empty, string.Empty);
                return false;
            }
        }
    }
}