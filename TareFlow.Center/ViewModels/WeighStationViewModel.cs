using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using TareFlow.Center.Services;

namespace TareFlow.Center.ViewModels;

/// <summary>
/// Tüm tartım işlemlerini tek ekranda toplayan istasyon. Üstte sürekli açık kamera
/// (geçiş + tam ekran), altta sekmeli formlar: 1. Tartım, 2. Tartım, Sabit Dara.
/// </summary>
public sealed partial class WeighStationViewModel : ObservableObject, IActivatable
{
    private readonly CameraService _camera;

    public WeighViewModel Weigh { get; }
    public SecondWeighViewModel Second { get; }
    public ManualWeighViewModel Manual { get; }
    public TareWeighViewModel Tare { get; }
    public ScaleClient Scale { get; }

    /// <summary>Tanımlı kameralar (geçiş butonları için).</summary>
    public ObservableCollection<CameraConfig> Cameras { get; } = new();

    /// <summary>Seçili/aktif kamera indeksi (kamera geçiş ListBox'ına iki yönlü bağlı).</summary>
    [ObservableProperty] private int _selectedCameraIndex = -1;

    /// <summary>Aktif kamera PTZ destekliyorse true (yön kontrol paneli görünürlüğü).</summary>
    [ObservableProperty] private bool _activeCameraHasPtz;

    /// <summary>0: 1. Tartım, 1: 2. Tartım, 2: Manuel Tartım, 3: Sabit Tartım, 4: Dara Tanımla.</summary>
    [ObservableProperty] private int _selectedTabIndex;

    public WeighStationViewModel(
        WeighViewModel weigh,
        SecondWeighViewModel second,
        ManualWeighViewModel manual,
        TareWeighViewModel tare,
        ScaleClient scale,
        CameraService camera)
    {
        Weigh = weigh;
        Second = second;
        Manual = manual;
        Tare = tare;
        Scale = scale;
        _camera = camera;
        _camera.ActiveCameraChanged += () => SelectedCameraIndex = _camera.ActiveCamera;
    }

    partial void OnSelectedCameraIndexChanged(int value)
    {
        _camera.SwitchTo(value);
        UpdatePtz();
    }

    private void RefreshCameras()
    {
        Cameras.Clear();
        foreach (var c in _camera.Cameras)
            Cameras.Add(c);
        SelectedCameraIndex = _camera.ActiveCamera;
        UpdatePtz();
    }

    private void UpdatePtz()
    {
        int i = SelectedCameraIndex;
        ActiveCameraHasPtz = i >= 0 && i < Cameras.Count && Cameras[i].HasPtz;
    }

    public void OnActivated()
    {
        RefreshCameras();
        ActivateCurrentTab();
    }

    partial void OnSelectedTabIndexChanged(int value) => ActivateCurrentTab();

    private void ActivateCurrentTab()
    {
        switch (SelectedTabIndex)
        {
            case 0: Weigh.OnActivated(); break;
            case 1: Second.OnActivated(); break;
            case 2: Manual.OnActivated(); break;
            case 3: Tare.OnActivated(); break;
            case 4: Tare.OnActivated(); break;
        }
    }
}
