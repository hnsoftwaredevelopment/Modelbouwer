namespace Modelbouwer.Model;
public partial class ReceiptsReportModel : ObservableObject
{
	public DateOnly OrderDate { get; set; }
	public string? OrderDateString { get; set; }
	public string? OrderNumber { get; set; }
	public string? Supplier { get; set; }
	public string? Shortname { get; set; }
	public string? Description { get; set; }
	public decimal Ordered { get; set; }
	public DateOnly ReceivedDate { get; set; }
	public string? ReceivedDateString { get; set; }
	public decimal Received { get; set; }
	public int IsOrderLine { get; set; }
	public int RowClosed { get; set; }
	public bool RowClosedCheck { get; set; }
	public string RowClosedDate { get; set; }
	public bool IsOrderLineRow => IsOrderLine == 0;
}
