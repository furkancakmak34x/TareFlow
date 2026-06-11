using System.Collections.Concurrent;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TareFlow.Core;

namespace TareFlow.Agent;

/// <summary>
/// COM portundaki indikatörü okuyup, bağlanan tüm Merkez istemcilerine
/// TCP üzerinden satır-bazlı JSON ağırlık yayını yapar.
/// </summary>
public sealed class ScaleBridge : IDisposable
{
    private readonly ConcurrentDictionary<TcpClient, byte> _clients = new();
    private SerialPort? _port;
    private TcpListener? _listener;
    private CancellationTokenSource? _cts;

    public bool IsRunning { get; private set; }
    public int ClientCount => _clients.Count;

    /// <summary>Yeni ağırlık okunduğunda tetiklenir (UI göstergesi için).</summary>
    public event Action<ScaleReading>? ReadingReceived;
    /// <summary>Bilgi/hata mesajı (UI log için).</summary>
    public event Action<string>? Log;

    public void Start(AgentSettings settings)
    {
        if (IsRunning)
            return;

        _cts = new CancellationTokenSource();

        _port = new SerialPort(settings.PortName, settings.BaudRate)
        {
            DiscardNull = true,
            ReadTimeout = 500
        };
        _port.Open();
        Log?.Invoke($"Seri port açıldı: {settings.PortName} @ {settings.BaudRate}");

        _listener = new TcpListener(IPAddress.Any, settings.ListenPort);
        _listener.Start();
        Log?.Invoke($"TCP dinleme başladı: 0.0.0.0:{settings.ListenPort}");

        IsRunning = true;
        _ = Task.Run(() => AcceptLoopAsync(_cts.Token));
        _ = Task.Run(() => SerialLoopAsync(_cts.Token));
    }

    public void Stop()
    {
        if (!IsRunning)
            return;
        IsRunning = false;
        _cts?.Cancel();

        try { _listener?.Stop(); } catch { }
        foreach (var c in _clients.Keys)
        {
            try { c.Close(); } catch { }
        }
        _clients.Clear();

        try { if (_port?.IsOpen == true) _port.Close(); } catch { }
        _port?.Dispose();
        _port = null;
        Log?.Invoke("Köprü durduruldu.");
    }

    private async Task AcceptLoopAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested && _listener is not null)
        {
            try
            {
                var client = await _listener.AcceptTcpClientAsync(token);
                client.NoDelay = true;
                _clients.TryAdd(client, 0);
                Log?.Invoke($"Merkez bağlandı: {client.Client.RemoteEndPoint} (toplam {_clients.Count})");
            }
            catch (OperationCanceledException) { break; }
            catch (Exception ex) when (!token.IsCancellationRequested)
            {
                Log?.Invoke("Bağlantı kabul hatası: " + ex.Message);
            }
        }
    }

    private async Task SerialLoopAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested && _port is not null)
        {
            try
            {
                _port.DiscardInBuffer();
                await Task.Delay(120, token);

                int bytesToRead = _port.BytesToRead;
                if (bytesToRead <= 0)
                    continue;

                var buffer = new byte[bytesToRead];
                int read = _port.Read(buffer, 0, bytesToRead);
                string data = Encoding.ASCII.GetString(buffer, 0, read);

                if (!ScaleParser.TryParse(data, out int weight))
                    continue;

                var reading = new ScaleReading { Weight = weight, Stable = true };
                ReadingReceived?.Invoke(reading);
                Broadcast(ScaleProtocol.Serialize(reading));
            }
            catch (OperationCanceledException) { break; }
            catch (Exception ex) when (!token.IsCancellationRequested)
            {
                Log?.Invoke("Seri okuma hatası: " + ex.Message);
                await Task.Delay(500, token);
            }
        }
    }

    private void Broadcast(string line)
    {
        var bytes = Encoding.ASCII.GetBytes(line);
        foreach (var client in _clients.Keys)
        {
            try
            {
                if (!client.Connected)
                {
                    RemoveClient(client);
                    continue;
                }
                client.GetStream().Write(bytes, 0, bytes.Length);
            }
            catch
            {
                RemoveClient(client);
            }
        }
    }

    private void RemoveClient(TcpClient client)
    {
        if (_clients.TryRemove(client, out _))
        {
            try { client.Close(); } catch { }
            Log?.Invoke($"Merkez ayrıldı (kalan {_clients.Count}).");
        }
    }

    public void Dispose() => Stop();
}
