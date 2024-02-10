namespace nexIRC.MatrixProtocol.Core.Domain.RoomEvent {
    /// <summary>
    /// Base Room Event
    /// </summary>
    /// <param name="RoomId"></param>   
    /// <param name="SenderUserId"></param>
    public abstract record BaseRoomEvent(string RoomId, string SenderUserId);
}