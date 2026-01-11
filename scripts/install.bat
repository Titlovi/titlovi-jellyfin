@echo off
setlocal enabledelayedexpansion

REM Configuration
set "SOLUTION_NAME=Titlovi.sln"
set "PLUGIN_NAME=Titlovi.Plugin"
set "SOURCE_DIR=src\%PLUGIN_NAME%"
set "JELLYFIN_PLUGINS_DIR=%LocalAppData%\jellyfin\plugins"
set "TARGET_DIR=%JELLYFIN_PLUGINS_DIR%\JellyfinTitlovi"

echo Building and deploying %PLUGIN_NAME% plugin...
echo.

REM Check if source directory exists
if not exist "%SOURCE_DIR%" (
    echo [ERROR] Source directory '%SOURCE_DIR%' not found!
    exit /b 1
)

REM Check if Jellyfin plugins directory exists
if not exist "%JELLYFIN_PLUGINS_DIR%" (
    echo [ERROR] Jellyfin plugins directory '%JELLYFIN_PLUGINS_DIR%' not found!
    echo Make sure Jellyfin is installed and the plugins directory exists.
    exit /b 1
)

REM Create target directory if it doesn't exist
if not exist "%TARGET_DIR%" (
    echo Creating target directory: %TARGET_DIR%
    mkdir "%TARGET_DIR%"
)

REM Build the plugin
echo Building %PLUGIN_NAME% project...
dotnet publish "%SOLUTION_NAME%"
if errorlevel 1 (
    echo [ERROR] Build failed!
    exit /b 1
)
echo.

REM Copy only DLL files
echo Copying DLL files to %TARGET_DIR%...
set DLL_COUNT=0
for /r "%SOURCE_DIR%\bin\Release\net9.0\publish\" %%f in (*.dll) do (
    copy /Y "%%f" "%TARGET_DIR%\" >nul
    set /a DLL_COUNT+=1
)

if !DLL_COUNT! equ 0 (
    echo [WARNING] No DLL files found in %SOURCE_DIR%
    echo Make sure you've built the plugin first.
) else (
    echo Copied !DLL_COUNT! DLL file(s)
)
echo.

echo [SUCCESS] Plugin '%PLUGIN_NAME%' successfully deployed to %TARGET_DIR%
echo.

pause