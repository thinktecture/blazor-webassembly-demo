using Microsoft.JSInterop;
using Nito.AsyncEx;
using System;
using System.Threading.Tasks;

namespace ConfTool.Client.Services
{
    public class DialogService : IDialogService
    {
        private static IJSRuntime _jsRuntime;
        private static AsyncLazy<JSObjectReference> _module = new AsyncLazy<JSObjectReference>(async () =>
        {
            return await _jsRuntime.InvokeAsync<JSObjectReference>("import", "./jsinterop/dialog.js");
        });

        public DialogService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<bool> ConfirmAsync(string message)
        {
            var module = await _module;

            return await module.InvokeAsync<bool>("confirm", message);
        }

        public Task AlertAsync(string message)
        {
            throw new NotImplementedException();
        }
    }
}
