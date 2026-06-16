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
    [ObservableProperty] private string _printerName;

    [ObservableProperty] private CameraConfig? _selectedCamera;

    public ObservableCollection<string> Printers { get; } = new();

    /// <summary>Düzenlenebilir kamera listesi.</summary>
    public ObservableCollection<CameraConfig> Cameras { get; } = new();

    public SettingsViewModel(CenterSettings settings, WeighRepository repo, ScaleClient scale, CameraService camera)
    {
        _settings = settings;
        _repo = repo;
        _scale = scale;
        _camera = camera;

        _agentHost = settings.AgentHost;
        _agentPort = settings.AgentPort;
        _printerName = settings.PrinterName;

        foreach (var c in settings.Cameras)
            Cameras.Add(c);
        SelectedCamera = Cameras.FirstOrDefault();
    }

    [RelayCommand]
    private void AddCamera()
    {
        var cam = new CameraConfig
        {
            Name = $"Kamera {Cameras.Count + 1}",
            RtspUrl = "rtsp://192.168.1.190:3050/1/1?transmode=unicast&profile=va",
            User = "admin",
            Password = ""
        };
        Cameras.Add(cam);
        SelectedCamera = cam;
    }

    [RelayCommand]
    private void RemoveCamera()
    {
        if (SelectedCamera is null)
            return;
        if (Cameras.Count <= 1)
        {
            MessageBox.Show("En az bir kamera tanımlı kalmalı.", "Kamera",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        Cameras.Remove(SelectedCamera);
        SelectedCamera = Cameras.FirstOrDefault();
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
        _settings.PrinterName = PrinterName ?? "";
        _settings.Cameras = Cameras.ToList();
        // Eski tek-kamera alanları artık kullanılmıyor; temizle.
        _settings.RtspUrl = _settings.CameraUser = _settings.CameraPassword = "";
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
