# ?? SOLUCIÓN DE PROBLEMAS - Media Downloader

Guía para resolver los problemas más comunes que encontrarás.

---

## ? "Acceso denegado" o "Error 403"

### Problema
YouTube/X rechaza la descarga diciendo "Acceso denegado" o mostrando error 403.

### Causas
- Contenido restringido geográficamente
- Necesita autenticación
- IP bloqueada temporalmente
- Descarga demasiado rápida (detección anti-bot)

### Soluciones

**Solución 1: Esperar y reintentar (LA MÁS COMÚN)**
```bash
# Espera 1-2 horas y vuelve a intentar
# YouTube bloquea temporalmente después de descargas rápidas
```

**Solución 2: Actualizar yt-dlp**
```bash
pip install --upgrade yt-dlp
```

**Solución 3: Usar cookies del navegador**
1. Abre tu navegador (Firefox o Chrome)
2. Inicia sesión en YouTube/X
3. Instala extensión para exportar cookies: "EditThisCookie" o "Get cookies.txt"
4. Exporta las cookies a un archivo `cookies.txt`
5. Coloca el archivo en la misma carpeta que la aplicación
6. Modifica `DownloadService.cs` para usar:
   ```csharp
   sb.Append("--cookies cookies.txt ");
   ```

**Solución 4: Cambiar servidor DNS**
```bash
# Usa Google DNS o Cloudflare
# En Windows: Panel de Control > Red > Cambiar configuración del adaptador
# DNS: 8.8.8.8 o 1.1.1.1
```

**Solución 5: Usar VPN (última opción)**
- Conecta a VPN
- Reinicia la aplicación
- Intenta nuevamente

---

## ? "El video no está disponible"

### Problema
Error: "This video is not available" o "Este video no está disponible"

### Causas
- Video fue eliminado
- Video es privado
- Video está restringido para tu país
- Canal/Usuario bloqueó la descarga

### Soluciones
- ? Verifica que puedas ver el video en el navegador
- ? Si es privado o eliminado, no se puede descargar
- ? Si está restringido geográficamente, usa VPN

---

## ? "No se encontró yt-dlp"

### Problema
Mensaje: "yt-dlp no se encontró"

### Causa
yt-dlp no está instalado o no está en PATH

### Soluciones

**Solución 1: Ejecutar script de instalación**
```bash
./install-dependencies.bat
```

**Solución 2: Instalar manualmente**
```bash
pip install yt-dlp
```

**Solución 3: Verificar instalación**
```bash
# Abre PowerShell o CMD y ejecuta:
yt-dlp --version

# Si ves un número (ej: 2024.01.01), está instalado correctamente
```

**Solución 4: Agregar a PATH manualmente**
1. Si yt-dlp está en una carpeta específica:
2. Copia `yt-dlp.exe` a la carpeta de la aplicación
3. Edita `DownloadService.cs` línea 117:
   ```csharp
   FileName = "yt-dlp.exe",  // O la ruta completa
   ```

---

## ? "URL no válida"

### Problema
La aplicación dice "La URL debe ser de YouTube o X"

### Causas
- URL copiada incorrectamente
- URL acortada (bit.ly, tinyurl, etc.)
- Espacios extras en la URL

### Soluciones
- ? Copia la URL completa desde la barra de direcciones
- ? Asegúrate de incluir `https://`
- ? No copies URLs acortadas, expande primero
- ? Elimina espacios al principio/final

**URLs correctas:**
```
? https://www.youtube.com/watch?v=dQw4w9WgXcQ
? https://youtu.be/dQw4w9WgXcQ
? https://x.com/usuario/status/1234567890
? https://twitter.com/usuario/status/1234567890
```

**URLs incorrectas:**
```
? http://youtu.be/dQw4w9WgXcQ  (falta 's' en https)
? youtu.be/dQw4w9WgXcQ  (sin protocolo)
? youtube.com/watch?v=dQw4w9WgXcQ  (sin https://)
```

---

## ? "No se puede escribir en la carpeta"

### Problema
Error: "No se puede escribir en la carpeta de destino"

### Causas
- No tienes permisos en la carpeta
- La carpeta está en una unidad de red con problemas
- La carpeta está en almacenamiento protegido

