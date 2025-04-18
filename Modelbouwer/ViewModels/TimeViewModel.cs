using System.ComponentModel;

using CommunityToolkit.Mvvm.Input;

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

	#region HasChanges
	[ObservableProperty]
	private bool _hasChanges;

	private void UpdateHasChanges()
	{
		HasChanges = FilteredTimeEntries?.Any( e => e.State != TimeModel.RecordState.Unchanged ) ?? false;
	}

	private void SetupChangeTracking()
	{
		FilteredTimeEntries.CollectionChanged += ( s, e ) => UpdateHasChanges();

		// Event handler voor PropertyChanged op individuele items
		PropertyChangedEventHandler handler = (s, e) =>
		{
			if (e.PropertyName == nameof(TimeModel.State))
			{
				UpdateHasChanges();
			}
		};

		foreach ( TimeModel item in FilteredTimeEntries )
		{
			item.PropertyChanged += handler;
		}

		// Voor nieuwe items die worden toegevoegd
		FilteredTimeEntries.CollectionChanged += ( s, e ) =>
		{
			if ( e.NewItems != null )
			{
				foreach ( TimeModel item in e.NewItems )
				{
					item.PropertyChanged += handler;
				}
			}
		};
	}

	private void Item_PropertyChanged( object sender, PropertyChangedEventArgs e )
	{
		if ( e.PropertyName == nameof( TimeModel.State ) )
		{
			UpdateHasChanges();
		}
	}
	#endregion

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

	#region Toolbar commands
	public ICommand AddNewRowCommand { get; }
	public ICommand SaveCommand { get; }

	#region Add new row to the TimeEntries
	private void ExectuteAddNewRow()
	{
		TimeModel newEntry = new()
		{
			DateTimeDate = DateTime.Today,
			DateTimeStart = DateTime.Now.AddHours(-1),
			DateTimeEnd = DateTime.Now,
			State = TimeModel.RecordState.Added
		};

		FilteredTimeEntries.Insert( 0, newEntry );

		UpdateHasChanges();

		SelectedTimeEntry = newEntry;
	}
	#endregion

	private void ExecuteSave()
	{
		//Execuyte the save command
		List<TimeModel> added = FilteredTimeEntries.Where(e => e.State == TimeModel.RecordState.Added).ToList();
		List<TimeModel> modified = FilteredTimeEntries.Where(e => e.State == TimeModel.RecordState.Modified).ToList();

		//Add new records to the TimeTable
		foreach ( TimeModel? entry in added )
		{
			// Voeg code toe om nieuwe records in te voegen
			// Table: Time
			// Fields: project_Id, worktype_Id, Workdate (yyyy-mm-dd), StartTime (hh:mm:ss), EndTime (hh:mm:ss), Comment

			// set state to Unchanged when record has been stored
			entry.State = TimeModel.RecordState.Unchanged;
		}

		//Change existing records in TimeTable
		foreach ( TimeModel entry in modified )
		{
			// Voeg code toe om bestaande records bij te werken
			// set state to Unchanged when record has been stored
			entry.State = TimeModel.RecordState.Unchanged;
		}

		UpdateHasChanges();
	}
	#endregion

	public void LoadTimeEntriesForSelectedProject( int projectId )
	{
		try
		{
			ObservableCollection<TimeModel> TimeModelLines = DBCommands.GetTimeList(projectId);


			// Set all entrie on Status: Unchanged by default
			foreach ( TimeModel entry in TimeModelLines )
			{
				entry.State = TimeModel.RecordState.Unchanged;
			}

			FilteredTimeEntries = new ObservableCollection<TimeModel>( TimeModelLines );

			// Setup change tracking for new items
			foreach ( TimeModel item in FilteredTimeEntries )
			{
				item.PropertyChanged += Item_PropertyChanged;
			}

			UpdateHasChanges();

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
		_filteredTimeEntries = new ObservableCollection<TimeModel>();

		PropertyChanged += TimeViewModel_PropertyChanged;

		SetupChangeTracking();

		// When there is an initial projectID, load the timeentries
		if ( SelectedProject != 0 )
		{
			LoadTimeEntriesForSelectedProject( SelectedProject );
		}

		AddNewRowCommand = new RelayCommand( ExectuteAddNewRow );
		SaveCommand = new RelayCommand( ExecuteSave );

		//FilteredTimeEntries.CollectionChanged += ( sender, e ) => UpdateCanExecuteSave();
	}

	public void Refresh()
	{
		if ( SelectedProject != 0 )
		{
			LoadTimeEntriesForSelectedProject( SelectedProject );
		}
		OnPropertyChanged( nameof( FilteredTimeEntries ) );
	}
}