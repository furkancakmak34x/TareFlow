using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TareFlow.Center.Services;
using TareFlow.Core;

namespace TareFlow.Center.ViewModels;

/// <summary>2. Tartım: bekleyen aracın çıkış tartısını alır, net hesaplar, fiş basar.</summary>
public sealed partial class SecondWeighViewModel : ObservableObject, IActivatable
{
    private readonly WeighRepository _repo;
    private readonly CameraService _camera;
    private readonly ReceiptPrinter _printer;

    public ScaleClient Scale { get; }

    public ObservableCollection<WeightRecord> Pending { get; } = new();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Total))]
    private WeightRecord? _selected;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Total))]
    private int _secWeightValue;

    /// <summary>true: ağırlık kantardan canlı izlenir; false: elle giriş.</summary>
    [ObservableProperty]
    private bool _liveWeight = true;

    [ObservableProperty] private bool _isPaid;
    [ObservableProperty] private bool _shouldPrint = true;
    [ObservableProperty] private bool _isVan; // false = Truck

    public string SecDate { get; private set; } = "";

    public int Total => Selected is null ? 0 : Math.Abs(Selected.Weight - SecWeightValue);

    public SecondWeighViewModel(WeighRepository repo, ScaleClient scale, CameraService camera, ReceiptPrinter printer)
    {
        _repo = repo;
        Scale = scale;
        _camera = camera;
        _printer = printer;
        Scale.PropertyChanged += OnScaleChanged;
    }

    private void OnScaleChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (LiveWeight && e.PropertyName == nameof(ScaleClient.CurrentWeight))
            SecWeightValue = Scale.CurrentWeight;
    }

    public void OnActivated()
    {
        SecDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm", new CultureInfo("tr-TR"));
        OnPropertyChanged(nameof(SecDate));
        Refresh();
    }

    private void Refresh()
    {
        Pending.Clear();
        foreach (var w in _repo.ListWeight())
            Pending.Add(w);
        Selected = Pending.FirstOrDefault();
    }

    [RelayCommand]
    private void Save()
    {
        if (Selected is null)
        {
            MessageBox.Show("Önce bekleyen bir araç seçin.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        if (SecWeightValue <= 0)
        {
            MessageBox.Show("Geçerli bir 2. tartım değeri yok.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var rec = new SecWeightRecord
        {
            Plate = Selected.Plate,
            Date = Selected.Date,
            SecDate = SecDate,
            Customer = Selected.Customer,
            Vendor = Selected.Vendor,
            Product = Selected.Product,
            Weight = Selected.Weight,
            SecWeight = SecWeightValue,
            Total = Total
        };

        var confirm = MessageBox.Show(
            $"Plaka: {rec.Plate}\n1. Tartım: {rec.Weight} kg\n2. Tartım: {rec.SecWeight} kg\nNet: {rec.Total} kg\n\nKayıt onaylansın mı?",
            "2. Tartım Onayı", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (confirm != MessageBoxResult.Yes)
            return;

        try
        {
            _camera.TakeSnapshot(rec.Plate);
            _repo.AddSecWeight(rec);
            _repo.DeleteWeight(rec.Plate, rec.Date);

            if (!IsPaid)
            {
                int fee = _repo.GetFee(IsVan ? VehicleType.Van : VehicleType.Truck);
                _repo.AddReceivable(rec.Plate, rec.Date, fee);
            }

            if (ShouldPrint)
                _printer.Print(rec);

            MessageBox.Show("İkinci tartım işlemi başarılı.", "Kayıt", MessageBoxButton.OK, MessageBoxImage.Information);
            Refresh();
        }
        catch (Exception ex)
        {
            MessageBox.Show("İşlem sırasında hata oluştu: " + ex.Message, "Hata",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    private void Delete()
    {
        if (Selected is null)
            return;
        if (MessageBox.Show("Seçili bekleyen kaydı silmek istediğinizden emin misiniz?",
                "Kayıt Silme", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
            return;
        _repo.DeleteWeight(Selected.Plate, Selected.Date);
        Refresh();
    }
}
