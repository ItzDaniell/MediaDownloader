# FAQ - Preguntas Frecuentes

## Instalación y Requisitos

### P: ¿Qué necesito para ejecutar Media Downloader?
**R**: Necesitas:
- Windows 7 o superior (recomendado Windows 10/11)
- .NET 8.0 Runtime o superior
- yt-dlp instalado en tu sistema
- Conexión a internet

### P: ¿Cómo instalo .NET 8.0?
**R**: Descárgalo desde:
```
https://dotnet.microsoft.com/download/dotnet/8.0
```
Selecciona el instalador apropiado para tu sistema operativo.

### P: ¿Cómo instalo yt-dlp?
**R**: Tienes varias opciones:

**Opción 1: Ejecutar el script (RECOMENDADO)**
```bash
install-dependencies.bat
```

**Opción 2: Instalar con pip**
```bash
pip install yt-dlp
```

**Opción 3: Instalar con Chocolatey**
```bash
choco install yt-dlp
```

**Opción 4: Descarga manual**
- Ve a: https://github.com/yt-dlp/yt-dlp/releases
- Descarga `yt-dlp.exe`
- Colócalo en una carpeta en tu PATH

### P: ¿Cómo sé si yt-dlp está correctamente instalado?
**R**: Abre PowerShell o CMD y escribe:
```bash
yt-dlp --version
```
Si ves un número de versión, ¡está correctamente instalado!

### P: ¿Puedo ejecutar esto en Mac o Linux?
**R**: No directamente, pero podrías intentar:
- **Mac**: Requiere .NET 8.0 para Mac (disponible) pero WPF no es totalmente compatible
- **Linux**: Similar a Mac, WPF tiene soporte limitado
- Alternativamente: Puedes usar yt-dlp directamente desde terminal

## Uso de la Aplicación

### P: ¿Cómo empiezo a descargar?
**R**: Es muy simple:
1. Abre Media Downloader
2. Copia la URL del video (YouTube o X)
3. Pégala en el campo "URL del contenido"
4. Elige formato (MP3 o MP4)
5. Haz clic en "?? Descargar"
6. ¡Espera a que se complete!

### P: ¿Dónde se guardan las descargas?
**R**: Por defecto en:
```
C:\Users\[TuNombre]\Downloads\MediaDownloader
```

Puedes cambiar esto:
1. Haz clic en "Seleccionar..." junto a "Carpeta de destino"
2. Elige una nueva carpeta
3. Las futuras descargas irán allí

### P: ¿Puedo cambiar la carpeta de destino en cada descarga?
**R**: Sí. Antes de descargar, usa el botón "Seleccionar..." para cambiar la carpeta. Se mantendrá para futuras descargas hasta que la cambies de nuevo.

### P: ¿Cuál es la diferencia entre MP3 y MP4?
**R**:
- **MP3**: Solo descarga el audio, archivo más pequeño, ideal para música
- **MP4**: Descarga el video completo, archivo más grande, mantiene la calidad visual

### P: ¿Cuánto tiempo tarda una descarga?
**R**: Depende de:
- Tamaño del video (los videos largos tardan más)
- Tu velocidad de internet
- Calidad del video (videos 4K tardan más)

Una canción típica: 1-2 minutos
Un video completo: 5-15 minutos

### P: ¿Puedo descargar playlists completas?
**R**: No en la versión actual. Pero puedes:
- Descargar videos uno por uno
- O usar yt-dlp directamente desde terminal para playlists

### P: ¿Puedo pausar una descarga?
**R**: No en la interfaz actual. Si necesitas detenerla:
- Espera a que se complete
- O cancela e intenta con otra URL

## Problemas Técnicos

### P: Recibo error "yt-dlp no se encontró"
**R**: Significa que yt-dlp no está instalado o no está en PATH:
1. Verifica instalación: `yt-dlp --version`
2. Si no funciona, instálalo: `pip install yt-dlp`
3. Si sigue sin funcionar, copia `yt-dlp.exe` a la carpeta de la aplicación
4. Reinicia la aplicación

### P: La descarga falla con error
**R**: Revisa estos puntos:
1. **URL correcta**: Asegúrate de copiar bien la URL
2. **Video disponible**: El video puede estar privado o eliminado
3. **Conexión**: Verifica tu conexión a internet
4. **yt-dlp actualizado**: `pip install --upgrade yt-dlp`
5. **Permisos**: ¿Tienes permisos para escribir en la carpeta?

