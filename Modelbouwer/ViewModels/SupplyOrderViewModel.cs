using System.ComponentModel;

namespace Modelbouwer.ViewModels;
public partial class SupplyOrderViewModel : ObservableObject
{

	[ObservableProperty]
	public int supplyOrderId;

	[ObservableProperty]
	public int supplyOrderSupplierId;

	[ObservableProperty]
	public int supplyOrderCurrencyId;

	[ObservableProperty]
	public string? supplyOrderNumber;

	[ObservableProperty]
	public string? supplyOrderDate;

	[ObservableProperty]
	public string? supplyOrderCurrencySymbol;

	[ObservableProperty]
	public double supplyOrderCurrencyRate;

	[ObservableProperty]
	public double supplyOrderShippingCosts;

	[ObservableProperty]
	public double supplyOrderOrderCosts;

	[ObservableProperty]
	public string? supplyOrderMemo;

	[ObservableProperty]
	public int supplyOrderClosed;

	[ObservableProperty]
	public string? supplyOrderClosedDate;

	private SupplyOrderModel? selectedOrder;
	public SupplyOrderModel? SelectedOrder
	{
		get => selectedOrder;
		set
		{
			if ( selectedOrder != value )
			{
				selectedOrder = value;
				IsNewOrder = selectedOrder == null;
				OnPropertyChanged( nameof( SelectedOrder ) );
			}
		}
	}
	public ObservableCollection<SupplierModel> SupplierList { get; set; } = [ ];
	public ObservableCollection<SupplyOrderModel> SupplierOrderList { get; set; } = [ ];

	#region Filtered Orders   
	private ObservableCollection<SupplyOrderModel> _filteredOrders = [];
	public ObservableCollection<SupplyOrderModel> FilteredOrders
	{
		get => _filteredOrders;
		set
		{
			if ( _filteredOrders != value )
			{
				_filteredOrders = value ?? [ ];
				OnPropertyChanged( nameof( FilteredOrders ) );
			}
		}
	}
	#endregion

	#region Order List
	private ObservableCollection<InventoryOrderModel> _orderList;
	public ObservableCollection<InventoryOrderModel> OrderList
	{
		get => _orderList;
		set
		{
			if ( _orderList != value )
			{
				if ( _orderList != null )
				{
					foreach ( var order in _orderList )
					{
						order.PropertyChanged -= SelectedOrder_PropertyChanged;
					}
				}

				_orderList = value;

				if ( _orderList != null )
				{
					foreach ( var order in _orderList )
					{
						order.PropertyChanged += SelectedOrder_PropertyChanged;
					}
				}

				OnPropertyChanged( nameof( OrderList ) );
			}
		}
	}

	private void SelectedOrder_PropertyChanged( object sender, PropertyChangedEventArgs e )
	{
		if ( e.PropertyName == nameof( SupplyOrderModel.IsSelected ) )
		{
			var selectedOrder = sender as SupplyOrderModel;
			if ( selectedOrder != null )
			{
				SelectedOrder = selectedOrder.IsSelected ? selectedOrder : null;
			}
		}
	}
	#endregion

	public event EventHandler<int>? SelectedSupplierChanged;

	private int selectedSupplierId;
	public int SelectedSupplier
	{
		get => selectedSupplierId;
		set
		{
			if ( selectedSupplierId != value )
			{
				selectedSupplierId = value;
				OnPropertyChanged( nameof( SelectedSupplier ) );

				UpdateFilteredOrders();

				SelectedSupplierChanged?.Invoke( this, selectedSupplierId );
			}
		}
	}

	private void UpdateFilteredOrders()
	{
		if ( SelectedSupplier > 0 )
		{
			// Filter de bestellingen voor de geselecteerde leverancier
			FilteredOrders = [ .. SupplierOrderList.Where( order => order.SupplyOrderSupplierId == SelectedSupplier ) ];

		}
		else
		{
			// Leeg de lijst als er geen leverancier is geselecteerd
			FilteredOrders = new ObservableCollection<SupplyOrderModel>();
		}

		OnPropertyChanged( nameof( FilteredOrders ) );
	}

	private bool isNewOrder = true;
	public bool IsNewOrder
	{
		get => isNewOrder;
		set
		{
			if ( isNewOrder != value )
			{
				isNewOrder = value;
				OnPropertyChanged( nameof( IsNewOrder ) );
			}
		}
	}

	public SupplyOrderViewModel()
	{
		SupplierList = [ .. DBCommands.GetSupplierList() ];
		SupplierOrderList = [ .. DBCommands.GetSupplierOrderList() ];
	}
}
