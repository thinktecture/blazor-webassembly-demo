using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Thinktecture.WebAssembly.WebAssemblyPrerenderingNoop
{
    public class WebAssemblyPrerenderingNoopRemoteAuthenticationService : ServerAuthenticationStateProvider, IRemoteAuthenticationService<RemoteAuthenticationState>
    {
        public Task<RemoteAuthenticationResult<RemoteAuthenticationState>> CompleteSignInAsync(RemoteAuthenticationContext<RemoteAuthenticationState> context)
        {
            return Success(context);
        }

        public Task<RemoteAuthenticationResult<RemoteAuthenticationState>> CompleteSignOutAsync(RemoteAuthenticationContext<RemoteAuthenticationState> context)
        {
            return Success(context);
        }

        public Task<RemoteAuthenticationResult<RemoteAuthenticationState>> SignInAsync(RemoteAuthenticationContext<RemoteAuthenticationState> context)
        {
            return Success(context);
        }

        public Task<RemoteAuthenticationResult<RemoteAuthenticationState>> SignOutAsync(RemoteAuthenticationContext<RemoteAuthenticationState> context)
        {
            return Success(context);
        }

        private static Task<RemoteAuthenticationResult<RemoteAuthenticationState>> Success(RemoteAuthenticationContext<RemoteAuthenticationState> context)
        {
            return Task.FromResult(new RemoteAuthenticationResult<RemoteAuthenticationState>
            {
                State = context.State,
                Status = RemoteAuthenticationStatus.Success
            });
        }
    }
}