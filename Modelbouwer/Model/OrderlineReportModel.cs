namespace Modelbouwer.Model;
public partial class OrderlineReportModel : ObservableObject
{
	public int OrderId { get; set; }
	public string? OrderNumber { get; set; }
	public DateOnly OrderDate { get; set; }
	public int SupplierId { get; set; }
	public string? SupplierName { get; set; }
	public int ProductId { get; set; }
	public string? ProductCode { get; set; }
	public string? ProductName { get; set; }
	public decimal UnitPrice { get; set; }
	public decimal Ordered { get; set; }
	public decimal Received { get; set; }
	public decimal Expected { get; set; }
	public int Closed { get; set; }
	public DateOnly ClosedDate { get; set; }
}
