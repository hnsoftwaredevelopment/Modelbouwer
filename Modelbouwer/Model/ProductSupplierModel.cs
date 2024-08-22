namespace Modelbouwer.Model;
public class ProductSupplierModel
{
	public int ProductSupplierId { get; set; }
	public int ProductSupplierProductId { get; set; }
	public int ProductSupplierSupplierId { get; set; }
	public int ProductSupplierCurrencyId { get; set; }
	public string? ProductSupplierProductNumber { get; set; }
	public string? ProductSupplierProductName { get; set; }
	public double ProductSupplierPriceId { get; set; }
	public string? ProductSupplierURLId { get; set; }
	public string? ProductSupplierDefaultSupplier { get; set; }
}
