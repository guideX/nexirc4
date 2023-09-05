namespace nexIRC.MatrixProtocol.Core.Infrastructure.Dto.Room.Create {
    using System.Runtime.Serialization;
    /// <summary>
    /// Visibility
    /// </summary>
    public enum Visibility {
        /// <summary>
        /// Public
        /// </summary>
        [EnumMember(Value = "public")] Public,
        /// <summary>
        /// Private
        /// </summary>
        [EnumMember(Value = "private")] Private
    }
}