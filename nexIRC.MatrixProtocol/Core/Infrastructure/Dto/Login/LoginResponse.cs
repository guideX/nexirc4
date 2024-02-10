namespace nexIRC.MatrixProtocol.Core.Infrastructure.Dto.Login {
    /// <summary>
    /// Login Response
    /// </summary>
    /// <param name="UserId"></param>
    /// <param name="AccessToken"></param>
    /// <param name="HomeServer"></param>
    /// <param name="DeviceId"></param>
    public record LoginResponse(string UserId, string AccessToken, string HomeServer, string DeviceId) {
        /// <summary>
        /// UserID
        /// </summary>
        public string UserId { get; } = UserId;
        /// <summary>
        /// Access Token
        /// </summary>
        public string AccessToken { get; } = AccessToken;
        /// <summary>
        /// Home Server
        /// </summary>
        public string HomeServer { get; } = HomeServer;
        /// <summary>
        /// DeviceID
        /// </summary>
        public string DeviceId { get; } = DeviceId;
    }
}