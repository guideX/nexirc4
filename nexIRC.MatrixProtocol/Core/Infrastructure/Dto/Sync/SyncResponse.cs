namespace nexIRC.MatrixProtocol.Core.Infrastructure.Dto.Sync {
    using Event.Room;
    /// <summary>
    ///     Synchronization response.
    /// </summary>
    public record SyncResponse {
        /// <summary>
        ///     <b>Required.</b> The batch token to supply in the since param of the next /sync request.
        /// </summary>
        public string? NextBatch { get; init; }

        /// <summary>
        ///     <b>Updates</b> to rooms.
        /// </summary>
        public Rooms? Rooms { get; init; }
    }
    /// <summary>
    /// Rooms
    /// </summary>
    public record Rooms {
        /// <summary>
        ///     The rooms that the user has joined, mapped as room ID to room information.
        /// </summary>
        public Dictionary<string, JoinedRoom> Join { get; init; } = new Dictionary<string, JoinedRoom>();

        /// <summary>
        ///     The rooms that the user has been invited to, mapped as room ID to room information.
        /// </summary>
        public Dictionary<string, InvitedRoom> Invite { get; init; } = new Dictionary<string, InvitedRoom>();

        /// <summary>
        ///     The rooms that the user has left or been banned from, mapped as room ID to room information.
        /// </summary>
        public Dictionary<string, LeftRoom> Leave { get; init; } = new Dictionary<string, LeftRoom>();
    }
    /// <summary>
    /// Joined Room
    /// </summary>
    /// <param name="Timeline"></param>
    /// <param name="State"></param>
    public record JoinedRoom(Timeline Timeline, State State);
    /// <summary>
    /// Invited Room
    /// </summary>
    /// <param name="InviteState"></param>
    public record InvitedRoom(InviteState InviteState);
    /// <summary>
    /// Left Room
    /// </summary>
    /// <param name="Timeline"></param>
    /// <param name="State"></param>
    public record LeftRoom(Timeline Timeline, State State);
    /// <summary>
    ///     The timeline of messages and state changes in the room.
    /// </summary>
    public record Timeline(List<RoomEvent> Events);
    /// <summary>
    ///     Updates to the state, between the time indicated by the since parameter,
    ///     and the start of the timeline (or all state up to the start of the timeline,
    ///     if since is not given, or full_state is true).
    /// </summary>
    public record State(List<RoomStateEvent> Events);
    /// <summary>
    ///     The state of a room that the user has been invited to.
    ///     These state events may only have the <c>sender</c>, <c>type</c>, <c>state_key</c> and <c>content</c> keys present.
    ///     These events do not replace any state that the client already has for the room,
    ///     for example if the client has archived the room.
    ///     Instead the client should keep two separate copies of the state: the one from the <c>invite_state</c> and one from
    ///     the archived <c>state</c>.
    ///     If the client joins the room then the current state will be given as a delta against the archived <c>state</c> not
    ///     the <c>invite_state</c>.
    /// </summary>
    public record InviteState(List<RoomStrippedState> Events);
}