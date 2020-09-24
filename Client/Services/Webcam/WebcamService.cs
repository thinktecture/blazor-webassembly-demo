using Microsoft.JSInterop;
using Nito.AsyncEx;
using System.Threading.Tasks;

namespace ConfTool.Client.Services
{
    public class WebcamService : IWebcamService
    {
        private static IJSRuntime _jsRuntime;
        private static AsyncLazy<JSObjectReference> _module = new AsyncLazy<JSObjectReference>(async () =>
        {
            return await _jsRuntime.InvokeAsync<JSObjectReference>("import", "./jsinterop/webcam.js");
        });

        public WebcamService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task StartVideoAsync(WebcamOptions options)
        {
            await (await _module).InvokeVoidAsync("startVideo", options);
        }

        public async Task TakePictureAsync()
        {
            await (await _module).InvokeVoidAsync("takePicture");
        }
    }
}
