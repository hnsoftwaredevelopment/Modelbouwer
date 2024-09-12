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
	public int SupplierCountryId { get; set; }
	public string? SupplierCountry { get; set; }
}