using System.IO;
using LibVLCSharp.Shared;
using MediaPlayer = LibVLCSharp.Shared.MediaPlayer;

namespace TareFlow.Center.Services;

public sealed class CameraService : IDisposable
{
    private readonly CenterSettings _settings;
    private readonly object _ctrlLock = new();
    private LibVLC? _libVlc;
    private MediaPlayer? _player;
    private bool _disposed;
    private int _activeCamera;
    private string? _logPath;



    public CameraService(CenterSettings settings) => _settings = settings;
    public MediaPlayer? Player => _player;
    public IReadOnlyList<CameraConfig> Cameras => _settings.Cameras;
    public int ActiveCamera => _activeCamera;
    public event Action? ActiveCameraChanged;
    public string Status { get; private set; } = "Kamera başlatılıyor…";
    public event Action<string>? StatusChanged;

    private void SetStatus(string s)
    {
        Status = s;
        StatusChanged?.Invoke(s);
    }
    public void Start()
    {
        if (_libVlc is not null)
            return;

        try
        {
            string baseDir = AppContext.BaseDirectory;
            string arch = System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture switch
            {
                System.Runtime.InteropServices.Architecture.X86 => "win-x86",
                System.Runtime.InteropServices.Architecture.Arm64 => "win-arm64",
                _ => "win-x64"
            };

            string[] candidates =
            {
                Path.Combine(baseDir, "libvlc", arch),
                Path.Combine(baseDir, "runtimes", arch, "native"),
            };
            string? libDir = candidates.FirstOrDefault(d => File.Exists(Path.Combine(d, "libvlc.dll")));
            if (libDir is null)
            {
                SetStatus($"Bu mimari ({arch}) için libvlc.dll bulunamadı: " + string.Join("  |  ", candidates));
                return;
            }

            string pluginsDir = Path.Combine(libDir, "plugins");
            if (Directory.Exists(pluginsDir))
                Environment.SetEnvironmentVariable("VLC_PLUGIN_PATH", pluginsDir);
            LibVLCSharp.Shared.Core.Initialize(libDir);
            _libVlc = new LibVLC("--network-caching=400", "--live-caching=50");
        }
        catch (Exception ex)
        {
            SetStatus("LibVLC başlatılamadı: " + ex.Message);
            return;
        }

        try
        {
            string logPath = Path.Combine(CenterSettings.AppDataDir, "vlc.log");
            _logPath = logPath;
            try { File.WriteAllText(logPath, $"--- VLC Log Started {DateTime.Now} ---\n"); } catch { }

            _libVlc.Log += (_, e) =>
            {
                try
                {
                    lock (logPath)
                    {
                        File.AppendAllText(logPath, $"[{e.Level}] [{e.Module}] {e.Message}\n");
                    }
                }
                catch { }
            };
        }
        catch { }

        _player = new MediaPlayer(_libVlc);
        _player.Playing += (_, _) => { Task.Run(() => { try { _player.Mute = false; _player.Volume = 100; } catch { } }); SetStatus(""); };
        _player.Opening += (_, _) => SetStatus("Bağlanılıyor…");
        _player.EncounteredError += (_, _) => SetStatus("Yayın hatası: RTSP açılamadı (adres/şifre/port?)");
        _player.EndReached += (_, _) => SetStatus("Yayın sonlandı.");
    }

    public void EnsurePlaying()
    {
        if (_libVlc is null || _player is null)
        {
            Start();
        }
        if (_player is null)
            return;
        var st = _player.State;
        if (st is VLCState.NothingSpecial or VLCState.Stopped or VLCState.Ended or VLCState.Error)
            EnqueueControl(PlayInternal);
    }

    private void EnqueueControl(Action action)
    {
        Task.Run(() =>
        {
            lock (_ctrlLock)
            {
                if (_disposed)
                    return;
                try { action(); } catch { }
            }
        });
    }

    private void PlayInternal()
    {
        if (_libVlc is null || _player is null)
            return;

        if (_activeCamera < 0 || _activeCamera >= _settings.Cameras.Count)
            return;

        var cam = _settings.Cameras[_activeCamera];
        string mrl = cam.BuildRtspUrl();

        SetStatus("Bağlanılıyor: " + mrl);

        using var media = new Media(_libVlc, mrl, FromType.FromLocation);

        media.AddOption(":network-caching=400");
        media.AddOption(":live-caching=50");
        media.AddOption(":file-caching=0");
        media.AddOption(":drop-late-frames");
        media.AddOption(":skip-frames");
        media.AddOption(":rtsp-tcp");
        media.AddOption(":avcodec-hw=none");

        _player.Play(media);
    }

    private void StopInternal()
    {
        try { _player?.Stop(); } catch { }
    }

    public void Stop() => EnqueueControl(StopInternal);

    public void SetMuted(bool muted)
        => EnqueueControl(() => { try { if (_player is not null) _player.Mute = muted; } catch { } });

    public void SetFillAspect(int width, int height)
    {
        if (width <= 0 || height <= 0)
            return;
        EnqueueControl(() => { try { if (_player is not null) _player.AspectRatio = $"{width}:{height}"; } catch { } });
    }

    public void Restart()
    {
        if (_player is null)
        {
            Start();
            return;
        }
        if (_activeCamera >= _settings.Cameras.Count)
            _activeCamera = 0;
        EnqueueControl(() => { StopInternal(); PlayInternal(); });
    }

    public void SwitchTo(int index)
    {
        if (index < 0 || index >= _settings.Cameras.Count)
            return;
        if (index == _activeCamera && _player is not null)
            return;
        _activeCamera = index;
        if (_player is null)
            Start();
        else
            EnqueueControl(() => { StopInternal(); PlayInternal(); });
        ActiveCameraChanged?.Invoke();
    }

    public string? TakeSnapshot(string plate)
    {
        if (_player is null)
            return null;
        try
        {
            Directory.CreateDirectory(_settings.SnapshotFolder);
            string safe = string.Concat((plate ?? "arac").Split(Path.GetInvalidFileNameChars()));
            string path = Path.Combine(_settings.SnapshotFolder, $"{safe}_{DateTime.Now:yyyyMMdd_HHmmss}.jpg");
            return _player.TakeSnapshot(0, path, 0, 0) ? path : null;
        }
        catch { return null; }
    }

    public void Dispose()
    {
        if (_disposed)
            return;
        _disposed = true;
        lock (_ctrlLock)
        {
            try { _player?.Stop(); } catch { }
            try { _player?.Dispose(); } catch { }
            try { _libVlc?.Dispose(); } catch { }
        }
    }
}
