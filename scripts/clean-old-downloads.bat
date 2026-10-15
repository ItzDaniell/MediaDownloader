@echo off
REM Script para limpiar descargas antiguas
REM Elimina archivos más antiguos que 30 días de la carpeta MediaDownloader

setlocal enabledelayedexpansion

set "DOWNLOAD_FOLDER=%USERPROFILE%\Downloads\MediaDownloader"
set "DAYS_OLD=30"

if not exist "!DOWNLOAD_FOLDER!" (
    echo La carpeta de descargas no existe: !DOWNLOAD_FOLDER!
    pause
    exit /b 1
)

echo Limpiando descargas más antiguas que !DAYS_OLD! días...
echo Carpeta: !DOWNLOAD_FOLDER!
echo.

REM Calcular la fecha de hace 30 días
for /f "tokens=1-4 delims=/-" %%a in ('date /t') do (
    set "today=%%d%%b%%c"
)

REM Usar PowerShell para eliminar archivos antiguos
powershell -NoProfile -Command ^
    "Get-ChildItem '!DOWNLOAD_FOLDER!' -File | Where-Object {$_.LastWriteTime -lt (Get-Date).AddDays(-!DAYS_OLD!)} | Remove-Item -Force -ErrorAction SilentlyContinue; Write-Host 'Limpieza completada'"

echo.
echo ? Limpieza completada
pause
