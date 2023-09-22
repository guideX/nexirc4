using nexIRC.Olm.Core;
namespace nexIRC.Olm {
    /// <summary>
    /// Olm Byte Array Result Model
    /// </summary>
    public class OlmByteArrayResultModel : AjaxResult {
        /// <summary>
        /// Bytes
        /// </summary>
        public byte[]? Bytes { get; set; }
        /// <summary>
        /// Error
        /// </summary>
        public ErrorsEnum Error { get; set; }
        /// <summary>
        /// Error Str
        /// </summary>
        public string? ErrorStr { get; set; }
        /// <summary>
        /// Long Error Str
        /// </summary>
        public string? LongErrorStr { get; set; }
    }
}
