using System.Threading.Tasks;

namespace BlazorConfTool.Client.Services
{
    public interface IAlertService
    {
        Task<bool> ConfirmAsync(string message);
        Task AlertAsync(string message);
    }
}
