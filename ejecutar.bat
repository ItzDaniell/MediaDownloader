@echo off
cd /d "%~dp0"
echo ======================================================
echo    Iniciando Media Downloader...
echo ======================================================
echo.

if exist "MediaDownloader\bin\Debug\net8.0-windows\MediaDownloader.exe" (
    start "" "MediaDownloader\bin\Debug\net8.0-windows\MediaDownloader.exe"
) else (
    echo Compilando y ejecutando la aplicacion...
    dotnet run --project MediaDownloader
)
