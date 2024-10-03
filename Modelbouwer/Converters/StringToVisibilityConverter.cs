namespace Modelbouwer.Converters;
public class StringToVisibilityConverter : IValueConverter
{
	public object Convert( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
	{
		if ( string.IsNullOrWhiteSpace( value?.ToString() ) )
		{
			return Visibility.Collapsed;  // Verberg de Button als de TextBox leeg is
		}
		return Visibility.Visible;  // Toon de Button als er tekst is
	}

	public object ConvertBack( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
	{
		throw new NotImplementedException();
	}
}