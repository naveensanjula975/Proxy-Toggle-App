using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;
using ProxyToggleApp.Models;
using ProxyToggleApp.ViewModels;
using CommunityToolkit.Mvvm.Input;
using Application = System.Windows.Application;

namespace ProxyToggleApp.Services
{
    public interface ISystemTrayService
    {
        void Initialize(MainViewModel viewModel);
        void ShowTrayIcon();
        void HideTrayIcon();
        void UpdateTrayIcon(bool isProxyEnabled);
        void Dispose();
    }

    public class SystemTrayService : ISystemTrayService, IDisposable
    {
        private readonly ILogger<SystemTrayService> _logger;
        private readonly AppSettings _settings;
        private NotifyIcon? _notifyIcon;
        private MainViewModel? _viewModel;
        private bool _disposed;

        public SystemTrayService(ILogger<SystemTrayService> logger, AppSettings settings)
        {
            _logger = logger;
            _settings = settings;
        }

        public void Initialize(MainViewModel viewModel)
        {
            _viewModel = viewModel;
            
            if (_settings.MinimizeToTray)
            {
                CreateTrayIcon();
            }
        }

        public void ShowTrayIcon()
        {
            if (_notifyIcon == null)
            {
                CreateTrayIcon();
            }
            
            if (_notifyIcon != null)
            {
                _notifyIcon.Visible = true;
            }
        }

        public void HideTrayIcon()
        {
            if (_notifyIcon != null)
            {
                _notifyIcon.Visible = false;
            }
        }

        public void UpdateTrayIcon(bool isProxyEnabled)
        {
            if (_notifyIcon == null) return;

            _notifyIcon.Text = isProxyEnabled ? "Proxy Toggle - Enabled" : "Proxy Toggle - Disabled";
            
            // You would typically load different icons here for enabled/disabled states
            // For now, we'll just update the tooltip text
        }

        private void CreateTrayIcon()
        {
            try
            {
                _notifyIcon = new NotifyIcon
                {
                    Text = "Proxy Toggle",
                    Visible = true,
                    ContextMenuStrip = CreateContextMenu()
                };

                // Create a simple icon (you would typically load from resources)
                var bitmap = new Bitmap(16, 16);
                using (var g = Graphics.FromImage(bitmap))
                {
                    g.Clear(Color.Blue);
                    g.FillEllipse(Brushes.White, 4, 4, 8, 8);
                }
                _notifyIcon.Icon = Icon.FromHandle(bitmap.GetHicon());

                _notifyIcon.DoubleClick += (s, e) => ShowMainWindow();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create system tray icon");
            }
        }

        private ContextMenuStrip CreateContextMenu()
        {
            var contextMenu = new ContextMenuStrip();
            
            contextMenu.Items.Add("Show", null, (s, e) => ShowMainWindow());
            contextMenu.Items.Add(new ToolStripSeparator());
            contextMenu.Items.Add("Enable Proxy", null, async (s, e) => await EnableProxyAsync());
            contextMenu.Items.Add("Disable Proxy", null, async (s, e) => await DisableProxyAsync());
            contextMenu.Items.Add(new ToolStripSeparator());
            contextMenu.Items.Add("Exit", null, (s, e) => ExitApplication());
            
            return contextMenu;
        }

        private void ShowMainWindow()
        {
            var mainWindow = Application.Current.MainWindow;
            if (mainWindow != null)
            {
                mainWindow.Show();
                mainWindow.WindowState = WindowState.Normal;
                mainWindow.Activate();
            }
        }

        private async Task EnableProxyAsync()
        {
            if (_viewModel != null && _viewModel.EnableProxyCommand.CanExecute(null))
            {
                await ((AsyncRelayCommand)_viewModel.EnableProxyCommand).ExecuteAsync(null);
            }
        }

        private async Task DisableProxyAsync()
        {
            if (_viewModel != null && _viewModel.DisableProxyCommand.CanExecute(null))
            {
                await ((AsyncRelayCommand)_viewModel.DisableProxyCommand).ExecuteAsync(null);
            }
        }

        private void ExitApplication()
        {
            Application.Current.Shutdown();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _notifyIcon?.Dispose();
                _disposed = true;
            }
        }
    }
}
