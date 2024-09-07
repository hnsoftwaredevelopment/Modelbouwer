namespace Modelbouwer.Model;

public class ProductUsageModel
{
	public int ProductUsageId { get; set; }
	public int ProductUsageProjectId { get; set; }
	public int ProductUsageProductId { get; set; }
	public DateOnly ProductUsageUsageDate { get; set; }
	public double ProductUsageAmount { get; set; }
	public string? ProductUsageComment { get; set; }
}