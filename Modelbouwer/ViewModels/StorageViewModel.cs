namespace Modelbouwer.ViewModels;
public partial class StorageViewModel : ObservableObject
{
	[ObservableProperty]
	public string? storageName;

	[ObservableProperty]
	public int storageId;

	[ObservableProperty]
	public int storageParentId;

	[ObservableProperty]
	public bool _isStoragePopupOpen;

	public ObservableCollection<StorageModel> Storage { get; set; }

	public List<StorageModel> FlatStorage { get; set; }

	[ObservableProperty]
	private StorageModel? _selectedStorage;

	public StorageViewModel()
	{
		DBCommands dbCommands = new();
		Storage = new ObservableCollection<StorageModel>( dbCommands.GetStorageList() );
		FlatStorage = new List<StorageModel>( dbCommands.GetFlatStorageList() );

		SelectedStorage = Storage.FirstOrDefault( c => c.StorageId == 1 );
	}

	public void Refresh()
	{
		DBCommands dbCommands = new();
		Storage = [ .. dbCommands.GetStorageList() ];
		FlatStorage = [ .. dbCommands.GetFlatStorageList() ];
		OnPropertyChanged( nameof( Storage ) );
		OnPropertyChanged( nameof( FlatStorage ) );
	}
}
