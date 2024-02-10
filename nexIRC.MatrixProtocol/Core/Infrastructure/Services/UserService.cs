namespace nexIRC.MatrixProtocol.Core.Infrastructure.Services {
    using Dto.Login;
    using nexIRC.Business.Helper;
        public class UserService : BaseApiService {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpClientFactory"></param>
        public UserService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) {
        }
        /// <summary>
        /// Login Async
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="deviceId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<LoginResponse> LoginAsync(string user, string password, string deviceId, CancellationToken cancellationToken) {
            try {
                var model = new LoginRequest(
                    new Identifier(
                        "m.id.user",
                        user
                    ),
                    password,
                    deviceId,
                    "m.login.password"
                );
                HttpClient httpClient = CreateHttpClient();
                var path = $"{ResourcePath}/login";
                return await httpClient.PostAsJsonAsync<LoginResponse>(path, model, cancellationToken);
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.MatrixProtocol.Core.Infrastructure.Services.LoginAsync");
                throw;
            }
        }
    }
}