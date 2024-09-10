namespace Modelbouwer.ViewModels;
public partial class StorageViewModel : ObservableObject
{
	[ObservableProperty]
	public string? storageName;

	[ObservableProperty]
	public int storageId;

	[ObservableProperty]
	public int storageParentId;

	public ObservableCollection<StorageModel> Storage { get; set; }

	[ObservableProperty]
	private StorageModel? _selectedStorage;

	private readonly ObservableCollection<StorageModel>? _storage;

	[ObservableProperty]
	public StorageModel? selectedItem;

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
		// Voeg het nieuwe, lege item toe aan de lijst
		StorageModel newStorage = new()
		{
			StorageId = 0,
			StorageParentId = 0,
			StorageName = string.Empty
		};

		Storage.Add( newStorage );
		SelectedStorage = newStorage;
		IsAddingNew = true;
	}

	public StorageViewModel()
	{
		DBCommands dbCommands = new();
		Storage = new ObservableCollection<StorageModel>( dbCommands.GetStorageList() );

		if ( Storage != null && Storage.Any() )
		{
			SelectedStorage = Storage.First();
		}
	}
}
