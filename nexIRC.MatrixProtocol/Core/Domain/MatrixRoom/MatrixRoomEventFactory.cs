using nexIRC.Business.Helper;
namespace nexIRC.MatrixProtocol.Core.Domain.MatrixRoom {
    using Infrastructure.Dto.Sync;
    using Infrastructure.Dto.Sync.Event.Room;
    using RoomEvent;
    /// <summary>
    /// Constructor
    /// </summary>
    public class MatrixRoomEventFactory {
        /// <summary>
        /// Create From State Events
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="joinedRoom"></param>
        /// <returns></returns>
        public List<BaseRoomEvent> CreateFromStateEvents(string roomId, JoinedRoom joinedRoom) {
            var roomEvents = new List<BaseRoomEvent>();
            try {
                foreach (RoomStateEvent ev in joinedRoom.State.Events) {
                    if (EncryptionEvent.Factory.TryCreateFrom(ev, roomId, ev.state_key, out EncryptionEvent e)) {
                        roomEvents.Add(e);
                    } else if (RoomKeyEvent.Factory.TryCreateFrom(ev, roomId, out RoomKeyEvent e2)) {
                        roomEvents.Add(e2);
                    } else if (JoinRoomEvent.Factory.TryCreateFrom(ev, roomId, out JoinRoomEvent e3)) {
                        roomEvents.Add(e3);
                    } else if (InviteToRoomEvent.Factory.TryCreateFrom(ev, roomId, out InviteToRoomEvent e4)) {
                        roomEvents.Add(e4);
                    } else if (TextMessageEvent.Factory.TryCreateFrom(ev, roomId, out TextMessageEvent e5)) {
                        roomEvents.Add(e5);
                    }
                }
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.MatrixProtocol.Core.Domain.MatrixRoom.CreateFromStateEvents");
            }
            return roomEvents;
        }
        /// <summary>
        /// Create From Joined
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="joinedRoom"></param>
        /// <returns></returns>
        public List<BaseRoomEvent> CreateFromJoined(string roomId, JoinedRoom joinedRoom) {
            var roomEvents = new List<BaseRoomEvent>();
            try {
                foreach (RoomEvent ev in joinedRoom.Timeline.Events) {
                    if (JoinRoomEvent.Factory.TryCreateFrom(ev, roomId, out JoinRoomEvent joinRoomEvent)) {
                        roomEvents.Add(joinRoomEvent);
                    } else if (CreateRoomEvent.Factory.TryCreateFrom(ev, roomId, out CreateRoomEvent createRoomEvent)) {
                        roomEvents.Add(createRoomEvent);
                    } else if (InviteToRoomEvent.Factory.TryCreateFrom(ev, roomId, out InviteToRoomEvent inviteToRoomEvent)) {
                        roomEvents.Add(inviteToRoomEvent);
                    } else if (TextMessageEvent.Factory.TryCreateFrom(ev, roomId, out TextMessageEvent textMessageEvent)) {
                        roomEvents.Add(textMessageEvent);
                    } else if (EncryptedEvent.Factory.TryCreateFrom(ev, roomId, out EncryptedEvent encryptedEvent)) {
                        roomEvents.Add(encryptedEvent);
                    } else if (RoomKeyEvent.Factory.TryCreateFrom(ev, roomId, out RoomKeyEvent roomKeyEvent)) {
                        roomEvents.Add(roomKeyEvent);
                    } else if (UnknownEvent.Factory.TryCreateFrom(ev, roomId, out UnknownEvent unknownEvent)) {
                        roomEvents.Add(unknownEvent);
                    }
                }
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.MatrixProtocol.Core.Domain.MatrixRoom.CreateFromJoined");
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
            try {
                foreach (RoomStrippedState inviteStateEvent in invitedRoom.InviteState.Events) {
                    if (JoinRoomEvent.Factory.TryCreateFromStrippedState(inviteStateEvent, roomId, out JoinRoomEvent joinRoomEvent)) {
                        roomEvents.Add(joinRoomEvent!);
                    } else if (CreateRoomEvent.Factory.TryCreateFromStrippedState(inviteStateEvent, roomId, out CreateRoomEvent createRoomEvent)) {
                        roomEvents.Add(createRoomEvent!);
                    } else if (InviteToRoomEvent.Factory.TryCreateFromStrippedState(inviteStateEvent, roomId, out InviteToRoomEvent inviteToRoomEvent)) {
                        roomEvents.Add(inviteToRoomEvent!);
                    } else if (TextMessageEvent.Factory.TryCreateFromStrippedState(inviteStateEvent, roomId, out TextMessageEvent textMessageEvent)) {
                        roomEvents.Add(textMessageEvent);
                    } else if (EncryptionEvent.Factory.TryCreateFromStrippedState(inviteStateEvent, roomId, inviteStateEvent.state_key, out EncryptionEvent encryptionEvent)) {
                        roomEvents.Add(encryptionEvent);
                    } else if (EncryptedEvent.Factory.TryCreateFromStrippedState(inviteStateEvent, roomId, out EncryptedEvent encryptedEvent)) {
                        roomEvents.Add(encryptedEvent);
                    } else if (RoomKeyEvent.Factory.TryCreateFromStrippedState(inviteStateEvent, roomId, out RoomKeyEvent roomKeyEvent)) {
                        roomEvents.Add(roomKeyEvent);
                    } else if (UnknownEvent.Factory.TryCreateFromStrippedState(inviteStateEvent, roomId, out UnknownEvent unknownEvent)) {
                        roomEvents.Add(roomKeyEvent);
                    }
                }
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.MatrixProtocol.Core.Domain.MatrixRoom.CreateFromInvited");
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
            try {
                foreach (RoomEvent timelineEvent in leftRoom.Timeline.Events) {
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
                    } else if (RoomKeyEvent.Factory.TryCreateFrom(timelineEvent, roomId, out RoomKeyEvent roomKeyEvent)) {
                        roomEvents.Add(roomKeyEvent);
                    } else if (UnknownEvent.Factory.TryCreateFrom(timelineEvent, roomId, out UnknownEvent unknownEvent)) {
                        roomEvents.Add(unknownEvent);
                    }
                }
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.MatrixProtocol.Core.Domain.MatrixRoom.CreateFromLeft");
            }
            return roomEvents;
        }
    }
}