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

    /// <summary>Elle girilen dara (boş ağırlık) değeri — "Darayı Kaydet" bunu kullanır.</summary>
    [ObservableProperty] private int _tareInput;

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

    private bool _updatingFromScale;

    private void OnScaleChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (LiveWeight && e.PropertyName == nameof(ScaleClient.CurrentWeight))
        {
            _updatingFromScale = true;
            LoadedValue = Scale.CurrentWeight;
            _updatingFromScale = false;
        }
    }

    partial void OnLoadedValueChanged(int value)
    {
        if (!_updatingFromScale)
        {
            if (value == 0)
            {
                LiveWeight = true;
                _updatingFromScale = true;
                LoadedValue = Scale.CurrentWeight;
                _updatingFromScale = false;
            }
            else
            {
                LiveWeight = false;
            }
        }
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

    partial void OnPlateChanged(string value)
    {
        LookupTare();
        LiveWeight = true;
        _updatingFromScale = true;
        LoadedValue = Scale.CurrentWeight;
        _updatingFromScale = false;
    }

    partial void OnSelectedKnownChanged(TareRecord? value)
    {
        if (value is not null)
            Plate = value.Plate;
    }

    private void LookupTare()
    {
        var t = string.IsNullOrWhiteSpace(Plate) ? null : _repo.GetTare(Plate.Trim().ToUpperInvariant());
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

    /// <summary>Elle girilen dara (boş ağırlık) değerini bu plakanın sabit darası olarak kaydeder/günceller.</summary>
    [RelayCommand]
    private void SaveTare()
    {
        if (string.IsNullOrWhiteSpace(Plate))
        {
            MessageBox.Show("Plaka boş olamaz.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        if (TareInput <= 0)
        {
            MessageBox.Show("Dara değerini (boş ağırlık) elle girin.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        try
        {
            _repo.UpsertTare(Plate.Trim().ToUpperInvariant(), TareInput, DateTime.Now.ToString("yyyy-MM-dd HH:mm", new CultureInfo("tr-TR")));
            RefreshKnownPlates();
            LookupTare();
            TareInput = 0;
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
    private void CaptureTare() => TareInput = Scale.CurrentWeight;

    [RelayCommand]
    private void CaptureLoaded() => LoadedValue = Scale.CurrentWeight;

    [RelayCommand]
    private void DeleteTare()
    {
        if (string.IsNullOrWhiteSpace(Plate))
        {
            MessageBox.Show("Silinecek plakayı seçin veya girin.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        string cleanPlate = Plate.Trim().ToUpperInvariant();
        var t = _repo.GetTare(cleanPlate);
        if (t is null)
        {
            MessageBox.Show($"{cleanPlate} plakasına ait kayıtlı dara bulunamadı.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        if (MessageBox.Show($"{cleanPlate} plakasının kayıtlı darası silinsin mi?",
                "Dara Silme", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
            return;
        _repo.DeleteTare(cleanPlate);
        Plate = "";
        SelectedKnown = null;
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
            Weight = KnownTare,        // 1. tartım = sabit dara (boş)
            SecWeight = LoadedValue,   // 2. tartım = dolu
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
