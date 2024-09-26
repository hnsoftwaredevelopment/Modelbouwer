namespace Modelbouwer.Model;

public class StorageModel : INameable
{
	public int StorageId { get; set; }
	public int? StorageParentId { get; set; }
	public string? StorageName { get; set; }
	public ObservableCollection<StorageModel> SubStorage { get; set; } = [ ];
	public string Name => StorageName;

	// Mapping dictionary for mapping Database Header to Property name
	public static readonly Dictionary<string, string> HeaderToPropertyMap = new()
	{
		{ DBNames.StorageFieldNameId, "StorageId" },
		{ DBNames.StorageFieldNameParentId, "StorageParentId" },
		{ DBNames.StorageFieldNameName, "StorageName" }
	};
}