using System.Threading.Tasks;
using System.Windows;

namespace ProxyToggleApp.Services
{
    public class NotificationService : INotificationService
    {
        public async Task ShowSuccessAsync(string message)
        {
            await Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show(message, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                });
            });
        }

        public async Task ShowErrorAsync(string message)
        {
            await Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                });
            });
        }

        public async Task ShowInfoAsync(string message)
        {
            await Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show(message, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                });
            });
        }
    }
}
