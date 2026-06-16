using System.Windows;
using System.Windows.Controls;

namespace TareFlow.Center.Views;

public partial class WeighStationView : UserControl
{
    public WeighStationView() => InitializeComponent();

    private void BtnFullscreen_Click(object sender, RoutedEventArgs e)
    {
        var win = new CameraFullscreenWindow
        {
            Owner = Window.GetWindow(this)
        };
        win.ShowDialog();
    }
}
