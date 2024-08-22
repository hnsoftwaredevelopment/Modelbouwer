namespace Modelbouwer.Models;

public class ProjectModel : ObservableObject
{
	public int ProjectId { get; set; }
	public string? ProjectCode { get; set; }
	public string? ProjectName { get; set; }
	public string? ProjectStartDate { get; set; }
	public string? ProjectStartDateStr { get; set; }
	public string? ProjectEndDate { get; set; }
	public string? ProjectEndDateStr { get; set; }
	public string? ProjectExpectedTime { get; set; }
	public byte[]? ProjectImage { get; set; }
	public int ProjectImageRotationAngle { get; set; }
	public bool ProjectClosed { get; set; }
	public string? ProjectMemo { get; set; }
	public string? ProjectExpectedWorkdays { get; set; }
	public string? ProjectExpectedWorkdaysText { get; set; }
	public string? ProjectTodoTime { get; set; }
	public string? ProjectTodoWorkdays { get; set; }
	public string? ProjectTodoWorkdaysText { get; set; }
	public string? ProjectCreated { get; set; }
	public string? ProjectModified { get; set; }
	public string? ProjectSearchField { get; set; }
	public string? ProjectTotalTimeInHours { get; set; }
	public string? ProjectTotalTimeInText { get; set; }
	public string? ProjectShortestWorkday { get; set; }
	public string? ProjectShortestWorkdayHours { get; set; }
	public string? ProjectLongestWorkday { get; set; }
	public string? ProjectLongestWorkdayHours { get; set; }
	public string? ProjectAverageHoursPerDay { get; set; }
	public string? ProjectAverageHoursPerDayLong { get; set; }
	public string? ProjectBuildDays { get; set; }
}
