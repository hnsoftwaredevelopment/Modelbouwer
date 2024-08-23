namespace Modelbouwer.Model;
public class CountryModel
{
	public int CountryDefaultCurrencyId { get; set; }
	public int CountryId { get; set; }
	public string? CountryCode { get; set; }
	public string? CountryDefaultCurrencySymbol { get; set; }
	public string? CountryName { get; set; }

	// Define the property that you want to use in TLists (for example in the errorList
	public string Name => CountryName;

	// Mapping dictionary for mapping Database Header to Property name
	public static readonly Dictionary<string, string> HeaderToPropertyMap = new()
	{
		{ DBNames.CountryFieldNameDefCurrencyId, "CountryDefaultCurrencyId" },
		{ DBNames.CountryFieldNameId, "CountryId" },
		{ DBNames.CountryFieldNameCode, "CountryCode" },
		{ DBNames.CountryFieldNameDefCurrencySymbol, "CountryDefaultCurrencySymbol" },
		{ DBNames.CountryFieldNameName, "CountryName" }
	};

}
