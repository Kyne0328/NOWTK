using System.Windows;
using System.Windows.Controls;
using NOWT.Objects;

namespace NOWT.Controls;

public partial class EnhancedPlayerControl : UserControl
{
    public static readonly DependencyProperty PlayerProperty = DependencyProperty.Register(
        "PlayerCell",
        typeof(Player),
        typeof(EnhancedPlayerControl),
        new PropertyMetadata(new Player())
    );

    public EnhancedPlayerControl()
    {
        InitializeComponent();
    }

    public Player PlayerCell
    {
        get => (Player)GetValue(PlayerProperty);
        set => SetValue(PlayerProperty, value);
    }
}
