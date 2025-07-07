# GitHub Release Guide

## 📦 Creating a Release

### Option 1: Manual Release (Recommended for first release)

1. **Prepare the release package:**
   ```bash
   # Navigate to ProxyToggleApp directory
   cd ProxyToggleApp
   
   # Run the release builder
   build-release.bat
   ```

2. **Go to GitHub repository:**
   - Navigate to your repository on GitHub
   - Click on "Releases" (on the right side)
   - Click "Create a new release"

3. **Fill in release details:**
   - **Tag version:** `v0.1.0` (create new tag)
   - **Release title:** `Proxy Toggle App v0.1.0`
   - **Description:** Copy from `release/RELEASE_NOTES.md`

4. **Upload files:**
   - Drag and drop `ProxyToggleApp-v0.1.0-win-x64.zip`
   - Drag and drop `ProxyToggleApp-v0.1.0-win-x64.zip.sha256`

5. **Publish release:**
   - Check "Set as the latest release"
   - Click "Publish release"

### Option 2: Automated Release (GitHub Actions)

1. **Push a version tag:**
   ```bash
   git tag v0.1.0
   git push origin v0.1.0
   ```

2. **GitHub Actions will automatically:**
   - Build the application
   - Create release package
   - Create GitHub release
   - Upload assets

## 📋 Release Checklist

### Before Release:
- [ ] Test application on clean Windows machine
- [ ] Update version in `ProxyToggleApp.csproj`
- [ ] Update `RELEASE_NOTES.md` with changes
- [ ] Update `README.md` if needed
- [ ] Commit all changes

### During Release:
- [ ] Create and push version tag
- [ ] Verify release build succeeds
- [ ] Test downloaded package
- [ ] Verify SHA256 checksum

### After Release:
- [ ] Announce release (if applicable)
- [ ] Update documentation
- [ ] Plan next version features

## 🔄 Version Numbering

Use Semantic Versioning (SemVer):
- **Major.Minor.Patch** (e.g., 1.0.0)
- **Major:** Breaking changes
- **Minor:** New features (backward compatible)
- **Patch:** Bug fixes (backward compatible)

Examples:
- `v0.1.0` - Initial release
- `v0.1.1` - Bug fixes
- `v0.2.0` - New features
- `v1.0.0` - First stable release

## 📝 Release Notes Template

```markdown
# Proxy Toggle App v[VERSION]

## 🚀 New Features
- Feature 1
- Feature 2

## 🐛 Bug Fixes
- Fix 1
- Fix 2

## 📥 Download
- **Windows (64-bit):** `ProxyToggleApp-v[VERSION]-win-x64.zip`
- **SHA256:** `[HASH]`

## 📋 Requirements
- Windows 10 (version 1809) or Windows 11
- x64 architecture
```

## 🔒 Security Best Practices

1. **Always provide checksums** (SHA256)
2. **Use signed releases** (code signing certificate)
3. **Include virus scanner reports** (VirusTotal links)
4. **Provide source code links** for transparency
5. **Document security features** in release notes

## 📞 Support

- **Issues:** Use GitHub Issues for bug reports
- **Discussions:** Use GitHub Discussions for questions
- **Security:** Email for security-related issues
