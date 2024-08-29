using System.Collections.ObjectModel;

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

	public ObservableCollection<CategoryModel> Category
	{
		get => _category;
		set
		{
			if ( _category != value )
			{
				_category = value;
				OnPropertyChanged( nameof( Category ) );
			}
		}
	}
	private ObservableCollection<CategoryModel>? _category;

}
