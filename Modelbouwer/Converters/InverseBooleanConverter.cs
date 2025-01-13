namespace Modelbouwer.Converters;
public class InverseBooleanConverter : IValueConverter
{
	public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
	{
		if ( value is bool boolValue )
		{
			if ( parameter is string param && param == "Invert" )
			{
				return boolValue;
			}
			return !boolValue;
		}
		return value;
	}

	public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
	{
		return value;  // ConvertBack is niet nodig, maar vereist voor IValueConverter.
	}
}
