# Proxy Toggle App v0.1.0

## Quick Start Guide

### 🚀 Installation
1. Extract all files to a folder of your choice
2. Run `ProxyToggleApp.exe`
3. No additional installation required!

### 🎯 Basic Usage

#### Enable Proxy
1. Enter your proxy server address in the text field
   - Example: `127.0.0.1:8080`
   - Example: `proxy.company.com:3128`
2. Click "Enable Proxy" button
3. Green status indicator confirms activation

#### Disable Proxy
1. Click "Disable Proxy" button
2. Red status indicator confirms deactivation

#### Configure Settings
1. Click the Settings button (⚙️) in the top-right
2. Adjust preferences:
   - Default proxy server
   - Proxy override list
   - Auto-start options
   - Notification preferences
3. Settings are saved automatically

### 🔧 System Tray Features
- **Minimize to tray:** Close window to minimize to system tray
- **Right-click tray icon** for quick menu:
  - Enable/Disable Proxy
  - Open Main Window
  - Exit Application

### ⚙️ Default Settings
- **Proxy Server:** `127.0.0.1:8080`
- **Proxy Override:** Local networks (127.*, 192.168.*, etc.)
- **Timeout:** 30 seconds
- **Git Proxy:** Enabled
- **npm Proxy:** Enabled
- **Notifications:** Enabled

### 🔒 Security Notes
- Administrator privileges may be required for proxy changes
- Windows may show security warnings on first run
- Application only modifies Windows proxy settings

### 📁 File Locations
- **Application:** Where you extracted the files
- **Settings:** `%LocalAppData%\ProxyToggleApp\appsettings.json`
- **Logs:** Windows Event Viewer (if enabled)

### 🐛 Troubleshooting

**Application won't start:**
- Ensure all files are extracted
- Run as Administrator
- Check Windows Event Viewer

**Proxy not working:**
- Verify proxy server address format
- Check proxy server is running
- Test manually in Windows Settings

**Git/npm proxy issues:**
- Ensure Git and npm are installed
- Verify tools are in system PATH
- Check proxy override settings

### 📞 Need Help?
- **GitHub Issues:** [Report bugs and issues]
- **Documentation:** See README.md for full documentation
- **Support:** Check troubleshooting section

---

**Version:** 0.1.0  
**Release Date:** July 7, 2025  
**Platform:** Windows 10/11 x64
