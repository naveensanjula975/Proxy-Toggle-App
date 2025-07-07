using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using ProxyToggleApp.Models;

namespace ProxyToggleApp.Services
{
    public interface ISettingsService
    {
        Task<AppSettings> LoadSettingsAsync();
        Task SaveSettingsAsync(AppSettings settings);
        Task<bool> SettingsExistAsync();
        AppSettings GetDefaultSettings();
    }

    public class SettingsService : ISettingsService
    {
        private readonly string _settingsPath;
        private const string SettingsFileName = "appsettings.json";

        public SettingsService()
        {
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var appFolder = Path.Combine(appDataPath, "ProxyToggleApp");
            Directory.CreateDirectory(appFolder);
            _settingsPath = Path.Combine(appFolder, SettingsFileName);
        }

        public async Task<AppSettings> LoadSettingsAsync()
        {
            try
            {
                if (File.Exists(_settingsPath))
                {
                    var json = await File.ReadAllTextAsync(_settingsPath);
                    var settings = JsonSerializer.Deserialize<AppSettings>(json);
                    return settings ?? GetDefaultSettings();
                }
            }
            catch (Exception ex)
            {
                // Log exception if needed
                System.Diagnostics.Debug.WriteLine($"Failed to load settings: {ex.Message}");
            }

            return GetDefaultSettings();
        }

        public async Task SaveSettingsAsync(AppSettings settings)
        {
            try
            {
                var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                await File.WriteAllTextAsync(_settingsPath, json);
            }
            catch (Exception ex)
            {
                // Log exception if needed
                System.Diagnostics.Debug.WriteLine($"Failed to save settings: {ex.Message}");
            }
        }

        public async Task<bool> SettingsExistAsync()
        {
            return await Task.FromResult(File.Exists(_settingsPath));
        }

        public AppSettings GetDefaultSettings()
        {
            return new AppSettings();
        }
    }
}