### Soluciones

**Solución 1: Cambiar carpeta de destino**
1. Abre Media Downloader
2. Haz clic en "Seleccionar..."
3. Elige una carpeta diferente (tu Escritorio, por ejemplo)
4. Intenta descargar

**Solución 2: Ejecutar como administrador**
1. Cierra Media Downloader
2. Haz clic derecho en el ejecutable
3. "Ejecutar como administrador"
4. Intenta descargar

**Solución 3: Dar permisos a la carpeta**
1. Navega a la carpeta en Explorador
2. Haz clic derecho > Propiedades
3. Pestaña Seguridad
4. Elige tu usuario
5. Editar > Permitir Control Total

---

## ? Descarga muy lenta

### Problema
La descarga avanza muy lentamente

### Causas
- Velocidad de internet lenta
- Servidor congestionado
- Calidad muy alta (video 4K)
- Demasiadas conexiones simultáneas

### Soluciones

**Solución 1: Cambiar formato a menor calidad**
- Para YouTube/X: Selecciona MP3 en lugar de MP4
- MP3 es mucho más pequeño y rápido

**Solución 2: Aumentar fragmentos simultáneos**
Edita `DownloadService.cs` línea ~63:
```csharp
// Cambiar de 4 a 8
sb.Append("--concurrent-fragment-downloads 8 ");
```

**Solución 3: Esperar horario menos congestionado**
- Descarga por la noche o madrugada
- Los servidores tienen menos carga

---

## ? "Timeout: Descarga tardó demasiado"

### Problema
La aplicación dice que la descarga tardó demasiado

### Causas
- Video muy grande (muy larga duración)
- Internet lenta
- Servidor congestionado
- Timeout de 10 minutos alcanzado

### Soluciones

**Solución 1: Aumentar timeout**
Edita `DownloadService.cs` línea ~144:
```csharp
// Cambiar de 600000 a 1200000 (20 minutos)
bool completed = process.WaitForExit(1200000);
```

**Solución 2: Intentar formato más pequeño**
- MP3 en lugar de MP4
- Menor calidad

**Solución 3: Descargar en horario diferente**
- La noche suele ser más rápido
- Servidores menos congestionados

---

## ? El archivo descargado está corrupto

### Problema
El archivo MP3 o MP4 no se reproduce

### Causas
- Descarga interrumpida
- Conexión inestable
- Error en yt-dlp
- Reproductor incompatible

### Soluciones

**Solución 1: Eliminar y descargar nuevamente**
```bash
# Elimina el archivo descargado
# Intenta la descarga de nuevo
```

**Solución 2: Usar reproductor diferente**
- Intenta con VLC (recomendado)
- Windows Media Player
- Otros reproductores

**Solución 3: Verificar integridad**
```bash
# Si tienes ffmpeg instalado:
ffmpeg -v error -i archivo.mp4 -f null -
```

---

## ? El archivo tiene nombre extraño o truncado

### Problema
El archivo se descarga con nombre: "%(title)s.%(ext)s" o está cortado

### Causas
- Variables de yt-dlp no se expandieron
- Nombre del video es muy largo (Windows limita a 255 caracteres)
- Caracteres especiales en el nombre

### Soluciones

**Solución 1: Usar nombres más cortos**
Edita `DownloadService.cs` línea ~100:
```csharp
// Cambiar a nombre numérico
sb.Append($"-o \"{System.IO.Path.Combine(_downloadPath, "%(id)s.%(ext)s")}\" ");
```

**Solución 2: Renombrar el archivo manualmente**
1. Descarga el archivo
2. Haz clic derecho > Renombrar
3. Dale un nombre más corto

---

## ? X/Twitter: "Acceso denegado" específicamente

### Problema
X rechaza la descarga de un tweet

### Causas
- Tweet es privado/protegido
- Usuario bloqueó descargas
- Video tiene restricciones de edad
- Contenido no disponible en tu región

### Soluciones

**Solución 1: Verificar visibilidad del tweet**
- ¿Puedes ver el tweet en tu navegador sin iniciar sesión?
- Si no, es privado y no se puede descargar

