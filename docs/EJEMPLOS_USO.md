# Ejemplos de Uso - Media Downloader

## Escenario 1: Descargar una Canción de YouTube

### Objetivo
Descargar tu canción favorita de YouTube como MP3 para escuchar offline.

### Pasos
1. Ve a YouTube y encuentra la canción
2. Copia la URL:
   ```
   https://www.youtube.com/watch?v=dQw4w9WgXcQ
   ```
3. Abre Media Downloader
4. Pega la URL en el campo "URL del contenido"
5. Selecciona **MP3** en el formato
6. Haz clic en **Descargar**
7. Espera a ver el mensaje "¡Descarga completada correctamente!"
8. La canción estará en `C:\Users\[TuNombre]\Downloads\MediaDownloader\`

### Resultado
Archivo: `Never Gonna Give You Up.mp3` (~5-8 MB)

---

## Escenario 2: Guardar un Video Viral de X/Twitter

### Objetivo
Descargar un video viral de X para guardarlo en tu colección personal.

### Pasos
1. Encuentra el tweet con el video
2. Haz clic en los "..." y copia el enlace:
   ```
   https://x.com/usuario/status/1234567890123456789
   ```
3. Abre Media Downloader
4. Pega la URL
5. Selecciona **MP4** (para mantener el video)
6. Haz clic en **Descargar**
7. Espera (puede tomar 2-5 minutos según el tamaño)
8. El video estará listo cuando aparezca "¡Descarga completada correctamente!"

### Resultado
Archivo: `Nombre del Video.mp4` (~50-500 MB)

---

## Escenario 3: Cambiar la Carpeta de Destino

### Objetivo
Descargar videos a una carpeta específica en lugar de la carpeta por defecto.

### Pasos
1. Abre Media Downloader
2. Haz clic en **Seleccionar...** en la sección "Carpeta de destino"
3. Se abrirá una ventana de explorador
4. Navega a `D:\Mi Carpeta de Videos\Descargas`
5. Haz clic en "Seleccionar carpeta"
6. Verás que la ruta cambió en la parte inferior
7. Las siguientes descargas irán a esa carpeta

### Cambiar a otra carpeta
- Repite los pasos 2-6 con una nueva carpeta
- No hay límite de cambios

---

## Escenario 4: Descargar una Serie de Videos

### Objetivo
Descargar múltiples videos, uno tras otro.

### Pasos
1. Abre Media Downloader
2. **Primer video:**
   - Pega URL del video 1
   - Selecciona formato
   - Haz clic en Descargar
   - Espera a que termine

3. **Segundo video:**
   - Pega URL del video 2
   - Selecciona formato
   - Haz clic en Descargar
   - Espera a que termine

4. **Y así sucesivamente...**

### Nota
Actualmente, descarga un video a la vez. Espera a que termine cada descarga antes de iniciar la siguiente.

---

## Escenario 5: Extraer Solo el Audio de un Video

### Objetivo
Tienes un video que quieres guardar solo el audio (música de fondo, etc.)

### Pasos
1. Copia la URL del video (YouTube o X)
2. Abre Media Downloader
3. Pega la URL
4. **Importante**: Selecciona **MP3**
5. Haz clic en **Descargar**
6. La aplicación extraerá solo el audio

### Resultado
- Solo el audio del video
- Archivo mucho más pequeño que el video completo
- Perfecto para música, podcasts, tutoriales de audio

---

## Escenario 6: Resolver Error "yt-dlp no se encontró"

### Problema
Ves mensaje: "Error: yt-dlp no se encontró"

### Solución Paso a Paso
1. Abre PowerShell o CMD como administrador
2. Ejecuta:
   ```bash
   pip install yt-dlp
   ```
3. Espera a que termine (verás "Successfully installed")
4. Verifica:
   ```bash
   yt-dlp --version
   ```
5. Deberías ver un número de versión
6. Cierra PowerShell
7. Intenta la descarga nuevamente en Media Downloader

### Alternativa (Script automático)
```bash
install-dependencies.bat
```

---

## Escenario 7: Descargar Lista de Reproducción (Avanzado)

### Problema
Quieres descargar todos los videos de una playlist de YouTube.

### Solución
Con la interfaz actual, debes descargar uno a uno. Pero si quieres hacerlo automáticamente, usa yt-dlp directamente:

1. Abre PowerShell o CMD
2. Navega a la carpeta de destino:
   ```bash
   cd C:\Users\[TuNombre]\Downloads\MediaDownloader
   ```
3. Usa yt-dlp:
   ```bash
   # Descargar playlist completa como MP3
   yt-dlp -x --audio-format mp3 "https://www.youtube.com/playlist?list=PLxxxxx"
   ```

---

## Escenario 8: Ver los Archivos Descargados

### Objetivo
Encontrar y abrir los archivos que acabas de descargar.

### Método 1: Desde Media Downloader
1. Mira la ruta mostrada en "Carpeta de descargas:" (parte inferior)
2. Cópiala
3. Abre Explorador de Archivos (Windows + E)
4. Pega la ruta en la barra de direcciones
5. Presiona Enter
6. Verás tus descargas

### Método 2: Directo
1. Abre Explorador de Archivos
2. Navega a:
   ```
   C:\Users\[TuNombre]\Downloads\MediaDownloader
   ```
3. Verás tus archivos

### Método 3: Desde la app
En futuras versiones, podría haber un botón para abrir la carpeta directamente.

---

## Escenario 9: Reproducir Archivo Descargado

### Para MP3
1. Haz clic derecho en el archivo
2. Selecciona "Abrir con..."
3. Elige tu reproductor favorito:
   - Windows Media Player
   - VLC (recomendado)
   - iTunes
   - Spotify
   - Otros

### Para MP4
1. Haz doble clic en el archivo
2. Se abrirá con tu reproductor predeterminado
3. O haz clic derecho > "Abrir con..." y elige:
   - Windows Media Player
   - VLC (recomendado)
   - Movies & TV (Windows 10/11)
   - Otros

---

## Escenario 10: Crear Carpetas Organizadas

### Objetivo
Organizar descargas por categoría (Música, Videos, Tutoriales, etc.)

### Pasos
1. Abre Explorador de Archivos
2. Navega a `Downloads\MediaDownloader`
3. Crea carpetas:
   ```
   MediaDownloader/
   ??? Música/
   ??? Películas/
   ??? Tutoriales/
   ??? Otros/
   ```

4. En Media Downloader:
   - Antes de cada descarga
   - Haz clic en **Seleccionar...**
   - Elige la subcarpeta apropiada
   - Descarga
   - La descarga irá a esa carpeta

---

## Consejos y Trucos

### Consejo 1: Nombres de Archivo
Los archivos se guardan con el nombre del video/canción. Si es muy largo, considere moverlo después a una carpeta con nombre más corto.

### Consejo 2: Actualizar yt-dlp
YouTube cambia constantemente. Actualiza yt-dlp mensualmente:
```bash
pip install --upgrade yt-dlp
```

### Consejo 3: Verificar Espacio Disponible
Antes de descargar videos largos, verifica que tengas espacio:
```
Propiedades de la unidad C: (o D:, etc.)
```

### Consejo 4: Limpiar Descargas Antiguas
Después de un tiempo, limpia archivos que ya no necesites. Usa:
```bash
clean-old-downloads.bat
```

### Consejo 5: Hacer Backup
Si tienes descargas importantes, haz backup:
1. Copia la carpeta `MediaDownloader`
2. Guárdala en otra unidad o en la nube

---

## Solución de Problemas Comunes

### "La URL no es válida"
- Verifica que sea de YouTube o X
- Copia la URL correctamente desde el navegador

### "Descarga falló"
- Actualiza yt-dlp: `pip install --upgrade yt-dlp`
- Verifica tu conexión a internet
- Intenta con otra URL para descartar problemas específicos

### "No tengo espacio en disco"
- Abre Explorador de Archivos
- Haz clic derecho en la unidad > Propiedades
- Libera espacio eliminando archivos innecesarios

### "El archivo se ve corrompido"
- Intenta descargar nuevamente
- Si persiste, puede ser un problema del video original

---

**¡Disfruta descargando contenido!**

*Recuerda: Respeta los derechos de autor y las términos de servicio de las plataformas.*
