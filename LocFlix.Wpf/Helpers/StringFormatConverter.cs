using System;
using System.Globalization;
using System.Windows.Data;

namespace LocFlix.Wpf.Helpers
{
    public class StringFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var format = parameter as string;
            if (!string.IsNullOrEmpty(format))
                return string.Format(format, value);

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
