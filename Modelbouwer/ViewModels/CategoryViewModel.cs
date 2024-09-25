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
	public List<CategoryModel> FlatCategory { get; set; }

	[ObservableProperty]
	private CategoryModel? _selectedCategory;

	//public class CategoryComparer : IComparer<CategoryModel>
	//{
	//	public int Compare( CategoryModel x, CategoryModel y )
	//	{
	//		int result = x.IndentLevel.CompareTo(y.IndentLevel);
	//		if ( result == 0 )
	//		{
	//			return string.Compare( x.Name, y.Name, StringComparison.OrdinalIgnoreCase );
	//		}
	//		return result;
	//	}
	//}

	//public void SetSelectedCategory( int categoryId )
	//{
	//	SelectedCategory = Category.FirstOrDefault( c => c.CategoryId == categoryId )
	//					   ?? Category.FirstOrDefault( c => c.CategoryId == 1 );
	//}

	public CategoryViewModel()
	{
		DBCommands dbCommands = new();
		Category = new ObservableCollection<CategoryModel>( dbCommands.GetCategoryList() );
		FlatCategory = new List<CategoryModel>( dbCommands.GetFlatCategoryList() );

		SelectedCategory = Category.FirstOrDefault( c => c.CategoryId == 1 );
	}
}
