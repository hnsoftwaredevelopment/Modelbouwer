namespace Modelbouwer.ViewModels;
public partial class ProductViewModel : ObservableObject
{
	[ObservableProperty]
	public byte [ ] productImage;

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

	[ObservableProperty]
	private ProjectModel? _selectedProject;

	public ObservableCollection<ProductModel> Product { get; set; }

	private ObservableCollection<ProductModel>? _product;


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

	//public ProductModel SelectedProduct
	//{
	//	get => _selectedProduct;
	//	set
	//	{
	//		if ( SetProperty( ref _selectedProduct, value ) )
	//		{
	//			OnSelectedProductChanged();
	//		}
	//	}
	//}

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
			ProductImageRotationAngle = string.Empty,
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
		_selectedProduct = newProduct;
		IsAddingNew = true;
	}

	public ProductViewModel()
	{
		Product = new ObservableCollection<ProductModel>( DBCommands.GetProductList() );
	}
}
