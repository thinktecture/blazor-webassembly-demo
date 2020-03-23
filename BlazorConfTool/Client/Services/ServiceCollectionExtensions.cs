using Microsoft.Extensions.DependencyInjection;

namespace BlazorConfTool.Client.Services
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
