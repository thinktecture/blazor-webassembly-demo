using System.Threading.Tasks;

namespace ConfTool.Modules.Conferences.Services
{
    public interface IDialogService
    {
        Task<bool> ConfirmAsync(string message);
        Task AlertAsync(string message);
    }
}
