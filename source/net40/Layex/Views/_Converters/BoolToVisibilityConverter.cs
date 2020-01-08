using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Layex.Views
{
    public sealed class BoolToVisibilityConverter : IValueConverter
    {
        public Visibility TrueValue { get; set; }

        public Visibility FalseValue { get; set; }

        public BoolToVisibilityConverter()
        {
            TrueValue = Visibility.Visible;
            FalseValue = Visibility.Collapsed;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool visible = System.Convert.ToBoolean(value);
            return visible ? TrueValue : FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (Visibility)value == TrueValue;
        }
    }
}