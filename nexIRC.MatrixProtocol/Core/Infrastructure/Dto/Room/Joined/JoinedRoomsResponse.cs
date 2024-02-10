﻿namespace nexIRC.MatrixProtocol.Core.Infrastructure.Dto.Room.Joined {
    using System.Collections.Generic;
    using Newtonsoft.Json;
    /// <summary>
    /// Joined Room Response
    /// </summary>
    /// <param name="JoinedRoomIds"></param>
    public record JoinedRoomsResponse(List<string> JoinedRoomIds) {
        /// <summary>
        ///     <b>Required.</b> The ID of each room in which the user has joined membership.
        /// </summary>
        [JsonProperty("joined_rooms")]
        public List<string> JoinedRoomIds { get; } = JoinedRoomIds;
    }
}