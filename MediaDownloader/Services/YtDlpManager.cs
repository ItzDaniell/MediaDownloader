using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace MediaDownloader.Services
{
    /// <summary>
    /// Gestiona la descarga y uso portable de yt-dlp.exe.
    /// Descarga automáticamente el ejecutable standalone junto a la aplicación,
    /// sin necesidad de instalación del usuario ni configuración del PATH.
    /// </summary>
    public class YtDlpManager
    {
        private static readonly string YtDlpFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "yt-dlp");
        private static readonly string YtDlpExePath = Path.Combine(YtDlpFolder, "yt-dlp.exe");

        private static readonly HttpClient _httpClient = new HttpClient();

        /// <summary>
        /// Asegura que yt-dlp.exe esté disponible. Si no existe, lo descarga.
        /// </summary>
        public static async Task<bool> EnsureYtDlpAsync(Action<string>? onStatus = null)
        {
            // 1. Ya existe en la carpeta portable
            if (File.Exists(YtDlpExePath))
            {
                onStatus?.Invoke("yt-dlp encontrado (portable)");
                return true;
            }

            // 2. Verificar si está en el PATH del sistema
            string? systemPath = FindInSystemPath();
            if (systemPath != null)
            {
                onStatus?.Invoke($"yt-dlp encontrado en el sistema: {systemPath}");
                return true;
            }

            // 3. Descargar portable
            onStatus?.Invoke("Descargando yt-dlp (primera vez, ~12 MB)...");
            Directory.CreateDirectory(YtDlpFolder);

            try
            {
                return await DownloadYtDlpAsync(onStatus);
            }
            catch (Exception ex)
            {
                onStatus?.Invoke($"Error descargando yt-dlp: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Devuelve la ruta completa al ejecutable yt-dlp.exe.
        /// Prioridad: portable > sistema.
        /// </summary>
        public static string GetYtDlpPath()
        {
            // Prioridad 1: Portable junto a la app
            if (File.Exists(YtDlpExePath))
                return YtDlpExePath;

            // Prioridad 2: En el PATH del sistema
            string? systemPath = FindInSystemPath();
            if (systemPath != null)
                return systemPath;

            // Fallback: asume que está accesible (fallará si no lo está)
            return "yt-dlp";
        }

        /// <summary>
        /// Actualiza yt-dlp a la última versión descargando el exe de nuevo.
        /// </summary>
        public static async Task<bool> UpdateYtDlpAsync(Action<string>? onStatus = null)
        {
            onStatus?.Invoke("Actualizando yt-dlp...");

            // Primero intentar auto-update si es portable
            if (File.Exists(YtDlpExePath))
            {
                try
                {
                    // Borrar y re-descargar
                    File.Delete(YtDlpExePath);
                }
                catch (Exception ex)
                {
                    onStatus?.Invoke($"No se pudo eliminar la versión anterior: {ex.Message}");
                    return false;
                }
            }

            Directory.CreateDirectory(YtDlpFolder);
            return await DownloadYtDlpAsync(onStatus);
        }

        /// <summary>
        /// Obtiene la versión actual de yt-dlp.
        /// </summary>
        public static async Task<string?> GetVersionAsync()
        {
            string ytDlpPath = GetYtDlpPath();

            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = ytDlpPath,
                        Arguments = "--version",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                string version = (await process.StandardOutput.ReadToEndAsync()).Trim();
                await process.WaitForExitAsync();

                return string.IsNullOrEmpty(version) ? null : version;
            }
            catch
            {
                return null;
            }
        }

        private static async Task<bool> DownloadYtDlpAsync(Action<string>? onStatus = null)
        {
            // yt-dlp publica un .exe standalone en cada release de GitHub
            const string downloadUrl = "https://github.com/yt-dlp/yt-dlp/releases/latest/download/yt-dlp.exe";

            try
            {
                using var response = await _httpClient.GetAsync(downloadUrl, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();

                var totalBytes = response.Content.Headers.ContentLength;
                using var contentStream = await response.Content.ReadAsStreamAsync();
                using var fileStream = new FileStream(YtDlpExePath, FileMode.Create, FileAccess.Write, FileShare.None);

                var buffer = new byte[81920];
                long bytesRead = 0;
                int read;
                int lastPercent = -1;

                while ((read = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    await fileStream.WriteAsync(buffer, 0, read);
                    bytesRead += read;

                    if (totalBytes.HasValue && totalBytes > 0)
                    {
                        int percent = (int)(bytesRead * 100 / totalBytes.Value);
                        if (percent != lastPercent && percent % 10 == 0)
                        {
                            lastPercent = percent;
                            onStatus?.Invoke($"Descargando yt-dlp... {percent}%");
                        }
                    }
                }

                onStatus?.Invoke("yt-dlp descargado correctamente");
                return File.Exists(YtDlpExePath);
            }
            catch (Exception ex)
            {
                onStatus?.Invoke($"Error descargando yt-dlp: {ex.Message}");

                // Limpiar archivo parcial
                try { if (File.Exists(YtDlpExePath)) File.Delete(YtDlpExePath); } catch { }

                return false;
            }
        }

        private static string? FindInSystemPath()
        {
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "where",
                        Arguments = "yt-dlp",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    }
                };
                process.Start();
                string result = process.StandardOutput.ReadToEnd().Trim();
                process.WaitForExit();

                if (process.ExitCode == 0 && !string.IsNullOrEmpty(result))
                {
                    string firstLine = result.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)[0];
                    if (File.Exists(firstLine))
                        return firstLine;
                }
            }
            catch { }

            return null;
        }
    }
}
