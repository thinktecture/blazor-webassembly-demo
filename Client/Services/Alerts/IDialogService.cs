using System.Threading.Tasks;

namespace ConfTool.Client.Services
{
    public interface IDialogService
    {
        Task<bool> ConfirmAsync(string message);
        Task AlertAsync(string message);
    }
}
