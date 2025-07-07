using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ProxyToggleApp.Services
{
    public class GitNpmService : IGitNpmService
    {
        private readonly ILogger<GitNpmService> _logger;

        public GitNpmService(ILogger<GitNpmService> logger)
        {
            _logger = logger;
        }

        public async Task<bool> SetGitProxyAsync(string proxyUrl)
        {
            try
            {
                var result = await ExecuteCommandAsync("git", $"config --global http.proxy {proxyUrl}");
                if (result.Success)
                {
                    _logger.LogInformation("Git proxy set successfully");
                    return true;
                }
                else
                {
                    _logger.LogError("Failed to set Git proxy: {Error}", result.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting Git proxy");
                return false;
            }
        }

        public async Task<bool> UnsetGitProxyAsync()
        {
            try
            {
                var result = await ExecuteCommandAsync("git", "config --global --unset http.proxy");
                if (result.Success)
                {
                    _logger.LogInformation("Git proxy unset successfully");
                    return true;
                }
                else
                {
                    _logger.LogError("Failed to unset Git proxy: {Error}", result.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error unsetting Git proxy");
                return false;
            }
        }

        public async Task<bool> SetNpmProxyAsync(string proxyUrl)
        {
            try
            {
                var result = await ExecuteCommandAsync("npm", $"config set proxy {proxyUrl}");
                if (result.Success)
                {
                    _logger.LogInformation("npm proxy set successfully");
                    return true;
                }
                else
                {
                    _logger.LogError("Failed to set npm proxy: {Error}", result.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting npm proxy");
                return false;
            }
        }

        public async Task<bool> UnsetNpmProxyAsync()
        {
            try
            {
                var result = await ExecuteCommandAsync("npm", "config delete proxy");
                if (result.Success)
                {
                    _logger.LogInformation("npm proxy unset successfully");
                    return true;
                }
                else
                {
                    _logger.LogError("Failed to unset npm proxy: {Error}", result.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error unsetting npm proxy");
                return false;
            }
        }

        public async Task<bool> IsGitAvailableAsync()
        {
            try
            {
                var result = await ExecuteCommandAsync("git", "--version");
                return result.Success;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> IsNpmAvailableAsync()
        {
            try
            {
                var result = await ExecuteCommandAsync("npm", "--version");
                return result.Success;
            }
            catch
            {
                return false;
            }
        }

        private async Task<(bool Success, string Output, string Error)> ExecuteCommandAsync(string command, string arguments)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var processInfo = new ProcessStartInfo
                    {
                        FileName = command,
                        Arguments = arguments,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    };

                    using var process = Process.Start(processInfo);
                    if (process == null)
                    {
                        return (false, string.Empty, "Failed to start process");
                    }

                    process.WaitForExit();
                    var output = process.StandardOutput.ReadToEnd();
                    var error = process.StandardError.ReadToEnd();

                    return (process.ExitCode == 0, output, error);
                }
                catch (Exception ex)
                {
                    return (false, string.Empty, ex.Message);
                }
            });
        }
    }
}
