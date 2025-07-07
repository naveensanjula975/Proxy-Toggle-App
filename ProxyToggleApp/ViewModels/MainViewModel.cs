using System;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using ProxyToggleApp.Services;
using ProxyToggleApp.Views;

namespace ProxyToggleApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IProxyService _proxyService;
        private readonly IGitNpmService _gitNpmService;
        private readonly INotificationService _notificationService;
        private readonly ILogger<MainViewModel> _logger;

        [ObservableProperty]
        private bool _isProxyEnabled;

        [ObservableProperty]
        private string _proxyServer = "127.0.0.1:8080";

        [ObservableProperty]
        private string _statusText = "Checking proxy status...";

        [ObservableProperty]
        private bool _isOperationInProgress;

        public ICommand EnableProxyCommand { get; }
        public ICommand DisableProxyCommand { get; }
        public ICommand RefreshStatusCommand { get; }
        public ICommand OpenSettingsCommand { get; }

        public MainViewModel(
            IProxyService proxyService,
            IGitNpmService gitNpmService,
            INotificationService notificationService,
            ILogger<MainViewModel> logger)
        {
            _proxyService = proxyService;
            _gitNpmService = gitNpmService;
            _notificationService = notificationService;
            _logger = logger;

            EnableProxyCommand = new AsyncRelayCommand(EnableProxyAsync, () => !IsOperationInProgress);
            DisableProxyCommand = new AsyncRelayCommand(DisableProxyAsync, () => !IsOperationInProgress);
            RefreshStatusCommand = new AsyncRelayCommand(RefreshStatusAsync, () => !IsOperationInProgress);
            OpenSettingsCommand = new RelayCommand(OpenSettings);

            // Load initial status
            _ = Task.Run(RefreshStatusAsync);
        }

        private async Task EnableProxyAsync()
        {
            if (string.IsNullOrWhiteSpace(ProxyServer))
            {
                await _notificationService.ShowErrorAsync("Please enter a proxy server address.");
                return;
            }

            IsOperationInProgress = true;
            StatusText = "Enabling proxy...";

            try
            {
                var success = true;

                // Enable Windows proxy
                var proxyResult = await _proxyService.EnableProxyAsync(ProxyServer);
                if (!proxyResult)
                {
                    success = false;
                    _logger.LogError("Failed to enable Windows proxy");
                }

                // Enable Git proxy if available
                if (await _gitNpmService.IsGitAvailableAsync())
                {
                    var gitResult = await _gitNpmService.SetGitProxyAsync(ProxyServer);
                    if (!gitResult)
                    {
                        success = false;
                        _logger.LogError("Failed to set Git proxy");
                    }
                }

                // Enable npm proxy if available
                if (await _gitNpmService.IsNpmAvailableAsync())
                {
                    var npmResult = await _gitNpmService.SetNpmProxyAsync(ProxyServer);
                    if (!npmResult)
                    {
                        success = false;
                        _logger.LogError("Failed to set npm proxy");
                    }
                }

                if (success)
                {
                    await _notificationService.ShowSuccessAsync("Proxy enabled successfully!");
                    await RefreshStatusAsync();
                }
                else
                {
                    await _notificationService.ShowErrorAsync("Some proxy settings failed to apply. Check logs for details.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error enabling proxy");
                await _notificationService.ShowErrorAsync($"Error enabling proxy: {ex.Message}");
            }
            finally
            {
                IsOperationInProgress = false;
            }
        }

        private async Task DisableProxyAsync()
        {
            IsOperationInProgress = true;
            StatusText = "Disabling proxy...";

            try
            {
                var success = true;

                // Disable Windows proxy
                var proxyResult = await _proxyService.DisableProxyAsync();
                if (!proxyResult)
                {
                    success = false;
                    _logger.LogError("Failed to disable Windows proxy");
                }

                // Disable Git proxy if available
                if (await _gitNpmService.IsGitAvailableAsync())
                {
                    var gitResult = await _gitNpmService.UnsetGitProxyAsync();
                    if (!gitResult)
                    {
                        success = false;
                        _logger.LogError("Failed to unset Git proxy");
                    }
                }

                // Disable npm proxy if available
                if (await _gitNpmService.IsNpmAvailableAsync())
                {
                    var npmResult = await _gitNpmService.UnsetNpmProxyAsync();
                    if (!npmResult)
                    {
                        success = false;
                        _logger.LogError("Failed to unset npm proxy");
                    }
                }

                if (success)
                {
                    await _notificationService.ShowSuccessAsync("Proxy disabled successfully!");
                    await RefreshStatusAsync();
                }
                else
                {
                    await _notificationService.ShowErrorAsync("Some proxy settings failed to remove. Check logs for details.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error disabling proxy");
                await _notificationService.ShowErrorAsync($"Error disabling proxy: {ex.Message}");
            }
            finally
            {
                IsOperationInProgress = false;
            }
        }

        private async Task RefreshStatusAsync()
        {
            IsOperationInProgress = true;
            StatusText = "Checking proxy status...";

            try
            {
                IsProxyEnabled = await _proxyService.IsProxyEnabledAsync();
                var currentServer = await _proxyService.GetProxyServerAsync();
                
                if (IsProxyEnabled)
                {
                    StatusText = $"Proxy Enabled: {currentServer}";
                    if (!string.IsNullOrEmpty(currentServer))
                    {
                        ProxyServer = currentServer;
                    }
                }
                else
                {
                    StatusText = "Proxy Disabled";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refreshing proxy status");
                StatusText = "Error checking proxy status";
            }
            finally
            {
                IsOperationInProgress = false;
            }
        }

        private void OpenSettings()
        {
            var settingsDialog = new SettingsDialog();
            settingsDialog.ShowDialog();
        }
    }
}
