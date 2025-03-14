using System.ComponentModel;

namespace Modelbouwer.ViewModels;
public partial class SupplyReceiptViewModel : ObservableObject
{

	[ObservableProperty]
	public int orderId;
	[ObservableProperty]
	public string orderNumber;
	[ObservableProperty]
	public string orderDate;
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
	public decimal waitFor;
	[ObservableProperty]
	public decimal stockLogReceived;
	[ObservableProperty]
	public decimal inStock;
	public ObservableCollection<SupplyReceiptModel> SupplierReceiptOrderLines { get; set; } = [ ];
	public ObservableCollection<SupplyOrderModel> SupplierReceiptOrdersList { get; set; } = [ ];

	public event EventHandler<int>? SelectedSupplierChanged;

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

				OnPropertyChanged( nameof( OrderId ) );
				UpdateFilteredOrderLines();
			}

			// Since selectedOrder.SupplyOrderDate is a DateOnly, we need to convert it to a DateTime so it works with the DatePicker
			if ( selectedOrder != null )
			{
				//orderDate = selectedOrder.SupplyOrderDate?.ToDateTime( TimeOnly.MinValue );
			}
		}
	}

	private int _selectedOrderId;

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

	public void LoadLinesForSelectedOrder( int _orderId )
	{
		try
		{
			// Load the order lines for the selected order
			ObservableCollection<SupplyReceiptModel> orderLines = DBCommands.GetInventoryReceipt(orderId);

			// Update the FilteredReceiptLines property
			FilteredReceiptLines = orderLines;

			// Also update OrderList if you need it
			OrderList = new ObservableCollection<SupplyReceiptModel>( orderLines );
		}
		catch ( Exception ex )
		{
			Debug.WriteLine( $"Error loading order lines: {ex.Message}" );
			throw; // Re-throw de exception om het probleem niet te verbergen
		}
	}

	private void UpdateFilteredOrderLines()
	{
		if ( SelectedOrder != null )
		{
			Debug.WriteLine( "Filter de orders voor deze leverancier" );
			FilteredReceiptLines = DBCommands.GetInventoryReceipt( SelectedOrder.SupplyOrderId );
			// Filter de regels op basis van het geselecteerde OrderId
			//FilteredReceiptLines = new ObservableCollection<SupplyOrderLineModel>(
			//	SupplierOrderLineShortList.Where( line => line.SupplyOrderlineShortOrderId == SelectedOrder.SupplyOrderId )
			//);
		}
		else
		{
			// Leeg de lijst als er geen order is geselecteerd
			FilteredReceiptLines = [ ];
		}
	}

	private void SelectedOrder_PropertyChanged( object? sender, PropertyChangedEventArgs e )
	{
		if ( e.PropertyName == nameof( SupplyOrderModel.SupplyOrderNumber ) )
		{
			OnPropertyChanged( nameof( OrderNumber ) );
		}
		if ( e.PropertyName == nameof( SupplyOrderModel.SupplyOrderDate ) )
		{
			OnPropertyChanged( nameof( OrderDate ) );
		}
	}

	public ResourceDictionary? ResourceDictionary { get; set; }

	// Constructor
	public SupplyReceiptViewModel()
	{
		// Standaardwaarde instellen
		//SelectedOrderId = 0;
		ResourceDictionary = new ResourceDictionary
		{
			Source = new Uri( "pack://application:,,,/Modelbouwer;component/Resources/Languages/Language.xaml" )

		};

		SupplierReceiptOrdersList = [ .. DBCommands.GetSupplierReceiptOrders() ];
		SupplierReceiptOrderLines = [ .. DBCommands.GetSupplierReceiptLines() ];


		UpdateFilteredOrderLines();
	}

	public void Refresh()
	{
		SupplierReceiptOrdersList = [ .. DBCommands.GetSupplierReceiptOrders() ];
		SupplierReceiptOrderLines = [ .. DBCommands.GetSupplierReceiptLines() ];
		OnPropertyChanged( nameof( SupplierReceiptOrdersList ) );
		OnPropertyChanged( nameof( SupplierReceiptOrderLines ) );
	}

}
