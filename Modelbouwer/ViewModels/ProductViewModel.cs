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
}
