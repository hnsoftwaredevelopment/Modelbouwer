namespace Modelbouwer.Converters;
public class ZeroValueVisibilityConverter : IValueConverter
{
	public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
	{
		if ( value == null )
		{
			return Visibility.Visible;
		}

		bool isZero = false;

		if ( value is int intValue )
		{
			isZero = intValue == 0;
		}
		else if ( value is double doubleValue )
		{
			isZero = Math.Abs( doubleValue ) < 0.0001;
		}
		else if ( value is decimal decimalValue )
		{
			isZero = decimalValue == 0;
		}
		else
		{
			isZero = value.ToString() == "0";
		}

		return isZero ? Visibility.Collapsed : Visibility.Visible;
	}

	public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
	{
		throw new NotImplementedException();
	}
}