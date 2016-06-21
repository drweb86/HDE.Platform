//Taken from MSDN article by Stephen Cleary https://msdn.microsoft.com/en-us/magazine/dn605875.aspx

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace HDE.Platform.Wpf.Converters
{
    public sealed class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
