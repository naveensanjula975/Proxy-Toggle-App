using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProxyToggleApp.Services;
using ProxyToggleApp.ViewModels;
using ProxyToggleApp.Views;

namespace ProxyToggleApp
{
    public partial class App : Application
    {
        private IHost? _host;

        protected override async void OnStartup(StartupEventArgs e)
        {
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
                    // Register services
                    services.AddSingleton<IProxyService, ProxyService>();
                    services.AddSingleton<IGitNpmService, GitNpmService>();
                    services.AddSingleton<INotificationService, NotificationService>();

                    // Register ViewModels
                    services.AddTransient<MainViewModel>();

                    // Register Views
                    services.AddTransient<MainWindow>();
                });
        }
    }
}
