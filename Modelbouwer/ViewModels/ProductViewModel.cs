namespace Modelbouwer.ViewModels;
public partial class ProductViewModel : ObservableObject
{
	[ObservableProperty]
	private int _productId;

	[ObservableProperty]
	private string? _productCode;

	[ObservableProperty]
	private string? _productName;

	[ObservableProperty]
	private string? _productDimensions;

	[ObservableProperty]
	private double _productMinimalStock;

	[ObservableProperty]
	private int _productProjectCosts;

	[ObservableProperty]
	private int _productUnitId;

	[ObservableProperty]
	private string? _productImageRotationAngle;

	[ObservableProperty]
	private int _productBrandId;

	[ObservableProperty]
	private int _productCategoryId;

	[ObservableProperty]
	private int _productStorageId;

	[ObservableProperty]
	private string? _productMemo;

	[ObservableProperty]
	private ObservableCollection<ProductModel>? _product;

	[ObservableProperty]
	private ImageSource? _productImage;

	private ProductModel? _selectedProduct;
	public ProductModel? SelectedProduct
	{
		get => _selectedProduct;
		set
		{
			if ( _selectedProduct != value )
			{
				System.Diagnostics.Debug.WriteLine( $"ProductViewModel - SelectedProduct verandert van {_selectedProduct?.ProductName} naar {value?.ProductName}" );
				_selectedProduct = value;
				OnPropertyChanged( nameof( SelectedProduct ) );
			}
		}
	}

	[ObservableProperty]
	private bool _isAddingNew;

	//public ObservableCollection<ProductModel>? Product { get; set; }

	partial void OnProductImageChanged( ImageSource? value )
	{
		// If the image is null, try to find a default image resource
		if ( value == null )
		{
			value = System.Windows.Application.Current.TryFindResource( "noimage" ) as ImageSource
				?? new BitmapImage( new Uri( "pack://application:,,,/YourAssemblyName;component/Resources/noimage.png" ) );
		}
		_productImage = value;
	}

	public void AddNewItem()
	{
		if ( Product == null )
		{
			Product = new ObservableCollection<ProductModel>();
		}

		ProductModel newProduct = new()
		{
			ProductBrandId = 1,
			ProductCategoryId = 1,
			ProductCode = string.Empty,
			ProductDimensions = string.Empty,
			ProductId = 0,
			ProductImageRotationAngle = "0",
			ProductMemo = string.Empty,
			ProductMinimalStock = 0.00,
			ProductName = string.Empty,
			ProductPrice = 0.0000000M,
			ProductPackagePrice = 0.00M,
			ProductProjectCosts = 0,
			ProductStandardQuantity = 0.00M,
			ProductStorageId = 1,
			ProductUnitId = 1,
		};

		Product.Add( newProduct );
		SelectedProduct = newProduct;
		IsAddingNew = true;
	}

	public void RefreshProductList( int productIdToSelect )
	{
		if ( Product == null )
		{
			Product = new ObservableCollection<ProductModel>();
		}

		// Save the current scroll position
		int currentIndex = Product.IndexOf(SelectedProduct ?? Product.FirstOrDefault());

		// Update the collection
		ObservableCollection<ProductModel> updatedProducts = DBCommands.GetProductList();
		Product.Clear();
		foreach ( ProductModel product in updatedProducts )
		{
			Product.Add( product );
		}

		// Select the current product
		SelectedProduct = Product.FirstOrDefault( p => p.ProductId == productIdToSelect )
			?? Product [ currentIndex >= Product.Count ? Product.Count - 1 : currentIndex ];
	}

	#region Visibility of Fields in the ProductMaintanance.xaml
	private string _productOrderQuantity;
	private string _productPackagingUnit;

	public string ProductOrderQuantity
	{
		get => _productOrderQuantity;
		set
		{
			if ( SetProperty( ref _productOrderQuantity, value ) )
			{
				UpdateVisibilityStates();
			}
		}
	}

	public string ProductPackagingUnit
	{
		get => _productPackagingUnit;
		set
		{
			if ( SetProperty( ref _productPackagingUnit, value ) )
			{
				UpdateVisibilityStates();
			}
		}
	}

	private Visibility _pricePerSufixVisibility = Visibility.Collapsed;
	public Visibility PricePerSufixVisibility
	{
		get => _pricePerSufixVisibility;
		set => SetProperty( ref _pricePerSufixVisibility, value );
	}

	private Visibility _procuctPacketPriceBlockVisibility = Visibility.Collapsed;
	public Visibility ProcuctPacketPriceBlockVisibility
	{
		get => _procuctPacketPriceBlockVisibility;
		set => SetProperty( ref _procuctPacketPriceBlockVisibility, value );
	}

	private Visibility _packagePriceLabelVisibility = Visibility.Collapsed;
	public Visibility PackagePriceLabelVisibility
	{
		get => _packagePriceLabelVisibility;
		set => SetProperty( ref _packagePriceLabelVisibility, value );
	}

	private string _pricePerSufixText;
	public string PricePerSufixText
	{
		get => _pricePerSufixText;
		set => SetProperty( ref _pricePerSufixText, value );
	}

	private string _packagePriceLabelText;
	public string PackagePriceLabelText
	{
		get => _packagePriceLabelText;
		set => SetProperty( ref _packagePriceLabelText, value );
	}
	#endregion

	public ProductViewModel()
	{
		Product = [ .. DBCommands.GetProductList() ];
		SelectedProduct = Product [ 0 ];

		_productOrderQuantity = "1";
		_productPackagingUnit = "Stuk";
		UpdateVisibilityStates();
	}

	private void UpdateVisibilityStates()
	{
		if ( string.IsNullOrEmpty( ProductOrderQuantity ) || ProductOrderQuantity.Trim() == "1" )
		{
			string _quantity = ProductOrderQuantity;
			string _unit = ProductPackagingUnit;
			string _spacer = " ";

			if ( _quantity.Trim() == "1" ) { _quantity = ""; }
			if ( _quantity == "" ) { _spacer = ""; }

			ProcuctPacketPriceBlockVisibility = Visibility.Collapsed;
			PackagePriceLabelVisibility = Visibility.Collapsed;

			if ( string.IsNullOrEmpty( _unit?.Trim() ) )
			{
				PricePerSufixVisibility = Visibility.Collapsed;
			}
			else
			{
				PricePerSufixVisibility = Visibility.Visible;
				PricePerSufixText = $"per {_quantity}{_spacer}{_unit.ToLower()}";
			}
		}
		else
		{
			PricePerSufixVisibility = Visibility.Collapsed;
			ProcuctPacketPriceBlockVisibility = Visibility.Visible;
			PackagePriceLabelVisibility = Visibility.Visible;
			PackagePriceLabelText = $"Kostprijs per {ProductOrderQuantity} {ProductPackagingUnit?.ToLower()}";
		}
	}

	public void Refresh()
	{
		Product = [ .. DBCommands.GetProductList() ];
		OnPropertyChanged( nameof( Product ) );
	}
}
