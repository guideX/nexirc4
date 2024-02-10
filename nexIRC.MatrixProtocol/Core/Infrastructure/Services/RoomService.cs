namespace nexIRC.MatrixProtocol.Core.Infrastructure.Services {
    using Dto.Room.Create;
    using Dto.Room.Join;
    using Dto.Room.Joined;
    using nexIRC.Business.Helper;
    public class RoomService : BaseApiService {
        /// <summary>
        /// Room Service
        /// </summary>
        /// <param name="httpClientFactory"></param>
        public RoomService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) {
        }
        /// <summary>
        /// Create Room Async
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="members"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<CreateRoomResponse> CreateRoomAsync(string accessToken, string[]? members, CancellationToken cancellationToken) {
            try {
                var model = new CreateRoomRequest(
                    Invite: members,
                    Preset: Preset.TrustedPrivateChat,
                    IsDirect: true
                );
                HttpClient httpClient = CreateHttpClient(accessToken);
                var path = $"{ResourcePath}/createRoom";
                return await httpClient.PostAsJsonAsync<CreateRoomResponse>(path, model, cancellationToken);
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "CreateRoomAsync");
                throw;
            }
        }
        /// <summary>
        /// Join Room Async
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="roomId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<JoinRoomResponse> JoinRoomAsync(string accessToken, string roomId, CancellationToken cancellationToken) {
            try {
                HttpClient httpClient = CreateHttpClient(accessToken);
                var path = $"{ResourcePath}/rooms/{roomId}/join";
                return await httpClient.PostAsJsonAsync<JoinRoomResponse>(path, null, cancellationToken);
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "JoinRoomAsync");
                throw;
            }
        }
        /// <summary>
        /// Get Joined Rooms Async
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<JoinedRoomsResponse> GetJoinedRoomsAsync(string accessToken, CancellationToken cancellationToken) {
            HttpClient httpClient = CreateHttpClient(accessToken);
            var path = $"{ResourcePath}/joined_rooms";
            return await httpClient.GetAsJsonAsync<JoinedRoomsResponse>(path, cancellationToken);
        }
        /// <summary>
        /// Leave Room Async
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="roomId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task LeaveRoomAsync(string accessToken, string roomId,
            CancellationToken cancellationToken) {
            HttpClient httpClient = CreateHttpClient(accessToken);
            var path = $"{ResourcePath}/rooms/{roomId}/leave";
            await httpClient.PostAsync(path, cancellationToken);
        }
    }
}