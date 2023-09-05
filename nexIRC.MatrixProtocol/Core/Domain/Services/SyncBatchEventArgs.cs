namespace nexIRC.MatrixProtocol.Core.Domain.Services {
    using System;
    /// <summary>
    /// Sync Batch Event Args
    /// </summary>
    public class SyncBatchEventArgs : EventArgs {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="syncBatch"></param>
        public SyncBatchEventArgs(SyncBatch syncBatch) {
            SyncBatch = syncBatch;
        }
        /// <summary>
        /// Sync Batch
        /// </summary>
        public SyncBatch SyncBatch { get; }
    }
}