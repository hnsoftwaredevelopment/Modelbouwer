namespace Modelbouwer.Model;
public class ChartsWorkedHoursPerDayModel
{
	public int Weekday { get; set; }
	public string? DayNameShort => CultureInfo.GetCultureInfo( "nl-NL" ).DateTimeFormat.GetAbbreviatedDayName( ConvertToSystemDayOfWeek( Weekday ) );
	public string? DayNameLong => CultureInfo.GetCultureInfo( "nl-NL" ).DateTimeFormat.GetDayName( ConvertToSystemDayOfWeek( Weekday ) );
	public decimal WorkedHours { get; set; }
	public decimal Percentage { get; set; }
	public string LabelWorkedPerDay => $"{DayNameLong}: {WorkedHours:N1} uur ({Percentage:N1}%)";

	private DayOfWeek ConvertToSystemDayOfWeek( int europeanWeekday )
	{
		return ( DayOfWeek ) ( ( europeanWeekday + 1 ) % 7 );
	}
}
