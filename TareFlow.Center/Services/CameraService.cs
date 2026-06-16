using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using LibVLCSharp.Shared;
using MediaPlayer = LibVLCSharp.Shared.MediaPlayer;

namespace TareFlow.Center.Services;

/// <summary>
/// IP kamerayı uygulama açılışında BİR KEZ başlatır ve sürekli açık tutar.
/// Video, airspace (HwndHost) yerine bellek callback'leriyle paylaşılan bir
/// <see cref="WriteableBitmap"/>'e çizilir; tüm görünümler bu aynı kareyi gösterir.
/// Böylece sayfa geçişlerinde durdurma/yeniden başlatma yoktur → çökme ve siyah ekran olmaz.
/// </summary>
public sealed class CameraService : IDisposable
{
    private readonly CenterSettings _settings;
    private LibVLC? _libVlc;
    private MediaPlayer? _player;

    // Native callback referansları GC olmamalı.
    private MediaPlayer.LibVLCVideoFormatCb? _formatCb;
    private MediaPlayer.LibVLCVideoCleanupCb? _cleanupCb;
    private MediaPlayer.LibVLCVideoLockCb? _lockCb;
    private MediaPlayer.LibVLCVideoDisplayCb? _displayCb;

    private IntPtr _buffer;
    private int _width;
    private int _height;
    private int _stride;
    private readonly object _gate = new();
    private bool _disposed;
    private int _activeCamera;

    /// <summary>Tüm görünümlerin Image.Source olarak paylaştığı canlı kare.</summary>
    public WriteableBitmap? Frame { get; private set; }

    /// <summary>Kare (yeniden) oluşturulduğunda tetiklenir (boyut belli olunca).</summary>
    public event Action? FrameReady;

    /// <summary>Aktif kamera değişince tetiklenir (UI'da seçili kamerayı vurgulamak için).</summary>
    public event Action? ActiveCameraChanged;

    /// <summary>Tanımlı kameralar (ayarlardan).</summary>
    public IReadOnlyList<CameraConfig> Cameras => _settings.Cameras;

    /// <summary>Şu an yayını yapılan kameranın listedeki indeksi.</summary>
    public int ActiveCamera => _activeCamera;

    public CameraService(CenterSettings settings) => _settings = settings;

    private static Dispatcher Ui => Application.Current.Dispatcher;

    /// <summary>Açılışta bir kez: LibVLC + MediaPlayer kur, bellek callback'lerini bağla, oynat.</summary>
    public void Start()
    {
        if (_libVlc is not null)
            return;

        LibVLCSharp.Shared.Core.Initialize();
        _libVlc = new LibVLC(
            "--rtsp-tcp",
            "--network-caching=100",
            "--live-caching=100",
            "--clock-jitter=0",
            "--clock-synchro=0",
            "--drop-late-frames",
            "--skip-frames");

        _player = new MediaPlayer(_libVlc);
        _player.Playing += (_, _) => { _player.Mute = false; _player.Volume = 100; };

        _formatCb = OnFormat;
        _cleanupCb = OnCleanup;
        _lockCb = OnLock;
        _displayCb = OnDisplay;
        _player.SetVideoFormatCallbacks(_formatCb, _cleanupCb);
        _player.SetVideoCallbacks(_lockCb, null, _displayCb);

        Play();
    }

    /// <summary>Ayar (örn. RTSP adresi) değişince yeni adresle yeniden başlatır.</summary>
    public void Restart()
    {
        if (_player is null)
        {
            Start();
            return;
        }
        // Aktif indeks artık geçersizse başa sar.
        if (_activeCamera >= _settings.Cameras.Count)
            _activeCamera = 0;
        try { _player.Stop(); } catch { }
        Play();
    }

    /// <summary>Belirtilen kameraya geçiş yapar ve yayını yeniden başlatır.</summary>
    public void SwitchTo(int index)
    {
        if (index < 0 || index >= _settings.Cameras.Count)
            return;
        if (index == _activeCamera && _player is not null)
            return;
        _activeCamera = index;
        if (_player is null)
        {
            Start();
        }
        else
        {
            try { _player.Stop(); } catch { }
            Play();
        }
        ActiveCameraChanged?.Invoke();
    }

