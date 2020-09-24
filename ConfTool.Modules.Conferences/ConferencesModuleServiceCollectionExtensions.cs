using System;
using System.Net.Http;
using Blazored.Toast;
using ConfTool.Modules.Conferences.GrpcClient;
using ConfTool.Modules.Conferences.Services;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConfTool.Modules.Conferences
{
    public static class ConferencesModuleServiceCollectionExtensions
    {
        public static IServiceCollection AddConferencesModule(this IServiceCollection services)
        {
            services.AddScoped<IConferencesServiceClient, ConferencesServiceClientGrpc>();
            services.AddScoped<CountriesServiceClient>();

            var config = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

            services.AddHttpClient("ConfTool.ServerAPI.Anon", client =>
                client.BaseAddress = new Uri(config["BaseApiUrl"]));

            services.AddHttpClient("ConfTool.ServerAPI", client =>
                client.BaseAddress = new Uri(config["BaseApiUrl"]))
                    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            services.AddDialog();

            services.AddBlazoredToast();

            services.AddScoped<GrpcChannel>(services =>
            {
                var channel = BuildGrpcChannel(services);

                return channel;
            });

            services.AddScoped<CallInvoker>(services =>
            {
                var channel = BuildGrpcChannel(services);
                var invoker = channel.Intercept(new ClientLoggerInterceptor());

                return invoker;
            });

            return services;
        }

        private static GrpcChannel BuildGrpcChannel(IServiceProvider services)
        {
            var baseAddressMessageHandler = services.GetRequiredService<BaseAddressAuthorizationMessageHandler>();
            baseAddressMessageHandler.InnerHandler = new HttpClientHandler();
            var grpcWebHandler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, baseAddressMessageHandler);

            var config = services.GetRequiredService<IConfiguration>();
            var channel = GrpcChannel.ForAddress(config["BaseApiUrl"], new GrpcChannelOptions { HttpHandler = grpcWebHandler });

            return channel;
        }
    }
}
