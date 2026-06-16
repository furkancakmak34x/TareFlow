using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TareFlow.Center.Views;

/// <summary>
/// Kamerayı tam ekran gösterir. Paylaşılan canlı kareyi (App.Camera.Frame) bağlar;
/// üstte kameralar arası geçiş yapılabilir. Esc veya çift tıkla kapanır.
/// </summary>
public partial class CameraFullscreenWindow : Window
{
    private bool _suppressSelection;

    public CameraFullscreenWindow()
    {
        InitializeComponent();
        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
        KeyDown += OnKeyDown;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (App.Camera is not { } cam)
            return;

        // Kamera listesini doldur ve aktif olanı seç.
        _suppressSelection = true;
        CameraList.ItemsSource = cam.Cameras;
        CameraList.SelectedIndex = cam.ActiveCamera;
        _suppressSelection = false;

        cam.FrameReady -= OnFrameReady;
        cam.FrameReady += OnFrameReady;
        ApplyFrame();
        Focus();
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        if (App.Camera is { } cam)
            cam.FrameReady -= OnFrameReady;
    }

    private void OnFrameReady() => Dispatcher.Invoke(ApplyFrame);

    private void ApplyFrame()
    {
        var frame = App.Camera?.Frame;
        if (frame is null)
            return;
        image.Source = frame;
        placeholder.Visibility = Visibility.Collapsed;
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
