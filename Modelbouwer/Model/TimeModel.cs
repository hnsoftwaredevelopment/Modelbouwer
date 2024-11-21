namespace Modelbouwer.Model;

public class TimeModel
{
	public int TimeId { get; set; }
	public int TimeProjectId { get; set; }
	public string? TimeProjectName { get; set; }
	public int TimeWorktypeId { get; set; }
	public string? TimeWorktypeName { get; set; }
	public string? TimeWorkDate { get; set; }
	public string? TimeStartTime { get; set; }
	public string? TimeEndTime { get; set; }
	public double TimeElapsedMinutes { get; set; }
	public string? TimeElapsedTime { get; set; }
	public TimeSpan? TimeWorkedHours { get; set; }
	public string? TimeComment { get; set; }
	public int TimeYear { get; set; }
	public int TimeMonth { get; set; }
	public int TimeWorkday { get; set; }
	public string? TimeYearMonth { get; set; }
	public string? TimeYearWorkday { get; set; }
	public string? TimeSortIndex { get; set; }
	public string? TimeWorkdayName { get; set; }

	// Mapping dictionary for mapping Database Header to Property name
	public static readonly Dictionary<string, string> HeaderToPropertyMap = new()
	{
		{ DBNames.TimeViewFieldNameId, "TimeViewFieldNameId" },
		{ DBNames.TimeViewFieldNameProjectId, "TimeViewFieldNameProjectId" },
		{ DBNames.TimeViewFieldNameProjectName, "TimeViewFieldNameProjectName" },
		{ DBNames.TimeViewFieldNameWorktypeId, "TimeViewFieldNameWorktypeId" },
		{ DBNames.TimeViewFieldNameWorktypeName, "TimeViewFieldNameWorktypeName" },
		{ DBNames.TimeViewFieldNameWorkDate, "TimeViewFieldNameWorkDate" },
		{ DBNames.TimeViewFieldNameStartTime, "TimeViewFieldNameStartTime" },
		{ DBNames.TimeViewFieldNameEndTime, "TimeViewFieldNameEndTime" },
		{ DBNames.TimeViewFieldNameElapsedTime, "TimeViewFieldNameElapsedTime" },
		{ DBNames.TimeViewFieldNameElapsedMinutes, "TimeViewFieldNameElapsedMinutes" },
		{ DBNames.TimeViewFieldNameComment, "TimeViewFieldNameComment" },
		{ DBNames.TimeViewFieldNameYear, "TimeViewFieldNameYear" },
		{ DBNames.TimeViewFieldNameMonth, "TimeViewFieldNameMonth" },
		{ DBNames.TimeViewFieldNameWorkday, "TimeViewFieldNameWorkday" },
		{ DBNames.TimeViewFieldNameYearMonth, "TimeViewFieldNameYearMonth" },
		{ DBNames.TimeViewFieldNameYearWorkday, "TimeViewFieldNameYearWorkday" },
		{ DBNames.TimeViewFieldNameSortIndex, "TimeViewFieldNameSortIndex" }
	};
}