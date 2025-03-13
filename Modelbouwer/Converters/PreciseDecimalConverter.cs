namespace Modelbouwer.Converters;
public class PreciseDecimalConverter : IValueConverter
{
	public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
	{
		if ( value is decimal decimalValue )
		{
			// Formatteren als string met exact 6 decimalen
			return decimalValue.ToString( "0.000000" );
		}
		return value;
	}

	public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
	{
		if ( value is string stringValue )
		{
			if ( decimal.TryParse( stringValue, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result ) )
			{
				return result;
			}
		}
		return value;
	}
}
