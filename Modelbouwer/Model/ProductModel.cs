namespace Modelbouwer.Model;
public class ProductModel
{
	public int ProductId { get; set; }
	public string? ProductCode { get; set; }
	public string? ProductName { get; set; }
	public string? ProductDimensions { get; set; }
	public double ProductPrice { get; set; }
	public double ProductMinimalStock { get; set; }
	public double ProductStandardQuantity { get; set; }
	public int ProductCosts { get; set; }
	public int ProductUnitId { get; set; }
	public string? ProductImageRotationAngle { get; set; }
	public int ProductBrandId { get; set; }
	public int ProductCategoryId { get; set; }
	public int ProductStorageId { get; set; }
	public string? ProductMemo { get; set; }

	// Define the property that you want to use in TLists (for example in the errorList
	public string Name => ProductName;

	// Mapping dictionary for mapping Database Header to Property name
	public static readonly Dictionary<string, string> HeaderToPropertyMap = new()
	{
		{ DBNames.ProductFieldNameId, "ProductId" },
		{ DBNames.ProductFieldNameCode, "ProductCode" },
		{ DBNames.ProductFieldNameName, "ProductName" },
		{ DBNames.ProductFieldNameDimensions, "ProductDimensions" },
		{ DBNames.ProductFieldNamePrice, "ProductPrice" },
		{ DBNames.ProductFieldNameMinimalStock, "ProductMinimalStock" },
		{ DBNames.ProductFieldNameStandardOrderQuantity, "ProductStandardQuantity" },
		{ DBNames.ProductFieldNameProjectCosts, "ProductCosts" },
		{ DBNames.ProductFieldNameUnitId, "ProductUnitId" },
		{ DBNames.ProductFieldNameBrandId, "ProductBrandId" },
		{ DBNames.ProductFieldNameCategoryId, "ProductCategoryId" },
		{ DBNames.ProductFieldNameStorageId, "ProductStorageId" }
	};

}