### P: La aplicación se congela/no responde
**R**: Probablemente está descargando. Espera a que termine. Si realmente está congelada:
- Abre el Administrador de tareas (Ctrl + Shift + Esc)
- Busca "MediaDownloader" o "yt-dlp"
- Haz clic derecho > Finalizar tarea

### P: No puedo abrir archivos descargados
**R**: Revisa:
- ¿Tiene la extensión correcta? (.mp3 o .mp4)
- ¿Está en una carpeta accesible?
- ¿Tiene permisos de lectura?
- Prueba con otro reproductor (VLC, Windows Media Player, etc.)

### P: Descargo el mismo video y ocupa diferente tamaño
**R**: Es normal. La descarga intenta obtener la mejor calidad disponible. Si YouTube reduce la calidad disponible para ese video, ocupa menos espacio.

### P: ¿Por qué algunos videos de X no se descargan?
**R**: Razones posibles:
- Video privado o protegido
- Twitter/X ha eliminado el video
- Restricciones geográficas
- El usuario ha bloqueado descargas
- yt-dlp necesita actualización

## Legalidad y Derechos de Autor

### P: ¿Es legal descargar videos de YouTube?
**R**: Depende:
- ? Sí para contenido con licencia CC (Creative Commons)
- ? Sí para contenido de dominio público
- ? Sí si tienes permiso del autor
- ? No si viola derechos de autor
- ? No para contenido protegido o con DRM

### P: ¿Es legal descargar videos de X/Twitter?
**R**: Similar a YouTube:
- ? Respeta los derechos de autor
- ? Solo descarga contenido que puedas usar
- ? No descargues contenido privado sin permiso

### P: ¿Qué puedo hacer con los archivos descargados?
**R**: Depende de la licencia:
- **Uso personal**: Generalmente OK
- **Redistribución**: Necesitas permiso del autor
- **Modificación**: Revisa la licencia original
- **Comercial**: Necesitas licencia explícita

## Rendimiento

### P: ¿Cuánto espacio en disco necesito?
**R**: 
- Aplicación: ~100 MB
- Carpeta de descargas: Depende de lo que descargues
  - Canción MP3: 5-10 MB
  - Video MP4: 50-1000 MB (depende de duración)
- Espacio recomendado: Al menos 50 GB libres

### P: ¿Consume mucho ancho de banda?
**R**: Solo durante las descargas. Cuando no estás descargando, no consume nada.

### P: ¿Puedo hacer otras cosas mientras descargo?
**R**: Sí, la interfaz no se congela. Puedes:
- Usar otros programas
- Navegar por internet
- Incluso lanzar otra descarga (si lo deseas)

### P: ¿Por qué la velocidad es lenta?
**R**: Razones posibles:
- Tu conexión es lenta
- Los servidores están saturados
- La resolución de video es muy alta
- Otros programas usan tu ancho de banda

## Características Avanzadas

### P: ¿Puedo personalizar la calidad de descarga?
**R**: No en la interfaz actual, pero:
- MP3 se descarga a 192 kbps (buen balance)
- MP4 usa la mejor calidad disponible
- Para más control, usa yt-dlp directamente

### P: ¿Cómo uso yt-dlp directamente desde terminal?
**R**: Ejemplos:
```bash
# Descargar como MP3
yt-dlp -x --audio-format mp3 --audio-quality 192 "URL"

# Descargar como MP4
yt-dlp -f "best[ext=mp4]" "URL"

# Descargar a carpeta específica
yt-dlp -o "C:/destino/%(title)s.%(ext)s" "URL"
```

## Soporte y Contacto

### P: ¿Dónde reporto un bug?
**R**: Documenta:
- Lo que intentabas hacer
- El error exacto que recibiste
- Tu versión de .NET y yt-dlp
- El URL que usaste (si es posible)

### P: ¿Hay actualizaciones disponibles?
**R**: Mantén yt-dlp actualizado:
```bash
pip install --upgrade yt-dlp
```

La aplicación se puede recompilar desde el código fuente si hay mejoras.

### P: ¿Cómo contribuyo al proyecto?
**R**: ¡Todas las contribuciones son bienvenidas!
- Reporta bugs
- Sugiere mejoras
- Crea pull requests
- Mejora la documentación

---

**Última actualización**: 2024  
**Versión de FAQ**: 1.0
