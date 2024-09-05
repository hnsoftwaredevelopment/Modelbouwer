using System.Collections.ObjectModel;

namespace Modelbouwer.ViewModels;
public partial class StorageLocationViewModel : ObservableObject
{
	[ObservableProperty]
	public string? storagelocationName;

	[ObservableProperty]
	public string? storagelocationFullpath;

	[ObservableProperty]
	public int storagelocationId;

	[ObservableProperty]
	public int storagelocationParentId;

	public ObservableCollection<StorageLocationModel> StorageLocation
	{
		get => _storagelocation;
		set
		{
			if ( _storagelocation != value )
			{
				_storagelocation = value;
				OnPropertyChanged( nameof( StorageLocation ) );
			}
		}
	}
	private ObservableCollection<StorageLocationModel>? _storagelocation;
}
