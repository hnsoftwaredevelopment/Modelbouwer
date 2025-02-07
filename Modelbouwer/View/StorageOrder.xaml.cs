namespace Modelbouwer.View;

/// <summary>
/// Interaction logic for StorageOrder.xaml
/// </summary>
public partial class StorageOrder : Page
{
	double minShippingCosts= 0.00;

	/// <summary>
	/// Initializes a new instance of the <see cref="StorageOrder"/> class.
	/// </summary>
	public StorageOrder()
	{
		InitializeComponent();
		DataContext = new CombinedInventoryOrderViewModel();
	}

	#region Switch between search and filter button
	/// <summary>
	/// Toggles between the search and filter buttons, updating the visibility and tooltips accordingly.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	private void ToggleButton( object sender, RoutedEventArgs e )
	{
		var name = ((FrameworkElement)sender).Name;

		if ( !string.IsNullOrEmpty( name ) )
		{
			switch ( name )
			{
				case "FilterButton":
					// Current button is FilterButton, toggle to SearchButton
					FilterButton.Visibility = Visibility.Collapsed;
					SearchButton.Visibility = Visibility.Visible;
					FilterSearchText.Tag = ( string ) FindResource( "Edit.Order.DataGrid.FilterSearch.Search.Tag" );
					FilterSearchText.ToolTip = ( string ) FindResource( "Edit.Order.DataGrid.FilterSearch.Search.Tooltip" );
					ClearFilterSearch.ToolTip = ( string ) FindResource( "Edit.Order.DataGrid.FilterSearch.Search.Clear.Tooltip" );
					// Toggle filtering to show result of toggle directly in datagrid
					dataGrid.SearchHelper.AllowFiltering = false;
					break;
				case "SearchButton":
					// Current button is SearchButton, toggle to FilterButton
					SearchButton.Visibility = Visibility.Collapsed;
					FilterButton.Visibility = Visibility.Visible;
					FilterSearchText.Tag = ( string ) FindResource( "Edit.Order.DataGrid.FilterSearch.Filter.Tag" );
					FilterSearchText.ToolTip = ( string ) FindResource( "Edit.Order.DataGrid.FilterSearch.Filter.Tooltip" );
					ClearFilterSearch.ToolTip = ( string ) FindResource( "Edit.Order.DataGrid.FilterSearch.Filter.Clear.Tooltip" );
					// Toggle filtering to show result of toggle directly in datagrid
					dataGrid.SearchHelper.AllowFiltering = true;
					break;
			}
			// Show result of toggle directly in datagrid
			dataGrid.SearchHelper.Search( FilterSearchText.Text );
		}
	}
	#endregion

	#region Filter or search the datagrid
	/// <summary>
	/// Filters or searches the datagrid based on the text entered in the search/filter box.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	private void FilterSearch( object sender, TextChangedEventArgs e )
	{
		#region When text is entered in the search/filter box it should be possible to clear it, therefore the clear icon has to become visible
		ClearFilterSearch.Visibility = FilterSearchText.Text.Length > 0
			? Visibility.Visible
			: Visibility.Collapsed;
		#endregion

		var action = SearchButton.Visibility == Visibility.Visible
		? "search"
		: "filter";

		switch ( action )
		{
			case "search":
				dataGrid.SearchHelper.AllowFiltering = false;
				break;
			case "filter":
				dataGrid.SearchHelper.AllowFiltering = true;
				break;
		}
		dataGrid.SearchHelper.Search( FilterSearchText.Text );
	}
	#endregion

	#region Clear the search/filter field
	/// <summary>
	/// Clears the text in the search/filter field.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	private void ClearText( object sender, RoutedEventArgs e )
	{
		FilterSearchText.Clear();
	}
	#endregion

