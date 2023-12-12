using nexIRC.Enum;
namespace nexIRC.MatrixProtocol.Core.Domain.MatrixRoom {
    using Infrastructure.Dto.Sync;
    using Infrastructure.Dto.Sync.Event.Room;
    using nexIRC.Business.Helper;
    using nexIRC.Model.Matrix.Room;
    using RoomEvent;
    /// <summary>
    /// Matrix Room Factory
    /// </summary>
    public class MatrixRoomFactory {
        /// <summary>
        /// Create Joined
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="joinedRoom"></param>
        /// <returns></returns>
        public MatrixRoom? CreateJoined(string roomId, JoinedRoom joinedRoom) {
            MatrixRoom? result = null;
            try {
                var joinedUserIds = new List<string>();
                foreach (RoomEvent timelineEvent in joinedRoom.Timeline.Events)
                    if (JoinRoomEvent.Factory.TryCreateFrom(timelineEvent, roomId, out JoinRoomEvent joinRoomEvent)) joinedUserIds.Add(joinRoomEvent.SenderUserId);
                result = new MatrixRoom(roomId, MatrixRoomStatusEnum.Joined, joinedUserIds);
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.MatrixProtocol.Core.Domain.MatrixRoom.CreateFromInvited");
            }
            return result;
        }
        /// <summary>
        /// Create Invite
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="invitedRoom"></param>
        /// <returns></returns>
        public MatrixRoom? CreateInvite(string roomId, InvitedRoom invitedRoom) {
            try {
                var joinedUserIds = new List<string>();
                foreach (RoomStrippedState timelineEvent in invitedRoom.InviteState.Events)
                    if (JoinRoomEvent.Factory.TryCreateFromStrippedState(timelineEvent, roomId, out JoinRoomEvent joinRoomEvent))
                        joinedUserIds.Add(joinRoomEvent.SenderUserId);
                return new MatrixRoom(roomId, MatrixRoomStatusEnum.Invited, joinedUserIds);
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.MatrixProtocol.Core.Domain.MatrixRoom.CreateInvite");
            }
            return new MatrixRoom(new MatrixRoomInputModel());
        }
        /// <summary>
        /// Create Left
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="leftRoom"></param>
        /// <returns></returns>
        public MatrixRoom CreateLeft(string roomId, LeftRoom leftRoom) {
            try {
                var joinedUserIds = new List<string>();
                foreach (RoomEvent timelineEvent in leftRoom.Timeline.Events)
                    if (JoinRoomEvent.Factory.TryCreateFrom(timelineEvent, roomId, out JoinRoomEvent joinRoomEvent))
                        joinedUserIds.Add(joinRoomEvent.SenderUserId);
                return new MatrixRoom(roomId, MatrixRoomStatusEnum.Left, joinedUserIds);
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.MatrixProtocol.Core.Domain.MatrixRoom.CreateLeft");
            }
            return new MatrixRoom(new MatrixRoomInputModel());
        }
    }
}