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
    private List<SecWeightRecord> _all = new();

    public ObservableCollection<SecWeightRecord> Items { get; } = new();

    [ObservableProperty] private SecWeightRecord? _selected;

    [ObservableProperty] private string _filter = "";

    public RecordsViewModel(WeighRepository repo) => _repo = repo;

    public void OnActivated()
    {
        _all = _repo.ListSecWeight();
        ApplyFilter();
    }

    partial void OnFilterChanged(string value) => ApplyFilter();

    private void ApplyFilter()
    {
        Items.Clear();
        IEnumerable<SecWeightRecord> q = _all;
        if (!string.IsNullOrWhiteSpace(Filter))
        {
            string f = Filter.Trim();
            q = q.Where(x => x.Plate.Contains(f, StringComparison.OrdinalIgnoreCase)
                          || x.Date.Contains(f, StringComparison.OrdinalIgnoreCase));
        }
        foreach (var r in q)
            Items.Add(r);
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
}
