namespace Modelbouwer.Model;
public class ChartsWorkedHoursPerWorktypeModel
{
	public int WorktypeId { get; set; }
	public string? WorktypeName { get; set; }
	public decimal WorkedHours { get; set; }
	public decimal Percentage { get; set; }
}
