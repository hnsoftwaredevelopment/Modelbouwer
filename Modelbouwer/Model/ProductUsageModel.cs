namespace Modelbouwer.Model;

public class ProductUsageModel
{
	public int ProductUsageId { get; set; }
	public int ProductUsageProjectId { get; set; }
	public string? ProductUsageProjectName { get; set; }
	public int ProductUsageProductId { get; set; }
	public string? ProductUsageProductName { get; set; }
	public string? ProductUsageUsageDate { get; set; }
	public int ProductUsageCategoryId { get; set; }
	public string? ProductUsageCategoryName { get; set; }
	public double ProductUsageAmount { get; set; }
	public double ProductUsageProductPrice { get; set; }
	public double ProductUsageCosts { get; set; }
	public string? ProductUsageComment { get; set; }

	// Mapping dictionary for mapping Database Header to Property name
	public static readonly Dictionary<string, string> HeaderToPropertyMap = new()
	{
		{ DBNames.ProductUsageViewFieldNameProductName, "ProductUsageProductName" },
		{ DBNames.ProductUsageViewFieldTypeCategoryName, "ProductUsageCategoryName" },
		{ DBNames.ProductUsageViewFieldNameAmountUsed, "ProductUsageAmount" },
		{ DBNames.ProductUsageViewFieldNamePrice, "ProductUsageProductPrice" },
		{ DBNames.ProductUsageViewFieldNameTotalCosts, "ProductUsageCosts" },
		{ DBNames.ProductUsageViewFieldNameComment, "ProductUsageComment" }
	};
}