namespace Modelbouwer.Model;
public class TimeModel
{
	public int TimeId { get; set; }
	public int TimeProjectId { get; set; }
	public int TimeWorktypeId { get; set; }
	public DateOnly TimeWorkDate { get; set; }
	public TimeOnly TimeStartTime { get; set; }
	public TimeOnly TimeEndTime { get; set; }
	public string? TimeComment { get; set; }
}
