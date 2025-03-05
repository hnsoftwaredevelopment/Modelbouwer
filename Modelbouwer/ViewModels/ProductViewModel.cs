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

	[ObservableProperty]
	private ProductModel? _selectedProduct;

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
			ProductPrice = 0.00,
			ProductPackagePrice = 0.00,
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

	public ProductViewModel()
	{
		Product = [ .. DBCommands.GetProductList() ];
		SelectedProduct = Product [ 0 ];
	}

	public void Refresh()
	{
		Product = [ .. DBCommands.GetProductList() ];
		OnPropertyChanged( nameof( Product ) );
	}
}
