namespace Modelbouwer.Model;
public class StocklogModel
{
	public int StocklogId { get; set; }
	public int StocklogProductId { get; set; }
	public int StocklogSupplyOrderId { get; set; }
	public int StocklogSupplyOrderLineId { get; set; }
	public int StocklogUsageId { get; set; }
	public double StocklogAmountUsed { get; set; }
	public double StocklogAmountReceived { get; set; }
	public DateOnly StocklogDate { get; set; }
}
