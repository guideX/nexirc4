//"OUTPUT_BUFFER_TOO_SMALL", "BAD_MESSAGE_VERSION",
//"BAD_MESSAGE_FORMAT", "BAD_MESSAGE_MAC", "BAD_MESSAGE_KEY_ID", "INVALID_BASE64",
//"BAD_ACCOUNT_KEY", "UNKNOWN_PICKLE_VERSION", "CORRUPTED_PICKLE", "BAD_SESSION_KEY",
//"UNKNOWN_MESSAGE_INDEX", "BAD_LEGACY_ACCOUNT_PICKLE", "BAD_SIGNATURE",
//"OLM_INPUT_BUFFER_TOO_SMALL", "OLM_SAS_THEIR_KEY_NOT_SET", "OLM_PICKLE_EXTRA_DATA"
namespace nexIRC.Olm.Core {
    /// <summary>
    /// Errors Enum
    /// </summary>
    public enum ErrorsEnum {
        /// <summary>
        /// Success
        /// </summary>
        SUCCESS,
        /// <summary>
        /// Not Enough Random
        /// </summary>
        NOT_ENOUGH_RANDOM,
        /// <summary>
        /// Bad Message Format
        /// </summary>
        BAD_MESSAGE_FORMAT,
        /// <summary>
        /// Output Buffer too Small
        /// </summary>
        OUTPUT_BUFFER_TOO_SMALL,
        /// <summary>
        /// Invalid Base64
        /// </summary>
        INVALID_BASE64,
        /// <summary>
        /// Bad Message Version
        /// </summary>
        BAD_MESSAGE_VERSION,
        /// <summary>
        /// Unknown
        /// </summary>
        UNKNOWN,
        /// <summary>
        /// Bad Message Mac
        /// </summary>
        BAD_MESSAGE_MAC
    }
}