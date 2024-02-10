namespace nexIRC.MatrixProtocol.Wrapper {
    /// <summary>
    /// Matrix Result
    /// </summary>
    public abstract class MatrixResult {
        /// <summary>
        /// Success
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// Message
        /// </summary>
        public string? Message { get; set; }
    }
}