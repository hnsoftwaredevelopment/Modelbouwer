namespace Modelbouwer.Converters;
public class DecimalNumberConverter : IValueConverter
{
	public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
	{
		if ( value == null )
		{
			return "0,00";
		}

		return string.Format( culture, "{0:N2}", value );
	}

	public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
	{
		if ( string.IsNullOrEmpty( value as string ) )
		{
			return 0m;
		}

		string? input = value as string;

		// Handle the case where user inputs just "0"
		if ( input == "0" )
		{
			return 0m;
		}

		// Normal parsing for other cases
		if ( decimal.TryParse( input, NumberStyles.Any, culture, out decimal result ) )
		{
			return result;
		}

		return 0m;
	}
}
