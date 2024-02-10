namespace nexIRC.MatrixProtocol.Core.Infrastructure.Dto.Room.Create {
    using System.Runtime.Serialization;
    /// <summary>
    /// Preset
    /// </summary>
    public enum Preset {
        /// <summary>
        /// Private Chat
        /// </summary>
        [EnumMember(Value = "private_chat")] PrivateChat,
        /// <summary>
        /// Public Chat
        /// </summary>
        [EnumMember(Value = "public_chat")] PublicChat,
        /// <summary>
        /// Trusted Private Chat
        /// </summary>
        [EnumMember(Value = "trusted_private_chat")]
        TrustedPrivateChat
    }
}