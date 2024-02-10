namespace nexIRC.MatrixProtocol.Core.Infrastructure.Dto.Login {
    /// <summary>
    /// Identifier
    /// </summary>
    /// <param name="Type"></param>
    /// <param name="User"></param>
    public record Identifier(string Type, string User) {
        /// <summary>
        /// Type
        /// </summary>
        public string Type { get; } = Type;
        /// <summary>
        /// User
        /// </summary>
        public string User { get; } = User;
    }
}