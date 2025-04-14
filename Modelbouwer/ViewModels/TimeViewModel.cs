using System.ComponentModel;

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
	public DateTime dateTimeDate;

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

	//[ObservableProperty]
	//private bool isLoading;

	#region Selected Project
	public event EventHandler<int>? SelectedProjectChanged;

	private bool _isProjectSelected;
	public bool IsProjectSelected
	{
		get => _isProjectSelected;
		set
		{
			if ( _isProjectSelected != value )
			{
				_isProjectSelected = value;
				OnPropertyChanged( nameof( IsProjectSelected ) );
			}
		}
	}
	#endregion

	//public ObservableCollection<TimeModel> ProjectTime { get; set; }

	//public ObservableCollection<TimeModel> FilteredTimeEntries { get; private set; } = [ ];

	public bool HasFilteredTimeEntries => FilteredTimeEntries != null && FilteredTimeEntries.Any();

	#region Notify UI when list changes
	private void NotifyHasFilteredTimeEntries()
	{
		OnPropertyChanged( nameof( HasFilteredTimeEntries ) );
	}
	#endregion

	#region FilteredFrom Time Entries
	private ObservableCollection<TimeModel> _filteredTimeEntries = [];
	public ObservableCollection<TimeModel> FilteredTimeEntries
	{
		get => _filteredTimeEntries;
		set
		{
			if ( _filteredTimeEntries != value )
			{
				_filteredTimeEntries = value;
				OnPropertyChanged( nameof( FilteredTimeEntries ) );
				NotifyHasFilteredTimeEntries();
			}
		}
	}
	#endregion

	#region Selected Project
	private int _selectedProjectId;
	public int SelectedProject
	{
		get => _selectedProjectId;
		set
		{
			if ( _selectedProjectId != value )
			{
				_selectedProjectId = value;
				LoadTimeEntriesForSelectedProject( _selectedProjectId );
				SelectedProjectChanged?.Invoke( this, _selectedProjectId );
			}
			else
			{
				CommandManager.InvalidateRequerySuggested();
			}
		}
	}
	#endregion

	public void LoadTimeEntriesForSelectedProject( int projectId )
	{
		try
		{
			ObservableCollection<TimeModel> timeEntryLines = DBCommands.GetTimeList(projectId);

			FilteredTimeEntries = new ObservableCollection<TimeModel>( timeEntryLines );

			//ProjectTime = new ObservableCollection<TimeModel>( timeEntryLines );

			if ( FilteredTimeEntries.Any() )
			{
				SelectedTimeEntry = FilteredTimeEntries.First();
			}
		}
		catch ( Exception ex )
		{
			Console.WriteLine( $"Error loading time entries: {ex.Message}" );
			throw;
		}
	}

	private void TimeViewModel_PropertyChanged( object sender, PropertyChangedEventArgs e )
	{
		if ( e.PropertyName == nameof( FilteredTimeEntries ) )
		{
			NotifyHasFilteredTimeEntries();
		}
	}

	public TimeViewModel()
	{
		//ProjectTime = new ObservableCollection<TimeModel>();
		_filteredTimeEntries = new ObservableCollection<TimeModel>();

		// Zorg ervoor dat PropertyChanged events correct worden afgehandeld
		PropertyChanged += TimeViewModel_PropertyChanged;

		// Als er een initiële projectID is, laad dan timeentries
		if ( SelectedProject != 0 )
		{
			LoadTimeEntriesForSelectedProject( SelectedProject );
		}
	}

	public void Refresh()
	{
		//ProjectTime = new ObservableCollection<TimeModel>( DBCommands.GetTimeList() );
		//OnPropertyChanged( nameof( ProjectTime ) );
		OnPropertyChanged( nameof( FilteredTimeEntries ) );
	}
}