using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using MediaDownloader.Services;

namespace MediaDownloader
{
    public partial class MainWindow : Window
    {
        private DownloadService? _downloadService;
        private string _currentDownloadPath = "";

        // Win32 API para la barra de título oscura
        [DllImport("dwmapi.dll", PreserveSig = true)]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;

        public MainWindow()
        {
            InitializeComponent();
            InitializeApplication();
            ApplyDarkTitleBar();
        }

        /// <summary>
        /// Aplica el modo oscuro a la barra de título de Windows para que combine con el tema de la aplicación.
        /// </summary>
        private void ApplyDarkTitleBar()
        {
            // Se necesita esperar a que la ventana tenga un handle nativo
            if (PresentationSource.FromVisual(this) is HwndSource hwndSource)
            {
                SetDarkTitleBar(hwndSource.Handle);
            }
            else
            {
                // Si la ventana aún no tiene handle, registramos el evento
                this.SourceInitialized += (s, e) =>
                {
                    var source = (HwndSource)PresentationSource.FromVisual(this);
                    SetDarkTitleBar(source.Handle);
                };
            }
        }

        private static void SetDarkTitleBar(IntPtr handle)
        {
            int useImmersiveDarkMode = 1; // 1 = activar modo oscuro
            DwmSetWindowAttribute(handle, DWMWA_USE_IMMERSIVE_DARK_MODE, ref useImmersiveDarkMode, sizeof(int));
        }

        private void InitializeApplication()
        {
            _currentDownloadPath = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), 
                "Downloads", 
                "MediaDownloader");

