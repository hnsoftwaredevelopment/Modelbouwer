using static Modelbouwer.Helper.GeneralHelper;

namespace Modelbouwer.Converters;
public class MinutesToExtendedTimeConverter : IMultiValueConverter
{
	// Default number of workinghours per day
	private const int DefaultWorkHoursPerDay  = 8;

	public object Convert( object [ ] values, Type targetType, object parameter, CultureInfo culture )
	{
		// Check if there are any time values
		if ( values.Length < 2 || values [ 0 ] == null || !( values [ 0 ] is decimal ) )
		{
			return GetResourceString( "TimeFormatNoTime" );
		}

		//Get the total time in minutes
		decimal totalMinutes = (decimal)values[0];

		//Get the average number of working hours per day
		int workHoursPerDay = DefaultWorkHoursPerDay;
		if ( values.Length > 1 && values [ 1 ] != null )
		{
			// Probeer het aantal uren per dag uit de tweede waarde te halen
			if ( values [ 1 ] is int intValue )
			{
				workHoursPerDay = intValue;
			}
			else if ( values [ 1 ] is decimal decimalValue )
			{
				workHoursPerDay = ( int ) decimalValue;
			}
			else if ( values [ 1 ] is double doubleValue )
			{
				workHoursPerDay = ( int ) doubleValue;
			}
		}

		//Calculate the minutes per workingday based on the average number of working hours
		int minutesPerWorkDay = workHoursPerDay * 60;

		// Convert minutes to all timeunits
		int minutes = (int)(totalMinutes % 60);
		int hours = (int)(totalMinutes / 60 %  workHoursPerDay);
		int days = (int)(totalMinutes / minutesPerWorkDay % 7);
		int weeks = (int)(totalMinutes / (minutesPerWorkDay  * 7)) % 4;
		int months = (int)(totalMinutes / (minutesPerWorkDay * 7 * 4)) % 12;
		int years = (int)(totalMinutes / (minutesPerWorkDay * 7 * 4 * 12));

		// Make a list of all not-zero timeunitsn
		List<string> timeUnits = new();

		if ( years > 0 )
		{
			timeUnits.Add( $"{years} {( years == 1 ? GetResourceString( "TimeFormatYear" ) : GetResourceString( "TimeFormatYears" ) )}" );
		}

		if ( months > 0 )
		{
			timeUnits.Add( $"{months} {( months == 1 ? GetResourceString( "TimeFormatMonth" ) : GetResourceString( "TimeFormatMonths" ) )}" );
		}

		if ( weeks > 0 )
		{
			timeUnits.Add( $"{weeks} {( weeks == 1 ? GetResourceString( "TimeFormatWeek" ) : GetResourceString( "TimeFormatWeeks" ) )}" );
		}

		if ( days > 0 )
		{
			timeUnits.Add( $"{days} {( days == 1 ? GetResourceString( "TimeFormatDay" ) : GetResourceString( "TimeFormatDays" ) )}" );
		}

		if ( hours > 0 )
		{
			timeUnits.Add( $"{hours} {( hours == 1 ? GetResourceString( "TimeFormatHour" ) : GetResourceString( "TimeFormatHours" ) )}" );
		}

		if ( minutes > 0 )
		{
			timeUnits.Add( $"{minutes} {( minutes == 1 ? GetResourceString( "TimeFormatMinute" ) : GetResourceString( "TimeFormatMinutes" ) )}" );
		}

		// When there is no timeunit display there is no time available
		if ( timeUnits.Count == 0 )
		{
			return GetResourceString( "TimeFormatNoTime" );
		}

		// if there is only one timeunit, show that one
		if ( timeUnits.Count == 1 )
		{
			return timeUnits [ 0 ];
		}

		// if there are two timeunits connect them with " and "
		if ( timeUnits.Count == 2 )
		{
			return $"{timeUnits [ 0 ]} {GetResourceString( "TimeFormatAnd" )} {timeUnits [ 1 ]}";
		}


		//When more there are more then two timeunits available connect the last two with " and ", the others with ", "
		StringBuilder result = new();

		for ( int i = 0; i < timeUnits.Count - 1; i++ )
		{
			result.Append( timeUnits [ i ] );

			if ( i < timeUnits.Count - 2 )
			{
				result.Append( ", " );
			}
			else
			{
				result.Append( " en " );
			}
		}

		result.Append( timeUnits [ timeUnits.Count - 1 ] );

		return result.ToString();
	}

	public object [ ] ConvertBack( object value, Type [ ] targetTypes, object parameter, CultureInfo culture )
	{
		throw new NotImplementedException();
	}

	// Behoud de originele IValueConverter-implementatie voor backwards compatibility
	public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
	{
		return Convert( new [ ] { value }, targetType, parameter, culture );
	}

	public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
	{
		throw new NotImplementedException();
	}
}
