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
}
