# Media Downloader - Guía Rápida

## Instalación Rápida

### Paso 1: Instalar yt-dlp

La forma más fácil es ejecutar el script proporcionado:

```bash
install-dependencies.bat
```

O manualmente:

```bash
pip install yt-dlp
```

### Paso 2: Compilar la aplicación

1. Abre la solución en Visual Studio
2. Presiona `Ctrl + Shift + B` para compilar
3. Presiona `F5` para ejecutar la aplicación

## Uso Básico

### Descargar un video de YouTube como MP3

1. Copia la URL: `https://www.youtube.com/watch?v=...`
2. Pégala en el campo URL
3. Selecciona formato: **MP3**
4. Haz clic en **?? Descargar**

### Descargar un video de X/Twitter

1. Copia la URL de un tweet con video
2. Pégala en el campo URL
3. Selecciona formato: **MP3** o **MP4**
4. Haz clic en **?? Descargar**

### Cambiar carpeta de descarga

1. Haz clic en el botón "Seleccionar..." junto a "Carpeta de destino"
2. Selecciona la carpeta donde guardar los archivos
3. Las descargas futuras irán a esa carpeta

## Formatos soportados

| Formato | Descripción | Extensión |
|---------|-------------|-----------|
| **MP3** | Audio de alta calidad | .mp3 |
| **MP4** | Video de máxima calidad disponible | .mp4 |

## Ejemplos de URLs

### YouTube
```
https://www.youtube.com/watch?v=dQw4w9WgXcQ
https://youtu.be/dQw4w9WgXcQ
```

### X / Twitter
```
https://twitter.com/usuario/status/1234567890123456789
https://x.com/usuario/status/1234567890123456789
```

## Preguntas Frecuentes

**P: ¿Por qué tarda la descarga?**
R: Depende del tamaño del video y tu conexión. Videos largos pueden tardar varios minutos.

**P: ¿Puedo descargar playlists?**
R: Actualmente la app descarga un video a la vez. Para playlists, usa yt-dlp directamente en terminal.

**P: ¿Qué pasa si la descarga falla?**
R: Lee el mensaje de error en la app. Generalmente se debe a:
- URL incorrecta
- Video privado o eliminado
- Problemas de conectividad

**P: ¿Es legal descargar videos?**
R: Respeta los derechos de autor. Solo descarga contenido que tengas permiso para descargar.

## Soporte técnico

Si encuentras un problema:

1. Verifica que yt-dlp está actualizado: `pip install --upgrade yt-dlp`
2. Comprueba tu conexión a internet
3. Revisa que la URL sea correcta
4. Intenta con otra URL para descartar problemas específicos del video

## Desarrollo

La aplicación se compone de:

- **MainWindow.xaml**: Interfaz de usuario
- **MainWindow.xaml.cs**: Lógica de la interfaz
- **Services/DownloadService.cs**: Lógica de descargas

Para compilar desde terminal:

```bash
dotnet build
dotnet run
```
