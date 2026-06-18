using System.Globalization;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TareFlow.Center.Services;
using TareFlow.Core;

namespace TareFlow.Center.ViewModels;

/// <summary>1. Tartım: aracın giriş tartısını alır, Weight tablosuna ekler.</summary>
public sealed partial class WeighViewModel : ObservableObject, IActivatable
{
    private readonly WeighRepository _repo;
    private readonly CameraService _camera;

    public ScaleClient Scale { get; }

    [ObservableProperty] private string _plate = "";
    [ObservableProperty] private string _date = "";
    [ObservableProperty] private string _customer = "";
    [ObservableProperty] private string _vendor = "";
    [ObservableProperty] private string _product = "";

    public WeighViewModel(WeighRepository repo, ScaleClient scale, CameraService camera)
    {
        _repo = repo;
        Scale = scale;
        _camera = camera;
    }

    public void OnActivated()
    {
        Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm", new CultureInfo("tr-TR"));
        Plate = Customer = Vendor = Product = "";
    }

    [RelayCommand]
    private void Save()
    {
        if (string.IsNullOrWhiteSpace(Plate))
        {
            MessageBox.Show("Plaka boş olamaz.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var record = new WeightRecord
        {
            Plate = Plate.Trim().ToUpperInvariant(),
            Date = Date,
            Weight = Scale.CurrentWeight,
            Customer = (Customer ?? "").Trim(),
            Vendor = (Vendor ?? "").Trim(),
            Product = (Product ?? "").Trim()
        };

        try
        {
            _repo.AddWeight(record);
            _camera.TakeSnapshot(record.Plate);
            OnActivated();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Kayıt eklenirken hata oluştu: " + ex.Message, "Hata",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
