@echo off
REM Instalar FFmpeg automáticamente en Windows

echo.
echo ============================================
echo Instalando FFmpeg
echo ============================================
echo.

REM Verificar si FFmpeg ya está instalado
ffmpeg -version >nul 2>&1
if %errorlevel% equ 0 (
    echo ? FFmpeg ya está instalado
    ffmpeg -version | findstr version
    pause
    exit /b 0
)

echo FFmpeg no encontrado. Descargando...
echo.

REM Descargar FFmpeg usando PowerShell
set FFMPEG_URL=https://github.com/BtbN/FFmpeg-Builds/releases/download/latest/ffmpeg-master-latest-win64-gpl.zip
set FFMPEG_ZIP=%temp%\ffmpeg.zip
set FFMPEG_FOLDER=%temp%\ffmpeg-extract

echo Descargando FFmpeg...
powershell -Command "try { [System.Net.ServicePointManager]::SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol -bor 3072; (New-Object System.Net.WebClient).DownloadFile('%FFMPEG_URL%', '%FFMPEG_ZIP%'); Write-Host 'Descarga completada' } catch { Write-Host 'Error al descargar'; exit 1 }"

if %errorlevel% neq 0 (
    echo Error al descargar FFmpeg
    echo Descárgalo manualmente desde: https://ffmpeg.org/download.html
    pause
    exit /b 1
)

echo.
echo Extrayendo FFmpeg...

REM Extraer ZIP
powershell -Command "Expand-Archive -Path '%FFMPEG_ZIP%' -DestinationPath '%FFMPEG_FOLDER%' -Force"

if %errorlevel% neq 0 (
    echo Error al extraer
    pause
    exit /b 1
)

REM Buscar la carpeta bin de FFmpeg
for /d %%D in ("%FFMPEG_FOLDER%\ffmpeg*") do (
    if exist "%%D\bin\ffmpeg.exe" (
        echo Copiando FFmpeg a carpeta de sistema...
        mkdir "%SYSTEMROOT%\ffmpeg" 2>nul
        xcopy "%%D\bin\*" "%SYSTEMROOT%\ffmpeg\" /Y /Q

        REM Agregar a PATH
        setx PATH "%PATH%;%SYSTEMROOT%\ffmpeg"

        echo.
        echo ? FFmpeg instalado correctamente

        REM Limpiar
        del /Q "%FFMPEG_ZIP%"
        rmdir /S /Q "%FFMPEG_FOLDER%"

        REM Verificar
        timeout /t 2
        ffmpeg -version | findstr version

        echo.
        echo Ahora ejecuta: dotnet run
        pause
        exit /b 0
    )
)

echo Error: No se encontró ffmpeg.exe en el ZIP
pause
exit /b 1
