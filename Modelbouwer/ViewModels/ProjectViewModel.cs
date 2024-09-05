using System.Collections.ObjectModel;

namespace Modelbouwer.ViewModels;
public partial class projectViewModel : ObservableObject
{
	[ObservableProperty]
	public int projectId;

	[ObservableProperty]
	public string? projectCode;

	[ObservableProperty]
	public string? projectName;

	[ObservableProperty]
	public string? projectStartDate;

	[ObservableProperty]
	public string? projectStartDateStr;

	[ObservableProperty]
	public string? projectEndDate;

	[ObservableProperty]
	public string? projectEndDateStr;

	[ObservableProperty]
	public string? projectExpectedTime;

	[ObservableProperty]
	public byte[]? projectImage;

	[ObservableProperty]
	public int projectImageRotationAngle;

	[ObservableProperty]
	public bool projectClosed;

	[ObservableProperty]
	public string? projectMemo;

	[ObservableProperty]
	public string? projectExpectedWorkdays;

	[ObservableProperty]
	public string? projectExpectedWorkdaysText;

	[ObservableProperty]
	public string? projectTodoTime;

	[ObservableProperty]
	public string? projectTodoWorkdays;

	[ObservableProperty]
	public string? projectTodoWorkdaysText;

	[ObservableProperty]
	public string? projectCreated;

	[ObservableProperty]
	public string? projectModified;

	[ObservableProperty]
	public string? projectSearchField;

	[ObservableProperty]
	public string? projectTotalTimeInHours;

	[ObservableProperty]
	public string? projectTotalTimeInText;

	[ObservableProperty]
	public string? projectShortestWorkday;

	[ObservableProperty]
	public string? projectShortestWorkdayHours;

	[ObservableProperty]
	public string? projectLongestWorkday;

	[ObservableProperty]
	public string? projectLongestWorkdayHours;

	[ObservableProperty]
	public string? projectAverageHoursPerDay;

	[ObservableProperty]
	public string? projectAverageHoursPerDayLong;

	[ObservableProperty]
	public string? projectBuildDays;

	public ObservableCollection<ProjectModel> Project
	{
		get => _project;
		set
		{
			if ( _project != value )
			{
				_project = value;
				OnPropertyChanged( nameof( Project ) );
			}
		}
	}
	private ObservableCollection<ProjectModel>? _project;
}
