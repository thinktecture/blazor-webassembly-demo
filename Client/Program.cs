using System.Threading.Tasks;
using ConfTool.Client.Services;
using ConfTool.Modules.Conferences;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConfTool.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddConferencesModule(builder.Configuration);

            builder.Services.AddWebcam();

            builder.Services.AddOidcAuthentication(options =>
            {
                builder.Configuration.Bind("Oidc", options.ProviderOptions);
            });
            
            builder.Services.AddApiAuthorization();

            await builder.Build().RunAsync();
        }
    }
}
