namespace Modelbouwer.Converters;
public class ByteToImageConverter : IValueConverter
{
	public object Convert( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
	{
		if ( value is byte [ ] imageBytes && imageBytes.Length > 0 )
		{
			using ( MemoryStream ms = new( imageBytes ) )
			{
				BitmapImage image = new();
				image.BeginInit();
				image.CacheOption = BitmapCacheOption.OnLoad;
				image.StreamSource = ms;
				image.EndInit();
				return image;
			}
		}

		return System.Windows.Application.Current.TryFindResource( "noimage" ) as DrawingImage;
		//return null;  // Return null if no image exists
	}

	public object ConvertBack( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
	{
		if ( value is BitmapSource bitmapSource )
		{
			using ( MemoryStream stream = new() )
			{
				BitmapEncoder encoder = new PngBitmapEncoder();
				encoder.Frames.Add( BitmapFrame.Create( bitmapSource ) );
				encoder.Save( stream );
				return stream.ToArray();
			}
		}

		return null;
	}
}
