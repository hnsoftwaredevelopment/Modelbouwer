namespace Modelbouwer.Model;
public class UnitModel
{
	public int UnitId { get; set; }
	public string? UnitName { get; set; }

	// Define the property that you want to use in TLists (for example in the errorList
	public string Name => UnitName;

	// Mapping dictionary for mapping Database Header to Property name
	public static readonly Dictionary<string, string> HeaderToPropertyMap = new()
	{
		{ DBNames.UnitFieldNameUnitId, "UnitId" },
		{ DBNames.UnitFieldNameUnitName, "UnitName" }
	};
}
