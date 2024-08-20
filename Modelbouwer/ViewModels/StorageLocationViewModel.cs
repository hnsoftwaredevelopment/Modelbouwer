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
}
