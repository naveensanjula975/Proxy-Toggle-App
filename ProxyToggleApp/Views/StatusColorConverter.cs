using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ProxyToggleApp.Views
{
    public class StatusColorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length > 0 && values[0] is bool isEnabled)
            {
                return isEnabled ? Colors.Green : Colors.Red;
            }
            return Colors.Gray;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
