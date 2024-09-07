namespace Modelbouwer.Model;

public class SupplyOrderModel
{
	public int SupplyOrderId { get; set; }
	public int SupplyOrderSupplierId { get; set; }
	public int SupplyOrderCurrencyId { get; set; }
	public string? SupplyOrderNumber { get; set; }
	public DateOnly SupplyOrderDate { get; set; }
	public string? SupplyOrderCurrencySymbol { get; set; }
	public double SupplyOrderCurrencyRate { get; set; }
	public double SupplyOrderShippingCosts { get; set; }
	public double SupplyOrderOrderCosts { get; set; }
	public string? SupplyOrderMemo { get; set; }
	public int SupplyOrderClosed { get; set; }
	public DateOnly SupplyOrderClosedDate { get; set; }
}