using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using ProxyToggleApp.Services;
using ProxyToggleApp.Views;
using ProxyToggleApp.Models;

namespace ProxyToggleApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IProxyService _proxyService;
        private readonly IGitNpmService _gitNpmService;
        private readonly INotificationService _notificationService;
        private readonly ISettingsService _settingsService;
        private readonly ILogger<MainViewModel> _logger;
        private readonly AppSettings _settings;

        [ObservableProperty]
        private bool _isProxyEnabled;

        [ObservableProperty]
        private string _proxyServer = "127.0.0.1:8080";

        [ObservableProperty]
        private string _statusText = "Checking proxy status...";

        [ObservableProperty]
        private bool _isOperationInProgress;

        [ObservableProperty]
        private string _detailedStatus = string.Empty;

        public ICommand EnableProxyCommand { get; }
        public ICommand DisableProxyCommand { get; }
        public ICommand RefreshStatusCommand { get; }
        public ICommand OpenSettingsCommand { get; }

        public MainViewModel(
            IProxyService proxyService,
            IGitNpmService gitNpmService,
            INotificationService notificationService,
            ISettingsService settingsService,
            ILogger<MainViewModel> logger,
            AppSettings settings)
        {
            _proxyService = proxyService;
            _gitNpmService = gitNpmService;
            _notificationService = notificationService;
            _settingsService = settingsService;
            _logger = logger;
            _settings = settings;

            EnableProxyCommand = new AsyncRelayCommand(EnableProxyAsync, () => !IsOperationInProgress);
            DisableProxyCommand = new AsyncRelayCommand(DisableProxyAsync, () => !IsOperationInProgress);
            RefreshStatusCommand = new AsyncRelayCommand(RefreshStatusAsync, () => !IsOperationInProgress);
            OpenSettingsCommand = new AsyncRelayCommand(OpenSettingsAsync);

            // Load settings and initial status
            _ = Task.Run(async () => 
            {
                await LoadSettingsAsync();
                await RefreshStatusAsync();
            });
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
            DetailedStatus = "Starting proxy configuration...";

            try
            {
                var success = true;
                var errors = new List<string>();

                // Enable Windows proxy
                DetailedStatus = "Configuring Windows proxy settings...";
                var proxyResult = await _proxyService.EnableProxyAsync(ProxyServer);
                if (!proxyResult)
                {
                    success = false;
                    errors.Add("Failed to enable Windows proxy");
                    _logger.LogError("Failed to enable Windows proxy");
                }

                // Enable Git proxy if available and enabled
                if (_settings.EnableGitProxy && await _gitNpmService.IsGitAvailableAsync())
                {
                    DetailedStatus = "Configuring Git proxy settings...";
                    var gitResult = await _gitNpmService.SetGitProxyAsync(ProxyServer);
                    if (!gitResult)
                    {
                        success = false;
                        errors.Add("Failed to set Git proxy");
                        _logger.LogError("Failed to set Git proxy");
                    }
                }

                // Enable npm proxy if available and enabled
                if (_settings.EnableNpmProxy && await _gitNpmService.IsNpmAvailableAsync())
                {
                    DetailedStatus = "Configuring npm proxy settings...";
                    var npmResult = await _gitNpmService.SetNpmProxyAsync(ProxyServer);
                    if (!npmResult)
                    {
                        success = false;
                        errors.Add("Failed to set npm proxy");
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
                    var errorMessage = _settings.ShowDetailedErrors 
                        ? $"Some proxy settings failed:\n{string.Join("\n", errors)}"
                        : "Some proxy settings failed to apply. Check logs for details.";
                    await _notificationService.ShowErrorAsync(errorMessage);
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
                DetailedStatus = string.Empty;
            }
        }

        private async Task DisableProxyAsync()
        {
            IsOperationInProgress = true;
            StatusText = "Disabling proxy...";
            DetailedStatus = "Starting proxy removal...";

            try
            {
                var success = true;
                var errors = new List<string>();

                // Disable Windows proxy
                DetailedStatus = "Removing Windows proxy settings...";
                var proxyResult = await _proxyService.DisableProxyAsync();
                if (!proxyResult)
                {
                    success = false;
                    errors.Add("Failed to disable Windows proxy");
                    _logger.LogError("Failed to disable Windows proxy");
                }

                // Disable Git proxy if available and enabled
                if (_settings.EnableGitProxy && await _gitNpmService.IsGitAvailableAsync())
                {
                    DetailedStatus = "Removing Git proxy settings...";
                    var gitResult = await _gitNpmService.UnsetGitProxyAsync();
                    if (!gitResult)
                    {
                        success = false;
                        errors.Add("Failed to unset Git proxy");
                        _logger.LogError("Failed to unset Git proxy");
                    }
                }

                // Disable npm proxy if available and enabled
                if (_settings.EnableNpmProxy && await _gitNpmService.IsNpmAvailableAsync())
                {
                    DetailedStatus = "Removing npm proxy settings...";
                    var npmResult = await _gitNpmService.UnsetNpmProxyAsync();
                    if (!npmResult)
                    {
                        success = false;
                        errors.Add("Failed to unset npm proxy");
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
                    var errorMessage = _settings.ShowDetailedErrors 
                        ? $"Some proxy settings failed to remove:\n{string.Join("\n", errors)}"
                        : "Some proxy settings failed to remove. Check logs for details.";
                    await _notificationService.ShowErrorAsync(errorMessage);
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
                DetailedStatus = string.Empty;
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

        private async Task OpenSettingsAsync()
        {
            var settingsDialog = new SettingsDialog();
            settingsDialog.DataContext = _settings;
            
            if (settingsDialog.ShowDialog() == true)
            {
                await _settingsService.SaveSettingsAsync(_settings);
                await _notificationService.ShowSuccessAsync("Settings saved successfully!");
            }
        }

        private async Task LoadSettingsAsync()
        {
            try
            {
                var loadedSettings = await _settingsService.LoadSettingsAsync();
                
                // Update UI with loaded settings
                ProxyServer = loadedSettings.DefaultProxyServer;
                
                _logger.LogInformation("Settings loaded successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading settings");
            }
        }
    }
}
