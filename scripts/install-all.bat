@echo off
REM Script de instalación completo para Media Downloader
REM Instala Python, pip y yt-dlp automáticamente

echo.
echo ============================================
echo Media Downloader - Instalación Automática
echo ============================================
echo.

REM Verificar si Python está instalado
python --version >nul 2>&1
if %errorlevel% equ 0 (
    echo ? Python ya está instalado
    goto check_pip
)

REM Python no está instalado, descargar e instalar
echo ? Python no encontrado. Descargando e instalando...
echo.

REM Descargar Python 3.11
set PYTHON_URL=https://www.python.org/ftp/python/3.11.8/python-3.11.8-amd64.exe
set PYTHON_INSTALLER=%temp%\python-installer.exe

echo Descargando Python 3.11...
powershell -Command "(New-Object System.Net.ServicePointManager).SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol -bor 3072; [System.Net.ServicePointManager]::SecurityProtocol = 'Tls12'; (New-Object System.Net.WebClient).DownloadFile('%PYTHON_URL%', '%PYTHON_INSTALLER%')"

if not exist "%PYTHON_INSTALLER%" (
    echo Error: No se pudo descargar Python
    echo Descárgalo manualmente desde: https://www.python.org/downloads/
    pause
    exit /b 1
)

echo Instalando Python...
"%PYTHON_INSTALLER%" /quiet InstallAllUsers=1 PrependPath=1 /log "%temp%\python-install.log"

if %errorlevel% neq 0 (
    echo Error al instalar Python
    echo Log: %temp%\python-install.log
    pause
    exit /b 1
)

del "%PYTHON_INSTALLER%"

REM Esperar a que Python se instale
timeout /t 3 /nobreak

:check_pip
REM Verificar que Python está disponible
python --version >nul 2>&1
if %errorlevel% neq 0 (
    echo Error: Python no se instaló correctamente
    pause
    exit /b 1
)

echo ? Python disponible
echo.

REM Instalar/actualizar pip
echo Actualizando pip...
python -m pip install --upgrade pip

REM Instalar yt-dlp
echo.
echo Instalando yt-dlp...
pip install yt-dlp

if %errorlevel% neq 0 (
    echo Error al instalar yt-dlp
    pause
    exit /b 1
)

echo.
echo ? yt-dlp instalado correctamente
echo.

REM Verificar instalación
yt-dlp --version >nul 2>&1
if %errorlevel% equ 0 (
    echo ? Verificación exitosa
    echo.
    yt-dlp --version
    echo.
    echo ============================================
    echo ? Instalación completada
    echo ============================================
    echo.
    echo Puedes cerrar esta ventana y ejecutar:
    echo   dotnet run
    echo.
) else (
    echo ? Error: yt-dlp no se encuentra en PATH
    echo Intenta reiniciar tu computadora
    pause
    exit /b 1
)

pause
