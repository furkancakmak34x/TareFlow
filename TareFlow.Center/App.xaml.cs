using System.Windows;
using TareFlow.Center.Services;
using TareFlow.Center.ViewModels;
using TareFlow.Center.Views;

namespace TareFlow.Center;

public partial class App : Application
{
    /// <summary>Kamera akışını görünümlerin paylaşması için basit servis erişimi.</summary>
    public static CameraService Camera { get; private set; } = null!;

    /// <summary>PTZ (ONVIF) kontrol servisi — görünümler doğrudan kullanır.</summary>
    public static PtzService Ptz { get; private set; } = null!;

    /// <summary>Bas-konuş servisi (mikrofon → Agent).</summary>
    public static TalkService Talk { get; private set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Composition root: servisler tek örnek (singleton) olarak kurulur.
        var settings = CenterSettings.Load();
        var repo = new WeighRepository();
        var scale = new ScaleClient(settings);
        var camera = new CameraService(settings);
        var printer = new ReceiptPrinter(settings);
        Camera = camera;
        Ptz = new PtzService();
        Talk = new TalkService(settings, camera);

        var weigh = new WeighViewModel(repo, scale, camera);
        var second = new SecondWeighViewModel(repo, scale, camera, printer);
        var manual = new ManualWeighViewModel(repo, scale, camera, printer);
        var tare = new TareWeighViewModel(repo, scale, camera, printer);
        var station = new WeighStationViewModel(weigh, second, manual, tare, scale, camera);
        var records = new RecordsViewModel(repo, printer);
        var receivable = new ReceivableViewModel(repo);
        var settingsVm = new SettingsViewModel(settings, repo, scale, camera, Ptz);

        var shell = new ShellViewModel(scale, station, records, receivable, settingsVm);

        scale.Start();

        var window = new ShellWindow { DataContext = shell };
        window.Closed += (_, _) =>
        {
            scale.Dispose();
            camera.Dispose();
            Talk.Dispose();
        };
        window.Show();

        // Kamerayı pencere gösterildikten ve dispatcher pompalamaya başladıktan sonra
        // bir kez başlat ve sürekli açık tut (bellek render). UI donmaz.
        Dispatcher.BeginInvoke(new Action(() =>
        {
            try { camera.Start(); } catch { /* kamera yoksa uygulama yine açılır */ }
        }), System.Windows.Threading.DispatcherPriority.Background);
    }
}
