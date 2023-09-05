namespace nexIRC.MatrixProtocol.Core.Infrastructure.Dto.Login {
    /// <summary>
    /// Login Request
    /// </summary>
    /// <param name="Identifier"></param>
    /// <param name="Password"></param>
    /// <param name="DeviceId"></param>
    /// <param name="Type"></param>
    public record LoginRequest(Identifier Identifier, string Password, string DeviceId, string Type) {
        /// <summary>
        /// Identifier
        /// </summary>
        public Identifier Identifier { get; } = Identifier;
        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; } = Password;
        /// <summary>
        /// Device ID
        /// </summary>
        public string DeviceId { get; } = DeviceId;
        /// <summary>
        /// Ty[e
        /// </summary>
        public string Type { get; } = Type;
    }
}