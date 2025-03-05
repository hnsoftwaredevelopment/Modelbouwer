namespace Modelbouwer.Converters;
public static class DynamicNumericStringConverter
{
	/// <summary>
	/// Formatteert een decimaal getal met flexibel aantal decimalen
	/// </summary>
	/// <param name="number">Te formatteren getal</param>
	/// <param name="maxDecimals">Maximaal aantal decimalen (standaard 2)
	///     </param>
	/// <param name="defaultValue">Standaardwaarde voor null of niet-numerieke
	///     invoer</param>
	/// <returns>Geformatteerde string</returns>
	public static string FormatNumber( decimal? number, int maxDecimals = 2, string defaultValue = "" )
	{
		// Controleer op null
		if ( !number.HasValue )
		{
			return defaultValue;
		}

		// Haal de waarde uit de nullable decimal
		decimal actualNumber = number.Value;

		// Als het een geheel getal is, gebruik duizendtal separator zonder decimalen
		if ( actualNumber == Math.Floor( actualNumber ) )
		{
			return actualNumber.ToString( "N0", new System.Globalization.CultureInfo( "nl-NL" ) );
		}

		// Bereken het werkelijke aantal decimalen
		int actualDecimals = GetDecimalPlaces(actualNumber);
		int displayDecimals = Math.Min(actualDecimals, maxDecimals);

		// Gebruik dynamische decimale precisie
		string formatString = $"N{displayDecimals}";
		return actualNumber.ToString( formatString, new System.Globalization.CultureInfo( "nl-NL" ) );
	}

	/// <summary>
	/// Bepaalt het aantal decimale plaatsen in een getal
	/// </summary>
	private static int GetDecimalPlaces( decimal number )
	{
		// Verwijder voorloopnullen door te vermenigvuldigen met 10^decimalen
		number = Math.Abs( number );
		number -= Math.Floor( number );

		int decimals = 0;
		while ( number > 0 )
		{
			number *= 10;
			number -= Math.Floor( number );
			decimals++;

			// Voorkom oneindige lus of te veel decimalen
			if ( decimals > 10 )
			{
				break;
			}
		}

		return decimals;
	}
}
