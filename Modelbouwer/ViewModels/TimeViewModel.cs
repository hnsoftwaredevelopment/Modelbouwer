namespace Modelbouwer.ViewModels;
public partial class TimeViewModel : ObservableObject
{
	[ObservableProperty]
	public int timeId;

	[ObservableProperty]
	public int timeProjectId;

	[ObservableProperty]
	public int timeWorktypeId;

	[ObservableProperty]
	public string? timeWorkDate;

	[ObservableProperty]
	public string? timeStartTime;

	[ObservableProperty]
	public string? timeEndTime;

	[ObservableProperty]
	public string? timeComment;

	[ObservableProperty]
	public double timeElapsedMinutes;

	[ObservableProperty]
	public string? timeElapsedTime;

	[ObservableProperty]
	public TimeSpan? timeWorkedHours;

	[ObservableProperty]
	public int timeYear;

	[ObservableProperty]
	public int timeMonth;

	[ObservableProperty]
	public int timeWorkday;

	[ObservableProperty]
	public string timeWorkdayName;

	[ObservableProperty]
	public string? timeYearMonth;

	[ObservableProperty]
	public string? timeYearWorkday;

	[ObservableProperty]
	public string? timeSortIndex;

	[ObservableProperty]
	public string? timeProjectName;

	[ObservableProperty]
	public string? timeWorktypeName;

	[ObservableProperty]
	private TimeModel? _selectedTimeEntry;

	private ProjectModel? _selectedProject;

	public ObservableCollection<TimeModel> ProjectTime { get; set; }

	public ObservableCollection<TimeModel> FilteredTimeEntries { get; private set; } = [ ];

	public bool HasFilteredTimeEntries => FilteredTimeEntries != null && FilteredTimeEntries.Any();

	#region Notify UI when list changes
	private void NotifyHasFilteredTimeEntries()
	{
		OnPropertyChanged( nameof( HasFilteredTimeEntries ) );
	}
	#endregion

	public void FilterTimeEntriesByProjectId( int projectId )
	{
		FilteredTimeEntries.Clear();
		foreach ( TimeModel timeEntry in ProjectTime.Where( c => c.TimeProjectId == projectId ) )
		{
			FilteredTimeEntries.Add( timeEntry );
		}

		//Select first time entry in the list if there are time entries
		if ( FilteredTimeEntries.Any() )
		{
			SelectedTimeEntry = FilteredTimeEntries.First();
		}

		// Force DataGrid to recognize changes
		OnPropertyChanged( nameof( FilteredTimeEntries ) );
		NotifyHasFilteredTimeEntries();
	}

	public TimeViewModel()
	{
		ProjectTime = new ObservableCollection<TimeModel>( DBCommands.GetTimeList() );
	}
}