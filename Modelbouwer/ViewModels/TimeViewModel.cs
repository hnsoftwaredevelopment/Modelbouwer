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
	public int timeYear;

	[ObservableProperty]
	public int timeMonth;

	[ObservableProperty]
	public int timeWorkday;

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

	public ObservableCollection<TimeModel> ProjectTime { get; set; }

	public ObservableCollection<TimeModel> FilteredTimeEntries { get; private set; } = [ ];


	public ObservableCollection<TimeModel> Time
	{
		get => _time;
		set
		{
			if ( _time != value )
			{
				_time = value;
				OnPropertyChanged( nameof( Time ) );
			}
		}
	}
	private ObservableCollection<TimeModel>? _time;

	public void FilterTimeEntriesByProjectId( int projectId )
	{
		FilteredTimeEntries.Clear();
		foreach ( TimeModel contact in Time.Where( c => c.TimeProjectId == projectId ) )
		{
			FilteredTimeEntries.Add( contact );
		}

		//Select first time entry in the list if there are time entries
		if ( FilteredTimeEntries.Any() )
		{
			SelectedTimeEntry = FilteredTimeEntries.First();
		}
	}

	public TimeViewModel()
	{
		ProjectTime = new ObservableCollection<TimeModel>( DBCommands.GetTimeList() );
	}
}