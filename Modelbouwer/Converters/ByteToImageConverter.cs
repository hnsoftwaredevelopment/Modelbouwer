namespace Modelbouwer.Converters;
public class ByteToImageConverter : IValueConverter
{
	public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
	{
		try
		{
			if ( value == null || !( value is byte [ ] imageData ) || imageData.Length == 0 )
			{
				return null;
			}

			BitmapImage image = new();
			using ( MemoryStream stream = new( imageData ) )
			{
				image.BeginInit();
				image.CacheOption = BitmapCacheOption.OnLoad; // Belangrijk! Sluit de stream na het laden
				image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
				image.StreamSource = stream;
				image.EndInit();
				image.Freeze(); // Zorgt voor thread-safety
			}
			return image;
		}
		catch ( Exception ex )
		{
			Debug.WriteLine( $"Fout bij afbeeldingsconversie: {ex.Message}" );
			// Retourneer een standaard of placeholder afbeelding
			return null; // Of een fallback afbeelding
		}
	}

	public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
	{
		if ( value is BitmapImage bitmapImage )
		{
			using ( MemoryStream stream = new() )
			{
				BitmapEncoder encoder = new PngBitmapEncoder();
				encoder.Frames.Add( BitmapFrame.Create( bitmapImage ) );
				encoder.Save( stream );
				return stream.ToArray();
			}
		}
		return null;
	}
}
