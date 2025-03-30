namespace Modelbouwer.ViewModels;
public partial class OrderlineReportViewModel : ObservableObject
{
	[ObservableProperty]
	public int orderId;

	[ObservableProperty]
	public string? orderNumber;

	[ObservableProperty]
	public DateOnly orderDate;

	[ObservableProperty]
	public int supplierId;

	[ObservableProperty]
	public string? supplierName;

	[ObservableProperty]
	public int productId;

	[ObservableProperty]
	public string? productCode;

	[ObservableProperty]
	public string? productName;

	[ObservableProperty]
	public decimal unitPrice;

	[ObservableProperty]
	public decimal ordered;

	[ObservableProperty]
	public decimal received;

	[ObservableProperty]
	public decimal expected;

	[ObservableProperty]
	public int closed;

	[ObservableProperty]
	public DateOnly closedDate;

	private ObservableCollection<OrderlineReportModel>? _orderlineReport;

	public ObservableCollection<OrderlineReportModel> OrderlineReport
	{
		get => _orderlineReport;
		set
		{
			if ( _orderlineReport != value )
			{
				_orderlineReport = value;
				OnPropertyChanged( nameof( OrderlineReport ) );
			}
		}
	}

	public OrderlineReportViewModel()
	{
		Refresh();
	}

	public void Refresh()
	{
		OrderlineReport = new ObservableCollection<OrderlineReportModel>( DBCommands.GetAllOrders() );
		OnPropertyChanged( nameof( OrderlineReport ) );
	}

}
