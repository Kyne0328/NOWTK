using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using NOWT.Objects;

namespace NOWT.Converters;

[ValueConversion(typeof(RoundStat), typeof(string))]
public class RoundResultColorConverter : IValueConverter
{
    #region IValueConverter Members

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not RoundStat roundStat)
            return "#8B8FA3"; // Default gray

        // Determine color based on kills/deaths comparison
        if (roundStat.PlayerKills > roundStat.OpponentKills)
            return "#FF4655"; // Red (win)
        else if (roundStat.PlayerKills < roundStat.OpponentKills)
            return "#46A0FF"; // Blue (loss)
        else
            return "#FFD700"; // Gold (draw)
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }

    #endregion
}
