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
	public string? SupplierURL { get; set; }
	public double ShippingCosts { get; set; }
	public double MinShippingCosts { get; set; }
	public double OrderCosts { get; set; }
	public string? SupplierMemo { get; set; }
	public int SupplierCurrencyId { get; set; }
	public int SupplierCountryId { get; set; }
}