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
			if ( SelectedOrder != null )
			{
				if ( SelectedOrder.SupplyOrderNumber != value )
				{
					SelectedOrder.SupplyOrderNumber = value;
					OnPropertyChanged( nameof( SupplyOrderNumber ) );
				}
				UpdateCanSave();
			}
		}
	}

	public DateTime? SupplyOrderDate
	{
		get => SelectedOrder?.SupplyOrderDate.HasValue == true ? SelectedOrder.SupplyOrderDate.Value.ToDateTime( TimeOnly.MinValue ) : null;
		set
		{
			if ( SelectedOrder != null )
			{
				if ( value.HasValue )
				{
					SelectedOrder.SupplyOrderDate = DateOnly.FromDateTime( value.Value );
				}
				else
				{
					SelectedOrder.SupplyOrderDate = null;
				}
				OnPropertyChanged( nameof( SupplyOrderDate ) );
				UpdateCanSave();
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

	partial void OnShippingCostsChanged( double value )
	{
		OnPropertyChanged( nameof( HasShippingCosts ) );
		UpdateCanSave();
		CalculateTotalOrderCost();
	}

	partial void OnOrderCostsChanged( double value )
	{
		OnPropertyChanged( nameof( HasOrderCosts ) );
		UpdateCanSave();
		CalculateTotalOrderCost();
	}

	public bool HasSubTotal => SubTotal > 0;
	public bool HasShippingCosts => ShippingCosts > 0;
	public bool HasOrderCosts => OrderCosts > 0;
	public bool HasGrandTotal =>
		SubTotal > 0 &&
		GrandTotalOrder != SubTotal &&
		( ShippingCosts > 0 || OrderCosts > 0 );

	partial void OnSubTotalChanged( double value )
	{
		OnPropertyChanged( nameof( HasSubTotal ) );
		UpdateCanSave();
	}

	partial void OnGrandTotalOrderChanged( double value )
	{
		OnPropertyChanged( nameof( HasGrandTotal ) );
		UpdateCanSave();
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
		double subTotalOrder = 0;
		double shippingCosts = ShippingCosts;
		double orderCosts = OrderCosts;

		if ( SelectedSupplier != 0 )
		{
			SupplierModel? selectedSupplier = SupplierList.FirstOrDefault(
				s => s.SupplierId == SelectedSupplier );

			if ( selectedSupplier != null )
			{
				// Only set orderCosts if it hasn't been manually set
				if ( orderCosts == 0 )
				{
					orderCosts = selectedSupplier.SupplierOrderCosts;
				}
			}

			// calculate subtotal for the selected products
			foreach ( InventoryOrderModel product in SelectedProducts )
			{
				subTotalOrder += product.ProductToOrder * product.SupplierPrice;
			}

			// Calculate send costs only if it hasn't been manually set
			if ( shippingCosts == 0 && subTotalOrder < selectedSupplier.SupplierMinShippingCosts )
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
			if ( selectedOrder != null )
			{
				selectedOrder.PropertyChanged -= SelectedOrder_PropertyChanged;
			}

			if ( SetProperty( ref selectedOrder, value ) )
			{
				if ( selectedOrder != null )
				{
					selectedOrder.PropertyChanged += SelectedOrder_PropertyChanged;
				}

				IsNewOrder = selectedOrder == null;
				OnPropertyChanged( nameof( SelectedOrder ) );
				UpdateFilteredOrderLines();
				UpdateCanSave();
				UpdateCanDelete();
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
					foreach ( InventoryOrderModel order in _orderList )
					{
						order.PropertyChanged -= SelectedOrder_PropertyChanged;
					}
				}

				_orderList = value;

				if ( _orderList != null )
				{
					foreach ( InventoryOrderModel order in _orderList )
					{
						order.PropertyChanged += SelectedOrder_PropertyChanged;
					}
				}

				OnPropertyChanged( nameof( OrderList ) );
			}
		}
	}

	private void SelectedOrder_PropertyChanged( object? sender, PropertyChangedEventArgs e )
	{
		if ( e.PropertyName == nameof( SupplyOrderModel.SupplyOrderNumber ) )
		{
			OnPropertyChanged( nameof( SupplyOrderNumber ) );
		}
		if ( e.PropertyName == nameof( SupplyOrderModel.SupplyOrderDate ) )
		{
			OnPropertyChanged( nameof( SupplyOrderDate ) );
		}
		UpdateCanSave();
		UpdateCanDelete();
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
					foreach ( InventoryOrderModel product in _productList )
					{
						product.PropertyChanged -= SelectedProduct_PropertyChanged;
					}
				}

				_productList = value;

				if ( _productList != null )
				{
					foreach ( InventoryOrderModel product in _productList )
					{
						product.PropertyChanged += SelectedProduct_PropertyChanged;
					}
				}

				CommandManager.InvalidateRequerySuggested();
				OnPropertyChanged( nameof( ProductList ) );
			}
		}
	}

	private void SelectedProduct_PropertyChanged( object? sender, PropertyChangedEventArgs e )
	{
		if ( e.PropertyName == nameof( SupplyOrderModel.IsSelected ) )
		{
			InventoryOrderModel? selectedProduct = sender as InventoryOrderModel;
			if ( selectedProduct != null )
			{
				if ( selectedProduct.IsSelected && !SelectedProducts.Contains( selectedProduct ) )
				{
					SelectedProducts.Add( selectedProduct );
				}
				else if ( !selectedProduct.IsSelected && SelectedProducts.Contains( selectedProduct ) )
				{
					SelectedProducts.Remove( selectedProduct );
				}
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
				else { IsNewOrder = false; UpdateCanDelete(); }

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
			ObservableCollection<InventoryOrderModel> products = DBCommands.GetInventoryOrder( supplierId );
			ProductList = [ .. products ];

			OnPropertyChanged( nameof( ProductList ) );
		}
		catch ( Exception )
		{
			throw; // Re-throw de exception om het probleem niet te verbergen
		}
	}

	private void UpdateFilteredOrders()
	{
		if ( SelectedSupplier > 0 )
		{
			// Filter the orders for the selected supplier
			FilteredOrders = new ObservableCollection<SupplyOrderModel>(
				SupplierOrderList.Where( order => order.SupplyOrderSupplierId == SelectedSupplier )
			);
		}
		else
		{
			// Clear the list if no supplier is selected
			FilteredOrders = new ObservableCollection<SupplyOrderModel>();
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
		foreach ( InventoryOrderModel item in ProductList )
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
		ShippingCosts = 0;
		OrderCosts = 0;

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

	[RelayCommand( CanExecute = nameof( CanDelete ) )]
	private void DeleteOrder()
	{
		Debug.WriteLine( "DeleteOrder" );
		//if ( SelectedOrder != null )
		//{
		//	DBCommands.DeleteOrder( SelectedOrder.SupplyOrderId );
		//	Refresh();
		//}
	}


	#region Save Order
	public IRelayCommand SaveCommand { get; }

	private bool _canSave;
	public bool CanSave
	{
		get => _canSave;
		set
		{
			if ( _canSave != value )
			{
				_canSave = value;
				OnPropertyChanged( nameof( CanSave ) );
				SaveCommand.NotifyCanExecuteChanged();
			}
		}
	}

	private void UpdateCanSave()
	{
		bool hasSupplier = SelectedSupplier != 0;
		bool hasOrderNumber = !string.IsNullOrWhiteSpace(SelectedOrder?.SupplyOrderNumber);
		bool hasOrderDate = SelectedOrder?.SupplyOrderDate != null;
		bool hasSelectedProducts = ProductList?.Any(p => p.IsSelected) ?? false;

		CanSave = IsNewOrder && hasSupplier && hasOrderNumber && hasOrderDate && hasSelectedProducts;
	}

	//public bool IsOrderSelected => SelectedOrder != null;
	private bool _isOrderSelected;
	public bool IsOrderSelected
	{
		get => _isOrderSelected;
		set
		{
			if ( _isOrderSelected != value )
			{
				_isOrderSelected = value;
				OnPropertyChanged( nameof( IsOrderSelected ) );
				UpdateCanDelete();
			}
		}
	}

	private bool _canDelete;
	public bool CanDelete
	{
		get => _canDelete;
		set
		{
			if ( _canDelete != value )
			{
				_canDelete = value;
				OnPropertyChanged( nameof( CanDelete ) );
				DeleteOrderCommand.NotifyCanExecuteChanged();
			}
		}
	}

	private void UpdateCanDelete()
	{
		CanDelete = IsOrderSelected;
	}


	[RelayCommand( CanExecute = nameof( CanSave ) )]
	private void SaveOrder()
	{
		#region Check if all the selected products are in the Products Per supplier table, if one is missing it should be added to that table
		// Retrieve the current list of product IDs from the database
		ObservableCollection<ProductModel> currentList = DBCommands.GetProductsForSupplierList(_selectedSupplierId);
		List<int> currentProductIds = currentList.Select( p => p.ProductId ).ToList();

		// Retrieve the list of selected product IDs from the selectedProductsDataGrid
		List<InventoryOrderModel> selectedProducts = SelectedProducts.ToList();

		// Find the product IDs that are in selectedProducts but not in currentProductIds
		List<InventoryOrderModel> newProducts = selectedProducts.Where( p => !currentProductIds.Contains( p.ProductId ) ).ToList();

		// Add the product to the ProductPerSupplier table with additional information
		foreach ( InventoryOrderModel? product in newProducts )
		{
			string [ , ] _addProductSupplierfields = new string [ 5, 3 ]
			{
				{ DBNames.ProductSupplierFieldNameSupplierId, DBNames.ProductSupplierFieldTypeSupplierId, _selectedSupplierId.ToString()  },
				{ DBNames.ProductSupplierFieldNameProductId, DBNames.ProductSupplierFieldTypeProductId, product.ProductId.ToString() },
				{ DBNames.ProductSupplierFieldNameProductNumber, DBNames.ProductSupplierFieldTypeProductNumber, product.SupplierProductNumber },
				{ DBNames.ProductSupplierFieldNameProductName, DBNames.ProductSupplierFieldTypeProductName, product.SupplierProductName },
				{ DBNames.ProductSupplierFieldNamePrice, DBNames.ProductSupplierFieldTypePrice, product.SupplierPrice.ToString() }
			};

			DBCommands.InsertInTable( DBNames.ProductSupplierTable, _addProductSupplierfields );
		}
		#endregion

		#region Add general Order information to the Order Table
		string [ , ] _addOrderFields = new string [ 6, 3 ]
		{
			{DBNames.OrderFieldNameSupplierId, DBNames.OrderFieldTypeSupplierId, _selectedSupplierId.ToString()},
			{DBNames.OrderFieldNameCurrencyId, DBNames.OrderFieldTypeCurrencyId, SupplyOrderCurrencyId.ToString()},
			{DBNames.OrderFieldNameOrderNumber, DBNames.OrderFieldTypeOrderNumber, SelectedOrder.SupplyOrderNumber},
			{DBNames.OrderFieldNameOrderDate, DBNames.OrderFieldTypeOrderDate, SelectedOrder.SupplyOrderDate.ToString()},
			{DBNames.OrderFieldNameShippingCosts, DBNames.OrderFieldTypeShippingCosts, ShippingCosts.ToString()},
			{DBNames.OrderFieldNameOrderCosts, DBNames.OrderFieldTypeOrderCosts, OrderCosts.ToString()}
		};
		DBCommands.InsertInTable( DBNames.OrderTable, _addOrderFields );
		#endregion

		// Retrieve the Id of the latest added order
		int _orderId = DBCommands.GetLatestAddedId(DBNames.OrderTable, DBNames.OrderFieldNameId);

		#region Add Order lines to the Orderline table
		// To trough the rows of the slected products datagrid
		foreach ( InventoryOrderModel line in selectedProducts )
		{
			string [ , ] _addOrderLineFields = new string [ 5, 3 ]
			{
				{DBNames.OrderLineFieldNameOrderId, DBNames.OrderLineFieldTypeOrderId, _orderId.ToString()},
				{DBNames.OrderLineFieldNameProductId, DBNames.OrderLineFieldTypeProductId, line.ProductId.ToString()},
				{DBNames.OrderLineFieldNameAmount, DBNames.OrderLineFieldTypeAmount, line.ProductToOrder.ToString()},
				{DBNames.OrderLineFieldNameOpenAmount, DBNames.OrderLineFieldTypeOpenAmount, line.ProductToOrder.ToString()},
				{DBNames.OrderLineFieldNamePrice, DBNames.OrderLineFieldTypePrice, line.SupplierPrice.ToString()}
			};

			DBCommands.InsertInTable( DBNames.OrderLineTable, _addOrderLineFields );
		}
		#endregion


		// Refresh the data for the UI
		ClearAllFields();
		Refresh();
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

	public void Refresh()
	{
		SupplierList = [ .. DBCommands.GetSupplierList() ];
		SupplierOrderList = [ .. DBCommands.GetSupplierOrderList() ];
		SupplierOrderLineShortList = [ .. DBCommands.GetSupplierOrderLineShortList() ];
		OnPropertyChanged( nameof( SupplierList ) );
		OnPropertyChanged( nameof( SupplierOrderList ) );
		OnPropertyChanged( nameof( SupplierOrderLineShortList ) );
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
