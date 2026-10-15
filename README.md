# 📥 Media Downloader

Aplicación de escritorio en **WPF (.NET 8.0)** para descargar audio y video desde **YouTube** y **X (Twitter)** en formatos **MP3** y **MP4**, utilizando `yt-dlp` y `FFmpeg`.

---

## 📁 Estructura del Proyecto

El repositorio ha sido reorganizado para mantener la raíz limpia y clara:

```
MediaDownloader/
├── docs/                        # 📚 Toda la documentación técnica y guías de uso
│   ├── 00-INICIO.md
│   ├── ARCHITECTURE.md
│   ├── CHANGELOG.md
│   ├── COMPILACION.md
│   ├── EJEMPLOS_USO.md
│   ├── ENTREGA_FINAL.md
│   ├── FAQ.md
│   ├── GUIA_RAPIDA.md
│   ├── INDICE.md
│   ├── PROJECT_SUMMARY.md
│   └── TROUBLESHOOTING.md
│
├── scripts/                     # ⚙️ Scripts .bat para instalación y utilidades
│   ├── install-all.bat          # Instala dependencias y prepara el entorno
│   ├── install-dependencies.bat # Instalación de yt-dlp
│   ├── install-ffmpeg.bat       # Instalación de FFmpeg
│   ├── install-ffmpeg-simple.bat
│   ├── install-ytdlp.bat
│   └── clean-old-downloads.bat  # Limpieza de descargas antiguas
│
├── MediaDownloader/             # 💻 Código fuente C# / WPF
│   ├── Services/
│   │   ├── DownloadService.cs   # Lógica de descarga e integración yt-dlp
│   │   └── FFmpegManager.cs     # Detección y gestión portable de FFmpeg
│   ├── MainWindow.xaml          # Interfaz visual
│   ├── MainWindow.xaml.cs
│   └── MediaDownloader.csproj
│
├── MediaDownloader.sln          # Solución Visual Studio
└── README.md                    # Este archivo
```

---

## 🚀 Requisitos

1. **.NET 8.0 SDK** (o superior).
2. **yt-dlp**: Instalado en el sistema (`pip install yt-dlp` o mediante los scripts en `scripts/`).
3. **FFmpeg**: Gestionado automáticamente por la aplicación (descarga portable a la carpeta de ejecución) o disponible en el PATH del sistema.

---

## 🛠️ Compilación y Ejecución

Compilar el proyecto con .NET CLI:
```cmd
dotnet build
```

Ejecutar la aplicación:
```cmd
dotnet run --project MediaDownloader
```

O abrir `MediaDownloader.sln` directamente en Visual Studio.

---

## 📖 Documentación

Toda la documentación detallada se encuentra en la carpeta [`docs/`](file:///d:/VS%23/MediaDownloader/docs):
- [Índice de documentación](file:///d:/VS%23/MediaDownloader/docs/INDICE.md)
- [Guía rápida de uso](file:///d:/VS%23/MediaDownloader/docs/GUIA_RAPIDA.md)
- [Ejemplos de uso](file:///d:/VS%23/MediaDownloader/docs/EJEMPLOS_USO.md)
- [Resolución de problemas](file:///d:/VS%23/MediaDownloader/docs/TROUBLESHOOTING.md)
- [Arquitectura del software](file:///d:/VS%23/MediaDownloader/docs/ARCHITECTURE.md)
