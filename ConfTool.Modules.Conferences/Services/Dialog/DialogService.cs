using Microsoft.JSInterop;
using Nito.AsyncEx;
using System.Threading.Tasks;

namespace ConfTool.Modules.Conferences.Services
{
    public class DialogService : IDialogService
    {
        private static IJSRuntime _jsRuntime;
        private static AsyncLazy<JSObjectReference> _module = new AsyncLazy<JSObjectReference>(async () =>
        {
            return await _jsRuntime.InvokeAsync<JSObjectReference>("import", "./_content/ConfTool.Modules.Conferences/jsinterop/dialog.js");
        });

        public DialogService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<bool> ConfirmAsync(string message)
        {
            return await (await _module).InvokeAsync<bool>("confirm", message);
        }

        public async Task AlertAsync(string message)
        {
            await (await _module).InvokeVoidAsync("alert", message);
        }
    }
}
