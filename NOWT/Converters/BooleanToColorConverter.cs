using System;
using System.Globalization;
using System.Windows.Data;

namespace NOWT.Converters;

[ValueConversion(typeof(bool), typeof(string))]
public class BooleanToColorConverter : IValueConverter
{
    #region IValueConverter Members

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (targetType != typeof(string))
            throw new InvalidOperationException("The target must be a string");

        return (bool)value ? "#00FF00" : "#FF4655"; // Green when connected, Red when disconnected
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }

    #endregion
}
