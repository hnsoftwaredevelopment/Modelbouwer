namespace Modelbouwer.Model;

public class ProductModel
{
	public byte [ ]? ProductImage { get; set; }
	public double ProductMinimalStock { get; set; }
	public double ProductPrice { get; set; }
	public double ProductStandardQuantity { get; set; }
	public int ProductBrandId { get; set; }
	public int ProductCategoryId { get; set; }
	public int ProductId { get; set; }
	public int ProductProjectCosts { get; set; }
	public int ProductStorageId { get; set; }
	public int ProductUnitId { get; set; }
	public string? ProductCode { get; set; }
	public string? ProductDimensions { get; set; }
	public string? ProductImageRotationAngle { get; set; }
	public string? ProductMemo { get; set; }
	public string? ProductName { get; set; }

	// Define the property that you want to use in TLists (for example in the errorList
	public string Name => ProductName;

	// Mapping dictionary for mapping Database Header to Property name
	public static readonly Dictionary<string, string> HeaderToPropertyMap = new()
	{
		{ DBNames.ProductFieldNameBrandId, "ProductBrandId" },
		{ DBNames.ProductFieldNameCategoryId, "ProductCategoryId" },
		{ DBNames.ProductFieldNameCode, "ProductCode" },
		{ DBNames.ProductFieldNameDimensions, "ProductDimensions" },
		{ DBNames.ProductFieldNameId, "ProductId" },
		{ DBNames.ProductFieldNameImage, "ProductImage" },
		{ DBNames.ProductFieldNameImageRotationAngle, "ProductImageRotationAngle" },
		{ DBNames.ProductFieldNameMinimalStock, "ProductMinimalStock" },
		{ DBNames.ProductFieldNameName, "ProductName" },
		{ DBNames.ProductFieldNamePrice, "ProductPrice" },
		{ DBNames.ProductFieldNameProjectCosts, "ProductProjectCosts" },
		{ DBNames.ProductFieldNameStandardOrderQuantity, "ProductStandardQuantity" },
		{ DBNames.ProductFieldNameStorageId, "ProductStorageId" },
		{ DBNames.ProductFieldNameUnitId, "ProductUnitId" },
	};
}