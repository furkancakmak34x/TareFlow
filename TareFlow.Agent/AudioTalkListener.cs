using System.Net;
using System.Net.Sockets;
using NAudio.Wave;

namespace TareFlow.Agent;

/// <summary>
/// Merkezden (TareFlow.Center) TCP ile gelen bas-konuş sesini kantar tarafındaki
/// hoparlörden çalar. Ses: 8 kHz, 16-bit, mono ham PCM.
/// </summary>
public sealed class AudioTalkListener : IDisposable
{
    private static readonly WaveFormat Fmt = new(8000, 16, 1);

    private readonly int _port;
    private TcpListener? _listener;
    private CancellationTokenSource? _cts;

    public event Action<string>? Log;

    public AudioTalkListener(int port) => _port = port;

    public void Start()
    {
        if (_cts is not null)
            return;
        _cts = new CancellationTokenSource();
        _ = Task.Run(() => RunAsync(_cts.Token));
    }

    public void Stop()
    {
        _cts?.Cancel();
        _cts = null;
        try { _listener?.Stop(); } catch { }
    }

    private async Task RunAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            try
            {
                _listener = new TcpListener(IPAddress.Any, _port);
                _listener.Start();
                Log?.Invoke($"Bas-konuş sesi dinleniyor (port {_port}).");

                while (!token.IsCancellationRequested)
                {
                    using var client = await _listener.AcceptTcpClientAsync(token);
                    Log?.Invoke("Merkez sesi bağlandı.");
                    try { await PlayAsync(client, token); }
                    catch (OperationCanceledException) { throw; }
                    catch (Exception ex) { Log?.Invoke("Ses oturumu hata: " + ex.Message); }
                    Log?.Invoke("Merkez sesi kapandı.");
                }
            }
            catch (OperationCanceledException) { break; }
            catch (Exception ex)
            {
                Log?.Invoke("Ses dinleyici hata: " + ex.Message);
            }
            finally
            {
                try { _listener?.Stop(); } catch { }
            }

            try { await Task.Delay(1500, token); } catch { break; }
        }
    }

    private static async Task PlayAsync(TcpClient client, CancellationToken token)
    {
        client.NoDelay = true;
        using var output = new WaveOutEvent { DesiredLatency = 120 };
        var buffer = new BufferedWaveProvider(Fmt)
        {
            DiscardOnBufferOverflow = true,
            BufferDuration = TimeSpan.FromSeconds(3)
        };
        output.Init(buffer);
        output.Play();

        using var stream = client.GetStream();
        var buf = new byte[3200];
        while (!token.IsCancellationRequested)
        {
            int n = await stream.ReadAsync(buf, token);
            if (n <= 0)
                break;
            buffer.AddSamples(buf, 0, n);
        }

        try { output.Stop(); } catch { }
    }

    public void Dispose() => Stop();
}
