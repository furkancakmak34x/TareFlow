using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TareFlow.Center.Services;

namespace TareFlow.Center.Views;

public partial class WeighStationView : UserControl
{
    private bool _isTalking;

    public WeighStationView()
    {
        InitializeComponent();
        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (App.Camera is { } cam)
        {
            if (cam.Player is null)
                cam.Start();                      // oynatıcı henüz hazır değilse kur
            VideoView.MediaPlayer = cam.Player;   // önce VideoView'a bağla
            cam.EnsurePlaying();                   // sonra oynat (vout VideoView'a gider)
            cam.StatusChanged -= OnStatus;
            cam.StatusChanged += OnStatus;
            cam.ActiveCameraChanged -= OnActiveCameraChanged;
            cam.ActiveCameraChanged += OnActiveCameraChanged;
            ShowStatus(cam.Status);
            ApplyFill();
        }
    }

    // Görüntüyü alana tam doldur: en-boy oranını VideoView'ın piksel boyutuna eşitle (siyah kenar yok).
    private void ApplyFill()
        => App.Camera?.SetFillAspect((int)VideoView.ActualWidth, (int)VideoView.ActualHeight);

    private void VideoView_SizeChanged(object sender, SizeChangedEventArgs e) => ApplyFill();

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        if (_isTalking)
        {
            _isTalking = false;
            App.Talk?.Stop();
            App.Camera?.SetMuted(false);
        }
        if (App.Camera is { } cam)
        {
            cam.StatusChanged -= OnStatus;
            cam.ActiveCameraChanged -= OnActiveCameraChanged;
            VideoView.MediaPlayer = null;   // VideoView'dan ayır
            cam.Stop();                     // görünüm yokken yayını durdur (popup/dead-hwnd önlenir)
        }
    }

    // Kamera değişince vout'u temiz tutmak için VideoView'ı yeniden bağla (null → player).
    private void OnActiveCameraChanged() => Dispatcher.BeginInvoke(() =>
    {
        if (App.Camera is { } cam)
        {
            VideoView.MediaPlayer = null;
            VideoView.MediaPlayer = cam.Player;
            ApplyFill();
        }
    });

    private void OnStatus(string s) => Dispatcher.BeginInvoke(() =>
    {
        ShowStatus(s);
        if (string.IsNullOrWhiteSpace(s))   // "" → oynatılıyor: vout hazır, oranı şimdi uygula
            ApplyFill();
    });

    private void ShowStatus(string s)
    {
        CamStatus.Text = string.IsNullOrWhiteSpace(s) ? "" : s;
        CamStatus.Visibility = string.IsNullOrWhiteSpace(s) ? Visibility.Collapsed : Visibility.Visible;
    }

    private bool _fullscreen;

    private void BtnFullscreen_Click(object sender, RoutedEventArgs e)
    {
        // YERİNDE tam ekran: sağ paneli gizle, kamerayı tüm pencereye yay.
        // (Ayrı pencere + oynatıcı devri yapmıyoruz → siyah/beyaz takılma olmaz.)
        _fullscreen = !_fullscreen;
        if (_fullscreen)
        {
            Tabs.Visibility = Visibility.Collapsed;
            GapCol.Width = new GridLength(0);
            TabsCol.MinWidth = 0;
            TabsCol.Width = new GridLength(0);
        }
        else
        {
            Tabs.Visibility = Visibility.Visible;
            GapCol.Width = new GridLength(16);
            TabsCol.MinWidth = 320;
            TabsCol.Width = new GridLength(3, GridUnitType.Star);
        }
        if (sender is System.Windows.Controls.Button b)
            b.Content = _fullscreen ? "Çık" : "Tam Ekran";
    }

    // ---- Bas-Konuş (tıkla aç / tıkla kapa) ----

    private async void BtnTalk_Click(object sender, RoutedEventArgs e)
    {
        if (!_isTalking)
        {
            _isTalking = true;
            BtnTalk.Content = "Konuşuyor…";
            App.Talk?.Start();
            App.Camera?.SetMuted(true);
        }
        else
        {
            _isTalking = false;
            BtnTalk.Content = "Bas-Konuş";
            App.Talk?.Stop();
            // 1 saniye sonra kameranın sesini geri aç
            await System.Threading.Tasks.Task.Delay(1000);
            if (!_isTalking) // 1 saniye içinde tekrar basılmamışsa aç
            {
                App.Camera?.SetMuted(false);
            }
        }
    }

    // ---- PTZ (basılı tut → hareket, bırak → dur) ----

    private static CameraConfig? ActiveCam()
        => App.Camera?.Cameras.ElementAtOrDefault(App.Camera.ActiveCamera);

    private void PtzDown(object sender, MouseButtonEventArgs e)
    {
        if (App.Ptz is null || ActiveCam() is not { } cam)
            return;
        var (p, t, z) = Velocity((string)((FrameworkElement)sender).Tag);
        _ = App.Ptz.MoveAsync(cam, p, t, z);
    }

    private void PtzUp(object sender, MouseButtonEventArgs e) => StopPtz();

    private void PtzLeave(object sender, MouseEventArgs e)
    {
        // Yalnızca buton basılıyken ayrılınca durdur.
        if (e.LeftButton == MouseButtonState.Pressed)
            StopPtz();
    }

    private void StopPtz()
    {
        if (App.Ptz is null || ActiveCam() is not { } cam)
            return;
        _ = App.Ptz.StopAsync(cam);
    }

    private static (double pan, double tilt, double zoom) Velocity(string dir)
    {
        const double s = 0.6;
        return dir switch
        {
            "U" => (0, s, 0),
            "D" => (0, -s, 0),
            "L" => (-s, 0, 0),
            "R" => (s, 0, 0),
            "UL" => (-s, s, 0),
            "UR" => (s, s, 0),
            "DL" => (-s, -s, 0),
            "DR" => (s, -s, 0),
            "ZI" => (0, 0, s),
            "ZO" => (0, 0, -s),
            _ => (0, 0, 0)
        };
    }
}
