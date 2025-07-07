using System.Threading.Tasks;

namespace ProxyToggleApp.Services
{
    public interface INotificationService
    {
        Task ShowSuccessAsync(string message);
        Task ShowErrorAsync(string message);
        Task ShowInfoAsync(string message);
    }
}
