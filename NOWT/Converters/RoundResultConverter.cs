using System;
using System.Globalization;
using System.Windows.Data;
using FontAwesome6;
using NOWT.Objects;

namespace NOWT.Converters;

[ValueConversion(typeof(RoundStat), typeof(EFontAwesomeIcon))]
public class RoundResultConverter : IValueConverter
{
    #region IValueConverter Members

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not RoundStat roundStat)
            return EFontAwesomeIcon.None;

        // Determine result based on kills/deaths comparison
        if (roundStat.PlayerKills > roundStat.OpponentKills)
            return EFontAwesomeIcon.Solid_Check; // Win
        else if (roundStat.PlayerKills < roundStat.OpponentKills)
            return EFontAwesomeIcon.Solid_Xmark; // Loss
        else
            return EFontAwesomeIcon.Solid_Equals; // Draw/Tie

    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }

    #endregion
}
