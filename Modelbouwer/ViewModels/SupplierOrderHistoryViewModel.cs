namespace Modelbouwer.ViewModels;
public partial class SupplierOrderHistoryViewModel : ObservableObject
{
	[ObservableProperty]
	public int supplierOrderHistoryOrderId;

	[ObservableProperty]
	public int supplierOrderHistorySupplierId;

	[ObservableProperty]
	public string? supplierOrderHistoryOrderNumber;

	[ObservableProperty]
	public string? supplierOrderHistoryOrderDate;

	[ObservableProperty]
	public decimal supplierOrderHistoryOrderCosts;

	[ObservableProperty]
	public decimal supplierOrderHistoryShippingCosts;

	[ObservableProperty]
	public decimal supplierOrderHistoryCurrencyConversionRate;

	[ObservableProperty]
	public string? supplierOrderHistoryReceived;

	[ObservableProperty]
	public int supplierOrderHistoryProductId;

	[ObservableProperty]
	public string? supplierOrderHistoryProductNumber;

	[ObservableProperty]
	public string? supplierOrderHistoryProductDescription;

	[ObservableProperty]
	public decimal supplierOrderHistoryPrice;

	[ObservableProperty]
	public decimal supplierOrderHistoryAmount;

	[ObservableProperty]
	public decimal supplierOrderHistoryRowTotal;

	[ObservableProperty]
	public decimal supplierOrderHistoryOrderTotal;

	public ObservableCollection<SupplierOrderHistoryModel>? OrderHistory { get; set; }

	public ObservableCollection<OrderHeaderModel> GroupedOrderHistory { get; set; }

	#region Selected Supplier
	private int _selectedSupplierId;
	public int SelectedSupplierId
	{
		get => _selectedSupplierId;
		set
		{
			if ( _selectedSupplierId != value )
			{
				_selectedSupplierId = value;
				OnPropertyChanged( nameof( SelectedSupplierId ) );
				SelectedSupplierChanged?.Invoke( this, _selectedSupplierId );
			}
		}
	}

	public event EventHandler<int>? SelectedSupplierChanged;
	#endregion

	public SupplierOrderHistoryViewModel( int supplierId = 0 )
	{
		if ( supplierId != 0 )
		{
			GroupedOrderHistory = DBCommands.GetOrderHistoryHierarchical( supplierId );
		}

	}

	public void Refresh( int supplierId = 0 )
	{
		if ( supplierId != 0 )
		{
			GroupedOrderHistory = [ .. DBCommands.GetOrderHistoryHierarchical( supplierId ) ];
			OnPropertyChanged( nameof( OrderHistory ) );
		}
	}

	public void RefreshGrouped( int supplierId = 0 )
	{
		if ( supplierId != 0 )
		{
			GroupedOrderHistory = DBCommands.GetOrderHistoryHierarchical( supplierId );
			OnPropertyChanged( nameof( GroupedOrderHistory ) );
		}
	}
}
