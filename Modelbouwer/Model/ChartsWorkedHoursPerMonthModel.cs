namespace Modelbouwer.Model;
public class ChartsWorkedHoursPerMonthModel
{
	public int Month { get; set; }
	public string? MonthNameShort => CultureInfo.GetCultureInfo( "nl-NL" ).DateTimeFormat.GetAbbreviatedMonthName( Month );
	public string? MonthNameLong => CultureInfo.GetCultureInfo( "nl-NL" ).DateTimeFormat.GetMonthName( Month );
	public decimal WorkedHours { get; set; }
	public decimal Percentage { get; set; }
	public string LabelWorkedPerMonth => $"{MonthNameLong}: {WorkedHours:N1} uur ({Percentage:N1}%)";
}
