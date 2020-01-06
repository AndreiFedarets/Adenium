using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Layex.Views
{
    public sealed class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool visible = System.Convert.ToBoolean(value);
            bool reverse = System.Convert.ToBoolean(parameter);
            if (reverse)
            {
                visible = !visible;
            }
            if (visible)
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}