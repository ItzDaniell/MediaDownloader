using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaDownloader.Services
{
    public class DownloadService
    {
        private readonly string _downloadPath;
        private static readonly System.Net.Http.HttpClient _httpClient = new System.Net.Http.HttpClient();

        public event EventHandler<string>? StatusChanged;
        public event EventHandler<double>? ProgressChanged;

        public DownloadService(string downloadPath = "")
        {
            _downloadPath = string.IsNullOrEmpty(downloadPath)
                ? System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "MediaDownloader")
                : downloadPath;

            Directory.CreateDirectory(_downloadPath);
        }

        public async Task<bool> DownloadFromYoutubeAsync(string url, string format = "mp3")
        {
            return await DownloadAsync(url, format, "YouTube");
        }

        public async Task<bool> DownloadFromXAsync(string url, string format = "mp3")
        {
            return await DownloadAsync(url, format, "X");
        }

        private async Task<bool> DownloadAsync(string url, string format, string platform)
        {
            try
            {
                // Limpiar parámetros adicionales de listas o mix (?list=...)
                url = CleanUrl(url);

                NotifyStatus($"Iniciando descarga de {platform}...");

                // Asegurar yt-dlp portable
                NotifyStatus("Verificando componentes...");
                bool ytDlpReady = await YtDlpManager.EnsureYtDlpAsync(msg => NotifyStatus(msg));
                if (!ytDlpReady)
                {
                    NotifyStatus("Error: No se pudo obtener yt-dlp. Verifica tu conexión a internet.");
                    return false;
                }

                // Asegurar FFmpeg si no está disponible para MP3 o MP4
                bool ffmpegReady = await FFmpegManager.EnsureFFmpegAsync();

                if (!ffmpegReady && format.Equals("mp3", StringComparison.OrdinalIgnoreCase))
                {
                    NotifyStatus("Aviso: FFmpeg no disponible. Descargando como WebM...");
                    format = "webm";
                }

                string downloadUrl = url;
                string? customTitle = null;

                // Para X (Twitter), intentar resolver el stream directo primero porque
                // la API de invitados de X bloquea tweets sensibles o con amplify videos en yt-dlp
                if (platform == "X")
                {
                    NotifyStatus("Obteniendo video de X...");
                    var (directUrl, title) = await TryResolveXVideoAsync(url);
                    if (!string.IsNullOrEmpty(directUrl))
                    {
                        downloadUrl = directUrl;
                        customTitle = title;
                    }
                }

                // Construir argumentos
                var args = BuildYtDlpArguments(downloadUrl, format, customTitle);
                NotifyStatus("Conectando y descargando...");

                // Ejecutar descarga
                var (exitCode, output, error) = await ExecuteYtDlpAsync(args);

                // Si falló con URL de X directa o sin ella, intentar con la URL original o viceversa
                if (exitCode != 0 && platform == "X" && downloadUrl != url)
                {
                    NotifyStatus("Reintentando con método alternativo...");
                    args = BuildYtDlpArguments(url, format);
                    (exitCode, output, error) = await ExecuteYtDlpAsync(args);
                }

                // Verificar resultado
                if (exitCode == 0)
                {
                    NotifyStatus("Descarga completada correctamente");
                    ProgressChanged?.Invoke(this, 100);
                    return true;
                }
                else
                {
                    var errorMsg = ParseError(error);
                    NotifyStatus($"Error: {errorMsg}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                NotifyStatus($"Error: {ex.Message}");
                return false;
            }
        }

        private async Task<(string? directUrl, string? title)> TryResolveXVideoAsync(string url)
        {
            try
            {
                var match = System.Text.RegularExpressions.Regex.Match(
                    url, 
                    @"(?:twitter\.com|x\.com)\/(?:#!\/)?(\w+)\/status\/(\d+)", 
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                if (!match.Success)
                    return (null, null);

                string username = match.Groups[1].Value;
                string statusId = match.Groups[2].Value;

                // Intentar a través de endpoints de resolución de medios de Twitter/X
                string[] endpoints = new[]
                {
                    $"https://api.fxtwitter.com/{username}/status/{statusId}",
                    $"https://api.vxtwitter.com/{username}/status/{statusId}"
                };

                foreach (var endpoint in endpoints)
                {
                    try
                    {
                        using var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, endpoint);
                        request.Headers.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");

                        using var response = await _httpClient.SendAsync(request);
                        if (!response.IsSuccessStatusCode)
                            continue;

                        var json = await response.Content.ReadAsStringAsync();
                        using var doc = System.Text.Json.JsonDocument.Parse(json);
                        var root = doc.RootElement;

                        if (root.TryGetProperty("tweet", out var tweet) &&
                            tweet.TryGetProperty("media", out var media) &&
                            media.TryGetProperty("videos", out var videos) &&
                            videos.ValueKind == System.Text.Json.JsonValueKind.Array &&
                            videos.GetArrayLength() > 0)
                        {
                            var firstVideo = videos[0];
                            string? videoUrl = null;

                            if (firstVideo.TryGetProperty("url", out var urlElem))
                            {
                                videoUrl = urlElem.GetString();
                            }

                            if (!string.IsNullOrEmpty(videoUrl))
                            {
                                string title = $"{username}_{statusId}";
                                return (videoUrl, title);
                            }
                        }
                    }
                    catch
                    {
                        // Intentar siguiente endpoint
                    }
                }
            }
            catch
            {
                // Silencioso
            }

            return (null, null);
        }

        private string CleanUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return url;

            if (url.Contains("youtube.com/watch") || url.Contains("youtu.be/"))
            {
                try
                {
                    var uri = new Uri(url);
                    if (uri.Host.Contains("youtu.be"))
                    {
                        string videoId = uri.AbsolutePath.TrimStart('/');
                        if (!string.IsNullOrEmpty(videoId))
                        {
                            return $"https://www.youtube.com/watch?v={videoId}";
                        }
                    }
                    else if (uri.Host.Contains("youtube.com"))
                    {
                        var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
                        string? videoId = query["v"];
                        if (!string.IsNullOrEmpty(videoId))
                        {
                            return $"https://www.youtube.com/watch?v={videoId}";
                        }
                    }
                }
                catch
                {
                    // Conservar original
                }
            }

            return url;
        }

        private string BuildYtDlpArguments(string url, string format, string? customOutputName = null)
        {
            var args = new List<string>();

            // Argumentos base estables
            args.Add("--no-warnings");
            args.Add("--no-playlist");

            string ffmpegPath = FFmpegManager.GetFFmpegPath();

            // Formato
            if (format.Equals("mp3", StringComparison.OrdinalIgnoreCase))
            {
                args.Add("-x");
                args.Add("--audio-format");
                args.Add("mp3");
                args.Add("--audio-quality");
                args.Add("192");

                if (!ffmpegPath.Equals("ffmpeg", StringComparison.OrdinalIgnoreCase))
                {
                    args.Add("--ffmpeg-location");
                    args.Add(ffmpegPath);
                }
            }
            else if (format.Equals("webm", StringComparison.OrdinalIgnoreCase))
            {
                args.Add("-f");
                args.Add("bestaudio[ext=webm]");
            }
            else if (format.Equals("mp4", StringComparison.OrdinalIgnoreCase))
            {
                // Formato anterior estable y compatible: H.264 (avc1) + audio AAC (m4a)
                // Se reproduce en cualquier reproductor sin requerir codecs AV1 y descarga rápido
                args.Add("-f");
                args.Add("bv*[vcodec^=avc1]+ba[ext=m4a]/bv*[vcodec^=mp4v]+ba/b[ext=mp4]/best");

                if (!ffmpegPath.Equals("ffmpeg", StringComparison.OrdinalIgnoreCase))
                {
                    args.Add("--ffmpeg-location");
                    args.Add(ffmpegPath);
                }
            }

            // Ruta de salida con plantilla segura
            args.Add("-o");
            string outputTemplate = string.IsNullOrEmpty(customOutputName)
                ? Path.Combine(_downloadPath, "%(title)s.%(ext)s")
                : Path.Combine(_downloadPath, $"{customOutputName}.%(ext)s");
            args.Add(outputTemplate);

            // URL
            args.Add(url);

            return string.Join(" ", args.Select(a => a.Contains(" ") ? $"\"{a}\"" : a));
        }

        private async Task<(int exitCode, string output, string error)> ExecuteYtDlpAsync(string arguments)
        {
            return await Task.Run(() =>
            {
                var outputBuilder = new StringBuilder();
                var errorBuilder = new StringBuilder();

                try
                {
                    var process = new Process();
                    process.StartInfo = new ProcessStartInfo
                    {
                        FileName = YtDlpManager.GetYtDlpPath(),
                        Arguments = arguments,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true,
                        StandardOutputEncoding = Encoding.UTF8,
                        StandardErrorEncoding = Encoding.UTF8
                    };

                    process.OutputDataReceived += (s, e) =>
                    {
                        if (!string.IsNullOrWhiteSpace(e.Data))
                        {
                            if (e.Data.Contains("[download]") && e.Data.Contains("%"))
                            {
                                NotifyStatus(e.Data.Trim());

                                var match = System.Text.RegularExpressions.Regex.Match(e.Data, @"(\d+(\.\d+)?)%");
                                if (match.Success && double.TryParse(match.Groups[1].Value, System.Globalization.CultureInfo.InvariantCulture, out double percent))
                                {
                                    ProgressChanged?.Invoke(this, percent);
                                }
                            }
                            outputBuilder.AppendLine(e.Data);
                        }
                    };

                    process.ErrorDataReceived += (s, e) =>
                    {
                        if (!string.IsNullOrWhiteSpace(e.Data))
                        {
                            errorBuilder.AppendLine(e.Data);
                        }
                    };

                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    if (!process.WaitForExit(900000)) // 15 minutos
                    {
                        try { process.Kill(); } catch { }
                        return (-1, "", "Timeout: La descarga tardó demasiado");
                    }

                    return (process.ExitCode, outputBuilder.ToString(), errorBuilder.ToString());
                }
                catch (Exception ex)
                {
                    return (-1, "", ex.Message);
                }
            });
        }

        private string ParseError(string error)
        {
            if (string.IsNullOrEmpty(error))
                return "Error desconocido en la descarga";

            if (error.Contains("No video could be found in this tweet", StringComparison.OrdinalIgnoreCase))
                return "Este tweet no contiene ningún video disponible (o es solo imagen/texto)";

            if (error.Contains("unavailable") || error.Contains("This video is unavailable"))
                return "El video no está disponible (puede estar restringido por país o eliminado)";

            if (error.Contains("not available") || error.Contains("removed") || error.Contains("deleted"))
                return "El contenido no está disponible (privado, eliminado o restringido)";

            if (error.Contains("403") || error.Contains("access denied", StringComparison.OrdinalIgnoreCase))
                return "Acceso denegado (posible bloqueo temporal)";

            if (error.Contains("private") || error.Contains("protected"))
                return "El contenido es privado o protegido";

            if (error.Contains("sign in") || error.Contains("authentication"))
                return "Se requiere iniciar sesión o verificación captcha";

            if (error.Contains("404") || error.Contains("not found"))
                return "URL no encontrada (video eliminado)";

            var lines = error.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            return lines.LastOrDefault(l => !l.StartsWith("WARNING:")) ?? lines.FirstOrDefault() ?? "Error en la descarga";
        }

        private void NotifyStatus(string message)
        {
            StatusChanged?.Invoke(this, message);
        }

        public bool IsValidUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return false;

            url = url.ToLowerInvariant();
            return (url.Contains("youtube.com") || url.Contains("youtu.be")) ||
                   (url.Contains("twitter.com") || url.Contains("x.com"));
        }

        public string GetPlatform(string url)
        {
            url = url.ToLowerInvariant();

            if (url.Contains("youtube.com") || url.Contains("youtu.be"))
                return "YouTube";
            if (url.Contains("twitter.com") || url.Contains("x.com"))
                return "X";

            return "Desconocida";
        }

        public string GetDownloadPath() => _downloadPath;
    }
}
