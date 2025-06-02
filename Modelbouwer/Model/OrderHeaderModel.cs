namespace Modelbouwer.Model;
public class OrderHeaderModel
{
	public int OrderId { get; set; }
	public string? OrderNumber { get; set; }
	public string? OrderDate { get; set; }
	public decimal ShippingCosts { get; set; }
	public decimal OrderCosts { get; set; }
	public decimal OrderTotal { get; set; }
	public string OrderSummary =>
	$"Besteldatum: {OrderDate}   " +
	$"Ordernummer: {OrderNumber}   " +
	$"Verzendkosten: {ShippingCosts:C2}   " +
	$"Orderkosten: {OrderCosts:C2}   " +
	$"Totaal: {OrderTotal:C2}";

	public ObservableCollection<OrderLineModel> OrderLines { get; set; } = [ ];
}
