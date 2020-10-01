using System;
using System.Globalization;
using System.Windows.Data;

namespace CalibrationInstructionsManager.Core.Converters
{
    public class PrefixValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var bindedString = value.ToString();

            // Don't convert if string is < 20
            if (bindedString.Length < 20)
            {
                return bindedString;
            }
            return bindedString.Substring(0, 20) + " ...";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
