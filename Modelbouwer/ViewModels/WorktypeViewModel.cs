namespace Modelbouwer.ViewModels;
public partial class WorktypeViewModel : ObservableObject
{
	[ObservableProperty]
	public string? worktypeName;

	[ObservableProperty]
	public int worktypeId;

	[ObservableProperty]
	public int worktypeParentId;

	public ObservableCollection<WorktypeModel>? Worktype { get; set; }

	[ObservableProperty]
	private WorktypeModel? _selectedWorktype;

	[ObservableProperty]
	public WorktypeModel? selectedItem;

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

	public WorktypeViewModel()
	{
		DBCommands dbCommands = new();
		Worktype = new ObservableCollection<WorktypeModel>( dbCommands.GetWorktypeList() );

		if ( Worktype != null && Worktype.Any() )
		{
			SelectedWorktype = Worktype.First();
		}
	}

}
