namespace Modelbouwer.Converters;
public class EmptyCellConverter : IMultiValueConverter
{
	public object Convert( object [ ] values, Type targetType, object parameter, CultureInfo culture )
	{
		if ( values.Length < 2 || values [ 0 ] == DependencyProperty.UnsetValue )
		{
			return string.Empty;
		}

		int isOrderLine = (int)values[1];
		object value = values[0];

		if ( isOrderLine == 0 )
		{
			if ( value is decimal decimalValue ) { return decimalValue.ToString( culture ); }
			else if ( value is int intValue ) { return intValue.ToString( culture ); }
			else if ( value is double doubleValue ) { return doubleValue.ToString( culture ); }
			return values [ 0 ];
		}

		return string.Empty;
	}

	public object [ ] ConvertBack( object value, Type [ ] targetTypes, object parameter, CultureInfo culture )
	{
		throw new NotImplementedException();
	}
}