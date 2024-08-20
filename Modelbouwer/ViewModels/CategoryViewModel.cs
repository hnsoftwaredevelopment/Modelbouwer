namespace Modelbouwer.ViewModels;
public partial class CategoryViewModel : ObservableObject
{
	[ObservableProperty]
	public string? categoryName;

	[ObservableProperty]
	public string? categoryFullpath;

	[ObservableProperty]
	public int categoryId;

	[ObservableProperty]
	public int categoryParentId;
}
