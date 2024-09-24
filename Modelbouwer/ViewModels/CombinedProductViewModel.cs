namespace Modelbouwer.ViewModels;
public class CombinedProductViewModel
{
	public BrandViewModel BrandViewModel { get; set; }
	public CategoryViewModel CategoryViewModel { get; set; }
	public ProductViewModel ProductViewModel { get; set; }
	public StorageViewModel StorageViewModel { get; set; }
	public SupplierViewModel SupplierViewModel { get; set; }
	public UnitViewModel UnitViewModel { get; set; }

	public CombinedProductViewModel()
	{
		BrandViewModel = new();
		CategoryViewModel = new();
		ProductViewModel = new();
		StorageViewModel = new();
		SupplierViewModel = new();
		UnitViewModel = new();
	}

	//private void OnSelectedProductChanged()
	//{
	//	SetSelectedCategory();
	//}

	//public void SetSelectedCategory()
	//{
	//	// Zorg ervoor dat je hier toegang hebt tot ProductViewModel en CategoryViewModel
	//	if ( ProductViewModel.SelectedProduct != null )
	//	{
	//		var categoryId = ProductViewModel.SelectedProduct.ProductCategoryId;
	//		var selectedCategory = CategoryViewModel.Category.FirstOrDefault(c => c.CategoryId == categoryId);

	//		if ( selectedCategory != null )
	//		{
	//			CategoryViewModel.SelectedCategory = selectedCategory;
	//		}
	//		else
	//		{
	//			// Als er geen categorie is geselecteerd, stel je de standaardwaarde in
	//			CategoryViewModel.SelectedCategory = CategoryViewModel.Category.FirstOrDefault( c => c.CategoryId == 1 ); // Standaardwaarde 1
	//		}
	//	}
	//}
}
