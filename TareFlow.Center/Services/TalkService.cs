using System.Net.Sockets;
using NAudio.Wave;

namespace TareFlow.Center.Services;

/// <summary>
/// Bas-konuş: merkezdeki mikrofonu yakalayıp TCP ile kantar tarafına (Agent) gönderir.
/// Konuşurken eko olmaması için gelen kamera sesi kısılır; bırakınca 1 sn sonra geri açılır.
/// Ses: 8 kHz, 16-bit, mono ham PCM.
/// </summary>
public sealed class TalkService : IDisposable
{
    private static readonly WaveFormat Fmt = new(8000, 16, 1);

    private readonly CenterSettings _settings;
    private readonly CameraService _camera;

    private WaveInEvent? _waveIn;
    private TcpClient? _client;
    private NetworkStream? _stream;
    private CancellationTokenSource? _unmuteCts;
    private volatile bool _talking;

    public bool IsTalking => _talking;

    public TalkService(CenterSettings settings, CameraService camera)
    {
        _settings = settings;
        _camera = camera;
    }

    /// <summary>Butona basılınca: mikrofonu aç, sesi Agent'a göndermeye başla, kamera sesini kıs.</summary>
    public void Start()
    {
        if (_talking)
            return;
        _talking = true;

        // Eko önleme: bekleyen "sesi geri aç" görevini iptal et ve kamera sesini hemen kıs.
        _unmuteCts?.Cancel();
        _camera.SetMuted(true);

        // Bağlanmayı arka planda yap — Agent kapalıysa UI donmasın.
        Task.Run(() =>
        {
            try
            {
                var client = new TcpClient { NoDelay = true };
                client.Connect(_settings.AgentHost, _settings.AudioPort);
                if (!_talking) { try { client.Dispose(); } catch { } return; }  // bu arada bırakıldı

                _client = client;
                _stream = client.GetStream();

                var waveIn = new WaveInEvent { WaveFormat = Fmt, BufferMilliseconds = 50 };
                waveIn.DataAvailable += OnData;
                _waveIn = waveIn;
                waveIn.StartRecording();

                if (!_talking)   // bağlanırken bırakıldıysa mikrofonu açık bırakma
                    StopInternal(restoreImmediately: true);
            }
            catch
            {
                StopInternal(restoreImmediately: true);
            }
        });
    }

    private void OnData(object? sender, WaveInEventArgs e)
    {
        try { _stream?.Write(e.Buffer, 0, e.BytesRecorded); }
        catch { /* bağlantı koptu; bırakınca temizlenir */ }
    }

    /// <summary>Buton bırakılınca: mikrofonu kapat; 1 sn sonra kamera sesini geri aç.</summary>
    public void Stop() => StopInternal(restoreImmediately: false);

    private void StopInternal(bool restoreImmediately)
    {
        if (!_talking && _waveIn is null)
        {
            if (restoreImmediately) _camera.SetMuted(false);
            return;
        }
        _talking = false;

        try { if (_waveIn is not null) { _waveIn.DataAvailable -= OnData; _waveIn.StopRecording(); _waveIn.Dispose(); } } catch { }
        _waveIn = null;
        try { _stream?.Dispose(); } catch { }
        try { _client?.Dispose(); } catch { }
        _stream = null;
        _client = null;

        if (restoreImmediately)
        {
            _camera.SetMuted(false);
            return;
        }

        // 1 sn gecikmeyle kamera sesini geri aç (eko kuyruğunu önlemek için).
        _unmuteCts = new CancellationTokenSource();
        var token = _unmuteCts.Token;
        _ = Task.Run(async () =>
        {
            try { await Task.Delay(1000, token); _camera.SetMuted(false); }
            catch { /* yeni konuşma başladı; iptal */ }
        });
    }

    public void Dispose() => StopInternal(restoreImmediately: true);
}
