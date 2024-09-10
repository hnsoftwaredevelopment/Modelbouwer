namespace Modelbouwer.Model;

public class StorageModel : INameable
{
	public int StorageId { get; set; }
	public int? StorageParentId { get; set; }
	public string? StorageName { get; set; }

	public ObservableCollection<StorageModel> SubStorage { get; set; } = [ ];

	// Define the property that you want to use in TLists (for example in the errorList
	public string Name => StorageName;

	// Mapping dictionary for mapping Database Header to Property name
	public static readonly Dictionary<string, string> HeaderToPropertyMap = new()
	{
		{ DBNames.StorageFieldNameId, "StorageId" },
		{ DBNames.StorageFieldNameParentId, "StorageParentId" },
		{ DBNames.StorageFieldNameName, "StorageName" }
	};
}