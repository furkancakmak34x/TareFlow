using System.IO;
using System.Text.Json;
using TareFlow.Core;

namespace TareFlow.Center.Services;

/// <summary>Tek bir IP kameranın tanımı (isim + RTSP adresi + kimlik).</summary>
public sealed class CameraConfig
{
    public string Name { get; set; } = "Kamera";
    public string RtspUrl { get; set; } = "";
    public string User { get; set; } = "";
    public string Password { get; set; } = "";

    /// <summary>Kullanıcı adı/şifre gömülü tam RTSP adresini üretir.</summary>
    public string BuildRtspUrl()
    {
        if (string.IsNullOrWhiteSpace(User) || RtspUrl.Contains('@'))
            return RtspUrl;
        int idx = RtspUrl.IndexOf("://", StringComparison.Ordinal);
        if (idx < 0)
            return RtspUrl;
        string scheme = RtspUrl[..(idx + 3)];
        string rest = RtspUrl[(idx + 3)..];
        return $"{scheme}{User}:{Password}@{rest}";
    }
}

/// <summary>Merkez uygulaması ayarları (JSON dosyasında saklanır).</summary>
public sealed class CenterSettings
{
    // Kantar köprüsü (Agent) bağlantısı
    public string AgentHost { get; set; } = "192.168.1.10";
    public int AgentPort { get; set; } = ScaleProtocol.DefaultPort;

    // --- Kameralar (çoklu) ---
    // Birden fazla IP kamera tanımlanabilir; tartım ekranında aralarında geçiş yapılır.
    public List<CameraConfig> Cameras { get; set; } = new();

    // --- Eski tek-kamera ayarları (geriye dönük uyumluluk / migrasyon) ---
    // Eski sürümden gelen JSON'larda bu alanlar dolu olabilir; Load() bunları
    // Cameras listesine taşır. Yeni sürümde doğrudan kullanılmaz.
    public string RtspUrl { get; set; } = "";
    public string CameraUser { get; set; } = "";
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

    /// <summary>En az bir kamera tanımlı olduğundan emin olur; eski ayardan migrasyon yapar.</summary>
    private void EnsureCameras()
    {
        Cameras ??= new List<CameraConfig>();

        // Eski tek-RTSP ayarı varsa ve liste boşsa, tek kamera olarak taşı.
        if (Cameras.Count == 0 && !string.IsNullOrWhiteSpace(RtspUrl))
        {
            Cameras.Add(new CameraConfig
            {
                Name = "Kamera 1",
                RtspUrl = RtspUrl,
                User = CameraUser,
                Password = CameraPassword
            });
            RtspUrl = CameraUser = CameraPassword = "";
        }

        // Hiç kamera yoksa varsayılan bir tane ekle (TTEC IPBP-2330M-M-Lite ana akış).
        if (Cameras.Count == 0)
        {
            Cameras.Add(new CameraConfig
            {
                Name = "Kamera 1",
                RtspUrl = "rtsp://192.168.1.190:3050/1/1?transmode=unicast&profile=va",
                User = "admin",
                Password = ""
            });
        }
    }

    public static CenterSettings Load()
    {
        CenterSettings result;
        try
        {
            if (File.Exists(FilePath))
            {
                var s = JsonSerializer.Deserialize<CenterSettings>(File.ReadAllText(FilePath));
                result = s ?? new CenterSettings();
            }
            else
            {
                result = new CenterSettings();
            }
        }
        catch { result = new CenterSettings(); }

        result.EnsureCameras();
        return result;
    }

    public void Save()
    {
        Directory.CreateDirectory(AppDataDir);
        File.WriteAllText(FilePath, JsonSerializer.Serialize(this,
            new JsonSerializerOptions { WriteIndented = true }));
    }
}