	/// <summary>
	/// Handles the selection of an order in the datagrid and updates the view model accordingly.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	private void OrderSelected( object sender, System.Windows.Controls.SelectionChangedEventArgs e )
	{
		var viewModel = (CombinedInventoryOrderViewModel)this.DataContext;

		foreach ( var line in viewModel.SupplyOrderViewModel.SupplierOrderLineShortList )
		{
			var _productId = line.SupplyOrderlineShortProductId;

			var records = dataGrid.View.Records;

			foreach ( var record in records )
			{
				var dataRow = record.Data as SupplyOrderLineModel;
				if ( line.SupplyOrderlineShortProductId == ( ( Modelbouwer.Model.InventoryOrderModel ) record.Data ).ProductId )
				{
					( ( Modelbouwer.Model.InventoryOrderModel ) record.Data ).IsSelected = true;
					( ( Modelbouwer.Model.InventoryOrderModel ) record.Data ).ProductShortInventory = line.SupplyOrderlineShortAmount;
				}
			}
		}
		CalculateTotalOrderCost();
	}

	/// <summary>
	/// Handles the selection change event in the selected products datagrid and recalculates the total order cost.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	private void SelectedProductsDataGrid_SelectionChanged( object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs e )
	{
		CalculateTotalOrderCost();
	}

	/// <summary>
	/// Handles the cell edit end event in the selected products datagrid and recalculates the total order cost.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	private void SelectedProductsDataGrid_CellEditEnd( object sender, Syncfusion.UI.Xaml.Grid.CurrentCellEndEditEventArgs e )
	{
		CalculateTotalOrderCost();
	}

	/// <summary>
	/// Calculates the total cost of the order based on the selected products and updates the total order cost text.
	/// </summary>
	private void CalculateTotalOrderCost()
	{
		var viewModel = (CombinedInventoryOrderViewModel)this.DataContext;
		var selectedSupplier = viewModel.SupplyOrderViewModel.SupplierList.FirstOrDefault(s => s.SupplierId == viewModel.SupplyOrderViewModel.SelectedSupplier);

		double subTotalOrder = 0.00;
		double shippingCosts = 0.00;
		double orderCosts = 0.00;

		if ( selectedSupplier != null )
		{
			orderCosts = selectedSupplier.SupplierOrderCosts;
		}

		if ( selectedProductsDataGrid.View?.Records != null )
		{
			foreach ( var record in selectedProductsDataGrid.View.Records )
			{
				var inventoryOrder = (Modelbouwer.Model.InventoryOrderModel)record.Data;
				subTotalOrder += inventoryOrder.ProductShortInventory * inventoryOrder.SupplierPrice;
			}

			if ( subTotalOrder < minShippingCosts )
			{
				shippingCosts = selectedSupplier.SupplierShippingCosts;
			}
		}

		viewModel.SupplyOrderViewModel.UpdateOrderTotals( subTotalOrder, shippingCosts, orderCosts );
	}

	/// <summary>
	/// Update Shipping and order costs for selected supplier, since there are no orderlines (yet) SubTotal will always be 0
using System.Diagnostics.Eventing.Reader;

namespace Modelbouwer.View;

/// <summary>
/// Interaction logic for CurrencyManagement.xaml
/// </summary>
public partial class CurrencyManagement : Page
{
	/// <summary>
	/// Initializes a new instance of the <see cref="CurrencyManagement"/> class.
	/// </summary>
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	private void SupplierSelectionChanged( object sender, SelectionChangedEventArgs e )
	{
		var viewModel = (CombinedInventoryOrderViewModel)this.DataContext;
		var selectedSupplier = viewModel.SupplyOrderViewModel.SupplierList.FirstOrDefault(s => s.SupplierId == viewModel.SupplyOrderViewModel.SelectedSupplier);
		if ( selectedSupplier != null )
		{
			var shippingCosts = selectedSupplier.SupplierShippingCosts;
			minShippingCosts = selectedSupplier.SupplierMinShippingCosts;
			var orderCosts = selectedSupplier.SupplierOrderCosts;

			viewModel.SupplyOrderViewModel.UpdateOrderTotals( 0, shippingCosts, orderCosts );
		}
	}
}

/// <summary>
/// ViewModel for combined inventory and order data.
/// </summary>
public class CombinedInventoryOrderViewModel
{
	/// <summary>
	/// Gets or sets the supply order view model.
	/// </summary>
	public SupplyOrderViewModel SupplyOrderViewModel { get; set; }

