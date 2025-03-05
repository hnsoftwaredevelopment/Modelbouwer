namespace Modelbouwer.Converters;
public class DynamicPriceConverter : IValueConverter
{
	public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
	{
		if ( value is decimal price )
		{
			// Remove trailing zero's but keep a minumum of 2 decimals
			string formattedPrice = price.ToString("0.##", new CultureInfo("nl-NL"));

			// Show at least 2 decimals
			if ( !formattedPrice.Contains( "," ) )
			{
				formattedPrice += ",00";
			}
			else if ( formattedPrice.Split( ',' ) [ 1 ].Length == 1 )
			{
				formattedPrice += "0";
			}

			return formattedPrice;
		}
		return value;
	}

	public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
	{
		if ( value is string priceString )
		{
			if ( decimal.TryParse( priceString.Replace( ',', '.' ), NumberStyles.Number, new CultureInfo( "nl-NL" ), out decimal price ) )
			{
				return price;
			}
		}
		return value;
	}
}
