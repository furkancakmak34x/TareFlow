using System.IO;
using System.Text.Json;
using TareFlow.Core;

namespace TareFlow.Center.Services;

/// <summary>Merkez uygulaması ayarları (JSON dosyasında saklanır).</summary>
public sealed class CenterSettings
{
    // Kantar köprüsü (Agent) bağlantısı
    public string AgentHost { get; set; } = "192.168.1.10";
    public int AgentPort { get; set; } = ScaleProtocol.DefaultPort;

    // IP kamera (TTEC IPBP-2330M-M-Lite). ONVIF ile keşfedilen gerçek yollar:
    //   Ana akış (1080p): rtsp://192.168.1.190:3050/1/1?transmode=unicast&profile=va
    //   Alt akış (D1)   : rtsp://192.168.1.190:3050/1/2?transmode=unicast&profile=va
    public string RtspUrl { get; set; } = "rtsp://192.168.1.190:3050/1/1?transmode=unicast&profile=va";
    public string CameraUser { get; set; } = "admin";
    public string CameraPassword { get; set; } = "";

    // 80mm USB termal yazıcı (Windows'a kurulu yazıcı adı)
    public string PrinterName { get; set; } = "";

    // Kantar ücretleri (Fee tablosuna yedek/varsayılan; tahsilat için)
    public int TruckFee { get; set; } = 0;
    public int VanFee { get; set; } = 0;

    public static string AppDataDir => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TareFlow");

    public string SnapshotFolder => Path.Combine(AppDataDir, "snapshots");

    private static string FilePath => Path.Combine(AppDataDir, "center-settings.json");

    /// <summary>Kullanıcı adı/şifre gömülü tam RTSP adresini üretir.</summary>
    public string BuildRtspUrl()
    {
        if (string.IsNullOrWhiteSpace(CameraUser) || RtspUrl.Contains('@'))
            return RtspUrl;
        int idx = RtspUrl.IndexOf("://", StringComparison.Ordinal);
        if (idx < 0)
            return RtspUrl;
        string scheme = RtspUrl[..(idx + 3)];
        string rest = RtspUrl[(idx + 3)..];
        return $"{scheme}{CameraUser}:{CameraPassword}@{rest}";
    }

    public static CenterSettings Load()
    {
        try
        {
            if (File.Exists(FilePath))
            {
                var s = JsonSerializer.Deserialize<CenterSettings>(File.ReadAllText(FilePath));
                if (s is not null)
                    return s;
            }
        }
        catch { /* bozuk → varsayılan */ }
        return new CenterSettings();
    }

    public void Save()
    {
        Directory.CreateDirectory(AppDataDir);
        File.WriteAllText(FilePath, JsonSerializer.Serialize(this,
            new JsonSerializerOptions { WriteIndented = true }));
    }
}
