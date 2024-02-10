using Microsoft.Extensions.DependencyInjection;
using nexIRC.Business.Helper;
namespace nexIRC.MatrixProtocol
{
    using Core.Domain.Services;
    using Core.Infrastructure.Services;
    using nexIRC.MatrixProtocol.Interfaces;

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
            try {
                services.AddSingleton<IHttpClientFactory, SingletonHttpFactory>();
                services.AddSingleton<ClientService>();
                services.AddSingleton<EventService>();
                services.AddSingleton<RoomService>();
                services.AddSingleton<UserService>();
                services.AddTransient<IPollingService, PollingService>();
                services.AddTransient<IMatrixClient, MatrixClient>();
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.MatrixProtocol.AddMatrixClient");
            }
            return services;
        }
    }
}