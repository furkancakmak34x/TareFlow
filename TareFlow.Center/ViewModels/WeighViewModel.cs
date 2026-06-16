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
    [ObservableProperty] private bool _useCustomer;
    [ObservableProperty] private bool _useVendor;
    [ObservableProperty] private bool _useProduct;
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
        UseCustomer = UseVendor = UseProduct = false;
    }

    [RelayCommand]
    private void Save()
    {
        if (string.IsNullOrWhiteSpace(Plate))
        {
            MessageBox.Show("Plaka boş olamaz.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        if (!Scale.IsStable || Scale.CurrentWeight <= 0)
        {
            if (MessageBox.Show("Kararlı bir tartım değeri yok. Yine de devam edilsin mi?",
                    "Tartım", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;
        }

        var record = new WeightRecord
        {
            Plate = Plate.Trim(),
            Date = Date,
            Weight = Scale.CurrentWeight,
            Customer = UseCustomer ? Customer.Trim() : "",
            Vendor = UseVendor ? Vendor.Trim() : "",
            Product = UseProduct ? Product.Trim() : ""
        };

        var confirm = MessageBox.Show(
            $"Plaka: {record.Plate}\n1. Tartım: {record.Weight} kg\n\nKayıt onaylansın mı?",
            "1. Tartım Onayı", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (confirm != MessageBoxResult.Yes)
            return;

        try
        {
            _repo.AddWeight(record);
            _camera.TakeSnapshot(record.Plate);
            MessageBox.Show("İlk tartım işlemi başarılı.", "Kayıt", MessageBoxButton.OK, MessageBoxImage.Information);
            OnActivated();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Kayıt eklenirken hata oluştu: " + ex.Message, "Hata",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
