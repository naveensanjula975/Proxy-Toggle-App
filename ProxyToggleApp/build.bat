@echo off
echo Building Proxy Toggle App...

dotnet restore
if %errorlevel% neq 0 (
    echo Error: Failed to restore packages
    pause
    exit /b 1
)

dotnet build --configuration Release
if %errorlevel% neq 0 (
    echo Error: Failed to build application
    pause
    exit /b 1
)

echo Build completed successfully!
pause
