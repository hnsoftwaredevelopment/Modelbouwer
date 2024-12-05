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

	[ObservableProperty]
	public SupplierModel? selectedItem;

	public ObservableCollection<SupplierModel>? Supplier { get; set; }

	[ObservableProperty]
	private SupplierModel? _selectedSupplier;

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

	private int selectedSupplierId;
	public int SelectedSupplierId
	{
		get => selectedSupplierId;
		set
		{
			if ( selectedSupplierId != value )
			{
				selectedSupplierId = value;
				OnPropertyChanged( nameof( SelectedSupplierId ) );
				SelectedSupplierChanged?.Invoke( this, selectedSupplierId );
			}
		}
	}


	public event EventHandler<int>? SelectedSupplierChanged;

	partial void OnSelectedSupplierChanged( SupplierModel? value )
	{
		if ( value != null )
			SelectedSupplierChanged?.Invoke( this, value.SupplierId );
	}

	public void AddNewItem()
	{
		SupplierModel newSupplier = new()
		{
			SupplierId = 0,
			SupplierCurrencyId = 1,
			SupplierCountryId = 1,
			SupplierCode = string.Empty,
			SupplierName = string.Empty,
			SupplierAddress1 = string.Empty,
			SupplierAddress2 = string.Empty,
			SupplierZip = string.Empty,
			SupplierCity = string.Empty,
			SupplierUrl = string.Empty,
			SupplierMemo = string.Empty,
			SupplierShippingCosts = 0.00,
			SupplierMinShippingCosts = 0.00,
			SupplierOrderCosts = 0.00
		};

		Supplier.Add( newSupplier );
		SelectedSupplier = newSupplier;
		IsAddingNew = true;
	}

	public SupplierViewModel( SupplierContactViewModel supplierContactViewModel )
	{
		Supplier = new ObservableCollection<SupplierModel>( DBCommands.GetSupplierList() );
		var _supplierContactViewModel = supplierContactViewModel;
	}

	public SupplierViewModel()
	{
		Supplier = new ObservableCollection<SupplierModel>( DBCommands.GetSupplierList() );
	}
}
