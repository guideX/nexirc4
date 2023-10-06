namespace nexIRC.Model {
    /// <summary>
    /// Pickle Result Model
    /// </summary>
    public class PickleResultModel {
        /// <summary>
        /// Result
        /// </summary>
        public uint Result { get; set; }
        /// <summary>
        /// Pickle Length
        /// </summary>
        public int Length { get; set; }
        /// <summary>
        /// Bytes
        /// </summary>
        public byte[]? Bytes { get; set; }
    }
}