using System.ComponentModel;

using CommunityToolkit.Mvvm.Input;

namespace Modelbouwer.ViewModels;
public partial class SupplyReceiptViewModel : ObservableObject
{

	[ObservableProperty]
	public int supplyOrderId;

	[ObservableProperty]
	public int supplyOrderSupplierId;

	[ObservableProperty]
	public string orderDate;

	[ObservableProperty]
	public DateOnly? receiptDate = DateOnly.FromDateTime(DateTime.Now);

	[ObservableProperty]
	public int orderLineId;

	[ObservableProperty]
	public int productId;

	[ObservableProperty]
	public string? supplierNumber;

	[ObservableProperty]
	public string? supplierDescription;

	[ObservableProperty]
	public decimal ordered;

	[ObservableProperty]
	public decimal received;

	[ObservableProperty]
	public decimal waitFor;

	[ObservableProperty]
	public decimal stockLogReceived;

	[ObservableProperty]
	public decimal inStock;

	public ObservableCollection<SupplyReceiptModel> SupplierReceiptOrderLines { get; set; } = [ ];
	public ObservableCollection<SupplyReceiptModel> SupplierReceiptOrdersList { get; set; } = [ ];

	public event EventHandler<int>? SelectedSupplierChanged;

	#region Order Number
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
			}
		}
	}
	#endregion

	#region Order Date
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
			}
		}
	}
	#endregion

	#region Selected Order
	private SupplyReceiptModel? selectedOrder;
	public SupplyReceiptModel? SelectedOrder
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
					int orderId = selectedOrder.SupplyOrderId;
					LoadLinesForSelectedOrder( orderId );
					IsOrderSelected = true;
				}

				OnPropertyChanged( nameof( SelectedOrder ) );
				OnPropertyChanged( nameof( SupplyOrderDate ) );
				UpdateFilteredOrderLines();
			}
		}
	}
	#endregion

	#region Suppliers List
	public ObservableCollection<SupplierModel> SupplierList { get; set; } = [ ];

	#endregion

	private int _filteredOrdersCount;
	public int FilteredOrdersCount
	{
		get => _filteredOrdersCount;
		set
		{
			if ( _filteredOrdersCount != value )
			{
				_filteredOrdersCount = value;
				OnPropertyChanged( nameof( FilteredOrdersCount ) );
			}
		}
	}

	#region Order List
	//public ObservableCollection<SupplyReceiptModel> SupplierOrderList { get; set; } = [ ];

	private ObservableCollection<SupplyReceiptModel> _orderList;
	public ObservableCollection<SupplyReceiptModel> OrderList
	{
		get => _orderList;
		set
		{
			if ( _orderList != value )
			{
				if ( _orderList != null )
				{
					foreach ( SupplyReceiptModel order in _orderList )
					{
						order.PropertyChanged -= SelectedOrder_PropertyChanged;
					}
				}

				_orderList = value;

				if ( _orderList != null )
				{
					foreach ( SupplyReceiptModel order in _orderList )
					{
						order.PropertyChanged += SelectedOrder_PropertyChanged;
					}
				}

				CommandManager.InvalidateRequerySuggested();
				OnPropertyChanged( nameof( OrderList ) );
			}
		}
	}

	#region Filtered Orders   
	private ObservableCollection<SupplyReceiptModel> _filteredOrders = [];
	public ObservableCollection<SupplyReceiptModel> FilteredOrders
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
	#endregion

	private ObservableCollection<SupplyReceiptModel> _filteredReceiptLines = [];
	public ObservableCollection<SupplyReceiptModel> FilteredReceiptLines
	{
		get => _filteredReceiptLines;
		set
		{
			if ( _filteredReceiptLines != value )
			{
				_filteredReceiptLines = value;
				OnPropertyChanged( nameof( FilteredReceiptLines ) );
			}
		}
	}

	private int _selectedSupplierId;
	public int SelectedSupplier
	{
		get => _selectedSupplierId;
		set
		{
			if ( _selectedSupplierId != value )
			{
				_selectedSupplierId = value;
				UpdateFilteredOrders();
				SelectedSupplierChanged?.Invoke( this, _selectedSupplierId );
			}
			else
			{
				CommandManager.InvalidateRequerySuggested();
			}
		}
	}

	private void UpdateFilteredOrders()
	{
		if ( SelectedSupplier > 0 )
		{
			// Filter the orders for the selected supplier
			FilteredOrders = [ .. SupplierReceiptOrdersList.Where( order => order.SupplyOrderSupplierId == SelectedSupplier ) ];
		}
		else
		{
			// Clear the list if no supplier is selected
			FilteredOrders = [ ];
		}

		FilteredOrdersCount = FilteredOrders.Count;
		OnPropertyChanged( nameof( FilteredOrders ) );
	}

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
				UpdateCanSave();
			}
		}
	}


	private int _selectedOrderId;

	#region I order completely received
	private bool _isComplete;
	public bool IsComplete
	{
		get => _isComplete;
		set
		{
			if ( _isComplete != value )
			{
				_isComplete = value;
				OnPropertyChanged( nameof( IsComplete ) );
			}
		}
	}
	#endregion

	public void LoadLinesForSelectedOrder( int _orderId )
	{
		try
		{
			// Load the order lines for the selected order
			ObservableCollection<SupplyReceiptModel> orderLines = DBCommands.GetInventoryReceipt(_orderId);

			// Update the FilteredReceiptLines property
			FilteredReceiptLines = orderLines;

			// Also update OrderList if you need it
			OrderList = new ObservableCollection<SupplyReceiptModel>( orderLines );
		}
		catch ( Exception ex )
		{
			throw;
		}
	}

	public void UpdateFilteredOrderLines()
	{
		if ( SelectedOrder != null )
		{
			ObservableCollection<SupplyReceiptModel> lines = DBCommands.GetInventoryReceipt( SelectedOrder.SupplyOrderId );
			FilteredReceiptLines = lines;
		}
		else
		{
			FilteredReceiptLines = [ ];
		}
	}

	private void SelectedOrder_PropertyChanged( object? sender, PropertyChangedEventArgs e )
	{
		if ( e.PropertyName == nameof( SupplyReceiptModel.SupplyOrderNumber ) )
		{
			OnPropertyChanged( nameof( SupplyReceiptModel.SupplyOrderNumber ) );
		}
		if ( e.PropertyName == nameof( SupplyReceiptModel.SupplyOrderDate ) )
		{
			OnPropertyChanged( nameof( OrderDate ) );
		}
	}

	#region Clear all fields
	public IRelayCommand ClearSelectionCommand { get; }

	private void ClearAllFields()
	{
		// Reset other fields
		if ( selectedOrder != null )
		{
			selectedOrder.PropertyChanged -= SelectedOrder_PropertyChanged;
		}
		SelectedSupplier = 0;
		SelectedOrder = null;
		OrderDate = string.Empty;
		ReceiptDate = DateOnly.FromDateTime( DateTime.Now );
		IsComplete = false;

		// Notify property changes
		OnPropertyChanged( nameof( SelectedSupplier ) );
		OnPropertyChanged( nameof( SelectedOrder ) );
	}
	#endregion

	#region Save receipt
	private bool _canSaveReceipt;
	public bool CanSaveReceipt
	{
		get => _canSaveReceipt;
		set => SetProperty( ref _canSaveReceipt, value );
	}

	private RelayCommand _saveCommand;
	public IRelayCommand SaveCommand => _saveCommand ??= new RelayCommand(
		SaveReceipt,
		() => CanSaveReceipt );

	public void UpdateCanSave()
	{
		CanSaveReceipt = IsOrderSelected;
		( SaveCommand as RelayCommand )?.NotifyCanExecuteChanged();
	}

	private void SaveReceipt()
	{
		//Retrieve the list of products to save
		foreach ( SupplyReceiptModel line in FilteredReceiptLines )
		{
			//Saveg receipts to stocklog
			string [ , ] _registerStockLogReceipts = new string [ 5, 3 ]
			{
				{ DBNames.StocklogFieldNameProductId, DBNames.StocklogFieldTypeProductId, line.ProductId.ToString() },
				{ DBNames.StocklogFieldNameSupplyOrderId, DBNames.StocklogFieldTypeSupplyOrderId, SelectedOrder.SupplyOrderId.ToString() },
				{ DBNames.StocklogFieldNameSupplyOrderlineId, DBNames.StocklogFieldTypeSupplyOrderlineId, line.OrderLineId.ToString() },
				{ DBNames.StocklogFieldNameAmountReceived, DBNames.StocklogFieldTypeAmountReceived, line.WaitFor.ToString() },
				{ DBNames.StocklogFieldNameLogDate, DBNames.StocklogFieldTypeLogDate, (ReceiptDate ?? DateOnly.FromDateTime(DateTime.Now)).ToString() }
			};
			DBCommands.InsertInTable( DBNames.StocklogTable, _registerStockLogReceipts );

			//Update receipts to supplyorderline
			decimal _openAmount = Math.Max(0, line.Ordered - line.StockLogReceived - line.WaitFor);
			int _orderLineClosed = _openAmount == 0 ? 1 : 0;

			string [ , ] _registerSupplyOrderLineReceipts = new string [ 3, 3 ]
			{
				{ DBNames.OrderLineFieldNameOpenAmount, DBNames.OrderLineFieldTypeOpenAmount, _openAmount.ToString() },
				{ DBNames.OrderLineFieldNameClosed, DBNames.OrderLineFieldTypeClosed, _orderLineClosed.ToString() },
				{ DBNames.OrderLineFieldNameClosedDate, DBNames.OrderLineFieldTypeClosedDate, (ReceiptDate ?? DateOnly.FromDateTime(DateTime.Now)).ToString() },
			};

			string [ , ] _whereSupplyOrderLineReceipts = new string [ 1, 3 ]
			{
				{ DBNames.OrderLineFieldNameId, DBNames.OrderLineFieldTypeId, line.OrderLineId.ToString() }
			};
			DBCommands.UpdateInTable( DBNames.OrderLineTable, _registerSupplyOrderLineReceipts, _whereSupplyOrderLineReceipts );

			//Update orderstatus in supplyorder when IsComplete == true
			if ( IsComplete )
			{
				int _orderClosed = IsComplete == true ? 1 : 0;

				string [ , ] _registerOrderComplete = new string [ 2, 3 ]
				{
					{ DBNames.OrderFieldNameClosed, DBNames.OrderFieldTypeClosed, _orderClosed.ToString() },
					{ DBNames.OrderFieldNameClosedDate, DBNames.OrderFieldTypeClosedDate, (ReceiptDate ?? DateOnly.FromDateTime(DateTime.Now)).ToString() },
				};

				string [ , ] _whereOrderComplete = new string [ 1, 3 ]
				{
					{ DBNames.OrderFieldNameId, DBNames.OrderFieldTypeId, SelectedOrder.SupplyOrderId.ToString() }
				};
				DBCommands.UpdateInTable( DBNames.OrderTable, _registerOrderComplete, _whereOrderComplete );
			}

		}

		// Refresh the data for the UI
		ClearAllFields();
		Refresh();
	}
	#endregion

	public ResourceDictionary? ResourceDictionary { get; set; }

	// Constructor
	public SupplyReceiptViewModel()
	{
		ResourceDictionary = new ResourceDictionary
		{
			Source = new Uri( "pack://application:,,,/Modelbouwer;component/Resources/Languages/Language.xaml" )

		};

		ClearSelectionCommand = new RelayCommand( ClearAllFields );
	}

	public void Initialize()
	{
		Refresh();
	}

	public void Refresh()
	{
		SupplierList = [ .. DBCommands.GetSupplierList() ];
		SupplierReceiptOrdersList = [ .. DBCommands.GetSupplierReceiptOrders() ];
		SupplierReceiptOrderLines = [ .. DBCommands.GetSupplierReceiptLines() ];

		UpdateFilteredOrderLines();

		OnPropertyChanged( nameof( SupplierList ) );
		OnPropertyChanged( nameof( SupplierReceiptOrdersList ) );
		OnPropertyChanged( nameof( SupplierReceiptOrderLines ) );
	}

}
