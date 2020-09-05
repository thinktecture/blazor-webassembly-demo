using Microsoft.Extensions.DependencyInjection;

namespace ConfTool.Client.Services
{
    public static class WebcamServiceCollectionExtensions
    {
        public static IServiceCollection AddWebcam(this IServiceCollection services)
        {
            services.AddSingleton<IWebcamService, WebcamService>();

            return services;
        }
    }
}
