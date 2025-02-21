namespace Modelbouwer.ViewModels;
public partial class ProductViewModel : ObservableObject
{
	[ObservableProperty]
	public int productId;

	[ObservableProperty]
	public string? productCode;

	[ObservableProperty]
	public string? productName;

	[ObservableProperty]
	public string? productDimensions;

	[ObservableProperty]
	public double productPrice;

	[ObservableProperty]
	public double productMinimalStock;

	[ObservableProperty]
	public double productStandardQuantity;

	[ObservableProperty]
	public int productProjectCosts;

	[ObservableProperty]
	public int productUnitId;

	[ObservableProperty]
	public string? productImageRotationAngle;

	[ObservableProperty]
	public int productBrandId;

	[ObservableProperty]
	public int productCategoryId;

	[ObservableProperty]
	public int productStorageId;

	[ObservableProperty]
	public string? productMemo;

	public ObservableCollection<ProductModel>? Product { get; set; }

	private ImageSource? _productImage;
	public ImageSource? ProductImage
	{
		get => _productImage;
		set
		{
			//_productImage = value ?? System.Windows.Application.Current.TryFindResource( "noimage" ) as ImageSource;
			_productImage = value ?? ( System.Windows.Application.Current.TryFindResource( "noimage" ) as ImageSource )
				?? new BitmapImage( new Uri( "pack://application:,,,/YourAssemblyName;component/Resources/noimage.png" ) );

			OnPropertyChanged( nameof( ProductImage ) );
		}
	}


	[ObservableProperty]
	private ProductModel? _selectedProduct;



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

	public void AddNewItem()
	{
		ProductModel newProduct = new()
		{
			ProductBrandId = 1,
			ProductCategoryId = 1,
			ProductCode = string.Empty,
			ProductDimensions = string.Empty,
			ProductId = 0,
			//ProductImage = NULL,
			ProductImageRotationAngle = "0",
			ProductMemo = string.Empty,
			ProductMinimalStock = 0.00,
			ProductName = string.Empty,
			ProductPrice = 0.00,
			ProductProjectCosts = 0,
			ProductStandardQuantity = 0.00,
			ProductStorageId = 1,
			ProductUnitId = 1,
		};

		Product.Add( newProduct );
		SelectedProduct = newProduct;
		IsAddingNew = true;
	}

	public void RefreshProductList( int productIdToSelect )
	{
		// Save the current scroll position
		var currentIndex = Product.IndexOf(SelectedProduct);

		// Update the collection
		var updatedProducts = DBCommands.GetProductList();
		Product.Clear();
		foreach ( var product in updatedProducts )
		{
			Product.Add( product );
		}

		// Select the current product
		SelectedProduct = Product.FirstOrDefault( p => p.ProductId == productIdToSelect )
			?? Product [ currentIndex >= Product.Count ? Product.Count - 1 : currentIndex ];
	}


	public ProductViewModel()
	{
		Product = new ObservableCollection<ProductModel>( DBCommands.GetProductList() );
		SelectedProduct = Product [ 0 ];
	}
}
