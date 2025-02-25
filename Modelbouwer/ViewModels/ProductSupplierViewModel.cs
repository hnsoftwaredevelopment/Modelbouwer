namespace Modelbouwer.ViewModels;

public partial class ProductSupplierViewModel : ObservableObject
{
	[ObservableProperty]
	private int productSupplierId;

	[ObservableProperty]
	private int productSupplierProductId;

	[ObservableProperty]
	private int productSupplierSupplierId;

	[ObservableProperty]
	private string? productSupplierSupplierName;

	[ObservableProperty]
	private ProductSupplierModel? selectedSupplier;

	[ObservableProperty]
	private int productSupplierCurrencyId;

	[ObservableProperty]
	private string? productSupplierProductNumber;

	[ObservableProperty]
	private string? productSupplierProductName;

	[ObservableProperty]
	private double productSupplierPrice;

	[ObservableProperty]
	private string? productSupplierURL;

	[ObservableProperty]
	private bool? productSupplierDefaultSupplier;

	[ObservableProperty]
	private bool? productSupplierDefaultSupplierCheck;

	public ObservableCollection<ProductSupplierModel> FilteredSuppliers { get; private set; } = new();

	public ObservableCollection<ProductSupplierModel> ProductSupplier { get; set; }

	private ProductModel? _selectedProduct;

	public ObservableCollection<SupplierModel> SupplierList { get; set; } = new();

	private bool _hasSuppliers;
	public bool HasSuppliers
	{
		get => _hasSuppliers;
		set
		{
			if ( _hasSuppliers != value )
			{
				_hasSuppliers = value;
				OnPropertyChanged( nameof( HasSuppliers ) );
			}
		}
	}

	private bool _isAddingNew;

	public bool IsAddingNew
	{
		get => _isAddingNew;
		set => SetProperty( ref _isAddingNew, value );
	}

	private bool _hasUnsavedChanges;

	public bool HasUnsavedChanges
	{
		get => _hasUnsavedChanges;
		set
		{
			_hasUnsavedChanges = value;
			OnPropertyChanged( nameof( HasUnsavedChanges ) );
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
			ProductSupplierDefaultSupplier = false,
			ProductSupplierSupplierName = string.Empty,
			ProductSupplierCurrencySymbol = string.Empty,
			ProductSupplierDefaultSupplierCheck = false
		};

		ProductSupplier.Add( newSupplier );
		SelectedSupplier = newSupplier;
		IsAddingNew = true;

		// Refresh the list to display
		OnPropertyChanged( nameof( FilteredSuppliers ) );
		OnPropertyChanged( nameof( HasSuppliers ) );
		OnPropertyChanged( nameof( HasUnsavedChanges ) );
		FilterSuppliersByProductId( int.Parse( productId ) );
	}

	public void LoadSuppliersForProduct( int productId )
	{
		FilterSuppliersByProductId( productId );

		// Ensure the first supplier is selected if available
		if ( FilteredSuppliers.Any() )
		{
			SelectedSupplier = FilteredSuppliers.First();
			ProductSupplierSupplierId = SelectedSupplier.ProductSupplierSupplierId; // Ensure the ID is also set
																					//OnPropertyChanged( nameof( HasSuppliers ) ); // Notify that there is a supplier now (to enable edit and save)
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
					LoadSuppliersForProduct( _selectedProduct.ProductId );
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

		// Select first supplier in the list if there are suppliers
		if ( FilteredSuppliers.Any() )
		{
			SelectedSupplier = FilteredSuppliers.First();
		}
		OnPropertyChanged( nameof( FilteredSuppliers ) );

		// Update HasSuppliers property
		HasSuppliers = FilteredSuppliers.Any();
		OnPropertyChanged( nameof( HasSuppliers ) );
	}

	partial void OnProductSupplierSupplierIdChanged( int value )
	{
		var selectedSupplier = SupplierList.FirstOrDefault(s => s.SupplierId == value);

		if ( selectedSupplier != null && SelectedSupplier != null )
		{
			SelectedSupplier.ProductSupplierSupplierName = selectedSupplier.SupplierName;
			SelectedSupplier.ProductSupplierSupplierId = selectedSupplier.SupplierId;

			FilterSuppliersByProductId( SelectedProduct?.ProductId ?? 0 );
			OnPropertyChanged( nameof( FilteredSuppliers ) );
			OnPropertyChanged( nameof( SelectedSupplier ) );
		}
	}

	public void RaisePropertyChanged( string propertyName )
	{
		OnPropertyChanged( propertyName );
	}

	public void RefreshProductSupplierList( int productId, int? supplierIdToSelect = null )
	{
		// Bewaar huidige selectie als geen specifieke supplier is opgegeven
		var currentSupplierId = supplierIdToSelect ?? SelectedSupplier?.ProductSupplierId;

		// Update hoofdcollectie
		var updatedSuppliers = DBCommands.GetProductSupplierList();
		ProductSupplier.Clear();
		foreach ( var supplier in updatedSuppliers )
		{
			ProductSupplier.Add( supplier );
		}

		// Update gefilterde lijst voor het huidige product
		FilterSuppliersByProductId( productId );

		// Selecteer de juiste supplier
		if ( FilteredSuppliers.Any() )
		{
			if ( currentSupplierId.HasValue )
			{
				// Probeer de vorige selectie te behouden
				SelectedSupplier = FilteredSuppliers.FirstOrDefault( s =>
					s.ProductSupplierId == currentSupplierId ) ?? FilteredSuppliers.First();
			}
			else
			{
				// Als er geen vorige selectie was, neem de eerste
				SelectedSupplier = FilteredSuppliers.First();
			}
		}
		else
		{
			SelectedSupplier = null;
		}

		// Update alle relevante properties
		OnPropertyChanged( nameof( FilteredSuppliers ) );
		OnPropertyChanged( nameof( HasSuppliers ) );
		OnPropertyChanged( nameof( HasUnsavedChanges ) );
	}

	public ProductSupplierViewModel()
	{
		SupplierList = [ .. DBCommands.GetSupplierList() ];
		ProductSupplier = [ .. DBCommands.GetProductSupplierList() ];
	}

	public void Refresh()
	{
		SupplierList = [ .. DBCommands.GetSupplierList() ];
		ProductSupplier = [ .. DBCommands.GetProductSupplierList() ];

		OnPropertyChanged( nameof( SupplierList ) );
		OnPropertyChanged( nameof( ProductSupplier ) );
	}
}