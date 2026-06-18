using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;
using TareFlow.Core;

namespace TareFlow.Center.Services;

/// <summary>Fiş yazıcısı türü.</summary>
public enum PrinterKind
{
    /// <summary>80mm USB termal yazıcı (ESC/POS, otomatik kesme).</summary>
    Thermal80,
    /// <summary>Nokta vuruşlu yazıcı (ESC/P) — eskiden LPT, şimdi USB'ye bağlı.</summary>
    DotMatrix
}

/// <summary>Tek bir IP kameranın tanımı (isim + RTSP adresi + kimlik).</summary>
public sealed partial class CameraConfig : ObservableObject
{
    [ObservableProperty] private string _name = "Kamera";
    [ObservableProperty] private string _rtspUrl = "";
    [ObservableProperty] private string _user = "";
    [ObservableProperty] private string _password = "";

    // PTZ (ONVIF) — yön/zoom kontrolü olan kameralar için
    [ObservableProperty] private bool _hasPtz;
    [ObservableProperty] private int _onvifPort = 80;
    /// <summary>Elle belirtilen ONVIF profil token'ı (boşsa otomatik keşfedilir).</summary>
    [ObservableProperty] private string _ptzProfileToken = "";

    /// <summary>RTSP'yi TCP ile zorla. Kapalıysa UDP kullanılır (varsayılan).
    /// Bazı kameralar TCP'de takılır, UDP ister; bazıları tersi. Kamera açılmıyorsa bunu değiştir.</summary>
    [ObservableProperty] private bool _useTcp;

    /// <summary>
    /// Yerel SDK protokolü (XiongMai/Sofia, TCP 34567). RTSP yerine kameranın kendi
    /// yerel akış protokolünü kullanır — ucuz XM kameralarda RTSP titrek olsa da bu yol stabildir
    /// (üreticinin oynatıcısının akıcı olmasının sebebi). Açıkken RTSP adresi yok sayılır;
    /// IP, SdkPort, kullanıcı adı/şifre kullanılır.
    /// </summary>
    [ObservableProperty] private bool _useSdk;

    [JsonIgnore]
    public bool IsSdkSettingsEnabled => UseSdk;

    partial void OnUseSdkChanged(bool value) => OnPropertyChanged(nameof(IsSdkSettingsEnabled));

    /// <summary>Kamera IP adresi (yerel SDK protokolü için).</summary>
    [ObservableProperty] private string _host = "";

    /// <summary>Yerel SDK (Sofia/DVRIP) TCP portu — XM kameralarda varsayılan 34567.</summary>
    [ObservableProperty] private int _sdkPort = 34567;

    /// <summary>SDK kanal numarası (0 tabanlı).</summary>
    [ObservableProperty] private int _sdkChannel;

    /// <summary>SDK alt akış kullan (ExtraStream1) — true ise daha düşük çözünürlük/daha akıcı.</summary>
    [ObservableProperty] private bool _sdkSubStream;

    /// <summary>Kullanıcı adı/şifre gömülü tam RTSP adresini üretir.</summary>
    [JsonIgnore]
    public string RtspUrlBuilt => BuildRtspUrl();

    /// <summary>Kullanıcı adı/şifre gömülü tam RTSP adresini üretir.</summary>
    public string BuildRtspUrl()
    {
        string url = RtspUrl ?? "";
        if (string.IsNullOrWhiteSpace(User) || url.Contains('@'))
            return url;

        // Kimlik bilgisi adresin yolunda gömülüyse (ör. ONVIF'ten gelen
        // "rtsp://ip:554/user=admin_password=xxx_channel=0..." gibi Çin tipi kameralar)
        // tekrar user:pass@ enjekte etme — yoksa adres bozulur.
        string lower = url.ToLowerInvariant();
        if (lower.Contains("user=") || lower.Contains("username=") ||
            lower.Contains("password=") || lower.Contains("pwd=") || lower.Contains("usr="))
            return url;

        int idx = url.IndexOf("://", StringComparison.Ordinal);
        if (idx < 0)
            return url;
        string scheme = url[..(idx + 3)];
        string rest = url[(idx + 3)..];
        return $"{scheme}{User}:{Password}@{rest}";
    }
}

/// <summary>Merkez uygulaması ayarları (JSON dosyasında saklanır).</summary>
public sealed class CenterSettings
{
    // Kantar köprüsü (Agent) bağlantısı
    public string AgentHost { get; set; } = "192.168.1.10";
    public int AgentPort { get; set; } = ScaleProtocol.DefaultPort;
    /// <summary>Bas-konuş sesinin Agent'a gönderileceği TCP portu.</summary>
    public int AudioPort { get; set; } = 9101;

    // --- Kameralar (çoklu) ---
    // Birden fazla IP kamera tanımlanabilir; tartım ekranında aralarında geçiş yapılır.
    public List<CameraConfig> Cameras { get; set; } = new();

    // --- Eski tek-kamera ayarları (geriye dönük uyumluluk / migrasyon) ---
    // Eski sürümden gelen JSON'larda bu alanlar dolu olabilir; Load() bunları
    // Cameras listesine taşır. Yeni sürümde doğrudan kullanılmaz.
    public string RtspUrl { get; set; } = "";
    public string CameraUser { get; set; } = "";
    public string CameraPassword { get; set; } = "";

    // Fiş yazıcısı (Windows'a kurulu yazıcı adı — USB termal veya USB'ye bağlı nokta vuruşlu)
    public string PrinterName { get; set; } = "";

    // Yazıcı türü: 80mm termal (ESC/POS) veya nokta vuruşlu (ESC/P, LPT mantığı ama USB'den).
    public PrinterKind PrinterKind { get; set; } = PrinterKind.Thermal80;

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
