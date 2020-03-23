using BlazorConfTool.Client.Services;
using Blazored.Toast;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using GrpcGreeter;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Sotsera.Blazor.Oidc;
using System;
using System.Net.Http;

namespace BlazorConfTool.Client
{
    public class ClientStartup
    {
        public static void PopulateServices(IServiceCollection services)
        {
            services.AddBaseAddressHttpClient();

            services.AddScoped<ConferencesService>();
            services.AddScoped<CountriesService>();

            services.AddSingleton(services =>
            {
                var httpClient = new HttpClient(
                    new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
                var baseUri = services.GetRequiredService<NavigationManager>().BaseUri;
                var channel = GrpcChannel.ForAddress(
                    baseUri, new GrpcChannelOptions { HttpClient = httpClient });

                return new Greeter.GreeterClient(channel);
            });

            services.AddAlerts();

            services.AddBlazoredToast();

            services.AddOptions();
            services.AddOidc(new Uri("https://demo.identityserver.io"), (settings, siteUri) =>
            {
                settings.UseDefaultCallbackUris(siteUri);
                settings.ClientId = "spa";
                settings.ResponseType = "code";
                settings.Scope = "openid profile email api";
            });
        }
    }
}
