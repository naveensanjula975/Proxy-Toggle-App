# Proxy Toggle App - Release Package Script
# This script creates a release package for GitHub

param(
    [string]$Version = "0.1.0",
    [string]$OutputPath = ".\release-package"
)

Write-Host "Creating Proxy Toggle App Release Package v$Version" -ForegroundColor Green

# Create output directory
if (Test-Path $OutputPath) {
    Remove-Item $OutputPath -Recurse -Force
}
New-Item -ItemType Directory -Path $OutputPath -Force

# Copy release files
Write-Host "Copying release files..." -ForegroundColor Yellow
Copy-Item ".\release\*" -Destination $OutputPath -Recurse -Force

# Create version-specific zip file
$ZipName = "ProxyToggleApp-v$Version-win-x64.zip"
$ZipPath = ".\$ZipName"

Write-Host "Creating zip package: $ZipName" -ForegroundColor Yellow

# Remove existing zip if it exists
if (Test-Path $ZipPath) {
    Remove-Item $ZipPath -Force
}

# Create zip file
Add-Type -AssemblyName System.IO.Compression.FileSystem
[System.IO.Compression.ZipFile]::CreateFromDirectory($OutputPath, $ZipPath)

# Calculate file hash
$Hash = Get-FileHash $ZipPath -Algorithm SHA256
$HashString = $Hash.Hash

# Display package information
Write-Host "`n=== RELEASE PACKAGE CREATED ===" -ForegroundColor Green
Write-Host "Package: $ZipName" -ForegroundColor White
Write-Host "Size: $([math]::Round((Get-Item $ZipPath).Length / 1MB, 2)) MB" -ForegroundColor White
Write-Host "SHA256: $HashString" -ForegroundColor White
Write-Host "Location: $(Resolve-Path $ZipPath)" -ForegroundColor White

# Create checksum file
$HashString | Out-File -FilePath ".\$ZipName.sha256" -Encoding utf8

Write-Host "`n=== GITHUB RELEASE INSTRUCTIONS ===" -ForegroundColor Cyan
Write-Host "1. Go to your GitHub repository" -ForegroundColor White
Write-Host "2. Click 'Releases' > 'Create a new release'" -ForegroundColor White
Write-Host "3. Tag version: v$Version" -ForegroundColor White
Write-Host "4. Release title: Proxy Toggle App v$Version" -ForegroundColor White
Write-Host "5. Upload files:" -ForegroundColor White
Write-Host "   - $ZipName" -ForegroundColor Gray
Write-Host "   - $ZipName.sha256" -ForegroundColor Gray
Write-Host "6. Copy release notes from RELEASE_NOTES.md" -ForegroundColor White
Write-Host "7. Mark as 'Latest release' if this is the newest version" -ForegroundColor White

Write-Host "`n=== RELEASE NOTES TEMPLATE ===" -ForegroundColor Cyan
Write-Host @"
# Proxy Toggle App v$Version

## 🚀 Features
- System-wide proxy management for Windows
- Git & npm proxy integration
- Modern Fluent Design UI
- System tray integration
- Settings persistence

## 📥 Download
- **Windows (64-bit):** ``$ZipName``
- **SHA256:** ``$HashString``

## 📋 Requirements
- Windows 10 (1809) or Windows 11
- x64 architecture

## 🛠️ Installation
1. Download and extract the zip file
2. Run ``ProxyToggleApp.exe``
3. No additional installation required!

For detailed instructions, see ``QUICKSTART.md`` in the package.
"@ -ForegroundColor Gray

Write-Host "`nRelease package ready for GitHub!" -ForegroundColor Green
