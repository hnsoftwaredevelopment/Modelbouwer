namespace Modelbouwer.ViewModels;
public partial class WorktypeViewModel : ObservableObject
{
	[ObservableProperty]
	public string? worktypeName;

	[ObservableProperty]
	public int worktypeId;

	[ObservableProperty]
	public int worktypeParentId;

	[ObservableProperty]
	public bool isWorktypePopupOpen;


	public ObservableCollection<WorktypeModel>? Worktype { get; set; }

	private WorktypeModel? _selectedWorktype;

	[ObservableProperty]
	public WorktypeModel? selectedItem;

	public List<WorktypeModel> FlatWorktype { get; set; }

	private bool _isAddingNew;

	public bool IsAddingNew
	{
		get => _isAddingNew;
		set
		{
			if ( _isAddingNew != value )
			{
				_isAddingNew = value;
				OnPropertyChanged( nameof( IsAddingNew ) );
			}
		}
	}

	public void AddNewItem()
	{
		WorktypeModel newWorktype = new()
		{
			WorktypeId = 0,
			WorktypeParentId = 0,
			WorktypeName = string.Empty
		};

		Worktype.Add( newWorktype );
		SelectedWorktype = newWorktype;
		IsAddingNew = true;
	}

	#region for the popup view in the TimeManagement datagrid
	private TimeModel _currentTimeEntry;
	public TimeModel CurrentTimeEntry
	{
		get => _currentTimeEntry;
		set => SetProperty( ref _currentTimeEntry, value );
	}

	public WorktypeModel SelectedWorktype
	{
		get => _selectedWorktype;
		set
		{
			if ( SetProperty( ref _selectedWorktype, value ) && value != null && CurrentTimeEntry != null )
			{
				// Update de huidige timeEntry met het nieuwe werktype
				CurrentTimeEntry.TimeWorktypeId = value.WorktypeId;
				CurrentTimeEntry.TimeWorktypeName = value.WorktypeName;

				// Sluit de popup
				IsWorktypePopupOpen = false;
			}
		}
	}
	#endregion

	public WorktypeViewModel()
	{
		Refresh();
	}

	public void Refresh()
	{
		DBCommands dbCommands = new();
		Worktype = [ .. dbCommands.GetWorktypeList() ];
		FlatWorktype = dbCommands.GetFlatWorktypeList();

		if ( Worktype != null && Worktype.Any() )
		{
			SelectedWorktype = Worktype.First();
		}
		OnPropertyChanged( nameof( Worktype ) );
	}

}
