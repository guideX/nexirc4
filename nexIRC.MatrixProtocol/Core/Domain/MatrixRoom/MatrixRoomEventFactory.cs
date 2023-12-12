namespace nexIRC.MatrixProtocol.Core.Domain.MatrixRoom {
    using System.Collections.Generic;
    using Infrastructure.Dto.Sync;
    using Infrastructure.Dto.Sync.Event.Room;
    using RoomEvent;
    /// <summary>
    /// Constructor
    /// </summary>
    public class MatrixRoomEventFactory {
        /// <summary>
        /// Create From Joined
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="joinedRoom"></param>
        /// <returns></returns>
        public List<BaseRoomEvent> CreateFromJoined(string roomId, JoinedRoom joinedRoom) {
            var roomEvents = new List<BaseRoomEvent>();
            foreach (RoomEvent timelineEvent in joinedRoom.Timeline.Events) {
                if (JoinRoomEvent.Factory.TryCreateFrom(timelineEvent, roomId, out JoinRoomEvent joinRoomEvent)) {
                    roomEvents.Add(joinRoomEvent);
                } else if (CreateRoomEvent.Factory.TryCreateFrom(timelineEvent, roomId, out CreateRoomEvent createRoomEvent)) {
                    roomEvents.Add(createRoomEvent);
                } else if (InviteToRoomEvent.Factory.TryCreateFrom(timelineEvent, roomId, out InviteToRoomEvent inviteToRoomEvent)) {
                    roomEvents.Add(inviteToRoomEvent);
                } else if (TextMessageEvent.Factory.TryCreateFrom(timelineEvent, roomId, out TextMessageEvent textMessageEvent)) {
                    roomEvents.Add(textMessageEvent);
                } else if (EncryptedEvent.Factory.TryCreateFrom(timelineEvent, roomId, out EncryptedEvent encryptedEvent)) {
                    roomEvents.Add(encryptedEvent);
                } else if (EncryptionEvent.Factory.TryCreateFrom(timelineEvent, roomId, out EncryptionEvent encryptionEvent)) {
                    roomEvents.Add(encryptionEvent);
                } else if (RoomKeyEvent.Factory.TryCreateFrom(timelineEvent, roomId, out RoomKeyEvent roomKeyEvent)) {
                    roomEvents.Add(roomKeyEvent);
                } else if (UnknownEvent.Factory.TryCreateFrom(timelineEvent, roomId, out UnknownEvent unknownEvent)) {
                    roomEvents.Add(unknownEvent);
                }
            }
            return roomEvents;
        }
        /// <summary>
        /// Create from Invited
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="invitedRoom"></param>
        /// <returns></returns>
        public List<BaseRoomEvent> CreateFromInvited(string roomId, InvitedRoom invitedRoom) {
            var roomEvents = new List<BaseRoomEvent>();
            foreach (RoomStrippedState inviteStateEvent in invitedRoom.InviteState.Events) {
                if (JoinRoomEvent.Factory.TryCreateFromStrippedState(inviteStateEvent, roomId, out JoinRoomEvent joinRoomEvent)) {
                    roomEvents.Add(joinRoomEvent!);
                } else if (CreateRoomEvent.Factory.TryCreateFromStrippedState(inviteStateEvent, roomId, out CreateRoomEvent createRoomEvent)) {
                    roomEvents.Add(createRoomEvent!);
                } else if (InviteToRoomEvent.Factory.TryCreateFromStrippedState(inviteStateEvent, roomId, out InviteToRoomEvent inviteToRoomEvent)) {
                    roomEvents.Add(inviteToRoomEvent!);
                } else if (TextMessageEvent.Factory.TryCreateFromStrippedState(inviteStateEvent, roomId, out TextMessageEvent textMessageEvent)) {
                    roomEvents.Add(textMessageEvent);
                } else if (EncryptionEvent.Factory.TryCreateFromStrippedState(inviteStateEvent, roomId, out EncryptionEvent encryptionEvent)) {
                    roomEvents.Add(encryptionEvent);
                } else if (EncryptedEvent.Factory.TryCreateFromStrippedState(inviteStateEvent, roomId, out EncryptedEvent encryptedEvent)) {
                    roomEvents.Add(encryptedEvent);
                } else if (RoomKeyEvent.Factory.TryCreateFromStrippedState(inviteStateEvent, roomId, out RoomKeyEvent roomKeyEvent)) {
                    roomEvents.Add(roomKeyEvent);
                } else if (UnknownEvent.Factory.TryCreateFromStrippedState(inviteStateEvent, roomId, out UnknownEvent unknownEvent)) {
                    roomEvents.Add(roomKeyEvent);
                }
            }
            return roomEvents;
        }
        /// <summary>
        /// Create from Left
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="leftRoom"></param>
        /// <returns></returns>
        public List<BaseRoomEvent> CreateFromLeft(string roomId, LeftRoom leftRoom) {
            var roomEvents = new List<BaseRoomEvent>();
            foreach (RoomEvent timelineEvent in leftRoom.Timeline.Events) {
                if (JoinRoomEvent.Factory.TryCreateFrom(timelineEvent, roomId, out JoinRoomEvent joinRoomEvent)) {
                    roomEvents.Add(joinRoomEvent);
                } else if (CreateRoomEvent.Factory.TryCreateFrom(timelineEvent, roomId, out CreateRoomEvent createRoomEvent)) {
                    roomEvents.Add(createRoomEvent);
                } else if (InviteToRoomEvent.Factory.TryCreateFrom(timelineEvent, roomId, out InviteToRoomEvent inviteToRoomEvent)) {
                    roomEvents.Add(inviteToRoomEvent);
                } else if (TextMessageEvent.Factory.TryCreateFrom(timelineEvent, roomId, out TextMessageEvent textMessageEvent)) {
                    roomEvents.Add(textMessageEvent);
                } else if (EncryptionEvent.Factory.TryCreateFrom(timelineEvent, roomId, out EncryptionEvent encryptionEvent)) {
                    roomEvents.Add(encryptionEvent);
                } else if (EncryptedEvent.Factory.TryCreateFrom(timelineEvent, roomId, out EncryptedEvent encryptedEvent)) {
                    roomEvents.Add(encryptedEvent);
                } else if (RoomKeyEvent.Factory.TryCreateFrom(timelineEvent, roomId, out RoomKeyEvent roomKeyEvent)) {
                    roomEvents.Add(roomKeyEvent);
                } else if (UnknownEvent.Factory.TryCreateFrom(timelineEvent, roomId, out UnknownEvent unknownEvent)) {
                    roomEvents.Add(unknownEvent);
                }
            }
            return roomEvents;
        }
    }
}