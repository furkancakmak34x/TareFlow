using System.Windows;
using TareFlow.Center.Services;
using TareFlow.Center.ViewModels;
using TareFlow.Center.Views;

namespace TareFlow.Center;

public partial class App : Application
{
    /// <summary>Kamera akışını görünümlerin paylaşması için basit servis erişimi.</summary>
    public static CameraService Camera { get; private set; } = null!;

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

        var weigh = new WeighViewModel(repo, scale, camera);
        var second = new SecondWeighViewModel(repo, scale, camera, printer);
        var records = new RecordsViewModel(repo);
        var receivable = new ReceivableViewModel(repo);
        var settingsVm = new SettingsViewModel(settings, repo, scale, camera);

        var shell = new ShellViewModel(scale, weigh, second, records, receivable, settingsVm);

        scale.Start();

        var window = new ShellWindow { DataContext = shell };
        window.Closed += (_, _) =>
        {
            scale.Dispose();
            camera.Dispose();
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