**Solución 2: Iniciar sesión**
1. Abre tu navegador
2. Inicia sesión en X
3. Copia la URL de tu navegador
4. Pega en la aplicación
5. Intenta descargar

**Solución 3: Usar tweet alternativo**
- Si el tweet está protegido, intenta otro
- Algunos usuarios tienen descargas permitidas, otros no

---

## ? La aplicación se congela

### Problema
La aplicación deja de responder

### Causas
- Descarga en progreso (es normal, espera)
- Proceso yt-dlp congelado
- Permiso de escritura esperando
- Bug en la aplicación

### Soluciones

**Solución 1: Esperar (recomendado)**
- Las descargas pueden tardar varios minutos
- Espera a que termine

**Solución 2: Forzar cierre**
```bash
# Abre Administrador de Tareas (Ctrl + Shift + Esc)
# Busca "MediaDownloader"
# Haz clic derecho > Finalizar tarea
```

**Solución 3: Reiniciar la aplicación**
1. Cierra Media Downloader
2. Abre nuevamente
3. Intenta con una URL diferente

---

## ? YouTube: "Sign in to confirm you're not a bot"

### Problema
YouTube pide confirmar que no eres un bot

### Causas
- Descargaste demasiados videos muy rápido
- YouTube está detectando actividad sospechosa
- IP bloqueada temporalmente

### Soluciones

**Solución 1: Esperar 24 horas**
- YouTube bloquea automáticamente por 24 horas
- Después se debloquea

**Solución 2: Usar cookies de navegador**
1. Abre Firefox/Chrome
2. Inicia sesión en YouTube
3. Exporta cookies (extensión EditThisCookie)
4. Coloca en archivo `cookies.txt` en carpeta de la app

**Solución 3: Usar VPN diferente**
- Cambia de IP usando VPN
- Prueba con otra ubicación

**Solución 4: Usar servidor proxy (avanzado)**
Edita `DownloadService.cs`:
```csharp
sb.Append("--proxy [protocolo]://[IP]:[puerto] ");
```

---

## ? Descarga Exitosa pero sin archivo

### Problema
La aplicación dice que la descarga se completó, pero no ves el archivo

### Causas
- Archivo se guardó en carpeta diferente
- Explorador no actualizado
- Archivo oculto

### Soluciones

**Solución 1: Actualizar explorador**
1. Abre Explorador
2. Presiona F5 para refrescar
3. Busca el archivo

**Solución 2: Verificar carpeta correcta**
1. Abre Media Downloader
2. Mira la ruta en "Carpeta de descargas:"
3. Navega allí en Explorador

**Solución 3: Mostrar archivos ocultos**
1. Explorador > Ver > Opciones
2. Pestaña: Ver
3. Marcar: "Mostrar archivos, carpetas y unidades ocultas"

---

## ?? Si nada funciona

### Pasos de diagnóstico

1. **Verifica yt-dlp:**
   ```bash
   yt-dlp --version
   yt-dlp "https://youtu.be/wreeHQQOIZg?list=RDwreeHQQOIZg"
   ```

2. **Si funciona con yt-dlp directo, es un problema de la app**
   - Recompila: `dotnet build`
   - Ejecuta nuevamente: `dotnet run`

3. **Si no funciona con yt-dlp directo:**
   ```bash
   pip install --upgrade yt-dlp
   pip install --upgrade yt-dlp-nightly
   ```

4. **Última opción: Usar youtube-dl alternativo**
   ```bash
   pip uninstall yt-dlp
   pip install youtube-dl
   # Modificar en código: FileName = "youtube-dl"
   ```

---

## ?? Reportar un Bug

Si seguiste todos estos pasos y sigue sin funcionar:

1. Documenta:
   - URL exacta que intentaste
   - Mensaje de error completo
   - Versión de yt-dlp: `yt-dlp --version`
   - Versión de Python: `python --version`
   - Sistema operativo

2. Intenta en terminal:
   ```bash
   yt-dlp -v "TU_URL"  # -v para modo verbose
   ```

3. Comparte la salida completa

---

**¡Esperamos que hayas resuelto tu problema!**

Si tienes otra pregunta, consulta [FAQ.md](FAQ.md)
