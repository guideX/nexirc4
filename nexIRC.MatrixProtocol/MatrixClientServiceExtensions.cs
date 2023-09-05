namespace nexIRC.MatrixProtocol {
    using Core.Domain.Services;
    using Core.Infrastructure.Services;
    using Microsoft.Extensions.DependencyInjection;
    /// <summary>
    /// Matrix Client Service Extension
    /// </summary>
    public static class MatrixClientServiceExtensions {
        /// <summary>
        /// Add Matrix Client
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddMatrixClient(this IServiceCollection services) {
            services.AddSingleton<IHttpClientFactory, SingletonHttpFactory>();
            services.AddSingleton<ClientService>();
            services.AddSingleton<EventService>();
            services.AddSingleton<RoomService>();
            services.AddSingleton<UserService>();
            services.AddTransient<IPollingService, PollingService>();
            services.AddTransient<IMatrixClient, MatrixClient>();
            return services;
        }
    }
}