namespace Modelbouwer.Model;
public class BrandModel
{
	public string? BrandName { get; set; }
	public int BrandId { get; set; }

	// Define the property that you want to use in TLists (for example in the errorList
	public string Name => BrandName;

	// Mapping dictionary for mapping Database Header to Property name
	public static readonly Dictionary<string, string> HeaderToPropertyMap = new()
	{
		{ DBNames.BrandFieldNameId, "BrandId" },
		{ DBNames.BrandFieldNameName, "BrandName" }
	};
}
