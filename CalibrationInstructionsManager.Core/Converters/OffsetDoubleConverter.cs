using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CalibrationInstructionsManager.Core.Converters
{
    public class OffsetDoubleConverter : IValueConverter
    {
        #region IValueConverter Members
        public double Offset { get; set; }
        public bool KeepPositive { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double number = (double)value + Offset;
            if ((KeepPositive) && (number < 0.0))
            {
                number = 0.0;
            }
            return number;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double number = (double)value - Offset;
            if ((KeepPositive) && (number < 0.0))
            {
                number = 0.0;
            }
            return number;
        }

        #endregion
    }
}
