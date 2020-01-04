using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace Layex.Converters
{
    public class BitmapToBitmapSourceConverter : IValueConverter
    {
        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DeleteObject(IntPtr handle);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Bitmap)
            {
                return Convert((Bitmap)value);
            }
            if (value is Uri)
            {
                return Convert((Uri)value);
            }
            if (value is string)
            {
                return Convert((string)value);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
            //BitmapSource bitmapSource = (BitmapSource) value;
            //return value.ToBitmap();
        }

        private BitmapSource Convert(Bitmap bitmap)
        {
            BitmapSource bitmapSource;
            IntPtr hBitmap = bitmap.GetHbitmap();
            try
            {
                bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            catch (Win32Exception)
            {
                bitmapSource = null;
            }
            finally
            {
                DeleteObject(hBitmap);
            }
            return bitmapSource;
        }

        private BitmapSource Convert(string uri)
        {
            BitmapSource bitmapSource = null;
            if (!string.IsNullOrEmpty(uri))
            {
                return Convert(new Uri(uri, UriKind.RelativeOrAbsolute));
            }
            return bitmapSource;
        }

        private BitmapSource Convert(Uri uri)
        {
            return new BitmapImage(uri);
        }
    }
}
