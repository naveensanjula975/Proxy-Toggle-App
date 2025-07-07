using CommunityToolkit.Mvvm.ComponentModel;

namespace ProxyToggleApp.Models
{
    public partial class AppSettings : ObservableObject
    {
        [ObservableProperty]
        private bool _startWithWindows;

        [ObservableProperty]
        private bool _minimizeToTray;

        [ObservableProperty]
        private bool _showNotifications = true;

        [ObservableProperty]
        private string _defaultProxyServer = "127.0.0.1:8080";

        [ObservableProperty]
        private string _proxyOverride = "localhost;127.*;10.*;172.16.*;172.17.*;172.18.*;172.19.*;172.20.*;172.21.*;172.22.*;172.23.*;172.24.*;172.25.*;172.26.*;172.27.*;172.28.*;172.29.*;172.30.*;172.31.*;192.168.*";

        [ObservableProperty]
        private int _timeoutSeconds = 30;

        [ObservableProperty]
        private bool _enableGitProxy = true;

        [ObservableProperty]
        private bool _enableNpmProxy = true;

        [ObservableProperty]
        private bool _setEnvironmentVariables = true;

        [ObservableProperty]
        private bool _showDetailedErrors;
    }
}
