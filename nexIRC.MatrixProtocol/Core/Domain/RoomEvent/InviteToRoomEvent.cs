using nexIRC.Business.Helper;
namespace nexIRC.MatrixProtocol.Core.Domain.RoomEvent {
    using Infrastructure.Dto.Sync.Event;
    using Infrastructure.Dto.Sync.Event.Room;
    using Infrastructure.Dto.Sync.Event.Room.State;
    /// <summary>
    /// Invite To Room Event
    /// </summary>
    /// <param name="RoomId"></param>
    /// <param name="SenderUserId"></param>
    public record InviteToRoomEvent(string RoomId, string SenderUserId) : BaseRoomEvent(RoomId, SenderUserId) {
        /// <summary>
        /// Factory
        /// </summary>
        public static class Factory {
            /// <summary>
            /// Try Create From
            /// </summary>
            /// <param name="roomEvent"></param>
            /// <param name="roomId"></param>
            /// <param name="inviteToRoomEvent"></param>
            /// <returns></returns>
            public static bool TryCreateFrom(RoomEvent roomEvent, string roomId, out InviteToRoomEvent inviteToRoomEvent) {
                try {
                    var content = roomEvent.Content.ToObject<RoomMemberContent>();
                    if (roomEvent.EventType == EventType.Member &&
                        content?.UserMembershipState == UserMembershipState.Invite) {
                        inviteToRoomEvent = new InviteToRoomEvent(roomId, roomEvent.SenderUserId);
                        return true;
                    }
                } catch (Exception ex) {
                    ExceptionHelper.HandleException(ex, "nexIRC.MatrixProtocol.Core.Domain.RoomEvent.TryCreateFrom");
                }
                inviteToRoomEvent = new InviteToRoomEvent(string.Empty, string.Empty);
                return false;
            }
            /// <summary>
            /// Try Create from Stripped State
            /// </summary>
            /// <param name="roomStrippedState"></param>
            /// <param name="roomId"></param>
            /// <param name="inviteToRoomEvent"></param>
            /// <returns></returns>
            public static bool TryCreateFromStrippedState(RoomStrippedState roomStrippedState, string roomId, out InviteToRoomEvent inviteToRoomEvent) {
                try {
                    var content = roomStrippedState.Content.ToObject<RoomMemberContent>();
                    if (roomStrippedState.EventType == EventType.Member &&
                        content?.UserMembershipState == UserMembershipState.Invite) {
                        inviteToRoomEvent = new InviteToRoomEvent(roomId, roomStrippedState.SenderUserId);
                        return true;
                    }
                } catch (Exception ex) {
                    ExceptionHelper.HandleException(ex, "nexIRC.MatrixProtocol.Core.Domain.RoomEvent.TryCreateFromStrippedState");
                }
                inviteToRoomEvent = new InviteToRoomEvent(string.Empty, string.Empty);
                return false;
            }
        }
    }
}