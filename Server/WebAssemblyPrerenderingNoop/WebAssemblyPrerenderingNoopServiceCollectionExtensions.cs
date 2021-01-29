using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace Thinktecture.WebAssembly.WebAssemblyPrerenderingNoop
{
    public static class WebAssemblyPrerenderingNoopAuthenticationServiceCollectionExtensions
    {
        public static void AddWebAssemblyPrerenderingNoopAuthentication(this IServiceCollection services)
        {
            services.AddRemoteAuthentication<RemoteAuthenticationState, RemoteUserAccount, OidcProviderOptions>();
            services.AddScoped<SignOutSessionStateManager>();
            services.AddScoped<AuthenticationStateProvider, WebAssemblyPrerenderingNoopRemoteAuthenticationService>();
            services.AddTransient<IJSRuntime, WebAssemblyPrerenderingNoopJSRuntime>();
        }
    }
}
