using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;

namespace NOWT.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableObject? _selectedViewModel;

    public MainViewModel()
    {
        try
        {
            SelectedViewModel = Ioc.Default.GetRequiredService<HomeViewModel>();
        }
        catch (Exception ex)
        {
            // Log the exception and rethrow so we can see what's happening
            Constants.Log.Error("Failed to initialize MainViewModel: {Exception}", ex);
            throw;
        }
    }
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
}
