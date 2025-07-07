using System;
using System.Threading.Tasks;
using Microsoft.Win32;
using Microsoft.Extensions.Logging;

namespace ProxyToggleApp.Services
{
    public class ProxyService : IProxyService
    {
        private readonly ILogger<ProxyService> _logger;
        private const string REGISTRY_PATH = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Internet Settings";
        private const string PROXY_ENABLE_KEY = "ProxyEnable";
        private const string PROXY_SERVER_KEY = "ProxyServer";
        private const string PROXY_OVERRIDE_KEY = "ProxyOverride";

        public ProxyService(ILogger<ProxyService> logger)
        {
            _logger = logger;
        }

        public async Task<bool> IsProxyEnabledAsync()
        {
            return await Task.Run(() =>
            {
                try
                {
                    using var key = Registry.CurrentUser.OpenSubKey(REGISTRY_PATH);
                    var proxyEnable = key?.GetValue(PROXY_ENABLE_KEY);
                    return proxyEnable is int value && value == 1;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error checking proxy status");
                    return false;
                }
            });
        }

        public async Task<bool> EnableProxyAsync(string proxyServer, string? proxyOverride = null)
        {
            return await Task.Run(() =>
            {
                try
                {
                    using var key = Registry.CurrentUser.OpenSubKey(REGISTRY_PATH, true);
                    if (key == null)
                    {
                        _logger.LogError("Cannot open registry key for writing");
                        return false;
                    }

                    key.SetValue(PROXY_ENABLE_KEY, 1, RegistryValueKind.DWord);
                    key.SetValue(PROXY_SERVER_KEY, proxyServer, RegistryValueKind.String);
                    
                    if (!string.IsNullOrEmpty(proxyOverride))
                    {
                        key.SetValue(PROXY_OVERRIDE_KEY, proxyOverride, RegistryValueKind.String);
                    }

                    // Set environment variables
                    Environment.SetEnvironmentVariable("HTTP_PROXY", proxyServer, EnvironmentVariableTarget.User);
                    Environment.SetEnvironmentVariable("HTTPS_PROXY", proxyServer, EnvironmentVariableTarget.User);

                    _logger.LogInformation("Proxy enabled successfully");
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error enabling proxy");
                    return false;
                }
            });
        }

        public async Task<bool> DisableProxyAsync()
        {
            return await Task.Run(() =>
            {
                try
                {
                    using var key = Registry.CurrentUser.OpenSubKey(REGISTRY_PATH, true);
                    if (key == null)
                    {
                        _logger.LogError("Cannot open registry key for writing");
                        return false;
                    }

                    key.SetValue(PROXY_ENABLE_KEY, 0, RegistryValueKind.DWord);
                    key.DeleteValue(PROXY_SERVER_KEY, false);
                    key.DeleteValue(PROXY_OVERRIDE_KEY, false);

                    // Remove environment variables
                    Environment.SetEnvironmentVariable("HTTP_PROXY", null, EnvironmentVariableTarget.User);
                    Environment.SetEnvironmentVariable("HTTPS_PROXY", null, EnvironmentVariableTarget.User);

                    _logger.LogInformation("Proxy disabled successfully");
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error disabling proxy");
                    return false;
                }
            });
        }

        public async Task<string> GetProxyServerAsync()
        {
            return await Task.Run(() =>
            {
                try
                {
                    using var key = Registry.CurrentUser.OpenSubKey(REGISTRY_PATH);
                    var proxyServer = key?.GetValue(PROXY_SERVER_KEY);
                    return proxyServer?.ToString() ?? string.Empty;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting proxy server");
                    return string.Empty;
                }
            });
        }
    }
}
