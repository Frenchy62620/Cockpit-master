using System;
using System.Windows.Data;

namespace Cockpit.General.Properties.Views.Converters
{
    public class EnumToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            if (targetType.IsEnum)
            {
                // convert int to enum
                return Enum.ToObject(targetType, value);
            }

            if (value.GetType().IsEnum)
            {
                // convert enum to int
                return System.Convert.ChangeType(
                    value,
                    Enum.GetUnderlyingType(value.GetType()));
            }

            return null;

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }
    }
}
