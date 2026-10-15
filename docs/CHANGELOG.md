# Changelog

Todos los cambios notables en este proyecto serán documentados en este archivo.

## [1.0.0] - 2024-01-XX

### Agregado
- Interfaz WPF moderna y fácil de usar
- Descarga de videos de YouTube a MP3 y MP4
- Descarga de videos de X (Twitter) a MP3 y MP4
- Selección de carpeta de destino personalizada
- Visualización del progreso de descarga
- Mensajes de estado en tiempo real
- Validación de URLs
- Detección automática de plataforma

### Características Técnicas
- Compilado con .NET 8.0
- Interfaz asíncrona para no bloquear la UI
- Manejo robusto de errores
- Integración con yt-dlp

### Requisitos
- .NET 8.0 Runtime o superior
- yt-dlp instalado y en PATH
- Windows 7 o superior

### Conocidos Limitaciones
- Descarga un video a la vez (sin soporte para playlists)
- Requiere yt-dlp preinstalado
- Algunos videos pueden no descargarse debido a restricciones de privacidad

## Futuras Mejoras Planeadas
- [ ] Soporte para descargar playlists completas
- [ ] Cola de descargas
- [ ] Historial de descargas
- [ ] Preferencias guardadas
- [ ] Soporte para más plataformas (Instagram, TikTok, etc.)
- [ ] Interfaz de línea de comandos
- [ ] Descarga paralela de múltiples videos
