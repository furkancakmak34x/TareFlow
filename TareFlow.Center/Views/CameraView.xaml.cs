using System.Windows.Controls;

namespace TareFlow.Center.Views;

/// <summary>
/// (Kullanım dışı) Eski paylaşılan-bitmap kamera görünümü. Artık tartım ekranı ve tam
/// ekran penceresi doğrudan LibVLCSharp.WPF VideoView kullanıyor. Geriye dönük uyumluluk
/// için boş bırakıldı.
/// </summary>
public partial class CameraView : UserControl
{
    public CameraView() => InitializeComponent();
}
