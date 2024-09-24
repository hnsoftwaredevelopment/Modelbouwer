using System.Windows.Media.Imaging;

namespace Modelbouwer.Converters;
public class ByteToImageConverter : IValueConverter
{
	public object Convert( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
	{
		if ( value is byte [ ] imageBytes && imageBytes.Length > 0 )
		{
			using ( var ms = new MemoryStream( imageBytes ) )
			{
				var image = new BitmapImage();
				image.BeginInit();
				image.CacheOption = BitmapCacheOption.OnLoad;
				image.StreamSource = ms;
				image.EndInit();
				return image;
			}
		}
		return null;  // Return null if no image exists
	}

	public object ConvertBack( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
	{
		throw new NotImplementedException();
	}
}
