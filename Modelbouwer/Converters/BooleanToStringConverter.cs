namespace Modelbouwer.Converters;
public class BooleanToStringConverter : IValueConverter
{
	public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
	{
		if ( value is bool booleanValue )
		{
			return booleanValue ? "*" : string.Empty;
		}
		return string.Empty;
	}

	public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
	{
		if ( value is string stringValue )
		{
			return stringValue == "*";
		}
		return false;
	}
}
