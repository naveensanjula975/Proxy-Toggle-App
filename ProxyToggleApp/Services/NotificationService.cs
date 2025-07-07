using System.Threading.Tasks;
using System.Windows;
using ProxyToggleApp.Models;
using ProxyToggleApp.Views;

namespace ProxyToggleApp.Services
{
    public class NotificationService : INotificationService
    {
        private readonly AppSettings _settings;
        private ToastNotification? _currentToast;

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
                    ShowToastNotification(message, NotificationType.Success);
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
                    ShowToastNotification(message, NotificationType.Error);
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
                    ShowToastNotification(message, NotificationType.Info);
                });
            });
        }

        private void ShowToastNotification(string message, NotificationType type)
        {
            // For now, use a simple MessageBox to prevent multiple windows
            // This ensures only one notification is shown at a time
            if (_currentToast != null)
            {
                return; // Don't show multiple notifications
            }

            // Set flag to prevent multiple notifications
            _currentToast = new ToastNotification(); // Just as a flag
            
            string title = type switch
            {
                NotificationType.Success => "Success",
                NotificationType.Error => "Error",
                NotificationType.Warning => "Warning",
                _ => "Information"
            };

            var icon = type switch
            {
                NotificationType.Success => MessageBoxImage.Information,
                NotificationType.Error => MessageBoxImage.Error,
                NotificationType.Warning => MessageBoxImage.Warning,
                _ => MessageBoxImage.Information
            };

            MessageBox.Show(message, title, MessageBoxButton.OK, icon);
            _currentToast = null; // Reset flag after showing
        }
    }
}
