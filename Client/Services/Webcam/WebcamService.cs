using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace ConfTool.Client.Services
{
    public class WebcamService : IWebcamService
    {
        private IJSRuntime _jsRuntime { get; }

        public WebcamService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task StartVideoAsync(WebcamOptions options)
        {
            await _jsRuntime.InvokeVoidAsync("interop.webcam.startVideo", options);
        }

        public async Task TakePictureAsync()
        {
            await _jsRuntime.InvokeVoidAsync("interop.webcam.takePicture");
        }
    }
}
