using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace NOWT.Converters;

[ValueConversion(typeof(int), typeof(Visibility))]
public class LeaderboardVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int position && position > 0)
        {
            return Visibility.Visible;
        }
        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
