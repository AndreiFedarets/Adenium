using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Layex.Views
{
    public sealed class ThicknessMaskConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Thickness thickness = (Thickness)value;
            string maskString = (string)parameter;
            byte[] mask = maskString.Split(',').Select(x => x.Trim()).Select(x => string.IsNullOrEmpty(x) ? 0.ToString() : x).Select(x => byte.Parse(x)).ToArray();
            if (mask.Length == 2)
            {
                if (mask[0] == 0)
                {
                    thickness.Left = 0;
                    thickness.Right = 0;
                }
                if (mask[1] == 0)
                {
                    thickness.Top = 0;
                    thickness.Bottom = 0;
                }
            }
            else if (mask.Length == 4)
            {
                if (mask[0] == 0)
                {
                    thickness.Left = 0;
                }
                if (mask[1] == 0)
                {
                    thickness.Top = 0;
                }
                if (mask[2] == 0)
                {
                    thickness.Right = 0;
                }
                if (mask[3] == 0)
                {
                    thickness.Bottom = 0;
                }
            }
            return thickness;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
