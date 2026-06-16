using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TareFlow.Center.Services;
using TareFlow.Core;

namespace TareFlow.Center.ViewModels;

/// <summary>
/// Sabit dara (plaka bazlı boş ağırlık) tartımı. Bir aracın boşu (darası) bir kez
/// kaydedilir; sonra her dolu gelişinde tek tartımla net = |dolu − dara| hesaplanır
/// ve anında fiş basılır. Aynı plaka birden çok kez tartılabilir.
/// </summary>
public sealed partial class TareWeighViewModel : ObservableObject, IActivatable
{
    private readonly WeighRepository _repo;
    private readonly CameraService _camera;
    private readonly ReceiptPrinter _printer;

    public ScaleClient Scale { get; }

    /// <summary>Kayıtlı daraya sahip plakalar (hızlı seçim için).</summary>
    public ObservableCollection<TareRecord> KnownPlates { get; } = new();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Net))]
    [NotifyPropertyChangedFor(nameof(TareDisplay))]
    private int _knownTare;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Net))]
    [NotifyPropertyChangedFor(nameof(TareDisplay))]
    private bool _hasTare;

    [ObservableProperty] private string _plate = "";

    /// <summary>Kayıtlı daralar listesinden seçilen plaka (seçilince forma yüklenir).</summary>
    [ObservableProperty] private TareRecord? _selectedKnown;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Net))]
    private int _loadedValue;

    /// <summary>true: dolu ağırlık kantardan canlı izlenir; false: elle giriş.</summary>
    [ObservableProperty] private bool _liveWeight = true;

    [ObservableProperty] private bool _useCustomer;
    [ObservableProperty] private bool _useVendor;
    [ObservableProperty] private bool _useProduct;
    [ObservableProperty] private string _customer = "";
    [ObservableProperty] private string _vendor = "";
    [ObservableProperty] private string _product = "";

    [ObservableProperty] private bool _isVan;
    [ObservableProperty] private bool _isPaid;
    [ObservableProperty] private bool _shouldPrint = true;

    public int Net => HasTare ? Math.Abs(LoadedValue - KnownTare) : 0;

    public string TareDisplay => HasTare ? $"{KnownTare} kg" : "— kayıtlı dara yok —";

    public TareWeighViewModel(WeighRepository repo, ScaleClient scale, CameraService camera, ReceiptPrinter printer)
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
            LoadedValue = Scale.CurrentWeight;
    }

    public void OnActivated()
    {
        RefreshKnownPlates();
        if (LiveWeight)
            LoadedValue = Scale.CurrentWeight;
    }

    private void RefreshKnownPlates()
    {
        KnownPlates.Clear();
        foreach (var t in _repo.ListTare())
            KnownPlates.Add(t);
    }

    partial void OnPlateChanged(string value) => LookupTare();

    partial void OnSelectedKnownChanged(TareRecord? value)
    {
        if (value is not null)
            Plate = value.Plate;
    }

    private void LookupTare()
    {
        var t = string.IsNullOrWhiteSpace(Plate) ? null : _repo.GetTare(Plate.Trim());
        if (t is null)
        {
            HasTare = false;
            KnownTare = 0;
        }
        else
        {
            HasTare = true;
            KnownTare = t.Weight;
        }
    }

    /// <summary>Kantardaki anlık (boş) ağırlığı bu plakanın sabit darası olarak kaydeder/günceller.</summary>
    [RelayCommand]
    private void SaveTare()
    {
        if (string.IsNullOrWhiteSpace(Plate))
        {
            MessageBox.Show("Plaka boş olamaz.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        int weight = Scale.CurrentWeight;
        if (!Scale.IsStable || weight <= 0)
        {
            if (MessageBox.Show("Kararlı bir tartım değeri yok. Yine de bu değer dara olarak kaydedilsin mi?",
                    "Dara", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;
        }

        var confirm = MessageBox.Show(
            $"Plaka: {Plate.Trim()}\nDara (boş): {weight} kg\n\nBu plakanın sabit darası kaydedilsin mi?",
            "Sabit Dara", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (confirm != MessageBoxResult.Yes)
            return;

        try
        {
            _repo.UpsertTare(Plate.Trim(), weight, DateTime.Now.ToString("yyyy-MM-dd HH:mm", new CultureInfo("tr-TR")));
            RefreshKnownPlates();
            LookupTare();
            MessageBox.Show("Sabit dara kaydedildi.", "Dara", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Dara kaydedilirken hata oluştu: " + ex.Message, "Hata",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    /// <summary>Listeden seçilen plakayı forma yükler.</summary>
    [RelayCommand]
    private void SelectPlate(TareRecord? rec)
    {
        if (rec is null)
            return;
        Plate = rec.Plate;
    }

    [RelayCommand]
    private void DeleteTare()
    {
        if (string.IsNullOrWhiteSpace(Plate) || !HasTare)
            return;
        if (MessageBox.Show($"{Plate.Trim()} plakasının kayıtlı darası silinsin mi?",
                "Dara Silme", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
            return;
        _repo.DeleteTare(Plate.Trim());
        RefreshKnownPlates();
        LookupTare();
    }

    /// <summary>Dolu ağırlıkla tek tartım: net hesaplar, kaydeder, fiş basar.</summary>
    [RelayCommand]
    private void Weigh()
    {
        if (string.IsNullOrWhiteSpace(Plate))
        {
            MessageBox.Show("Plaka boş olamaz.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        if (!HasTare)
        {
            MessageBox.Show("Bu plakanın kayıtlı darası yok. Önce 'Darayı Kaydet' ile boş ağırlığı kaydedin.",
                "Sabit Dara", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        if (LoadedValue <= 0)
        {
            MessageBox.Show("Geçerli bir dolu ağırlık değeri yok.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        if (LoadedValue <= KnownTare)
        {
            if (MessageBox.Show("Dolu ağırlık, kayıtlı daradan küçük/eşit. Yine de devam edilsin mi?",
                    "Sabit Dara", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;
        }

        string now = DateTime.Now.ToString("yyyy-MM-dd HH:mm", new CultureInfo("tr-TR"));
        var rec = new SecWeightRecord
        {
            Plate = Plate.Trim(),
            Date = now,
            SecDate = now,
            Customer = UseCustomer ? Customer.Trim() : "",
            Vendor = UseVendor ? Vendor.Trim() : "",
            Product = UseProduct ? Product.Trim() : "",
            Weight = KnownTare,        // 1. tartım = sabit dara (boş)
            SecWeight = LoadedValue,   // 2. tartım = dolu
            Total = Net
        };

        var confirm = MessageBox.Show(
            $"Plaka: {rec.Plate}\nDara: {rec.Weight} kg\nDolu: {rec.SecWeight} kg\nNet: {rec.Total} kg\n\nFiş basılsın mı?",
            "Sabit Dara Tartımı", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (confirm != MessageBoxResult.Yes)
            return;

        try
        {
            _camera.TakeSnapshot(rec.Plate);
            _repo.AddSecWeight(rec);

            if (!IsPaid)
            {
                int fee = _repo.GetFee(IsVan ? VehicleType.Van : VehicleType.Truck);
                string account = string.IsNullOrWhiteSpace(rec.Customer) ? rec.Plate : rec.Customer!;
                _repo.AddReceivable(account, rec.Plate, rec.SecDate, fee);
            }

            if (ShouldPrint)
                _printer.Print(rec);

            MessageBox.Show("Tartım kaydedildi ve fiş basıldı.", "Sabit Dara", MessageBoxButton.OK, MessageBoxImage.Information);

            // Plaka ve dara aynı kalır; bir sonraki dolu tartım için sadece dolu değeri sıfırlanır.
            if (!LiveWeight)
                LoadedValue = 0;
        }
        catch (Exception ex)
        {
            MessageBox.Show("İşlem sırasında hata oluştu: " + ex.Message, "Hata",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
