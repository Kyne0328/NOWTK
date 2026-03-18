using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FontAwesome6.Svg;
using NOWT.ViewModels;

namespace NOWT.Views;

public partial class Home : UserControl
{
    public static SvgAwesome ValorantStatus;
    public static SvgAwesome AccountStatus;
    public static SvgAwesome MatchStatus;

    private HomeViewModel? _viewModel;

    public Home()
    {
        InitializeComponent();
        DataContextChanged += DataContextChangedHandler;
        Unloaded += Home_Unloaded;

        ValorantStatus = ValorantStatusView;
        AccountStatus = AccountStatusView;
        MatchStatus = MatchStatusView;
    }

    private void Home_Unloaded(object sender, RoutedEventArgs e)
    {
        if (_viewModel != null)
        {
            _viewModel.GoMatchEvent -= GoMatchHandler;
            _viewModel = null;
        }
        DataContextChanged -= DataContextChangedHandler;
        Unloaded -= Home_Unloaded;
    }

    private void DataContextChangedHandler(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue is not HomeViewModel viewModel)
            return;

        // Detach from the old viewmodel's GoMatchEvent if any
        if (_viewModel != null)
        {
            _viewModel.GoMatchEvent -= GoMatchHandler;
        }

        _viewModel = viewModel;
        _viewModel.GoMatchEvent += GoMatchHandler;
    }

    private void GoMatchHandler()
    {
        Dispatcher.Invoke(() =>
        {
            if (GoMatch.Command.CanExecute(null))
                GoMatch.Command.Execute(null);
        });
    }
}