namespace Modelbouwer.Model;
public class SupplyOrderLineModel
{
	public int SupplyOrderLineId { get; set; }
	public int SupplyOrderLineSupplierId { get; set; }
	public int SupplyOrderLineProductId { get; set; }
	public string? SupplyOrderLineProductName { get; set; }
	public int SupplyOrderLineProjectId { get; set; }
	public int SupplyOrderLineCategoryId { get; set; }
	public double SupplyOrderLineAmount { get; set; }
	public double SupplyOrderLineOpenAmount { get; set; }
	public double SupplyOrderLinePrice { get; set; }
	public double SupplyOrderLineRowTotal { get; set; }
	public int SupplyOrderLineClosed { get; set; }
	public DateOnly SupplyOrderLineClosedDate { get; set; }
}
