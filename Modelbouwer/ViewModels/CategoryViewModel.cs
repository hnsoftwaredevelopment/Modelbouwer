﻿namespace Modelbouwer.ViewModels;
public partial class CategoryViewModel : ObservableObject
{
	[ObservableProperty]
	public string? categoryName;

	[ObservableProperty]
	public int categoryId;

	[ObservableProperty]
	public int categoryParentId;

	[ObservableProperty]
	public bool _isCategoryPopupOpen;

	public ObservableCollection<CategoryModel> Category { get; set; }

	public List<CategoryModel> FlatCategory { get; set; }
	//public ObservableCollection<CategoryModel> FlatCategory { get; set; }

	[ObservableProperty]
	private CategoryModel? _selectedCategory;

	public CategoryViewModel()
	{
		DBCommands dbCommands = new();
		Category = [ .. dbCommands.GetCategoryList() ];
		FlatCategory = [ .. dbCommands.GetFlatCategoryList() ];

		SelectedCategory = Category.FirstOrDefault( c => c.CategoryId == 1 );
	}

	public void Refresh()
	{
		DBCommands dbCommands = new();
		Category = [ .. dbCommands.GetCategoryList() ];
		FlatCategory = [ .. dbCommands.GetFlatCategoryList() ];

		OnPropertyChanged( nameof( Category ) );
		OnPropertyChanged( nameof( FlatCategory ) );
	}
}
