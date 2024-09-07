namespace Modelbouwer.Model;

public class CurrencyModel
{
	public double CurrencyConversionRate { get; set; }
	public int CurrencyId { get; set; }
	public string? CurrencyCode { get; set; }
	public string? CurrencyName { get; set; }
	public string? CurrencySymbol { get; set; }

	// Define the property that you want to use in TLists (for example in the errorList
	public string Name => CurrencyName;

	// Mapping dictionary for mapping Database Header to Property name
	public static readonly Dictionary<string, string> HeaderToPropertyMap = new()
	{
		{ DBNames.CurrencyFieldNameRate, "CurrencyConversionRate" },
		{ DBNames.CurrencyFieldNameId, "CurrencyId" },
		{ DBNames.CurrencyFieldNameCode, "CurrencyCode" },
		{ DBNames.CurrencyFieldNameName, "CurrencyName" },
		{ DBNames.CurrencyFieldNameSymbol, "CurrencySymbol" }
	};
}