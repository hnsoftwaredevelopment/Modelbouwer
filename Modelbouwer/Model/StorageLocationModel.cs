namespace Modelbouwer.Model;

public class StorageLocationModel : INameable
{
	public string? StorageLocationName { get; set; }
	public string? StorageLocationFullpath { get; set; }
	public int StorageLocationId { get; set; }
	public int StorageLocationParentId { get; set; }

	// Define the property that you want to use in TLists (for example in the errorList
	public string Name => StorageLocationName;

	// Mapping dictionary for mapping Database Header to Property name
	public static readonly Dictionary<string, string> HeaderToPropertyMap = new()
{
	{ DBNames.StorageFieldNameId, "StorageLocationId" },
	{ DBNames.StorageFieldNameParentId, "StorageLocationParentId" },
	{ DBNames.StorageFieldNameName, "StorageLocationName" },
	{ DBNames.StorageFieldNameFullpath, "StorageLocationFullpath" }
};
}