using static Modelbouwer.Helper.GeneralHelper;

namespace Modelbouwer.Converters;
public class MinutesToFormattedTimeConverter : IValueConverter
{
	public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
	{
		if ( value is decimal totalMinutes )
		{
			int hours = (int)(totalMinutes / 60);
			int minutes = (int)(totalMinutes % 60);

			string hourText = hours == 1 ? GetResourceString( "TimeFormatHour" ) : GetResourceString( "TimeFormatHours" );
			string minuteText = minutes == 1 ? GetResourceString( "TimeFormatMinute" ) : GetResourceString( "TimeFormatMinutes" );

			if ( hours == 0 )
			{
				// Geen uren, alleen minuten
				if ( minutes == 0 )
				{
					return GetResourceString( "TimeFormatNoTime" ); // Of een andere standaardwaarde voor 0 minuten
				}
				else
				{
					return $"{minutes} {minuteText}";
				}
			}
			else
			{
				// Er zijn uren
				if ( minutes == 0 )
				{
					return $"{hours} {hourText}";
				}
				else
				{
					return $"{hours} {hourText} en {minutes} {minuteText}";
				}
			}
		}

		return string.Empty;
	}

	public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
	{
		throw new NotImplementedException();
	}
}
