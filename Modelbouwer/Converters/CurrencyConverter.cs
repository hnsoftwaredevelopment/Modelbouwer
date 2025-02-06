namespace Modelbouwer.Converters;

public class CurrencyConverter : IValueConverter
{
	// From numeric value to formated string
	public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
	{
		if ( value is double numberValue )
		{
			return numberValue.ToString( "C", CultureInfo.CreateSpecificCulture( "nl-NL" ) );
		}
		return value;
	}

	// From formated string back to numeric value
	public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
	{
		if ( value is string stringValue )
		{
			// Verwijder valuta-symbool en witruimte
			stringValue = stringValue.Replace( "€", "" ).Trim();

			// Probeer te converteren met Nederlandse cultuurinstellingen
			if ( double.TryParse( stringValue,
				NumberStyles.Currency,
				CultureInfo.CreateSpecificCulture( "nl-NL" ),
				out double result ) )
			{
				return result;
			}
		}
		return value;
	}
}
