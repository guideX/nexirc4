namespace nexIRC.MatrixProtocol.Core.Infrastructure.Dto.Room.Create {
    /// <summary>
    /// Create Room Request
    /// </summary>
    /// <param name="Visibility"></param>
    /// <param name="RoomAliasName"></param>
    /// <param name="Name"></param>
    /// <param name="Topic"></param>
    /// <param name="Invite"></param>
    /// <param name="RoomVersion"></param>
    /// <param name="Preset"></param>
    /// <param name="IsDirect"></param>
    public record CreateRoomRequest(
        Visibility? Visibility = null,
        string? RoomAliasName = null,
        string? Name = null,
        string? Topic = null,
        string[]? Invite = null,
        string? RoomVersion = null,
        Preset? Preset = null,
        bool? IsDirect = null);
}