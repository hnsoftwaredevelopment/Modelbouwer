namespace Modelbouwer.Model;

public class CategoryModel : INameable
{
	public int CategoryId { get; set; }
	public int? CategoryParentId { get; set; }
	public string? CategoryName { get; set; }
	public ObservableCollection<CategoryModel> SubCategories { get; set; } = [ ];
	public string Name => CategoryName;

	// Mapping dictionary for mapping Database Header to Property name
	public static readonly Dictionary<string, string> HeaderToPropertyMap = new()
	{
		{ DBNames.CategoryFieldNameId, "CategoryId" },
		{ DBNames.CategoryFieldNameParentId, "CategoryParentId" },
		{ DBNames.CategoryFieldNameName, "CategoryName" }
	};
}