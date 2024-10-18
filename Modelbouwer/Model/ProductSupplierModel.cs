﻿namespace Modelbouwer.Model;

public class ProductSupplierModel
{
	public int ProductSupplierId { get; set; }
	public int ProductSupplierProductId { get; set; }
	public int ProductSupplierSupplierId { get; set; }
	public int ProductSupplierCurrencyId { get; set; }
	public string? ProductSupplierName { get; set; }
	public string? ProductSupplierProductNumber { get; set; }
	public string? ProductSupplierProductName { get; set; }
	public double ProductSupplierPrice { get; set; }
	public string? ProductSupplierURL { get; set; }
	public string? ProductSupplierDefaultSupplier { get; set; }
	public bool? ProductSupplierDefaultSupplierCheck { get; set; }
	public string? ProductSupplierSupplierName { get; set; }
	public string? ProductSupplierCurrencySymbol { get; set; }


	// Define the property that you want to use in TLists (for example in the errorList
	public string Name => ProductSupplierProductName;

	// Mapping dictionary for mapping Database Header to Property name
	public static readonly Dictionary<string, string> HeaderToPropertyMap = new()
	{
		{ DBNames.ProductSupplierFieldNameId, "ProductSupplierId" },
		{ DBNames.ProductSupplierFieldNameProductId, "ProductSupplierProductId" },
		{ DBNames.ProductSupplierFieldNameSupplierId, "ProductSupplierSupplierId" },
		{ DBNames.ProductSupplierFieldNameCurrencyId, "CurrencyId" },
		{ DBNames.ProductSupplierFieldNameProductNumber, "ProductSupplierProductNumber" },
		{ DBNames.ProductSupplierFieldNameProductName, "ProductSupplierProductName" },
		{ DBNames.ProductSupplierFieldNameSupplierName, "ProductSupplierSupplierName" },
		{ DBNames.ProductSupplierFieldNamePrice, "ProductSupplierPrice" },
		{ DBNames.ProductSupplierFieldNameProductUrl, "ProductSupplierURL" },
		{ DBNames.ProductSupplierFieldNameDefaultSupplier, "ProductSupplierDefaultSupplier" }
	};
}