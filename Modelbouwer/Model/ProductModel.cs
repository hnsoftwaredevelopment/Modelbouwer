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
}
