namespace nexIRC.MatrixProtocol {
    using Core.Domain.RoomEvent;
    /// <summary>
    /// Matrix Room Events Event Args
    /// </summary>
    public class MatrixRoomEventsEventArgs : EventArgs {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="matrixRoomEvents"></param>
        /// <param name="nextBatch"></param>
        public MatrixRoomEventsEventArgs(List<BaseRoomEvent> matrixRoomEvents, string nextBatch) {
            MatrixRoomEvents = matrixRoomEvents;
            NextBatch = nextBatch;
        }
        /// <summary>
        /// Matrix Room Events
        /// </summary>
        public List<BaseRoomEvent> MatrixRoomEvents { get; }
        /// <summary>
        /// Next Batch
        /// </summary>
        public string NextBatch { get; }
    }
}