    private void Play()
    {
        if (_libVlc is null || _player is null)
            return;
        if (_activeCamera < 0 || _activeCamera >= _settings.Cameras.Count)
            return;
        var media = new Media(_libVlc, new Uri(_settings.Cameras[_activeCamera].BuildRtspUrl()));
        media.AddOption(":network-caching=100");
        media.AddOption(":live-caching=100");
        _player.Play(media);
        media.Dispose();
    }

    // --- libvlc bellek render callback'leri ---

    private uint OnFormat(ref IntPtr opaque, IntPtr chroma, ref uint width, ref uint height,
                          ref uint pitches, ref uint lines)
    {
        WriteChroma(chroma, "RV32");
        int w = (int)width, h = (int)height;
        lock (_gate)
        {
            _width = w; _height = h; _stride = w * 4;
            if (_buffer != IntPtr.Zero) Marshal.FreeHGlobal(_buffer);
            _buffer = Marshal.AllocHGlobal(_stride * h);
        }
        pitches = (uint)(w * 4);
        lines = (uint)h;
        // Bitmap UI thread'de OnDisplay sırasında güncel boyuta göre oluşturulur/onarılır.
        return 1;
    }

    private void OnCleanup(ref IntPtr opaque)
    {
        // Buffer'ı burada serbest bırakma — oynatma sürekli açık; Dispose'da temizlenir.
    }

    private IntPtr OnLock(IntPtr opaque, IntPtr planes)
    {
        Marshal.WriteIntPtr(planes, _buffer);
        return IntPtr.Zero;
    }

    private void OnDisplay(IntPtr opaque, IntPtr picture)
    {
        if (_disposed)
            return;
        Ui.BeginInvoke(DispatcherPriority.Render, () =>
        {
            lock (_gate)
            {
                if (_disposed || _buffer == IntPtr.Zero)
                    return;
                int w = _width, h = _height, stride = _stride;
                if (w <= 0 || h <= 0)
                    return;
                try
                {
                    // Boyut değiştiyse (örn. 1080→1088) bitmap'i yeniden oluştur.
                    if (Frame is null || Frame.PixelWidth != w || Frame.PixelHeight != h)
                    {
                        Frame = new WriteableBitmap(w, h, 96, 96, PixelFormats.Bgr32, null);
                        FrameReady?.Invoke();
                    }
                    Frame.Lock();
                    Frame.WritePixels(new Int32Rect(0, 0, w, h), _buffer, stride * h, stride);
                    Frame.Unlock();
                }
                catch { /* boyut yarışı anlık olabilir; sonraki karede düzelir */ }
            }
        });
    }

    /// <summary>Anlık kareyi JPEG olarak kaydeder. Başarılıysa dosya yolunu döner.</summary>
    public string? TakeSnapshot(string plate)
    {
        var frame = Frame;
        if (frame is null)
            return null;

        return Ui.Invoke(() =>
        {
            try
            {
                Directory.CreateDirectory(_settings.SnapshotFolder);
                string safe = string.Concat((plate ?? "arac").Split(Path.GetInvalidFileNameChars()));
                string path = Path.Combine(_settings.SnapshotFolder, $"{safe}_{DateTime.Now:yyyyMMdd_HHmmss}.jpg");
                var enc = new JpegBitmapEncoder { QualityLevel = 90 };
                enc.Frames.Add(BitmapFrame.Create(frame.Clone()));
                using var fs = File.Create(path);
                enc.Save(fs);
                return path;
            }
            catch { return null; }
        });
    }

    private static void WriteChroma(IntPtr chroma, string fourcc)
    {
        var bytes = Encoding.ASCII.GetBytes(fourcc);
        Marshal.Copy(bytes, 0, chroma, Math.Min(4, bytes.Length));
    }

    public void Dispose()
    {
        if (_disposed)
            return;
        _disposed = true;
        try { _player?.Stop(); } catch { }
        try { _player?.Dispose(); } catch { }
        try { _libVlc?.Dispose(); } catch { }
        lock (_gate)
        {
            if (_buffer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(_buffer);
                _buffer = IntPtr.Zero;
            }
        }
    }
}
