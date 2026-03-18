using System;
using System.Globalization;
using System.Windows.Data;
using FontAwesome6;
using NOWT.Objects;

namespace NOWT.Converters;

[ValueConversion(typeof(RoundStat), typeof(FontAwesomeIcon))]
public class RoundResultConverter : IValueConverter
{
    #region IValueConverter Members

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not RoundStat roundStat)
            return FontAwesomeIcon.None;

        // Determine result based on kills/deaths comparison
        if (roundStat.PlayerKills > roundStat.OpponentKills)
            return FontAwesomeIcon.Solid_CheckCircle; // Win
        else if (roundStat.PlayerKills < roundStat.OpponentKills)
            return FontAwesomeIcon.Solid_TimesCircle; // Loss
        else
            return FontAwesomeIcon.Solid_Equals; // Draw/Tie

    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }

    #endregion
}
