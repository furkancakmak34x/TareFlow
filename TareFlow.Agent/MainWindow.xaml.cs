using System.IO.Ports;
using System.Windows;
using TareFlow.Core;

namespace TareFlow.Agent;

public partial class MainWindow : Window
{
    private readonly ScaleBridge _bridge = new();
    private readonly AgentSettings _settings;
    private AudioTalkListener? _audio;

    public MainWindow()
    {
        InitializeComponent();
        _settings = AgentSettings.Load();

        // Bas-konuş sesini dinle (merkezden gelen sesi kantar hoparlöründen çal).
        _audio = new AudioTalkListener(_settings.AudioPort);
        _audio.Log += OnLog;
        _audio.Start();

        foreach (var p in SerialPort.GetPortNames())
            cmbPort.Items.Add(p);
        cmbPort.Text = _settings.PortName;
        cmbBaud.Text = _settings.BaudRate.ToString();
        txtListenPort.Text = _settings.ListenPort.ToString();

        _bridge.Log += OnLog;
        _bridge.ReadingReceived += OnReading;

        Closing += (_, _) => { _bridge.Stop(); _audio?.Stop(); };

        if (_settings.AutoStart)
            Start(silent: true);
    }

    private void btnStart_Click(object sender, RoutedEventArgs e) => Start();

    private void btnStop_Click(object sender, RoutedEventArgs e)
    {
        _bridge.Stop();
        btnStart.IsEnabled = true;
        btnStop.IsEnabled = false;
    }

    private void Start(bool silent = false)
    {
        try
        {
            _settings.PortName = cmbPort.Text.Trim();
            _settings.BaudRate = int.TryParse(cmbBaud.Text, out var b) ? b : 9600;
            _settings.ListenPort = int.TryParse(txtListenPort.Text, out var lp) ? lp : ScaleProtocol.DefaultPort;
            _settings.Save();

            _bridge.Start(_settings);
            btnStart.IsEnabled = false;
            btnStop.IsEnabled = true;
        }
        catch (Exception ex)
        {
            OnLog("HATA: " + ex.Message);
            if (!silent)
                MessageBox.Show(ex.Message, "Başlatma Hatası", MessageBoxButton.OK, MessageBoxImage.Error);
            btnStart.IsEnabled = true;
            btnStop.IsEnabled = false;
        }
    }

    private void OnLog(string message) => Dispatcher.Invoke(() =>
    {
        txtLog.Text += $"[{DateTime.Now:HH:mm:ss}] {message}\n";
        logScroll.ScrollToEnd();
    });

    private void OnReading(ScaleReading r) => Dispatcher.Invoke(() =>
    {
        lblWeight.Text = $"{r.Weight} kg";
        lblClients.Text = _bridge.ClientCount.ToString();
    });
}
