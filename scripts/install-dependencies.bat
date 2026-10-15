@echo off
REM Script de instalación para Media Downloader
REM Instala las dependencias necesarias

echo ============================================
echo Media Downloader - Script de Instalación
echo ============================================
echo.

REM Verificar si Python está instalado
echo Verificando si Python está instalado...
python --version >nul 2>&1
if %errorlevel% neq 0 (
    echo ERROR: Python no está instalado o no está en PATH
    echo Por favor, instala Python desde: https://www.python.org
    pause
    exit /b 1
)

echo ? Python encontrado

REM Instalar yt-dlp
echo.
echo Instalando yt-dlp...
pip install yt-dlp

if %errorlevel% neq 0 (
    echo ERROR: Fallo al instalar yt-dlp
    pause
    exit /b 1
)

echo ? yt-dlp instalado correctamente

REM Verificar que yt-dlp está en PATH
echo.
echo Verificando que yt-dlp está en PATH...
yt-dlp --version >nul 2>&1
if %errorlevel% neq 0 (
    echo ADVERTENCIA: yt-dlp no se encontró en PATH
    echo Verifica que Python esté correctamente agregado a tu PATH
)

echo.
echo ============================================
echo ? Instalación completada
echo ============================================
echo.
echo Puedes cerrar esta ventana y ejecutar Media Downloader
pause
