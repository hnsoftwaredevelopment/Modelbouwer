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
	public StockOverviewViewModel StockOverviewViewModel { get; set; }

	public CombinedProductViewModel()
	{
		BrandViewModel = new();
		CategoryViewModel = new();
		ProductViewModel = new();
		ProductSupplierViewModel = new();
		StorageViewModel = new();
		SupplierViewModel = new();
		UnitViewModel = new();
		StockOverviewViewModel = new();

		ProductViewModel.PropertyChanged += async ( sender, e ) =>
		{
			if ( e.PropertyName == nameof( ProductViewModel.SelectedProduct ) )
			{
				ProductSupplierViewModel.SelectedProduct = ProductViewModel.SelectedProduct;
				OnPropertyChanged( nameof( SelectedProduct ) );
			}

			if ( ProductViewModel.SelectedProduct != null )
			{
				await LoadStockOverviewAsync( ProductViewModel.SelectedProduct.ProductId );
			}
		};
	}

	public ProductModel SelectedProduct
	{
		get => ProductViewModel.SelectedProduct;
		set => ProductViewModel.SelectedProduct = value;
	}

	private async Task LoadStockOverviewAsync( int productId )
	{
		StockOverviewModel result = await DBCommands.GetStockOverviewByProductAsync(productId);

		StockOverviewViewModel.CurrentStock = result.CurrentStock;
		StockOverviewViewModel.Mutations.Clear();

		foreach ( StockMutationModel mutationModel in result.Mutations )
		{
			StockMutation mutationViewModel = StockMutation.FromModel(mutationModel);
			StockOverviewViewModel.Mutations.Add( mutationViewModel );
		}
	}

	public void RefreshAll()
	{
		BrandViewModel.Refresh();
		CategoryViewModel.Refresh();
		ProductViewModel.Refresh();
		ProductSupplierViewModel.Refresh();
		StorageViewModel.Refresh();
		SupplierViewModel.Refresh();
		UnitViewModel.Refresh();
	}
}
