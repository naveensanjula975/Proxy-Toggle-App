# Proxy Toggle App - Release Notes

## Version 0.1.0 - Initial Release

### 🚀 Features
- **System-wide proxy management** - Enable/disable Windows proxy settings
- **Git & npm proxy integration** - Automatically configure development tool proxies
- **Modern Fluent Design UI** - Clean, intuitive interface with smooth animations
- **System tray integration** - Minimize to tray with quick access menu
- **Settings persistence** - All configurations saved automatically
- **Single instance protection** - Prevents multiple app instances
- **Toast notifications** - Non-intrusive status updates

### 📋 What's Included
- `ProxyToggleApp.exe` - Main application executable
- All required .NET 6.0 runtime dependencies (self-contained)
- No additional installation required

### 🛠️ System Requirements
- **Operating System:** Windows 10 (version 1809) or Windows 11
- **Architecture:** x64 (64-bit)
- **Memory:** 100 MB RAM minimum
- **Disk Space:** 150 MB free space

### 📥 Installation
1. Download `ProxyToggleApp-v0.1.0-win-x64.zip`
2. Extract to your preferred location (e.g., `C:\Program Files\ProxyToggleApp\`)
3. Run `ProxyToggleApp.exe`
4. (Optional) Create desktop shortcut or pin to taskbar

### 🎯 Quick Start
1. **Enable Proxy:**
   - Enter proxy server (e.g., `127.0.0.1:8080`)
   - Click "Enable Proxy"
   - Green indicator shows active status

2. **Access Settings:**
   - Click Settings button (⚙️)
   - Configure proxy preferences
   - Enable auto-start or tray options

3. **System Tray:**
   - Right-click tray icon for quick actions
   - Toggle proxy without opening main window

### 🔧 Configuration
- **Default Proxy:** `127.0.0.1:8080`
- **Proxy Override:** Local networks excluded by default
- **Timeout:** 30 seconds for operations
- **Settings Location:** `%LocalAppData%\ProxyToggleApp\`

### 🐛 Known Issues
- First run may require Windows Defender approval
- Administrator privileges may be needed for some proxy changes
- Git/npm proxy requires tools to be in PATH

### 📞 Support
- **Issues:** Report bugs on GitHub Issues
- **Feature Requests:** Use GitHub Discussions
- **Documentation:** See README.md

### 🔄 Future Updates
- Auto-update functionality
- Proxy validation and testing
- Import/export configuration
- Network detection and switching
- Additional proxy protocols

---

**Checksum (SHA256):**
`[Generated during release process]`

**Release Date:** July 7, 2025  
**Build Configuration:** Release, Self-Contained, win-x64
