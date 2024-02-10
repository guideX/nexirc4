namespace nexIRC.MatrixProtocol.Core.Domain {
    using System.Collections.Generic;
    using System.Linq;
    using Infrastructure.Dto.Sync;
    using MatrixRoom;
    using RoomEvent;
    /// <summary>
    /// Sync Batch
    /// </summary>
    public record SyncBatch {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nextBatch"></param>
        /// <param name="matrixRooms"></param>
        /// <param name="matrixRoomEvents"></param>
        private SyncBatch(string nextBatch, List<MatrixRoom.MatrixRoom> matrixRooms,
            List<BaseRoomEvent> matrixRoomEvents) {
            NextBatch = nextBatch;
            MatrixRooms = matrixRooms;
            MatrixRoomEvents = matrixRoomEvents;
        }
        /// <summary>
        /// Next Batch
        /// </summary>
        public string NextBatch { get; }
        /// <summary>
        /// Matrix Rooms
        /// </summary>
        public List<MatrixRoom.MatrixRoom> MatrixRooms { get; }
        /// <summary>
        /// Matrix Room Events
        /// </summary>
        public List<BaseRoomEvent> MatrixRoomEvents { get; }
        /// <summary>
        /// Factory
        /// </summary>
        internal static class Factory {
            /// <summary>
            /// Matrix Room Factory
            /// </summary>
            private static readonly MatrixRoomFactory MatrixRoomFactory = new();
            /// <summary>
            /// Matrix Room Event Factory
            /// </summary>
            private static readonly MatrixRoomEventFactory MatrixRoomEventFactory = new();
            /// <summary>
            /// Create From Sync
            /// </summary>
            /// <param name="nextBatch"></param>
            /// <param name="rooms"></param>
            /// <returns></returns>
            public static SyncBatch CreateFromSync(string nextBatch, Rooms rooms) {
                List<MatrixRoom.MatrixRoom> matrixRooms = GetMatrixRoomsFromSync(rooms);
                List<BaseRoomEvent> matrixRoomEvents = GetMatrixEventsFromSync(rooms);
                return new SyncBatch(nextBatch, matrixRooms, matrixRoomEvents);
            }
            /// <summary>
            /// Get Matrix Rooms From Sync
            /// </summary>
            /// <param name="rooms"></param>
            /// <returns></returns>
            private static List<MatrixRoom.MatrixRoom> GetMatrixRoomsFromSync(Rooms rooms) {
                List<MatrixRoom.MatrixRoom> joinedMatrixRooms = new List<MatrixRoom.MatrixRoom>();
                if (rooms != null && rooms.Join != null) joinedMatrixRooms = rooms.Join.Select(pair => MatrixRoomFactory.CreateJoined(pair.Key, pair.Value)).ToList()!;
                var invitedMatrixRooms = new List<MatrixRoom.MatrixRoom>();
                var leftMatrixRooms = new List<MatrixRoom.MatrixRoom>();
                if (rooms != null && rooms.Invite != null) invitedMatrixRooms = rooms.Invite.Select(pair => MatrixRoomFactory.CreateInvite(pair.Key, pair.Value)).ToList()!;
                if (rooms != null && rooms.Leave != null) leftMatrixRooms = rooms.Leave.Select(pair => MatrixRoomFactory.CreateLeft(pair.Key, pair.Value)).ToList();
                return joinedMatrixRooms.Concat(invitedMatrixRooms).Concat(leftMatrixRooms).ToList();
            }
            /// <summary>
            /// Get Matrix Events from Sync
            /// </summary>
            /// <param name="rooms"></param>
            /// <returns></returns>
            private static List<BaseRoomEvent> GetMatrixEventsFromSync(Rooms rooms) {
                var stateEvents = new List<BaseRoomEvent>();
                var joinedMatrixRoomEvents = new List<BaseRoomEvent>();
                var invitedMatrixRoomEvents = new List<BaseRoomEvent>();
                var leftMatrixRoomEvents = new List<BaseRoomEvent>();
                if (rooms != null && rooms.Join != null) stateEvents = rooms.Join.SelectMany(pair => MatrixRoomEventFactory.CreateFromStateEvents(pair.Key, pair.Value)).ToList();
                if (rooms != null && rooms.Join != null) joinedMatrixRoomEvents = rooms.Join.SelectMany(pair => MatrixRoomEventFactory.CreateFromJoined(pair.Key, pair.Value)).ToList();
                if (rooms != null && rooms.Invite != null) invitedMatrixRoomEvents = rooms.Invite.SelectMany(pair => MatrixRoomEventFactory.CreateFromInvited(pair.Key, pair.Value)).ToList();
                if (rooms != null && rooms.Leave != null) leftMatrixRoomEvents = rooms.Leave.SelectMany(pair => MatrixRoomEventFactory.CreateFromLeft(pair.Key, pair.Value)).ToList();
                return joinedMatrixRoomEvents.Concat(invitedMatrixRoomEvents).Concat(leftMatrixRoomEvents).Concat(stateEvents).ToList();
            }
        }
    }
}