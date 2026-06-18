using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TareFlow.Center.Services;
using TareFlow.Core;

namespace TareFlow.Center.ViewModels;

/// <summary>Kayıtlar: tamamlanmış tartımları listeler, filtreler, siler.</summary>
public sealed partial class RecordsViewModel : ObservableObject, IActivatable
{
    private readonly WeighRepository _repo;
    private readonly ReceiptPrinter _printer;
    private List<SecWeightRecord> _all = new();

    public ObservableCollection<SecWeightRecord> Items { get; } = new();

    [ObservableProperty] private SecWeightRecord? _selected;

    [ObservableProperty] private string _filterPlate = "";
    [ObservableProperty] private string _filterCustomer = "";
    [ObservableProperty] private string _filterVendor = "";
    [ObservableProperty] private DateTime? _filterStartDate;
    [ObservableProperty] private DateTime? _filterEndDate;

    public RecordsViewModel(WeighRepository repo, ReceiptPrinter printer)
    {
        _repo = repo;
        _printer = printer;
    }

    public void OnActivated()
    {
        _all = _repo.ListSecWeight();
        ApplyFilter();
    }

    partial void OnFilterPlateChanged(string value) => ApplyFilter();
    partial void OnFilterCustomerChanged(string value) => ApplyFilter();
    partial void OnFilterVendorChanged(string value) => ApplyFilter();
    partial void OnFilterStartDateChanged(DateTime? value) => ApplyFilter();
    partial void OnFilterEndDateChanged(DateTime? value) => ApplyFilter();

    private void ApplyFilter()
    {
        Items.Clear();
        IEnumerable<SecWeightRecord> q = _all;

        if (!string.IsNullOrWhiteSpace(FilterPlate))
        {
            string f = FilterPlate.Trim();
            q = q.Where(x => x.Plate.Contains(f, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(FilterCustomer))
        {
            string f = FilterCustomer.Trim();
            q = q.Where(x => x.Customer != null && x.Customer.Contains(f, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(FilterVendor))
        {
            string f = FilterVendor.Trim();
            q = q.Where(x => x.Vendor != null && x.Vendor.Contains(f, StringComparison.OrdinalIgnoreCase));
        }

        if (FilterStartDate.HasValue || FilterEndDate.HasValue)
        {
            var start = FilterStartDate ?? DateTime.MinValue;
            var end = FilterEndDate ?? DateTime.MaxValue;

            if (FilterStartDate.HasValue)
                start = FilterStartDate.Value.Date;
            if (FilterEndDate.HasValue)
                end = FilterEndDate.Value.Date.AddDays(1).AddTicks(-1);

            q = q.Where(x =>
            {
                string dateToParse = !string.IsNullOrWhiteSpace(x.SecDate) ? x.SecDate : x.Date;
                if (DateTime.TryParse(dateToParse, out var recordDate))
                {
                    return recordDate >= start && recordDate <= end;
                }
                return false;
            });
        }

        foreach (var r in q)
            Items.Add(r);
    }

    [RelayCommand]
    private void ClearFilters()
    {
        FilterPlate = "";
        FilterCustomer = "";
        FilterVendor = "";
        FilterStartDate = null;
        FilterEndDate = null;
    }

    [RelayCommand]
    private void Delete()
    {
        if (Selected is null)
            return;
        if (MessageBox.Show($"{Selected.Plate} kaydını silmek istediğinizden emin misiniz?",
                "Kayıt Silme", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
            return;
        _repo.DeleteSecWeight(Selected.Id);
        OnActivated();
    }

    [RelayCommand]
    private void Print()
    {
        if (Selected is null)
            return;
        try
        {
            _printer.Print(Selected);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Fiş yazdırılırken hata oluştu: {ex.Message}", "Yazdırma Hatası",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
