namespace Modelbouwer.Converters;

public class StringEmptyToVisibilityConverter : IValueConverter
{
	public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
	{
		return value is string text ? string.IsNullOrWhiteSpace( text ) ? Visibility.Collapsed : Visibility.Visible : Visibility.Visible;
	}

	public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
	{
		throw new NotImplementedException();
	}
}