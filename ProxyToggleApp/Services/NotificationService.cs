using System.Threading.Tasks;
using System.Windows;
using ProxyToggleApp.Models;

namespace ProxyToggleApp.Services
{
    public class NotificationService : INotificationService
    {
        private readonly AppSettings _settings;

        public NotificationService(AppSettings settings)
        {
            _settings = settings;
        }

        public async Task ShowSuccessAsync(string message)
        {
            if (!_settings.ShowNotifications) return;

            await Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (_settings.ShowDetailedErrors)
                    {
                        MessageBox.Show(message, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        // TODO: Implement toast notification
                        MessageBox.Show(message, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                });
            });
        }

        public async Task ShowErrorAsync(string message)
        {
            if (!_settings.ShowNotifications) return;

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
            if (!_settings.ShowNotifications) return;

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
