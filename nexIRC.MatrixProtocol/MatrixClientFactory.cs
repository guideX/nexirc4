namespace nexIRC.MatrixProtocol
{
    using System.Net.Http;
    using Core.Domain.Services;
    using Core.Infrastructure.Services;
    using Microsoft.Extensions.Logging;
    using nexIRC.Business.Helper;
    using nexIRC.MatrixProtocol.Interfaces;

    /// <summary>
    /// Singleton Http Factory
    /// </summary>
    public class SingletonHttpFactory : IHttpClientFactory {
        /// <summary>
        /// Http Client
        /// </summary>
        private readonly HttpClient _httpClient;
        /// <summary>
        /// Constructor
        /// </summary>
        public SingletonHttpFactory() {
            var httpClientHandler = new HttpClientHandler { ServerCertificateCustomValidationCallback = (_, _, _, _) => true };
            _httpClient = new HttpClient(httpClientHandler);
        }
        /// <summary>
        /// Create Client
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public HttpClient CreateClient(string name) => _httpClient;
    }
    /// <summary>
    /// Matrix Client Factory
    /// </summary>
    public class MatrixClientFactory {
        /// <summary>
        /// Http Client Factory
        /// </summary>
        private readonly SingletonHttpFactory _httpClientFactory = new();
        /// <summary>
        /// Client
        /// </summary>
        private MatrixClient? _client;
        /// <summary>
        /// Create
        /// </summary>
        /// <param name="logger"></param>
        /// <returns></returns>
        public IMatrixClient Create(ILogger<PollingService>? logger = null) {
            try {
                if (_client != null)
                    return _client;
                var eventService = new EventService(_httpClientFactory);
                var userService = new UserService(_httpClientFactory);
                var roomService = new RoomService(_httpClientFactory);
                var pollingService = new PollingService(eventService, logger!);
                _client = new MatrixClient(
                    pollingService,
                    userService,
                    roomService,
                    eventService);
                return _client;
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.MatrixProtocol.Create");
                throw;
            }
        }
    }
}