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
				Console.WriteLine( "Hello Fresh" );
				UpdateFilteredOrderLines();
			}
		}
	}
	public ObservableCollection<SupplierModel> SupplierList { get; set; } = [ ];
	public ObservableCollection<SupplyOrderLineModel> SupplierOrderLineShortList { get; set; } = [ ];
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

	#region selected products
	public ObservableCollection<InventoryOrderModel> SelectedProducts { get; set; } = [ ];

	private ObservableCollection<InventoryOrderModel> _productList;
	public ObservableCollection<InventoryOrderModel> ProductList
	{
		get => _productList;
		set
		{
			if ( _productList != value )
			{
				if ( _productList != null )
				{
					foreach ( var product in _productList )
					{
						product.PropertyChanged -= SelectedProduct_PropertyChanged;
					}
				}

				_productList = value;

				if ( _productList != null )
				{
					foreach ( var product in _productList )
					{
						product.PropertyChanged += SelectedProduct_PropertyChanged;
					}
				}

				OnPropertyChanged( nameof( ProductList ) );
			}
		}
	}

	private void SelectedProduct_PropertyChanged( object sender, PropertyChangedEventArgs e )
	{
		if ( e.PropertyName == nameof( SupplyOrderModel.IsSelected ) )
		{
			var selectedProduct = sender as InventoryOrderModel;
			if ( selectedProduct != null )
			{
				if ( selectedProduct.IsSelected && !SelectedProducts.Contains( selectedProduct ) )
					SelectedProducts.Add( selectedProduct );
				else if ( !selectedProduct.IsSelected && SelectedProducts.Contains( selectedProduct ) )
					SelectedProducts.Remove( selectedProduct );
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

				LoadProductsForSelectedSupplier( selectedSupplierId );
				UpdateFilteredOrders();

				SelectedSupplierChanged?.Invoke( this, selectedSupplierId );
			}
		}
	}

	private void LoadProductsForSelectedSupplier( int supplierId )
	{
		// Stel hier je DB-oproep in om de productenlijst te halen op basis van supplierId
		ProductList = [ .. DBCommands.GetInventoryOrder( supplierId ) ];
		// Zorg ervoor dat de UI wordt geïnformeerd over de wijziging
		OnPropertyChanged( nameof( ProductList ) );
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

	#region Filter the orderlnes based on the selected OrderId
	private ObservableCollection<SupplyOrderLineModel> _filteredOrderLines = new();
	public ObservableCollection<SupplyOrderLineModel> FilteredOrderLines
	{
		get => _filteredOrderLines;
		set
		{
			if ( _filteredOrderLines != value )
			{
				_filteredOrderLines = value;
				OnPropertyChanged( nameof( FilteredOrderLines ) );
			}
		}
	}

	private void UpdateFilteredOrderLines()
	{
		if ( SelectedOrder != null )
		{
			// Filter de regels op basis van het geselecteerde OrderId
			FilteredOrderLines = new ObservableCollection<SupplyOrderLineModel>(
				SupplierOrderLineShortList.Where( line => line.SupplyOrderlineShortOrderId == SelectedOrder.SupplyOrderId )
			);
			Console.WriteLine( "Hello" );
		}
		else
		{
			// Leeg de lijst als er geen order is geselecteerd
			FilteredOrderLines = new ObservableCollection<SupplyOrderLineModel>();
		}
	}

	public SupplyOrderViewModel()
	{
		SupplierList = [ .. DBCommands.GetSupplierList() ];
		SupplierOrderList = [ .. DBCommands.GetSupplierOrderList() ];
		SupplierOrderLineShortList = [ .. DBCommands.GetSupplierOrderLineShortList() ];
		SelectedProducts = [ ];

		UpdateFilteredOrderLines();
	}
}

//Select the productlines in the datagrid
public class SupplyOrderLineViewModel : ObservableObject
{
	public SupplyOrderLineModel Model { get; }

	public SupplyOrderLineViewModel( SupplyOrderLineModel model )
	{
		Model = model;
	}

	public bool IsSelected
	{
		get => Model.IsSelected;
		set
		{
			if ( Model.IsSelected != value )
			{
				Model.IsSelected = value;
				OnPropertyChanged();
			}
		}
	}
}
#endregion