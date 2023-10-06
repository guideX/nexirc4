namespace nexIRC.MatrixProtocol.Core.Infrastructure.Services {
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using nexIRC.Model.Matrix.Server;
    /// <summary>
    /// Client Service
    /// </summary>
    public class ClientService : BaseApiService {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpClientFactory"></param>
        public ClientService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) {
        }
        /// <summary>
        /// Resource Path
        /// </summary>
        protected override string ResourcePath => "_matrix/client/versions";
        /// <summary>
        /// Get Matrix Client Versions
        /// </summary>
        /// <param name="address"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<MatrixServerVersionsResponseModel> GetMatrixClientVersions(Uri address,
            CancellationToken cancellationToken) {
            HttpClient httpClient = CreateHttpClient();
            return await httpClient.GetAsJsonAsync<MatrixServerVersionsResponseModel>(ResourcePath, cancellationToken);
        }
    }
}