namespace nexIRC.MatrixProtocol.Core.Domain.MatrixRoom {
    using System.Collections.Generic;
    using Infrastructure.Dto.Sync;
    using Infrastructure.Dto.Sync.Event.Room;
    using nexIRC.Enum;
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
        public MatrixRoom CreateJoined(string roomId, JoinedRoom joinedRoom) {
            var joinedUserIds = new List<string>();
            foreach (RoomEvent timelineEvent in joinedRoom.Timeline.Events)
                if (JoinRoomEvent.Factory.TryCreateFrom(timelineEvent, roomId, out JoinRoomEvent joinRoomEvent))
                    joinedUserIds.Add(joinRoomEvent.SenderUserId);
            return new MatrixRoom(roomId, MatrixRoomStatusEnum.Joined, joinedUserIds);
        }
        /// <summary>
        /// Create Invite
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="invitedRoom"></param>
        /// <returns></returns>
        public MatrixRoom CreateInvite(string roomId, InvitedRoom invitedRoom) {
            var joinedUserIds = new List<string>();
            foreach (RoomStrippedState timelineEvent in invitedRoom.InviteState.Events)
                if (JoinRoomEvent.Factory.TryCreateFromStrippedState(timelineEvent, roomId,
                        out JoinRoomEvent joinRoomEvent))
                    joinedUserIds.Add(joinRoomEvent.SenderUserId);

            return new MatrixRoom(roomId, MatrixRoomStatusEnum.Invited, joinedUserIds);
        }
        /// <summary>
        /// Create Left
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="leftRoom"></param>
        /// <returns></returns>
        public MatrixRoom CreateLeft(string roomId, LeftRoom leftRoom) {
            var joinedUserIds = new List<string>();
            foreach (RoomEvent timelineEvent in leftRoom.Timeline.Events)
                if (JoinRoomEvent.Factory.TryCreateFrom(timelineEvent, roomId, out JoinRoomEvent joinRoomEvent))
                    joinedUserIds.Add(joinRoomEvent.SenderUserId);
            return new MatrixRoom(roomId, MatrixRoomStatusEnum.Left, joinedUserIds);
        }
    }
}