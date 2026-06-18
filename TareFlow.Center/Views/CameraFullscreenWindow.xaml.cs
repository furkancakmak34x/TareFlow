using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TareFlow.Center.Views;

/// <summary>
/// Kamerayı tam ekran gösterir. Paylaşılan oynatıcı (App.Camera.Player) açılışta bu
/// pencerenin VideoView'ına atanır, kapanışta bırakılır (istasyon geri alır).
/// Üstte kameralar arası geçiş; Esc veya çift tıkla kapanır.
/// </summary>
public partial class CameraFullscreenWindow : Window
{
    private bool _suppressSelection;

    public CameraFullscreenWindow()
    {
        InitializeComponent();
        Loaded += OnLoaded;
        Closed += OnClosed;
        KeyDown += OnKeyDown;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (App.Camera is not { } cam)
            return;

        VideoView.MediaPlayer = cam.Player;

        _suppressSelection = true;
        CameraList.ItemsSource = cam.Cameras;
        CameraList.SelectedIndex = cam.ActiveCamera;
        _suppressSelection = false;
        Focus();
    }

    private void OnClosed(object? sender, EventArgs e)
    {
        VideoView.MediaPlayer = null;   // oynatıcıyı bırak (istasyon geri alacak)
    }

    private void CameraList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (_suppressSelection)
            return;
        if (App.Camera is { } cam && CameraList.SelectedIndex >= 0)
            cam.SwitchTo(CameraList.SelectedIndex);
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
            Close();
    }

    private void Root_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ClickCount == 2)
            Close();
    }

    private void BtnClose_Click(object sender, RoutedEventArgs e) => Close();
}
