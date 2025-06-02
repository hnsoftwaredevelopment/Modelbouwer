namespace Modelbouwer.Model;
public class SupplierOrderHistoryModel
{
	public int SupplierOrderHistoryOrderId { get; set; }
	public int SupplierOrderHistorySupplierId { get; set; }
	public string? SupplierOrderHistoryOrderNumber { get; set; }
	public string? SupplierOrderHistoryOrderDate { get; set; }
	public decimal SupplierOrderHistoryOrderCosts { get; set; }
	public decimal SupplierOrderHistoryShippingCosts { get; set; }
	public decimal SupplierOrderHistoryCurrencyConversionRate { get; set; }
	public string? SupplierOrderHistoryReceived { get; set; }
	public int SupplierOrderHistoryProductId { get; set; }
	public string? SupplierOrderHistoryProductNumber { get; set; }
	public string? SupplierOrderHistoryProductDescription { get; set; }
	public decimal SupplierOrderHistoryPrice { get; set; }
	public decimal SupplierOrderHistoryAmount { get; set; }
	public decimal SupplierOrderHistoryRowTotal { get; set; }
	public decimal SupplierOrderHistoryOrderTotal { get; set; }
}
