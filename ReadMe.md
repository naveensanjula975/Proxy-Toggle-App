# Proxy Toggle App

![.NET 6.0](https://img.shields.io/badge/.NET-6.0-purple)
![WPF](https://img.shields.io/badge/WPF-Windows%20Presentation%20Foundation-blue)
![License](https://img.shields.io/badge/license-MIT-green)

A modern Windows application for managing proxy configurations with support for system-wide proxy settings, Git, and npm proxy configurations.

## 🚀 Features

### Core Functionality
- **System-wide proxy management** - Enable/disable Windows proxy settings
- **Git proxy integration** - Automatically configure Git proxy settings
- **npm proxy integration** - Manage npm proxy configurations
- **Real-time status monitoring** - Live proxy status with visual indicators
- **Single instance protection** - Prevents multiple app instances

### User Experience
- **Modern Fluent Design UI** - Clean, intuitive interface with smooth animations
- **System tray integration** - Minimize to tray with quick access menu
- **Toast notifications** - Non-intrusive status updates
- **Settings persistence** - All configurations saved automatically
- **Settings dialog** - Comprehensive configuration options

### Advanced Features
- **MVVM Architecture** - Clean separation of concerns with dependency injection
- **Async operations** - Non-blocking UI with proper error handling
- **Configurable timeouts** - Adjustable operation timeouts
- **Proxy override lists** - Bypass proxy for specific addresses
- **Auto-startup option** - Start with Windows (optional)

## 📋 Requirements

- **Operating System:** Windows 10/11
- **Framework:** .NET 6.0 Runtime
- **Dependencies:** Automatically managed via NuGet

## 🛠️ Installation

### Option 1: Download Release
1. Download the latest release from the [Releases](../../releases) page
2. Extract the zip file
3. Run `ProxyToggleApp.exe`

### Option 2: Build from Source
1. Clone the repository:
   ```bash
   git clone <repository-url>
   cd "proxy win/ProxyToggleApp"
   ```

2. Build the application:
   ```bash
   dotnet build --configuration Release
   ```

3. Run the application:
   ```bash
   dotnet run
   ```

## 🎯 Usage

### Basic Operations

1. **Enable Proxy:**
   - Enter your proxy server address (e.g., `127.0.0.1:8080`)
   - Click "Enable Proxy"
   - Green status indicator confirms activation

2. **Disable Proxy:**
   - Click "Disable Proxy"
   - Red status indicator confirms deactivation

3. **Configure Settings:**
   - Click the Settings button (⚙️)
   - Adjust proxy settings, timeouts, and preferences
   - Settings are saved automatically

### System Tray Features
- **Right-click tray icon** for quick actions:
  - Enable/Disable Proxy
  - Open Main Window
  - Exit Application

### Proxy Configuration Examples
- **Local proxy:** `127.0.0.1:8080`
- **Corporate proxy:** `proxy.company.com:3128`
- **SOCKS proxy:** `127.0.0.1:1080`

## ⚙️ Configuration

### Settings Options

| Setting | Description | Default |
|---------|-------------|---------|
| **Default Proxy Server** | Primary proxy server address | `127.0.0.1:8080` |
| **Proxy Override** | Addresses to bypass proxy | Local networks |
| **Timeout** | Operation timeout in seconds | 30 |
| **Start with Windows** | Auto-start with system | Disabled |
| **Minimize to Tray** | Minimize to system tray | Disabled |
| **Show Notifications** | Display status notifications | Enabled |
| **Enable Git Proxy** | Manage Git proxy settings | Enabled |
| **Enable npm Proxy** | Manage npm proxy settings | Enabled |

### Configuration File
Settings are stored in:
```
%LocalAppData%\ProxyToggleApp\appsettings.json
```

## 🏗️ Architecture

### Project Structure
```
ProxyToggleApp/
├── Models/
│   └── AppSettings.cs           # Application settings model
├── Services/
│   ├── IProxyService.cs         # Proxy management interface
│   ├── ProxyService.cs          # Windows proxy implementation
│   ├── IGitNpmService.cs        # Git/npm proxy interface
│   ├── GitNpmService.cs         # Git/npm proxy implementation
│   ├── INotificationService.cs  # Notification interface
│   ├── NotificationService.cs   # Notification implementation
│   ├── SettingsService.cs       # Settings persistence
│   └── SystemTrayService.cs     # System tray integration
├── ViewModels/
│   └── MainViewModel.cs         # Main window view model
├── Views/
│   ├── MainWindow.xaml          # Main application window
│   ├── SettingsDialog.xaml      # Settings configuration dialog
│   ├── ToastNotification.xaml   # Toast notification component
│   └── [Converters & Controls]  # UI helpers and custom controls
├── App.xaml                     # Application entry point
└── ProxyToggleApp.csproj        # Project configuration
```

### Key Technologies
- **WPF (Windows Presentation Foundation)** - UI framework
- **MVVM Pattern** - Clean architecture with data binding
- **Dependency Injection** - Microsoft.Extensions.DependencyInjection
- **MVVM Toolkit** - CommunityToolkit.Mvvm for observable properties
- **Async/Await** - Non-blocking operations
- **Windows Registry** - System proxy configuration
- **Process Management** - Git/npm proxy configuration

## 🔧 Development

### Prerequisites
- Visual Studio 2022 or VS Code
- .NET 6.0 SDK
- Windows 10/11 development environment

### Building
```bash
# Debug build
dotnet build

# Release build
dotnet build --configuration Release

# Run tests (if available)
dotnet test

# Publish self-contained
dotnet publish --configuration Release --self-contained
```

### Contributing
1. Fork the repository
2. Create a feature branch: `git checkout -b feature/amazing-feature`
3. Commit changes: `git commit -m 'Add amazing feature'`
4. Push to branch: `git push origin feature/amazing-feature`
5. Open a Pull Request

## 🐛 Troubleshooting

### Common Issues

**Application won't start:**
- Ensure .NET 6.0 Runtime is installed
- Check Windows Event Viewer for error details
- Run as Administrator if proxy changes fail

**Proxy settings not applying:**
- Verify proxy server address format
- Check Windows proxy settings manually
- Ensure no group policy restrictions

**Git/npm proxy not working:**
- Verify Git and npm are installed
- Check command-line access to git/npm
- Review proxy override settings

### Logs and Debugging
- Application logs: Windows Event Viewer
- Settings file: `%LocalAppData%\ProxyToggleApp\`
- Debug mode: Run with `--debug` flag (if implemented)

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🤝 Acknowledgments

- **Microsoft** - .NET Framework and WPF
- **CommunityToolkit** - MVVM helpers and utilities
- **Contributors** - Everyone who helped improve this project

## 📞 Support

- **Issues:** [GitHub Issues](../../issues)
- **Discussions:** [GitHub Discussions](../../discussions)
- **Email:** [Your contact email]

---

**Version:** 0.1.0  
**Last Updated:** July 7, 2025  