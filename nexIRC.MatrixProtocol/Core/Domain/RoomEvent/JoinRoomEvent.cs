namespace nexIRC.MatrixProtocol.Core.Domain.RoomEvent {
    using Infrastructure.Dto.Sync.Event;
    using Infrastructure.Dto.Sync.Event.Room;
    using Infrastructure.Dto.Sync.Event.Room.State;
    public record JoinRoomEvent(string RoomId, string SenderUserId) : BaseRoomEvent(RoomId, SenderUserId) {
        /// <summary>
        /// Factory
        /// </summary>
        public static class Factory {
            /// <summary>
            /// Try Create From
            /// </summary>
            /// <param name="roomEvent"></param>
            /// <param name="roomId"></param>
            /// <param name="joinRoomEvent"></param>
            /// <returns></returns>
            public static bool TryCreateFrom(RoomEvent roomEvent, string roomId, out JoinRoomEvent joinRoomEvent) {
                var content = roomEvent.Content.ToObject<RoomMemberContent>();
                if (roomEvent.EventType == EventType.Member && content?.UserMembershipState == UserMembershipState.Join) {
                    joinRoomEvent = new JoinRoomEvent(roomId, roomEvent.SenderUserId);
                    return true;
                }
                joinRoomEvent = new JoinRoomEvent(string.Empty, string.Empty);
                return false;
            }
            /// <summary>
            /// Try Create from Stripped State
            /// </summary>
            /// <param name="roomStrippedState"></param>
            /// <param name="roomId"></param>
            /// <param name="joinRoomEvent"></param>
            /// <returns></returns>
            public static bool TryCreateFromStrippedState(RoomStrippedState roomStrippedState, string roomId, out JoinRoomEvent joinRoomEvent) {
                var content = roomStrippedState.Content.ToObject<RoomMemberContent>();
                if (roomStrippedState.EventType == EventType.Member && content?.UserMembershipState == UserMembershipState.Join) {
                    joinRoomEvent = new JoinRoomEvent(roomId, roomStrippedState.SenderUserId);
                    return true;
                }
                joinRoomEvent = new JoinRoomEvent(string.Empty, string.Empty);
                return false;
            }
        }
    }
}