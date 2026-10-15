@echo off
REM Script simple para instalar FFmpeg

echo.
echo ============================================
echo Instalando FFmpeg
echo ============================================
echo.

REM Verificar si FFmpeg está instalado
ffmpeg -version >nul 2>&1
if %errorlevel% equ 0 (
    echo ? FFmpeg ya está instalado
    ffmpeg -version | findstr version
    pause
    exit /b 0
)

REM Intentar con Chocolatey primero
choco --version >nul 2>&1
if %errorlevel% equ 0 (
    echo Instalando FFmpeg con Chocolatey...
    choco install ffmpeg -y
    if %errorlevel% equ 0 (
        echo ? FFmpeg instalado con éxito
        ffmpeg -version | findstr version
        pause
        exit /b 0
    )
)

REM Si no funcionó Chocolatey, mostrar instrucción manual
echo.
echo FFmpeg no está instalado y Chocolatey no está disponible.
echo.
echo Opción 1: Instalar Chocolatey (recomendado)
echo   1. Abre PowerShell como administrador
echo   2. Ejecuta:
echo      Set-ExecutionPolicy Bypass -Scope Process -Force; [System.Net.ServicePointManager]::SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol -bor 3072; iex ((New-Object System.Net.WebClient).DownloadString('https://community.chocolatey.org/install.ps1'))
echo   3. Cierra y abre PowerShell nuevamente
echo   4. Ejecuta: choco install ffmpeg -y
echo   5. Reinicia esta computadora
echo.
echo Opción 2: Descarga manual
echo   1. Ve a: https://ffmpeg.org/download.html
echo   2. Descarga la versión Windows (full)
echo   3. Extrae a: C:\ffmpeg
echo   4. Abre PowerShell como administrador
echo   5. Ejecuta: [Environment]::SetEnvironmentVariable('PATH', $env:PATH + ';C:\ffmpeg\bin', 'Machine')
echo   6. Reinicia esta computadora
echo.
echo Opción 3: Usar Windows Package Manager (si tienes Windows 11)
echo   winget install ffmpeg
echo.
pause
