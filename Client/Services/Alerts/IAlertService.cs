using System.Threading.Tasks;

namespace ConfTool.Client.Services
{
    public interface IAlertService
    {
        Task<bool> ConfirmAsync(string message);
        Task AlertAsync(string message);
    }
}
