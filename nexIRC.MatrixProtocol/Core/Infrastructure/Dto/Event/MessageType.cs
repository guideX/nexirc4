namespace nexIRC.MatrixProtocol.Core.Infrastructure.Dto.Event {
    using System.Runtime.Serialization;
    /// <summary>
    /// Message Type
    /// </summary>
    public enum MessageType {
        /// <summary>
        /// Text
        /// </summary>
        [EnumMember(Value = "m.text")] Text
    }
}