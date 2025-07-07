using System.Threading.Tasks;

namespace ProxyToggleApp.Services
{
    public interface IGitNpmService
    {
        Task<bool> SetGitProxyAsync(string proxyUrl);
        Task<bool> UnsetGitProxyAsync();
        Task<bool> SetNpmProxyAsync(string proxyUrl);
        Task<bool> UnsetNpmProxyAsync();
        Task<bool> IsGitAvailableAsync();
        Task<bool> IsNpmAvailableAsync();
    }
}
