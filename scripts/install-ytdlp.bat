@echo off
REM Instalación simplificada - Solo instala yt-dlp
REM Requiere que Python ya esté en el PATH

echo.
echo ============================================
echo Instalación de yt-dlp
echo ============================================
echo.

REM Verificar Python
python --version >nul 2>&1
if %errorlevel% neq 0 (
    echo ERROR: Python no está instalado o no está en PATH
    echo.
    echo Solución rápida:
    echo 1. Ve a: https://www.python.org/downloads/
    echo 2. Descarga Python 3.11 o superior
    echo 3. Marca: "Add Python to PATH" durante la instalación
    echo 4. Reinicia tu computadora
    echo 5. Ejecuta este script de nuevo
    echo.
    pause
    exit /b 1
)

echo ? Python encontrado
python --version
echo.

REM Actualizar pip
echo Actualizando pip...
python -m pip install --upgrade pip
if %errorlevel% neq 0 (
    echo Error al actualizar pip
    pause
    exit /b 1
)

REM Instalar yt-dlp
echo.
echo Instalando yt-dlp...
pip install yt-dlp
if %errorlevel% neq 0 (
    echo Error al instalar yt-dlp
    pause
    exit /b 1
)

REM Verificar
echo.
echo Verificando yt-dlp...
yt-dlp --version
if %errorlevel% neq 0 (
    echo Error: yt-dlp no se instaló correctamente
    pause
    exit /b 1
)

echo.
echo ============================================
echo ? yt-dlp instalado correctamente
echo ============================================
echo.
echo Ahora ejecuta:
echo   dotnet run
echo.
pause
