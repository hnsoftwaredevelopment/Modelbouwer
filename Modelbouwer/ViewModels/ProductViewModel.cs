using System.Collections.ObjectModel;

namespace Modelbouwer.ViewModels;
public partial class productViewModel : ObservableObject
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
	public int productCosts;

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

	public ObservableCollection<ProductModel> Product
	{
		get => _product;
		set
		{
			if ( _product != value )
			{
				_product = value;
				OnPropertyChanged( nameof( Product ) );
			}
		}
	}
	private ObservableCollection<ProductModel>? _product;
}
