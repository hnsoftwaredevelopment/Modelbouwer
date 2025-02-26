namespace Modelbouwer.Converters;
public class DecimalConverter : IValueConverter
{
	public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
	{
		if ( value is string stringValue && double.TryParse( stringValue, out double doubleValue ) )
		{
			CultureInfo nlCulture = new("nl-NL");
			return doubleValue.ToString( "N2", nlCulture );
		}
		return value;
	}

	public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
	{
		return value;
	}
}
