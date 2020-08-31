using System;
using System.Net.Http;
using System.Threading.Tasks;
using Blazored.Toast;
using ConfTool.Client.GrpcClient;
using ConfTool.Client.Services;
using Grpc.Core;
using Grpc.Core.Interceptors;
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

            builder.Services.AddScoped<IConferencesServiceClient, ConferencesServiceClientGrpc>();
            builder.Services.AddScoped<CountriesServiceClient>();

            builder.Services.AddOidcAuthentication(options =>
            {
                builder.Configuration.Bind("Oidc", options.ProviderOptions);
            });

            builder.Services.AddHttpClient("ConfTool.ServerAPI.Anon", client => 
                client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

            builder.Services.AddHttpClient("ConfTool.ServerAPI", client => 
                client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            builder.Services.AddAlerts();

            builder.Services.AddBlazoredToast();

            builder.Services.AddScoped<GrpcChannel>(services =>
            {
                var channel = BuildGrpcChannel(builder, services);

                return channel;
            });

            builder.Services.AddScoped<CallInvoker>(services =>
            {
                var channel = BuildGrpcChannel(builder, services);
                var invoker = channel.Intercept(new ClientLoggerInterceptor());

                return invoker;
            });

            builder.Services.AddApiAuthorization();

            await builder.Build().RunAsync();
        }

        private static GrpcChannel BuildGrpcChannel(WebAssemblyHostBuilder builder, IServiceProvider services)
        {
            var baseAddressMessageHandler = services.GetRequiredService<BaseAddressAuthorizationMessageHandler>();
            baseAddressMessageHandler.InnerHandler = new HttpClientHandler();
            var grpcWebHandler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, baseAddressMessageHandler);

            var channel = GrpcChannel.ForAddress(builder.HostEnvironment.BaseAddress, new GrpcChannelOptions { HttpHandler = grpcWebHandler });

            return channel;
        }
    }
}
