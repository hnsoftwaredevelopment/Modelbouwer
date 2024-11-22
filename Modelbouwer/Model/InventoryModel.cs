namespace Modelbouwer.Model;
public class InventoryModel
{
	public int ProductId { get; set; }
	public string? ProductCode { get; set; }
	public string? ProductName { get; set; }
	public double ProductPrice { get; set; }
	public double ProductMinimalStock { get; set; }
	public string? ProductCategory { get; set; }
	public string? ProductStorageLocation { get; set; }
	public double ProductInventory { get; set; }
	public double ProductInventoryValue { get; set; }
	public double ProductInOrder { get; set; }
	public double ProductVirtualInventory { get; set; }
	public double ProductVirtualInventoryValue { get; set; }
	public double ProductShortInventory { get; set; }
	public double ProductTempShortInventory { get; set; }

	// Define the property that you want to use in TLists (for example in the errorList
	public string Name => ProductName;

	// Mapping dictionary for mapping Database Header to Property name
	public static readonly Dictionary<string, string> HeaderToPropertyMap = new()
	{
		{ DBNames.ProductInventoryViewFieldNameProductId, "Product_Id" },
		{ DBNames.ProductInventoryViewFieldNameProductCode, "Code" },
		{ DBNames.ProductInventoryViewFieldNameProductName, "Name" },
		{ DBNames.ProductInventoryViewFieldNameProductPrice, "Price" },
		{ DBNames.ProductInventoryViewFieldNameProductMinimalStock, "MinimalStock" },
		{ DBNames.ProductInventoryViewFieldNameProductCategory, "Category" },
		{ DBNames.ProductInventoryViewFieldNameProductStorage, "Location" },
		{ DBNames.ProductInventoryViewFieldNameProductMinimalInventory, "Inventory" },
		{ DBNames.ProductInventoryViewFieldNameProductMinimalValue, "Value" },
		{ DBNames.ProductInventoryViewFieldNameProductMinimalOrder, "InOrder" },
		{ DBNames.ProductInventoryViewFieldNameProductMinimalVirtualInventory, "VirtualInventory" },
		{ DBNames.ProductInventoryViewFieldNameProductMinimalVirtualValue, "VirtualValue" },
		{ DBNames.ProductInventoryViewFieldNameProductShort, "Short" },
		{ DBNames.ProductInventoryViewFieldNameProductTempShort, "TempShort" }
	};


}
