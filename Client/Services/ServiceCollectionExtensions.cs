using Microsoft.Extensions.DependencyInjection;

namespace ConfTool.Client.Services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAlerts(this IServiceCollection services)
        {
            services.AddSingleton<IAlertService, AlertService>();

            return services;
        }
    }
}
