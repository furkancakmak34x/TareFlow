using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TareFlow.Center.Services;
using TareFlow.Core;

namespace TareFlow.Center.ViewModels;

/// <summary>Tahsilat: ödenmemiş kantar ücretlerini listeler ve tahsil/siler.</summary>
public sealed partial class ReceivableViewModel : ObservableObject, IActivatable
{
    private readonly WeighRepository _repo;

    public ObservableCollection<ReceivableRecord> Items { get; } = new();

    [ObservableProperty] private ReceivableRecord? _selected;

    public int TotalFee => Items.Sum(x => x.Fee);

    public ReceivableViewModel(WeighRepository repo) => _repo = repo;

    public void OnActivated()
    {
        Items.Clear();
        foreach (var r in _repo.ListReceivable())
            Items.Add(r);
        OnPropertyChanged(nameof(TotalFee));
    }

    [RelayCommand]
    private void Collect()
    {
        if (Selected is null)
            return;
        if (MessageBox.Show($"{Selected.Plate} - {Selected.Fee} ₺ tahsil edildi olarak işaretlensin mi?",
                "Tahsilat", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            return;
        _repo.DeleteReceivable(Selected.Id);
        OnActivated();
    }
}
