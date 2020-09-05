using System.Threading.Tasks;

namespace ConfTool.Client.Services
{
    public interface IWebcamService
    {
        Task StartVideoAsync(WebcamOptions options);
        Task TakePictureAsync();
    }
}
