using System.Globalization;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TareFlow.Center.Services;
using TareFlow.Core;

namespace TareFlow.Center.ViewModels;

/// <summary>
/// Manuel tartım: 1. ve 2. tartım değerleri elle girilir (kantara bağlı değil),
/// net hesaplanır, tek seferde kaydedilir ve fiş basılır. Bekleyen kuyruğu yoktur.
/// </summary>
public sealed partial class ManualWeighViewModel : ObservableObject, IActivatable
{
    private readonly WeighRepository _repo;
    private readonly CameraService _camera;
    private readonly ReceiptPrinter _printer;

    public ScaleClient Scale { get; }

    [ObservableProperty] private string _plate = "";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Net))]
    private int _firstWeight;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Net))]
    private int _secondWeight;

    [ObservableProperty] private string _customer = "";
    [ObservableProperty] private string _vendor = "";
    [ObservableProperty] private string _product = "";

    [ObservableProperty] private bool _isVan;
    [ObservableProperty] private bool _isPaid;
    [ObservableProperty] private bool _shouldPrint = true;

    public int Net => Math.Abs(FirstWeight - SecondWeight);

    public ManualWeighViewModel(WeighRepository repo, ScaleClient scale, CameraService camera, ReceiptPrinter printer)
    {
        _repo = repo;
        Scale = scale;
        _camera = camera;
        _printer = printer;
    }

    public void OnActivated() { }

    /// <summary>Kantardaki anlık değeri 1. tartım alanına kopyalar.</summary>
    [RelayCommand] private void CaptureFirst() => FirstWeight = Scale.CurrentWeight;

    /// <summary>Kantardaki anlık değeri 2. tartım alanına kopyalar.</summary>
    [RelayCommand] private void CaptureSecond() => SecondWeight = Scale.CurrentWeight;

    [RelayCommand]
    private void Save()
    {
        if (string.IsNullOrWhiteSpace(Plate))
        {
            MessageBox.Show("Plaka boş olamaz.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        if (FirstWeight <= 0 || SecondWeight <= 0)
        {
            MessageBox.Show("1. ve 2. tartım değerleri geçerli olmalı.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        string customer = (Customer ?? "").Trim();
        if (!IsPaid && string.IsNullOrWhiteSpace(customer))
        {
            customer = Views.CustomerSelectionDialog.Show(_repo) ?? "";
            if (string.IsNullOrWhiteSpace(customer))
                return; // Abort saving if user cancelled
        }

        string now = DateTime.Now.ToString("yyyy-MM-dd HH:mm", new CultureInfo("tr-TR"));
        var rec = new SecWeightRecord
        {
            Plate = Plate.Trim().ToUpperInvariant(),
            Date = now,
            SecDate = now,
            Customer = customer,
            Vendor = (Vendor ?? "").Trim(),
            Product = (Product ?? "").Trim(),
            Weight = FirstWeight,
            SecWeight = SecondWeight,
            Total = Net
        };

        try
        {
            _camera.TakeSnapshot(rec.Plate);
            _repo.AddSecWeight(rec);

            if (!IsPaid)
            {
                int fee = _repo.GetFee(IsVan ? VehicleType.Van : VehicleType.Truck);
                _repo.AddReceivable(rec.Customer, rec.Plate, rec.SecDate, fee);
            }

            if (ShouldPrint)
                _printer.Print(rec);

            Reset();
        }
        catch (Exception ex)
        {
            MessageBox.Show("İşlem sırasında hata oluştu: " + ex.Message, "Hata",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Reset()
    {
        Plate = Customer = Vendor = Product = "";
        FirstWeight = SecondWeight = 0;
    }
}
