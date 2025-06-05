namespace Modelbouwer.Converters;
public class ZeroToEmptyConverter : IValueConverter
{
	public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
	{
		if ( value is decimal d )
		{
			return d != 0 ? d.ToString( "N2", culture ) : string.Empty;
		}

		return string.Empty;
	}

	public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
	{
		return System.Windows.Data.Binding.DoNothing;
	}
}