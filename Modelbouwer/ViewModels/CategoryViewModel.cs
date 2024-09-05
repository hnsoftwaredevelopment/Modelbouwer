using System.Collections.ObjectModel;

namespace Modelbouwer.ViewModels;
public partial class CategoryViewModel : ObservableObject
{
	[ObservableProperty]
	public string? categoryName;

	[ObservableProperty]
	public int categoryId;

	[ObservableProperty]
	public int categoryParentId;

	public ObservableCollection<CategoryModel> Category { get; set; }

	[ObservableProperty]
	private CategoryModel? _selectedCategory;

	public CategoryViewModel()
	{
		DBCommands dbCommands = new();
		Category = new ObservableCollection<CategoryModel>( dbCommands.GetCategoryList() );
	}
}
