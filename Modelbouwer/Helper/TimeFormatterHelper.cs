namespace Modelbouwer.Helper;
public static class TimeFormatterHelper
{
	public static string ConvertToTimeString( string totalHoursStr )
	{
		if ( !double.TryParse( totalHoursStr, out double totalHours ) )
		{
			return ( string ) System.Windows.Application.Current.Resources [ "Edit.Project.Tab.General.Group.Time.Error.InvalidTime" ];
		}
		double totalMinutes = totalHours * 60;

		int minutesInHour = 60;
		int hoursInDay = 24;
		int daysInWeek = 7;
		int daysInMonth = 30; // Gemiddelde maandlengte
		int daysInYear = 365;

		int years = (int) totalMinutes / (daysInYear * hoursInDay * minutesInHour);
		totalMinutes %= daysInYear * hoursInDay * minutesInHour;

		int months = (int) totalMinutes / (daysInMonth * hoursInDay * minutesInHour);
		totalMinutes %= daysInMonth * hoursInDay * minutesInHour;

		int weeks = (int) totalMinutes / (daysInWeek * hoursInDay * minutesInHour);
		totalMinutes %= daysInWeek * hoursInDay * minutesInHour;

		int days = (int) totalMinutes / (hoursInDay * minutesInHour);
		totalMinutes %= hoursInDay * minutesInHour;

		int hours = (int) totalMinutes / minutesInHour;
		int minutes = (int) totalMinutes % minutesInHour;

		string result = "";
		if ( years > 0 ) result += $"{years} {( years == 1 ? "jaar" : "jaren" )}, ";
		if ( months > 0 ) result += $"{months} {( months == 1 ? "maand" : "maanden" )}, ";
		if ( weeks > 0 ) result += $"{weeks} {( weeks == 1 ? "week" : "weken" )}, ";
		if ( days > 0 ) result += $"{days} {( days == 1 ? "dag" : "dagen" )}, ";
		if ( hours > 0 ) result += $"{hours} {( hours == 1 ? "uur" : "uren" )}, ";
		if ( minutes > 0 ) result += $"{minutes} {( minutes == 1 ? "minuut" : "minuten" )}";

		if ( result.EndsWith( ", " ) ) result = result.Substring( 0, result.Length - 2 );

		return result;
	}
}