            SetupDownloadService();
            UpdatePathDisplay();
        }

        private void SetupDownloadService()
        {
            _downloadService = new DownloadService(_currentDownloadPath);

            _downloadService.StatusChanged += (s, status) =>
            {
                Dispatcher.Invoke(() =>
                {
                    StatusTextBlock.Text = status;
                });
            };

            _downloadService.ProgressChanged += (s, percent) =>
            {
                Dispatcher.Invoke(() =>
                {
                    double clamped = Math.Clamp(percent, 0, 100);
                    ProgressBar.Value = clamped;
                    ProgressPercentageTextBlock.Text = $"{(int)clamped}%";
                });
            };
        }

        private void UpdatePathDisplay()
        {
            if (_downloadService != null)
            {
                PathTextBlock.Text = _downloadService.GetDownloadPath();
            }
        }

        private void UrlTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string url = UrlTextBox.Text.Trim();

            if (string.IsNullOrEmpty(url))
            {
                PlatformBadgeText.Text = "● Esperando enlace...";
                PlatformBadgeText.Foreground = new SolidColorBrush(Color.FromRgb(148, 163, 184)); // #94A3B8
                PlatformBadgeBorder.Background = new SolidColorBrush(Color.FromRgb(26, 36, 56));  // #1A2438
                PlatformBadgeBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(40, 56, 82)); // #283852
                return;
            }

            if (_downloadService == null) return;

            string platform = _downloadService.GetPlatform(url);

            if (platform == "YouTube")
            {
                PlatformBadgeText.Text = "● YouTube Detectado";
                PlatformBadgeText.Foreground = new SolidColorBrush(Color.FromRgb(248, 113, 113)); // #F87171
                PlatformBadgeBorder.Background = new SolidColorBrush(Color.FromRgb(69, 10, 10));  // #450A0A
                PlatformBadgeBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(153, 27, 27)); // #991B1B
            }
            else if (platform == "X")
            {
                PlatformBadgeText.Text = "● X (Twitter) Detectado";
                PlatformBadgeText.Foreground = new SolidColorBrush(Color.FromRgb(248, 250, 252)); // #F8FAFC
                PlatformBadgeBorder.Background = new SolidColorBrush(Color.FromRgb(24, 24, 27));  // #18181B
                PlatformBadgeBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(82, 82, 91)); // #52525B
            }
            else
            {
                PlatformBadgeText.Text = "● Enlace no reconocido";
                PlatformBadgeText.Foreground = new SolidColorBrush(Color.FromRgb(251, 146, 60)); // #FB923C
                PlatformBadgeBorder.Background = new SolidColorBrush(Color.FromRgb(67, 20, 7));   // #431407
                PlatformBadgeBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(154, 52, 18)); // #9A3412
            }
        }

        private void PasteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Clipboard.ContainsText())
                {
                    UrlTextBox.Text = Clipboard.GetText().Trim();
                    UrlTextBox.Focus();
                    UrlTextBox.CaretIndex = UrlTextBox.Text.Length;
                }
            }
            catch (Exception ex)
            {
                StatusTextBlock.Text = $"No se pudo leer el portapapeles: {ex.Message}";
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            UrlTextBox.Clear();
            ProgressBar.Value = 0;
            ProgressPercentageTextBlock.Text = "0%";
            StatusTextBlock.Text = "Sistema listo. Pega una URL y haz clic en Iniciar Descarga.";
            StatusDot.Fill = new SolidColorBrush(Color.FromRgb(16, 185, 129)); // #10B981
        }

        private void BrowseFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                FileName = "Seleccionar Carpeta",
                Filter = "Carpetas|",
                ValidateNames = false,
                CheckFileExists = false,
                CheckPathExists = true,
            };

            if (dialog.ShowDialog() == true)
            {
                string folderPath = System.IO.Path.GetDirectoryName(dialog.FileName) ?? _currentDownloadPath;
                _currentDownloadPath = folderPath;
                SetupDownloadService();
                UpdatePathDisplay();
                StatusTextBlock.Text = "Carpeta de almacenamiento actualizada correctamente.";
            }
        }

        private void OpenFolderButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!Directory.Exists(_currentDownloadPath))
                {
                    Directory.CreateDirectory(_currentDownloadPath);
                }

                Process.Start(new ProcessStartInfo
                {
                    FileName = _currentDownloadPath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                ShowNotification("Acceso a Disco", "No se pudo abrir el explorador", ex.Message, NotificationType.Error);
            }
        }

        private async void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            string url = UrlTextBox.Text.Trim();

            if (string.IsNullOrEmpty(url))
            {
                ShowNotification("URL Requerida", "Campo de dirección vacío", "Por favor ingresa o pega un enlace válido para iniciar la extracción.", NotificationType.Warning);
                return;
            }

            if (_downloadService == null)
            {
                ShowNotification("Error Interno", "Módulo no inicializado", "Error al inicializar el servicio de descarga en la memoria del sistema.", NotificationType.Error);
                return;
            }

            if (!_downloadService.IsValidUrl(url))
            {
                ShowNotification("Enlace No Soportado", "Plataforma no reconocida", "La URL introducida no pertenece a YouTube o X (Twitter). Por favor verifica el formato del enlace.", NotificationType.Warning);
                return;
            }

            string selectedFormatContent = ((ComboBoxItem)FormatComboBox.SelectedItem).Content.ToString() ?? "";
            string format = selectedFormatContent.Contains("MP3", StringComparison.OrdinalIgnoreCase) ? "mp3" : "mp4";
            string platform = _downloadService.GetPlatform(url);

            DownloadButton.IsEnabled = false;
            DownloadButtonText.Text = "Procesando y extrayendo...";
            UrlTextBox.IsEnabled = false;
            PasteButton.IsEnabled = false;
            ClearButton.IsEnabled = false;
            FormatComboBox.IsEnabled = false;
            BrowseFolderButton.IsEnabled = false;
            OpenFolderButton.IsEnabled = false;
            ProgressBar.Value = 0;
            ProgressPercentageTextBlock.Text = "0%";
            StatusDot.Fill = new SolidColorBrush(Color.FromRgb(59, 130, 246)); // #3B82F6 azul progreso

            try
            {
                StatusTextBlock.Text = $"Iniciando flujo de procesamiento desde {platform}...";

                bool success = false;

                if (platform == "YouTube")
                {
                    success = await _downloadService.DownloadFromYoutubeAsync(url, format);
                }
                else if (platform == "X")
                {
                    success = await _downloadService.DownloadFromXAsync(url, format);
                }

                if (success)
                {
                    StatusDot.Fill = new SolidColorBrush(Color.FromRgb(16, 185, 129)); // #10B981 verde éxito
                    ProgressBar.Value = 100;
                    ProgressPercentageTextBlock.Text = "100%";
                    StatusTextBlock.Text = "Extracción completada con éxito. Archivo disponible en la carpeta de destino.";

                    ShowNotification(
                        "¡Descarga Completada con Éxito!", 
                        $"Extracción de {platform} ({format.ToUpper()}) finalizada al 100%", 
                        "El archivo multimedia fue procesado y almacenado correctamente en tu equipo con la máxima fidelidad disponible.", 
                        NotificationType.Success, 
                        _currentDownloadPath);
                    UrlTextBox.Clear();
                }
                else
                {
                    StatusDot.Fill = new SolidColorBrush(Color.FromRgb(239, 68, 68)); // #EF4444 rojo error
                    ShowNotification(
                        "Error en la Extracción", 
                        "El proceso no pudo completarse", 
                        "La descarga no se pudo finalizar correctamente. Consulta el monitor de operación o verifica tu conexión a la red.", 
                        NotificationType.Error);
                }
            }
            catch (Exception ex)
            {
                StatusDot.Fill = new SolidColorBrush(Color.FromRgb(239, 68, 68));
                ShowNotification("Excepción en el Proceso", "Fallo durante la operación", ex.Message, NotificationType.Error);
                StatusTextBlock.Text = $"Error: {ex.Message}";
            }
            finally
            {
                DownloadButton.IsEnabled = true;
                DownloadButtonText.Text = "Iniciar Descarga de Medios";
                UrlTextBox.IsEnabled = true;
                PasteButton.IsEnabled = true;
                ClearButton.IsEnabled = true;
                FormatComboBox.IsEnabled = true;
                BrowseFolderButton.IsEnabled = true;
                OpenFolderButton.IsEnabled = true;
            }
        }

        public enum NotificationType
        {
            Success,
            Error,
            Warning,
            Info
        }

        private void ShowNotification(string title, string subtitle, string message, NotificationType type, string? targetPath = null)
        {
            ModalTitleText.Text = title;
            ModalSubtitleText.Text = subtitle;
            ModalMessageText.Text = message;

            if (!string.IsNullOrEmpty(targetPath))
            {
                ModalPathContainer.Visibility = Visibility.Visible;
                ModalPathText.Text = targetPath;
            }
            else
            {
                ModalPathContainer.Visibility = Visibility.Collapsed;
            }

            switch (type)
            {
                case NotificationType.Success:
                    ModalIconGrad1.Color = Color.FromRgb(16, 185, 129); // #10B981
                    ModalIconGrad2.Color = Color.FromRgb(5, 150, 105);  // #059669
                    ModalGlowBorder.Background = new SolidColorBrush(Color.FromRgb(16, 185, 129));
                    ModalGlowEffect.Color = Color.FromRgb(16, 185, 129);
                    ModalSubtitleText.Foreground = new SolidColorBrush(Color.FromRgb(52, 211, 153)); // #34D399
                    ModalIconPath.Data = Geometry.Parse("M 9 16.2 L 4.8 12 L 3.4 13.4 L 9 19 L 21 7 L 19.6 5.6 Z");
                    
                    ModalOpenFolderButton.Visibility = Visibility.Visible;
                    ModalCol0.Width = new GridLength(1, GridUnitType.Star);
                    ModalCol1.Width = new GridLength(12);
                    ModalCol2.Width = new GridLength(1, GridUnitType.Star);
                    Grid.SetColumn(ModalCloseButton, 2);
                    Grid.SetColumnSpan(ModalCloseButton, 1);
                    break;

                case NotificationType.Warning:
                    ModalIconGrad1.Color = Color.FromRgb(245, 158, 11); // #F59E0B
                    ModalIconGrad2.Color = Color.FromRgb(217, 119, 6);  // #D97706
                    ModalGlowBorder.Background = new SolidColorBrush(Color.FromRgb(245, 158, 11));
                    ModalGlowEffect.Color = Color.FromRgb(245, 158, 11);
                    ModalSubtitleText.Foreground = new SolidColorBrush(Color.FromRgb(251, 191, 36)); // #FBBF24
                    ModalIconPath.Data = Geometry.Parse("M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z");
                    
                    ModalOpenFolderButton.Visibility = Visibility.Collapsed;
                    ModalCol0.Width = new GridLength(0);
                    ModalCol1.Width = new GridLength(0);
                    ModalCol2.Width = new GridLength(1, GridUnitType.Star);
                    Grid.SetColumn(ModalCloseButton, 0);
                    Grid.SetColumnSpan(ModalCloseButton, 3);
                    break;

                case NotificationType.Error:
                    ModalIconGrad1.Color = Color.FromRgb(239, 68, 68); // #EF4444
                    ModalIconGrad2.Color = Color.FromRgb(185, 28, 28);  // #B91C1C
                    ModalGlowBorder.Background = new SolidColorBrush(Color.FromRgb(239, 68, 68));
                    ModalGlowEffect.Color = Color.FromRgb(239, 68, 68);
                    ModalSubtitleText.Foreground = new SolidColorBrush(Color.FromRgb(248, 113, 113)); // #F87171
                    ModalIconPath.Data = Geometry.Parse("M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z");
                    
                    ModalOpenFolderButton.Visibility = Visibility.Collapsed;
                    ModalCol0.Width = new GridLength(0);
                    ModalCol1.Width = new GridLength(0);
                    ModalCol2.Width = new GridLength(1, GridUnitType.Star);
                    Grid.SetColumn(ModalCloseButton, 0);
                    Grid.SetColumnSpan(ModalCloseButton, 3);
                    break;

                case NotificationType.Info:
                default:
                    ModalIconGrad1.Color = Color.FromRgb(37, 99, 235);  // #2563EB
                    ModalIconGrad2.Color = Color.FromRgb(29, 78, 216);  // #1D4ED8
                    ModalGlowBorder.Background = new SolidColorBrush(Color.FromRgb(37, 99, 235));
                    ModalGlowEffect.Color = Color.FromRgb(37, 99, 235);
                    ModalSubtitleText.Foreground = new SolidColorBrush(Color.FromRgb(56, 189, 248)); // #38BDF8
                    ModalIconPath.Data = Geometry.Parse("M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z");
                    
                    ModalOpenFolderButton.Visibility = Visibility.Collapsed;
                    ModalCol0.Width = new GridLength(0);
                    ModalCol1.Width = new GridLength(0);
                    ModalCol2.Width = new GridLength(1, GridUnitType.Star);
                    Grid.SetColumn(ModalCloseButton, 0);
                    Grid.SetColumnSpan(ModalCloseButton, 3);
                    break;
            }

            NotificationOverlay.Visibility = Visibility.Visible;
        }

        private void ModalCloseButton_Click(object sender, RoutedEventArgs e)
        {
            NotificationOverlay.Visibility = Visibility.Collapsed;
        }

        private void ModalOpenFolderButton_Click(object sender, RoutedEventArgs e)
        {
            NotificationOverlay.Visibility = Visibility.Collapsed;
            OpenFolderButton_Click(sender, e);
        }
    }
}
