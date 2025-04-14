using Binding = System.Windows.Data.Binding;

namespace Modelbouwer.Converters;
public class TimeInputConverter : IValueConverter
{
	public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
	{
		if ( value is DateTime dateTime )
		{
			return dateTime.ToString( "HH:mm" );
		}
		else if ( value is string timeString && !string.IsNullOrEmpty( timeString ) )
		{
			try
			{
				// Combineer huidige datum met de tijd string
				return DateTime.ParseExact( timeString, "HH:mm", CultureInfo.InvariantCulture );
			}
			catch
			{
				return DateTime.Now;
			}
		}
		return DateTime.Now;
	}

	public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
	{
		if ( value is string input )
		{
			if ( string.IsNullOrEmpty( input ) )
			{
				return DateTime.MinValue; // Of een andere standaardwaarde
			}

			string normalizedInput = NormalizeTimeString(input);

			if ( DateTime.TryParseExact( normalizedInput, "HHmm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDateTime ) )
			{
				return parsedDateTime;
			}
		}

		return Binding.DoNothing; // Geen geldige conversie
	}

	private string NormalizeTimeString( string input )
	{
		// Verwijder alle niet-cijfer tekens
		string digitsOnly = new(input.Where(char.IsDigit).ToArray());

		// Voeg een leidende nul toe als de lengte 3 is
		if ( digitsOnly.Length == 3 )
		{
			digitsOnly = "0" + digitsOnly;
		}

		return digitsOnly;
	}
}