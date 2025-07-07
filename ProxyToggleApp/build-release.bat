@echo off
echo ================================================
echo   Proxy Toggle App - Release Builder
echo ================================================
echo.

echo Building release...
dotnet publish -c Release -r win-x64 --self-contained true -o release

echo.
echo Creating release package...
powershell -ExecutionPolicy Bypass -File create-release.ps1

echo.
echo ================================================
echo   Release package created successfully!
echo ================================================
echo.
echo Files ready for GitHub release:
echo - ProxyToggleApp-v0.1.0-win-x64.zip
echo - ProxyToggleApp-v0.1.0-win-x64.zip.sha256
echo.
echo Next steps:
echo 1. Go to GitHub repository
echo 2. Create new release (tag: v0.1.0)
echo 3. Upload the zip file and checksum
echo 4. Copy release notes from release\RELEASE_NOTES.md
echo.
pause
