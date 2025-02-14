using System.ComponentModel;

using CommunityToolkit.Mvvm.Input;

namespace Modelbouwer.ViewModels;
public partial class SupplyOrderViewModel : ObservableObject
{
	[ObservableProperty]
	private double minShippingCosts;

	[ObservableProperty]
	public int supplyOrderId;

	[ObservableProperty]
	public int supplyOrderSupplierId;

	[ObservableProperty]
	public int supplyOrderCurrencyId;

	public string SupplyOrderNumber
	{
		get => SelectedOrder?.SupplyOrderNumber ?? string.Empty;
		set
		{
			if ( SelectedOrder != null && SelectedOrder.SupplyOrderNumber != value )
			{
				SelectedOrder.SupplyOrderNumber = value;
				UpdateCanSave();
			}
		}
	}

	public DateTime? SupplyOrderDate
	{
		get => SelectedOrder?.SupplyOrderDate.HasValue == true ? ( DateTime? ) SelectedOrder.SupplyOrderDate.Value.ToDateTime( TimeOnly.MinValue ) : null;
		set
		{
			if ( SelectedOrder != null && value.HasValue )
			{
				SelectedOrder.SupplyOrderDate = DateOnly.FromDateTime( value.Value );
				UpdateCanSave();
				Debug.WriteLine( $"SupplyOrderDate updated to: {value}" );
			}
		}
	}


	[ObservableProperty]
	public string? supplyOrderCurrencySymbol;

	[ObservableProperty]
	public double supplyOrderCurrencyRate;

	[ObservableProperty]
	public double supplyOrderShippingCosts;

	[ObservableProperty]
	public string? supplyOrderMemo;

	[ObservableProperty]
	public int supplyOrderClosed;

	[ObservableProperty]
	public string? supplyOrderClosedDate;

	[ObservableProperty]
	private double _subTotal;

	[ObservableProperty]
	private double _shippingCosts;

	[ObservableProperty]
	private double _orderCosts;

	[ObservableProperty]
	private double _grandTotalOrder;

	public bool HasSubTotal => SubTotal > 0;
	public bool HasShippingCosts => ShippingCosts > 0;
	public bool HasOrderCosts => OrderCosts > 0;
	public bool HasGrandTotal =>
		SubTotal > 0 &&
		GrandTotalOrder != SubTotal &&
		( ShippingCosts > 0 || OrderCosts > 0 );

	// Since we want to update the HasXXX properties when values change we can overwrite the OnPropertyChanged handlers
	partial void OnSubTotalChanged( double value )
	{
		OnPropertyChanged( nameof( HasSubTotal ) );
		UpdateCanSave();
		//CommandManager.InvalidateRequerySuggested();
	}

	partial void OnShippingCostsChanged( double value )
	{
		OnPropertyChanged( nameof( HasShippingCosts ) );
		UpdateCanSave();
		//CommandManager.InvalidateRequerySuggested();
	}

	partial void OnOrderCostsChanged( double value )
	{
		OnPropertyChanged( nameof( HasOrderCosts ) );
		UpdateCanSave();
		//CommandManager.InvalidateRequerySuggested();
	}

	partial void OnGrandTotalOrderChanged( double value )
	{
		OnPropertyChanged( nameof( HasGrandTotal ) );
		UpdateCanSave();
		//CommandManager.InvalidateRequerySuggested();
	}

	public void UpdateOrderTotals( double totalOrderCost, double shippingCosts, double orderCosts )
	{
		SubTotal = totalOrderCost;
		ShippingCosts = shippingCosts;
		OrderCosts = orderCosts;
		GrandTotalOrder = totalOrderCost + shippingCosts + orderCosts;
	}

	public void CalculateTotalOrderCost()
	{
		double subTotalOrder = 0.00;
		double shippingCosts = 0.00;
		double orderCosts = 0.00;

		if ( SelectedSupplier != 0 )
		{
			var selectedSupplier = SupplierList.FirstOrDefault(
				s => s.SupplierId == SelectedSupplier);

			if ( selectedSupplier != null )
			{
				orderCosts = selectedSupplier.SupplierOrderCosts;
			}

			// calculate subtotal for the selected products
			foreach ( var product in SelectedProducts )
			{
				subTotalOrder += product.ProductToOrder * product.SupplierPrice;
			}

			// Calculate send costs
			if ( subTotalOrder < MinShippingCosts )
			{
				shippingCosts = selectedSupplier.SupplierShippingCosts;
			}
		}

		UpdateOrderTotals( subTotalOrder, shippingCosts, orderCosts );
	}

