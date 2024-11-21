namespace Modelbouwer.Converters;
public class MinutesToTimeConverter : IValueConverter
{
	public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
	{
		if ( value is double totalMinutes )
		{
			int hours = (int)(totalMinutes / 60);
			int minutes = (int)(totalMinutes % 60);
			return $"{hours:D2}:{minutes:D2}";
		}
		return "00:00";
	}

	public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
	{
		throw new NotImplementedException();
	}
}
