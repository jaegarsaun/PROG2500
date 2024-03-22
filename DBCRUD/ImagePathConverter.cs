using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace DBCRUD
{
    public class ImagePathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string imagePath = value as string;
            if (!string.IsNullOrEmpty(imagePath))
            {
                string imagesDirectory = "./images"; 
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                string fullPath = Path.Combine(basePath, imagesDirectory, imagePath + ".png");

                if (File.Exists(fullPath))
                {
                    return new BitmapImage(new Uri(fullPath, UriKind.Absolute));
                }
            }
            return null; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}