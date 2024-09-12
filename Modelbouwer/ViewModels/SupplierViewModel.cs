namespace Modelbouwer.ViewModels;
public partial class SupplierViewModel : ObservableObject
{
	[ObservableProperty]
	public int supplierId;

	[ObservableProperty]
	public string? supplierCode;

	[ObservableProperty]
	public string? supplierName;

	[ObservableProperty]
	public string? supplierAddress1;

	[ObservableProperty]
	public string? supplierAddress2;

	[ObservableProperty]
	public string? supplierZip;

	[ObservableProperty]
	public string? supplierCity;

	[ObservableProperty]
	public string? supplierUrl;

	[ObservableProperty]
	public double supplierShippingCosts;

	[ObservableProperty]
	public double supplierMinShippingCosts;

	[ObservableProperty]
	public double supplierOrderCosts;

	[ObservableProperty]
	public string? supplierMemo;

	[ObservableProperty]
	public int supplierCurrencyId;

	[ObservableProperty]
	public string? supplierCurrency;

	[ObservableProperty]
	public int supplierCountryId;

	[ObservableProperty]
	public string? supplierCountry;

	public ObservableCollection<SupplierModel>? Supplier { get; set; }

	[ObservableProperty]
	private SupplierModel? _selectedSupplier;

	public SupplierViewModel()
	{
		Supplier = new ObservableCollection<SupplierModel>( DBCommands.GetSupplierList() );
	}
}
