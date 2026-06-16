using System.Collections.ObjectModel;
using System.IO;
using System.Printing;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using TareFlow.Center.Services;
using TareFlow.Core;

namespace TareFlow.Center.ViewModels;

/// <summary>Bağlantı, kamera, yazıcı ayarları ve CSV içe aktarma.</summary>
public sealed partial class SettingsViewModel : ObservableObject, IActivatable
{
    private readonly CenterSettings _settings;
    private readonly WeighRepository _repo;
    private readonly ScaleClient _scale;
    private readonly CameraService _camera;

    [ObservableProperty] private string _agentHost;
    [ObservableProperty] private int _agentPort;
    [ObservableProperty] private string _rtspUrl;
    [ObservableProperty] private string _cameraUser;
    [ObservableProperty] private string _cameraPassword;
    [ObservableProperty] private string _printerName;

    public ObservableCollection<string> Printers { get; } = new();

    public SettingsViewModel(CenterSettings settings, WeighRepository repo, ScaleClient scale, CameraService camera)
    {
        _settings = settings;
        _repo = repo;
        _scale = scale;
        _camera = camera;

        _agentHost = settings.AgentHost;
        _agentPort = settings.AgentPort;
        _rtspUrl = settings.RtspUrl;
        _cameraUser = settings.CameraUser;
        _cameraPassword = settings.CameraPassword;
        _printerName = settings.PrinterName;
    }

    public void OnActivated()
    {
        Printers.Clear();
        try
        {
            using var server = new LocalPrintServer();
            foreach (var q in server.GetPrintQueues())
                Printers.Add(q.Name);
        }
        catch { /* yazıcı listesi alınamadı */ }
    }

    [RelayCommand]
    private void Save()
    {
        _settings.AgentHost = AgentHost.Trim();
        _settings.AgentPort = AgentPort;
        _settings.RtspUrl = RtspUrl.Trim();
        _settings.CameraUser = CameraUser.Trim();
        _settings.CameraPassword = CameraPassword;
        _settings.PrinterName = PrinterName ?? "";
        _settings.Save();

        // Bağlantıları yeni ayarlarla yeniden başlat.
        _scale.Stop();
        _scale.Start();
        _camera.Restart();

        MessageBox.Show("Ayarlar kaydedildi ve bağlantılar yenilendi.", "Ayarlar",
            MessageBoxButton.OK, MessageBoxImage.Information);
    }

    [RelayCommand]
    private void ImportCsv()
    {
        var dlg = new OpenFileDialog { Filter = "CSV Dosyaları|*.csv", Title = "CSV Dosyasını Seçin" };
        if (dlg.ShowDialog() != true)
            return;

        int imported = 0, failed = 0;
        foreach (var line in File.ReadAllLines(dlg.FileName))
        {
            try
            {
                var p = line.Split(',');
                var rec = new SecWeightRecord
                {
                    Plate = p[1].Trim(),
                    Date = DateTime.Parse($"{p[2].Trim()} {p[3].Trim()}").ToString("yyyy-MM-dd HH:mm"),
                    SecDate = DateTime.Parse($"{p[4].Trim()} {p[5].Trim()}").ToString("yyyy-MM-dd HH:mm"),
                    Customer = p[6].Trim(),
                    Vendor = p[7].Trim(),
                    Product = p[8].Trim(),
                    Weight = int.TryParse(p[13].Trim(), out var w) ? w : 0,
                    SecWeight = int.TryParse(p[14].Trim(), out var sw) ? sw : 0,
                    Total = int.TryParse(p[15].Trim(), out var t) ? t : 0
                };
                _repo.AddSecWeight(rec);
                imported++;
            }
            catch { failed++; }
        }

        MessageBox.Show($"CSV aktarımı tamamlandı.\nBaşarılı: {imported}, Hatalı: {failed}",
            "CSV Aktarma", MessageBoxButton.OK, MessageBoxImage.Information);
    }
}
