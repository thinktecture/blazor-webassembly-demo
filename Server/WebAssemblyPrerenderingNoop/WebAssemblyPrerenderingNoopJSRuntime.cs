using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Thinktecture.WebAssembly.WebAssemblyPrerenderingNoop
{
    public class WebAssemblyPrerenderingNoopJSRuntime : IJSRuntime
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