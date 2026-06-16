using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TareFlow.Center.Services;

namespace TareFlow.Center.ViewModels;

public sealed partial class ShellViewModel : ObservableObject
{
    private readonly WeighViewModel _weigh;
    private readonly SecondWeighViewModel _secondWeigh;
    private readonly RecordsViewModel _records;
    private readonly ReceivableViewModel _receivable;
    private readonly SettingsViewModel _settings;

    [ObservableProperty]
    private ObservableObject? _currentViewModel;

    [ObservableProperty]
    private string _activePage = "";

    /// <summary>true: tile'lı anasayfa görünür; false: bir alt sayfa açık.</summary>
    [ObservableProperty]
    private bool _isHome = true;

    [ObservableProperty]
    private string _pageTitle = "";

    public ScaleClient Scale { get; }

    public ShellViewModel(
        ScaleClient scale,
        WeighViewModel weigh,
        SecondWeighViewModel secondWeigh,
        RecordsViewModel records,
        ReceivableViewModel receivable,
        SettingsViewModel settings)
    {
        Scale = scale;
        _weigh = weigh;
        _secondWeigh = secondWeigh;
        _records = records;
        _receivable = receivable;
        _settings = settings;
    }

    private void Navigate(ObservableObject vm, string page, string title)
    {
        CurrentViewModel = vm;
        ActivePage = page;
        PageTitle = title;
        IsHome = false;
        (vm as IActivatable)?.OnActivated();
    }

    [RelayCommand] private void GoHome() => IsHome = true;

    [RelayCommand] private void ShowWeigh() => Navigate(_weigh, "weigh", "1. Tartım");
    [RelayCommand] private void ShowSecondWeigh() => Navigate(_secondWeigh, "second", "2. Tartım");
    [RelayCommand] private void ShowRecords() => Navigate(_records, "records", "Kayıtlar");
    [RelayCommand] private void ShowReceivable() => Navigate(_receivable, "receivable", "Tahsilat");
    [RelayCommand] private void ShowSettings() => Navigate(_settings, "settings", "Ayarlar");
}

/// <summary>Görünüme geçildiğinde tetiklenir (liste yenileme vb.).</summary>
public interface IActivatable
{
    void OnActivated();
}
