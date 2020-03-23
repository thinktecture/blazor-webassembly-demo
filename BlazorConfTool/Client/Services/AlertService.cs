using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace BlazorConfTool.Client.Services
{
    public class AlertService : IAlertService
    {
        private IJSRuntime _jsRuntime { get; }

        public AlertService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<bool> ConfirmAsync(string message)
        {
            return await _jsRuntime.InvokeAsync<bool>("confToolInterop.dialogs.confirm", message);
        }

        public Task AlertAsync(string message)
        {
            throw new NotImplementedException();
        }
    }
}
