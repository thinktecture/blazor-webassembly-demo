using System;
using System.Net.Http;
using System.Threading.Tasks;
using Blazored.Toast;
using ConfTool.Client.Services;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
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
            builder.RootComponents.Add<App>("app");

            builder.Services.AddScoped<ConferencesService>();
            builder.Services.AddScoped<CountriesService>();

            builder.Services.AddOidcAuthentication(options =>
            {
                builder.Configuration.Bind("Oidc", options.ProviderOptions);
            });

            builder.Services.AddHttpClient("ConfTool.ServerAPI.Anon", client => 
                client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

            builder.Services.AddHttpClient("ConfTool.ServerAPI", client => 
                client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();
            
            builder.Services.AddScoped(services => services.GetRequiredService<IHttpClientFactory>().CreateClient("ConfTool.ServerAPI"));

            builder.Services.AddAlerts();

            builder.Services.AddBlazoredToast();

            // TODO: This should be a Singleton, right?
            builder.Services.AddScoped(services =>
            {
                var baseAddressMessageHandler = services.GetRequiredService<BaseAddressAuthorizationMessageHandler>();
                baseAddressMessageHandler.InnerHandler = new HttpClientHandler();
                var grpcWebHandler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, baseAddressMessageHandler);

                return GrpcChannel.ForAddress(builder.HostEnvironment.BaseAddress, new GrpcChannelOptions { HttpHandler = grpcWebHandler });
            });

            builder.Services.AddApiAuthorization();

            await builder.Build().RunAsync();
        }
    }
}
