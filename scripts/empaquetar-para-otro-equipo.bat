@echo off
setlocal enabledelayedexpansion

echo ======================================================
echo    Generando Paquete Portable para Compartir
echo    (Media Downloader - Windows x64)
echo ======================================================
echo.

cd /d "%~dp0\.."

set "OUTPUT_DIR=%~dp0..\dist\MediaDownloader-Portable"

if exist "%OUTPUT_DIR%" (
    echo [1/4] Limpiando carpeta anterior...
    rd /s /q "%OUTPUT_DIR%"
)

echo [2/4] Compilando version autocontenida (no requiere .NET instalado)...
dotnet publish MediaDownloader\MediaDownloader.csproj -c Release -r win-x64 --self-contained -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -o "%OUTPUT_DIR%"
if errorlevel 1 (
    echo Error al compilar. Verifica que tengas el SDK de .NET 8.
    pause
    exit /b 1
)

:: Eliminar archivo de símbolos .pdb si existe para dejarlo limpio
if exist "%OUTPUT_DIR%\MediaDownloader.pdb" (
    del /f /q "%OUTPUT_DIR%\MediaDownloader.pdb"
)

echo [3/4] Empaquetando complementos portables (FFmpeg y yt-dlp)...
:: Copiar ffmpeg si existe en Debug o dist
if exist "MediaDownloader\bin\Debug\net8.0-windows\ffmpeg" (
    echo Copiando FFmpeg...
    xcopy /E /I /Y "MediaDownloader\bin\Debug\net8.0-windows\ffmpeg" "%OUTPUT_DIR%\ffmpeg" >nul
    if exist "%OUTPUT_DIR%\ffmpeg\ffmpeg-master-latest-win64-gpl-shared" (
        rd /s /q "%OUTPUT_DIR%\ffmpeg\ffmpeg-master-latest-win64-gpl-shared"
    )
)

:: Copiar yt-dlp si existe en Debug o dist
if exist "MediaDownloader\bin\Debug\net8.0-windows\yt-dlp" (
    echo Copiando yt-dlp...
    xcopy /E /I /Y "MediaDownloader\bin\Debug\net8.0-windows\yt-dlp" "%OUTPUT_DIR%\yt-dlp" >nul
)

echo [4/4] Creando instrucciones de uso...
(
echo ======================================================
echo           MEDIA DOWNLOADER - VERSION PORTABLE
echo ======================================================
echo.
echo INSTRUCCIONES:
echo 1. No necesitas instalar nada (ni .NET, ni Python, ni FFmpeg^).
echo 2. Haz doble clic en "MediaDownloader.exe" para iniciar.
echo 3. Las descargas se guardaran automaticamente en tu carpeta:
echo    Descargas\MediaDownloader
echo.
echo Soporta enlaces de YouTube y X (Twitter^).
echo ======================================================
) > "%OUTPUT_DIR%\LEEME.txt"

echo.
echo ======================================================
echo   PAQUETE GENERADO CON EXITO EN:
echo   dist\MediaDownloader-Portable
echo.
echo   Solo debes comprimir esa carpeta en .ZIP y 
echo   enviarsela a cualquier persona con Windows 10/11.
echo ======================================================
echo.
pause
