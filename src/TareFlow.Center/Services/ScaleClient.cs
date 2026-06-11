using System.IO;
using System.Net.Sockets;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using TareFlow.Core;

namespace TareFlow.Center.Services;

/// <summary>
/// Kantar Agent'ına kalıcı TCP bağlantısı kurar, satır-bazlı JSON ağırlık akışını dinler
/// ve otomatik yeniden bağlanır. UI thread'ine marshalling yapar.
/// </summary>
public sealed partial class ScaleClient : ObservableObject, IDisposable
{
    private readonly CenterSettings _settings;
    private CancellationTokenSource? _cts;

    [ObservableProperty]
    private bool _isConnected;

    [ObservableProperty]
    private int _currentWeight;

    [ObservableProperty]
    private bool _isStable;

    public ScaleClient(CenterSettings settings) => _settings = settings;

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
        SetConnected(false);
    }

    private async Task RunAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            try
            {
                using var client = new TcpClient { NoDelay = true };
                await client.ConnectAsync(_settings.AgentHost, _settings.AgentPort, token);
                SetConnected(true);

                using var reader = new StreamReader(client.GetStream());
                while (!token.IsCancellationRequested)
                {
                    var line = await reader.ReadLineAsync(token);
                    if (line is null)
                        break; // bağlantı kapandı
                    var reading = ScaleProtocol.Deserialize(line);
                    if (reading is not null)
                        Apply(reading);
                }
            }
            catch (OperationCanceledException) { break; }
            catch { /* bağlantı koptu → yeniden dene */ }

            SetConnected(false);
            try { await Task.Delay(2000, token); } catch { break; }
        }
    }

    private void Apply(ScaleReading r) => OnUi(() =>
    {
        CurrentWeight = r.Weight;
        IsStable = r.Stable;
    });

    private void SetConnected(bool value) => OnUi(() => IsConnected = value);

    private static void OnUi(Action action)
    {
        var app = Application.Current;
        if (app is null)
            action();
        else
            app.Dispatcher.Invoke(action);
    }

    public void Dispose() => Stop();
}
