using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Eits.ThinkTecture.Blazor.Prerenderer.PrerenderAuth
{
    public class JSRuntime : IJSRuntime
    {
        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object[] args)
        {
            return new ValueTask<TValue>();
        }

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object[] args)
        {
            return new ValueTask<TValue>();
        }
    }
}