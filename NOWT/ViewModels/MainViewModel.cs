using System.Collections.Generic;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using NOWT.Helpers;

namespace NOWT.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableObject? _selectedViewModel;

    [ObservableProperty]
    private Dictionary<string, bool> _featureFlags = new();

    public MainViewModel()
    {
        SelectedViewModel = Ioc.Default.GetRequiredService<HomeViewModel>();
        LoadConfig();
    }

    private async void LoadConfig()
    {
        var config = await ConfigHelper.LoadConfigAsync();
        FeatureFlags = config.FeatureFlags;
    }

    public bool GetFeatureFlag(string key)
    {
        return FeatureFlags.TryGetValue(key, out var value) && value;
    }

    [ICommand]
    public void NavigateHome()
    {
        SelectedViewModel = Ioc.Default.GetRequiredService<HomeViewModel>();
    }

    [ICommand]
    public void NavigateInfo()
    {
        SelectedViewModel = Ioc.Default.GetRequiredService<InfoViewModel>();
    }

    [ICommand]
    public void NavigateSettings()
    {
        SelectedViewModel = Ioc.Default.GetRequiredService<SettingsViewModel>();
    }

    [ICommand]
    public void NavigateMatch()
    {
        SelectedViewModel = Ioc.Default.GetRequiredService<MatchViewModel>();
    }

    [ICommand]
    public void NavigateRoundStats()
    {
        SelectedViewModel = Ioc.Default.GetRequiredService<RoundStatsViewModel>();
    }
}
