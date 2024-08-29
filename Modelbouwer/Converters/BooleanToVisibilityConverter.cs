namespace Modelbouwer.Converters;
public class BooleanToVisibilityConverter : IValueConverter
{
	public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
	{
		if ( value is bool boolValue )
		{
			return boolValue ? Visibility.Visible : Visibility.Collapsed;
		}

		return Visibility.Collapsed; // Fallback value if input is not a bool
	}

	public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
	{
		if ( value is Visibility visibility )
		{
			return visibility == Visibility.Visible;
		}

		return false; // Fallback value if input is not Visibility
	}
}
