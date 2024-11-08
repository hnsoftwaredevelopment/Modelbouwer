namespace Modelbouwer.Converters;
public class TotalTimeConverter : IValueConverter
{
	public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
	{
		if ( value is string totalHours )
		{
			return TimeFormatterHelper.ConvertToTimeString( totalHours );
		}
		return string.Empty;
	}

	public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
	{
		throw new NotSupportedException( "ConvertBack is not supported." );
	}
}
