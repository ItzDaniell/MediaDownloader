using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MediaDownloader.Services
{
    public class FFmpegManager
    {
        private static readonly string FFmpegFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ffmpeg");
        private static readonly string FFmpegExePath = Path.Combine(FFmpegFolder, "ffmpeg.exe");

        public static async Task<bool> EnsureFFmpegAsync()
        {
            if (File.Exists(FFmpegExePath))
            {
                return true;
            }

            Directory.CreateDirectory(FFmpegFolder);

            try
            {
                return await DownloadFFmpegAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error descargando FFmpeg: {ex.Message}");
                return false;
            }
        }

        private static async Task<bool> DownloadFFmpegAsync()
        {
            const string downloadUrl = "https://github.com/BtbN/FFmpeg-Builds/releases/download/latest/ffmpeg-master-latest-win64-gpl-shared.zip";
            string zipPath = Path.Combine(FFmpegFolder, "ffmpeg.zip");

            try
            {
                Console.WriteLine("Descargando FFmpeg...");

                using (var httpClient = new System.Net.Http.HttpClient())
                using (var response = await httpClient.GetAsync(downloadUrl, System.Net.Http.HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();
                    using (var fs = new FileStream(zipPath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        await response.Content.CopyToAsync(fs);
                    }
                }

                Console.WriteLine("Extrayendo FFmpeg...");

                System.IO.Compression.ZipFile.ExtractToDirectory(zipPath, FFmpegFolder, overwriteFiles: true);

                var ffmpegExe = Directory.GetFiles(FFmpegFolder, "ffmpeg.exe", SearchOption.AllDirectories).FirstOrDefault();

                if (ffmpegExe != null)
                {
                    string binDir = Path.GetDirectoryName(ffmpegExe)!;
                    if (!string.Equals(binDir, FFmpegFolder, StringComparison.OrdinalIgnoreCase))
                    {
                        foreach (var file in Directory.GetFiles(binDir))
                        {
                            string destFile = Path.Combine(FFmpegFolder, Path.GetFileName(file));
                            File.Copy(file, destFile, overwrite: true);
                        }
                    }
                    Console.WriteLine("FFmpeg y dependencias extraidas correctamente");
                }

                try { File.Delete(zipPath); } catch { }

                return File.Exists(FFmpegExePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public static string GetFFmpegPath()
        {
            if (File.Exists(FFmpegExePath))
                return FFmpegFolder;

            if (Directory.Exists(FFmpegFolder))
            {
                var subExe = Directory.GetFiles(FFmpegFolder, "ffmpeg.exe", SearchOption.AllDirectories).FirstOrDefault();
                if (subExe != null)
                {
                    string binDir = Path.GetDirectoryName(subExe)!;
                    return binDir;
                }
            }

            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "where",
                        Arguments = "ffmpeg",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    }
                };
                process.Start();
                string result = process.StandardOutput.ReadToEnd().Trim();
                process.WaitForExit();

                if (!string.IsNullOrEmpty(result))
                {
                    string firstLine = result.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? "";
                    if (!string.IsNullOrEmpty(firstLine))
                    {
                        return Path.GetDirectoryName(firstLine) ?? firstLine;
                    }
                }

                return "ffmpeg";
            }
            catch
            {
                return "ffmpeg";
            }
        }
    }
}