	private SupplyOrderModel? selectedOrder;
	public SupplyOrderModel? SelectedOrder
	{
		get => selectedOrder;
		set
		{
			// First Remove event handler of the previous order
			if ( selectedOrder != null )
			{
				selectedOrder.PropertyChanged -= SelectedOrder_PropertyChanged;
			}

			if ( selectedOrder != value )
			{
				selectedOrder = value;

				if ( selectedOrder != null )
				{
					selectedOrder.PropertyChanged += SelectedOrder_PropertyChanged;
				}

				IsNewOrder = selectedOrder == null;
				OnPropertyChanged( nameof( SelectedOrder ) );
				UpdateFilteredOrderLines();
				UpdateCanSave();

				if ( selectedOrder != null )
				{
					selectedOrder.PropertyChanged += SelectedOrder_PropertyChanged;
				}

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
		if ( e.PropertyName == nameof( SupplyOrderModel.SupplyOrderNumber ) ||
			e.PropertyName == nameof( SupplyOrderModel.SupplyOrderDate ) )
		{
			UpdateCanSave();
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

				CommandManager.InvalidateRequerySuggested();
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

			CalculateTotalOrderCost();
		}
	}
	#endregion

	public event EventHandler<int>? SelectedSupplierChanged;

	private int _selectedSupplierId;
	public int SelectedSupplier
	{
		get => _selectedSupplierId;
		set
		{
			if ( _selectedSupplierId != value )
			{
				_selectedSupplierId = value;
				LoadProductsForSelectedSupplier( _selectedSupplierId );
				UpdateFilteredOrders();
				UpdateCanSave();

				// Create a new SelectedOrder if no existing order is selected
				if ( SelectedOrder == null )
				{
					SelectedOrder = new SupplyOrderModel
					{
						SupplyOrderNumber = string.Empty,
						SupplyOrderDate = null,
						SupplyOrderSupplierId = _selectedSupplierId
					};
					IsNewOrder = true;
				}
				else { IsNewOrder = false; }

				SelectedSupplierChanged?.Invoke( this, _selectedSupplierId );
			}
			else
			{
				CommandManager.InvalidateRequerySuggested();
			}
		}
	}


	public void LoadProductsForSelectedSupplier( int supplierId )
	{
		try
		{
			var products = DBCommands.GetInventoryOrder(supplierId);
			ProductList = [ .. products ];

			OnPropertyChanged( nameof( ProductList ) );
		}
		catch ( Exception ex )
		{
			throw; // Re-throw de exception om het probleem niet te verbergen
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
			FilteredOrders = [ ];
		}

		OnPropertyChanged( nameof( FilteredOrders ) );
	}

	private bool _isNewOrder = true;
	public bool IsNewOrder
	{
		get => _isNewOrder;
		set
		{
			if ( _isNewOrder != value )
			{
				_isNewOrder = value;
				OnPropertyChanged( nameof( IsNewOrder ) );
			}
		}
	}

	#region Clear all fields
	public IRelayCommand ClearSelectionCommand { get; }

	private void ClearAllFields()
	{
		// Clear checkbox selections in grid
		foreach ( var item in ProductList )
		{
			item.IsSelected = false;
		}

		// Reset other fields
		if ( selectedOrder != null )
		{
			selectedOrder.PropertyChanged -= SelectedOrder_PropertyChanged;
		}
		SelectedSupplier = 0;
		SelectedOrder = null;
		SupplyOrderNumber = string.Empty;
		SupplyOrderDate = null;
		SelectedProducts.Clear();

		if ( IsNewOrder )
		{
			// Als het een nieuwe order is, maak een nieuwe lege SelectedOrder aan
			SelectedOrder = new()
			{
				SupplyOrderNumber = string.Empty,
				SupplyOrderDate = null
			};
		}
		else
		{
			// Als het een bestaande order was, zet gewoon de velden op leeg
			if ( SelectedOrder != null )
			{
				SelectedOrder.SupplyOrderNumber = string.Empty;
				SelectedOrder.SupplyOrderDate = null;
			}
		}

		// Notify property changes
		OnPropertyChanged( nameof( ProductList ) );
		OnPropertyChanged( nameof( SelectedSupplier ) );
		UpdateCanSave();
	}
	#endregion

	#region Save Order
	public IRelayCommand SaveCommand { get; }

	private bool _canSave;
	public bool CanSave
	{
		get => _canSave;
		set => SetProperty( ref _canSave, value );
	}

	private void UpdateCanSave()
	{
		bool hasSupplier = SelectedSupplier != 0;
		bool hasOrderNumber = !string.IsNullOrWhiteSpace(SelectedOrder?.SupplyOrderNumber);
		bool hasOrderDate = SelectedOrder?.SupplyOrderDate != null;
		bool hasSelectedProducts = ProductList?.Any(p => p.IsSelected) ?? false;

		Debug.WriteLine( $"UpdateCanSave: Supplier({hasSupplier}) && OrderNumber({hasOrderNumber}) && OrderDate({hasOrderDate}) && Products({hasSelectedProducts})" );

		CanSave = hasSupplier && hasOrderNumber && hasOrderDate && hasSelectedProducts;
	}

	[RelayCommand( CanExecute = nameof( CanSave ) )]
	private void SaveOrder()
	{
		// Je bestaande save logica
		Debug.WriteLine( "Hello" );
	}
	#endregion

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
		}
		else
		{
			// Leeg de lijst als er geen order is geselecteerd
			FilteredOrderLines = new ObservableCollection<SupplyOrderLineModel>();
		}
	}

	public SupplyOrderViewModel()
	{
		ClearSelectionCommand = new RelayCommand( ClearAllFields );
		SaveCommand = new RelayCommand( SaveOrder, () => CanSave );

		SupplierList = [ .. DBCommands.GetSupplierList() ];
		SupplierOrderList = [ .. DBCommands.GetSupplierOrderList() ];
		SupplierOrderLineShortList = [ .. DBCommands.GetSupplierOrderLineShortList() ];

		SelectedProducts = [ ];

		// Ensure SelectedOrder is initialized
		if ( IsNewOrder )
		{
			SelectedOrder = new SupplyOrderModel
			{
				SupplyOrderNumber = string.Empty,
				SupplyOrderDate = null
			};
		}

		UpdateFilteredOrderLines();
		UpdateCanSave();
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