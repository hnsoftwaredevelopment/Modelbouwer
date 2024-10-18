namespace Modelbouwer.ViewModels;
public partial class ProductSupplierViewModel : ObservableObject
{
	[ObservableProperty]
	public int productSupplierId;

	[ObservableProperty]
	public int productSupplierProductId;

	[ObservableProperty]
	public int productSupplierSupplierId;

	[ObservableProperty]
	public string? productSupplierSupplierName;

	[ObservableProperty]
	private ProductSupplierModel? _selectedSupplier;

	[ObservableProperty]
	public int productSupplierCurrencyId;

	[ObservableProperty]
	public string? productSupplierProductNumber;

	[ObservableProperty]
	public string? productSupplierProductName;

	[ObservableProperty]
	public double productSupplierPriceId;

	[ObservableProperty]
	public string? productSupplierURLId;

	[ObservableProperty]
	public string? productSupplierDefaultSupplier;

	[ObservableProperty]
	public bool? productSupplierDefaultSupplierCheck;

	public ObservableCollection<ProductSupplierModel> FilteredSuppliers { get; private set; } = [ ];
	public ObservableCollection<ProductSupplierModel> ProductSupplier { get; set; }

	private ProductModel? _selectedProduct;

	public ObservableCollection<SupplierModel> SupplierList { get; set; } = [ ];

	private bool _isAddingNew;

	public bool IsAddingNew
	{
		get => _isAddingNew;
		set
		{
			if ( _isAddingNew != value )
			{
				_isAddingNew = value;
				OnPropertyChanged( nameof( IsAddingNew ) );
			}
		}
	}

	public void AddNewItem( string productId )
	{
		ProductSupplierModel newSupplier = new()
		{
			ProductSupplierId = 1,
			ProductSupplierProductId = int.Parse(productId),
			ProductSupplierSupplierId = 1,
			ProductSupplierCurrencyId = 1,
			ProductSupplierProductNumber = string.Empty,
			ProductSupplierProductName = string.Empty,
			ProductSupplierPrice = 0.00,
			ProductSupplierURL = string.Empty,
			ProductSupplierDefaultSupplier = string.Empty,
			ProductSupplierSupplierName = string.Empty,
			ProductSupplierCurrencySymbol = string.Empty,
			ProductSupplierDefaultSupplierCheck = false
		};

		ProductSupplier.Add( newSupplier );
		SelectedSupplier = newSupplier;
		IsAddingNew = true;

		// Refresh the list to display
		OnPropertyChanged( nameof( FilteredSuppliers ) );
		FilterSuppliersByProductId( int.Parse( productId ) );
	}

	public void LoadSuppliersForProduct( int productId )
	{
		FilterSuppliersByProductId( productId );

		// Zorg ervoor dat de eerste leverancier geselecteerd is als die beschikbaar is
		if ( FilteredSuppliers.Any() )
		{
			SelectedSupplier = FilteredSuppliers.First();
			ProductSupplierSupplierId = SelectedSupplier.ProductSupplierSupplierId; // Zorg ervoor dat de ID ook wordt ingesteld
		}
	}

	public ProductModel? SelectedProduct
	{
		get => _selectedProduct;
		set
		{
			if ( SetProperty( ref _selectedProduct, value ) )
			{
				if ( _selectedProduct != null )
				{
					FilterSuppliersByProductId( _selectedProduct.ProductId );
				}
			}
		}
	}

	public void FilterSuppliersByProductId( int productId )
	{
		FilteredSuppliers.Clear();
		foreach ( ProductSupplierModel supplier in ProductSupplier.Where( c => c.ProductSupplierProductId == productId ) )
		{
			FilteredSuppliers.Add( supplier );
		}

		//Select first supplier in the list if there are suppliers
		if ( FilteredSuppliers.Any() )
		{
			SelectedSupplier = FilteredSuppliers.First();
		}
	}

	partial void OnProductSupplierSupplierIdChanged( int value )
	{
		var selectedSupplier = SupplierList.FirstOrDefault(s => s.SupplierId == value);

		if ( selectedSupplier != null )
		{
			SelectedSupplier.ProductSupplierSupplierName = selectedSupplier.SupplierName;
			SelectedSupplier.ProductSupplierSupplierId = selectedSupplier.SupplierId;
			FilterSuppliersByProductId( SelectedProduct?.ProductId ?? 0 );
			OnPropertyChanged( nameof( FilteredSuppliers ) );
			OnPropertyChanged( nameof( SelectedSupplier ) );
		}
	}

	public ProductSupplierViewModel()
	{
		SupplierList = new ObservableCollection<SupplierModel>( DBCommands.GetSupplierList() );
		ProductSupplier = new ObservableCollection<ProductSupplierModel>( DBCommands.GetProductSupplierList() );
	}
}
