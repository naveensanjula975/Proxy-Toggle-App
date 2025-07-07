using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProxyToggleApp.Services;
using ProxyToggleApp.ViewModels;
using ProxyToggleApp.Views;
using ProxyToggleApp.Models;

namespace ProxyToggleApp
{
    public partial class App : Application
    {
        private IHost? _host;

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        protected override async void OnStartup(StartupEventArgs e)
        {
            // Check if another instance is already running
            string processName = Process.GetCurrentProcess().ProcessName;
            Process[] processes = Process.GetProcessesByName(processName);
            
            if (processes.Length > 1)
            {
                // Another instance is already running, bring it to front and exit
                var runningProcess = processes.FirstOrDefault(p => p.Id != Process.GetCurrentProcess().Id);
                if (runningProcess != null)
                {
                    // Try to bring the existing window to front
                    ShowWindow(runningProcess.MainWindowHandle, 9); // SW_RESTORE
                    SetForegroundWindow(runningProcess.MainWindowHandle);
                }
                Shutdown();
                return;
            }

            _host = CreateHostBuilder().Build();
            await _host.StartAsync();

            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            var viewModel = _host.Services.GetRequiredService<MainViewModel>();
            mainWindow.SetViewModel(viewModel);
            mainWindow.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            if (_host != null)
            {
                await _host.StopAsync();
                _host.Dispose();
            }
            base.OnExit(e);
        }

        private static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // Register settings as singleton
                    services.AddSingleton<AppSettings>();
                    
                    // Register services
                    services.AddSingleton<IProxyService, ProxyService>();
                    services.AddSingleton<IGitNpmService, GitNpmService>();
                    services.AddSingleton<INotificationService, NotificationService>();
                    services.AddSingleton<ISettingsService, SettingsService>();
                    services.AddSingleton<ISystemTrayService, SystemTrayService>();

                    // Register ViewModels
                    services.AddTransient<MainViewModel>();

                    // Register Views
                    services.AddTransient<MainWindow>();
                });
        }
    }
}
