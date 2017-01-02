using System;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace DotNetDash.CameraViews
{
    public class BitmapConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            var bitmap = value as Bitmap;
            try
            {
                using (var bitmapHandle = new SafeHBitmapHandle(bitmap.GetHbitmap(), true))
                {
                    return Imaging.CreateBitmapSourceFromHBitmap(bitmapHandle.DangerousGetHandle(), IntPtr.Zero, Int32Rect.Empty,
                          BitmapSizeOptions.FromWidthAndHeight(bitmap.Width, bitmap.Height));
                }
            }
            catch (ExternalException)
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
