namespace Modelbouwer.Model;

public class SupplierModel
{
	public int SupplierId { get; set; }
	public string? SupplierCode { get; set; }
	public string? SupplierName { get; set; }
	public string? SupplierAddress1 { get; set; }
	public string? SupplierAddress2 { get; set; }
	public string? SupplierZip { get; set; }
	public string? SupplierCity { get; set; }
	public string? SupplierUrl { get; set; }
	public double SupplierShippingCosts { get; set; }
	public double SupplierMinShippingCosts { get; set; }
	public double SupplierOrderCosts { get; set; }
	public string? SupplierMemo { get; set; }
	public int SupplierCurrencyId { get; set; }
	public string? SupplierCurrency { get; set; }
	public double SupplierCurrencyRate { get; set; }
	public int SupplierCountryId { get; set; }
	public string? SupplierCountry { get; set; }

	// Define the property that you want to use in TLists (for example in the errorList
	public string Name => SupplierName;

	// Mapping dictionary for mapping Database Header to Property name
	public static readonly Dictionary<string, string> HeaderToPropertyMap = new()
	{
		{ DBNames.SupplierFieldNameId, "SupplierId" },
		{ DBNames.SupplierFieldNameCurrencyId, "SupplierCurrencyId" },
		{ DBNames.SupplierFieldNameCountryId, "SupplierCountryId" },
		{ DBNames.SupplierFieldNameCode, "SupplierCode" },
		{ DBNames.SupplierFieldNameName, "SupplierName" },
		{ DBNames.SupplierFieldNameAddress1, "SupplierAddress1" },
		{ DBNames.SupplierFieldNameAddress2, "SupplierAddress2" },
		{ DBNames.SupplierFieldNameZip, "SupplierZip" },
		{ DBNames.SupplierFieldNameCity, "SupplierCity" },
		{ DBNames.SupplierFieldNameUrl, "SupplierUrl" },
		{ DBNames.SupplierFieldNameMemo, "SupplierMemo" },
		{ DBNames.SupplierFieldNameShippingCosts, "SupplierShippingCosts" },
		{ DBNames.SupplierFieldNameMinShippingCosts, "SupplierMinShippingCosts" },
		{ DBNames.SupplierFieldNameOrderCosts, "SupplierOrderCosts" },
	};
}