	/// <summary>
	/// Initializes a new instance of the <see cref="CombinedInventoryOrderViewModel"/> class.
	/// </summary>
	public CombinedInventoryOrderViewModel()
	{
		SupplyOrderViewModel = new();
	}
}

/// <summary>
/// ViewModel for supply orders.
/// </summary>
public partial class SupplyOrderViewModel : ObservableObject
{
	// Properties with ObservableProperty attribute
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

	// Additional properties
	public bool HasSubTotal => SubTotal > 0;
	public bool HasShippingCosts => ShippingCosts > 0;
	public bool HasOrderCosts => OrderCosts > 0;

	// Event handlers for property changes
	partial void OnSubTotalChanged( double value )
	{
		OnPropertyChanged( nameof( HasSubTotal ) );
	}

	partial void OnShippingCostsChanged( double value )
	{
		OnPropertyChanged( nameof( HasShippingCosts ) );
	}

	partial void OnOrderCostsChanged( double value )
	{
		OnPropertyChanged( nameof( HasOrderCosts ) );
	}

	/// <summary>
	/// Updates the order totals.
	/// </summary>
	/// <param name="totalOrderCost">The total order cost.</param>
	/// <param name="shippingCosts">The shipping costs.</param>
	/// <param name="orderCosts">The order costs.</param>
	public void UpdateOrderTotals( double totalOrderCost, double shippingCosts, double orderCosts )
	{
		SubTotal = totalOrderCost;
		ShippingCosts = shippingCosts;
		OrderCosts = orderCosts;
		GrandTotalOrder = totalOrderCost + shippingCosts + orderCosts;
	}

	// Other properties and methods
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

	#region Selected Products
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
		// Load products for the selected supplier
		ProductList = [ .. DBCommands.GetInventoryOrder( supplierId ) ];
		OnPropertyChanged( nameof( ProductList ) );
	}

	private void UpdateFilteredOrders()
	{
		if ( SelectedSupplier > 0 )
		{
			// Filter orders for the selected supplier
			FilteredOrders = [ .. SupplierOrderList.Where( order => order.SupplyOrderSupplierId == SelectedSupplier ) ];
		}
		else
		{
			// Clear the list if no supplier is selected
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

	#region Filter the order lines based on the selected OrderId
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
			// Filter lines based on the selected OrderId
			FilteredOrderLines = new ObservableCollection<SupplyOrderLineModel>(
				SupplierOrderLineShortList.Where( line => line.SupplyOrderlineShortOrderId == SelectedOrder.SupplyOrderId )
			);
		}
		else
		{
			// Clear the list if no order is selected
			FilteredOrderLines = new ObservableCollection<SupplyOrderLineModel>();
		}
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="SupplyOrderViewModel"/> class.
	/// </summary>
	public SupplyOrderViewModel()
	{
		SupplierList = [ .. DBCommands.GetSupplierList() ];
		SupplierOrderList = [ .. DBCommands.GetSupplierOrderList() ];
		SupplierOrderLineShortList = [ .. DBCommands.GetSupplierOrderLineShortList() ];

		SelectedProducts = [ ];

		UpdateFilteredOrderLines();
	}
}

// Select the product lines in the datagrid
/// <summary>
/// ViewModel for supply order lines.
/// </summary>
public class SupplyOrderLineViewModel : ObservableObject
{
	/// <summary>
	/// Gets the model for the supply order line.
	/// </summary>
	public SupplyOrderLineModel Model { get; }

	/// <summary>
	/// Initializes a new instance of the <see cref="SupplyOrderLineViewModel"/> class.
	/// </summary>
	/// <param name="model">The model for the supply order line.</param>
	public SupplyOrderLineViewModel( SupplyOrderLineModel model )
	{
		Model = model;
	}

	/// <summary>
	/// Gets or sets a value indicating whether the supply order line is selected.
	/// </summary>
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
