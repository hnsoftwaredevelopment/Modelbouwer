namespace Modelbouwer.ViewModels;
public class CombinedProductViewModel : ObservableObject
{
	public BrandViewModel BrandViewModel { get; set; }
	public CategoryViewModel CategoryViewModel { get; set; }
	public ProductViewModel ProductViewModel { get; set; }
	public ProductSupplierViewModel ProductSupplierViewModel { get; set; }
	public StorageViewModel StorageViewModel { get; set; }
	public SupplierViewModel SupplierViewModel { get; set; }
	public UnitViewModel UnitViewModel { get; set; }

	public CombinedProductViewModel()
	{
		BrandViewModel = new();
		CategoryViewModel = new();
		ProductViewModel = new();
		ProductSupplierViewModel = new();
		StorageViewModel = new();
		SupplierViewModel = new();
		UnitViewModel = new();

		ProductViewModel.PropertyChanged += ( sender, e ) =>
		{
			if ( e.PropertyName == nameof( ProductViewModel.SelectedProduct ) )
			{
				ProductSupplierViewModel.SelectedProduct = ProductViewModel.SelectedProduct;
				OnPropertyChanged( nameof( SelectedProduct ) );
			}
		};
	}

	public ProductModel SelectedProduct
	{
		get => ProductViewModel.SelectedProduct;
		set => ProductViewModel.SelectedProduct = value;
	}
}
