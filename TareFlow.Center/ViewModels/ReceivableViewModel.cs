using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TareFlow.Center.Services;
using TareFlow.Core;

namespace TareFlow.Center.ViewModels;

/// <summary>
/// Tahsilat: bekleyen kantar ücretlerini müşteri (cari) bazında listeler.
/// Bir müşterinin birden fazla tırı / tartımı tek hesapta toplanır; seçili
/// müşterinin detay kalemleri sağda gösterilir, müşteri bazında tahsil edilir.
/// </summary>
public sealed partial class ReceivableViewModel : ObservableObject, IActivatable
{
    private readonly WeighRepository _repo;

    /// <summary>Müşteri (cari) bazında toplam borçlar.</summary>
    public ObservableCollection<CustomerDebt> Customers { get; } = new();

    /// <summary>Seçili müşterinin borç kalemleri.</summary>
    public ObservableCollection<ReceivableRecord> Details { get; } = new();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SelectedCustomerName))]
    private CustomerDebt? _selected;

    [ObservableProperty] private ReceivableRecord? _selectedDetail;

    public int TotalFee => Customers.Sum(x => x.Total);

    public string SelectedCustomerName => Selected?.Customer ?? "";

    public ReceivableViewModel(WeighRepository repo) => _repo = repo;

    public void OnActivated()
    {
        string? keep = Selected?.Customer;
        Customers.Clear();
        foreach (var c in _repo.ListCustomerDebts())
            Customers.Add(c);
        OnPropertyChanged(nameof(TotalFee));

        Selected = Customers.FirstOrDefault(c => c.Customer == keep) ?? Customers.FirstOrDefault();
    }

    partial void OnSelectedChanged(CustomerDebt? value) => LoadDetails();

    private void LoadDetails()
    {
        Details.Clear();
        if (Selected is null)
            return;
        foreach (var r in _repo.ListReceivableByCustomer(Selected.Customer))
            Details.Add(r);
    }

    /// <summary>Seçili müşterinin tüm bekleyen borcunu tahsil edildi olarak işaretler.</summary>
    [RelayCommand]
    private void CollectCustomer()
    {
        if (Selected is null)
            return;
        if (MessageBox.Show(
                $"{Selected.Customer} adına bekleyen {Selected.Total} ₺ ({Selected.Count} kalem) tahsil edildi olarak işaretlensin mi?",
                "Tahsilat", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            return;
        _repo.DeleteReceivablesByCustomer(Selected.Customer);
        OnActivated();
    }

    /// <summary>Seçili tek bir borç kalemini tahsil edildi olarak siler.</summary>
    [RelayCommand]
    private void CollectRecord()
    {
        if (SelectedDetail is null)
            return;
        if (MessageBox.Show(
                $"{SelectedDetail.Plate} - {SelectedDetail.Fee} ₺ kalemi tahsil edildi olarak işaretlensin mi?",
                "Tahsilat", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            return;
        _repo.DeleteReceivable(SelectedDetail.Id);
        OnActivated();
    }
}
