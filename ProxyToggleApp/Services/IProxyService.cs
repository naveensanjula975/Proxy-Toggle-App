using System.Threading.Tasks;

namespace ProxyToggleApp.Services
{
    public interface IProxyService
    {
        Task<bool> IsProxyEnabledAsync();
        Task<bool> EnableProxyAsync(string proxyServer, string? proxyOverride = null);
        Task<bool> DisableProxyAsync();
        Task<string> GetProxyServerAsync();
    }
}
