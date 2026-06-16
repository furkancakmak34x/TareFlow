using System.Windows;
using System.Windows.Controls;

namespace TareFlow.Center.Views;

/// <summary>
/// Sürekli açık kameranın paylaşılan canlı karesini gösterir. Durdurma/başlatma yok;
/// sadece App.Camera.Frame'i Image.Source olarak bağlar. Sayfa geçişlerinde yayın açık kalır.
/// </summary>
public partial class CameraView : UserControl
{
    public CameraView()
    {
        InitializeComponent();
        Loaded += (_, _) => Bind();
        Unloaded += (_, _) =>
        {
            if (App.Camera is { } cam)
                cam.FrameReady -= OnFrameReady;
        };
    }

    private void Bind()
    {
        if (App.Camera is not { } cam)
            return;
        cam.FrameReady -= OnFrameReady;
        cam.FrameReady += OnFrameReady;
        ApplyFrame();
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
}
