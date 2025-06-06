namespace Modelbouwer.Model;
public class ChartsWorkedHoursPerYearModel
{
	public int Year { get; set; }
	public decimal WorkedHours { get; set; }
	public decimal Percentage { get; set; }
	public string LabelWorkedPerYear => $"{Year}: {WorkedHours:N1} uur ({Percentage:N1}%)";
}
