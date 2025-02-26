namespace Modelbouwer.ViewModels;
public partial class InventoryOrderSupplierViewModel : ObservableObject
{
	[ObservableProperty]
	public int productSupplierId;

	[ObservableProperty]
	public int productSupplierProductId;

	[ObservableProperty]
	public int productSupplierSupplierId;

	[ObservableProperty]
	public int orderSupplierSupplierId;

	[ObservableProperty]
	public string? productSupplierSupplierName;

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

	public ObservableCollection<SupplierModel> SupplierList { get; set; } = [ ];
	public ObservableCollection<SupplyOrderModel> SupplierOrderList { get; set; } = [ ];

	public event EventHandler<int>? SelectedSupplierChanged;

	public InventoryOrderSupplierViewModel()
	{
		SupplierList = [ .. DBCommands.GetSupplierList() ];
	}

	public void Refresh()
	{
		SupplierList = [ .. DBCommands.GetSupplierList() ];
		OnPropertyChanged( nameof( SupplierList ) );
	}
}
