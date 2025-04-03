namespace Modelbouwer.ViewModels;
public partial class ReceiptsReportViewModel : ObservableObject
{
	[ObservableProperty]
	public DateOnly orderDate;

	[ObservableProperty]
	public string? orderNumber;

	[ObservableProperty]
	public string? supplier;

	[ObservableProperty]
	public string? shortname;

	[ObservableProperty]
	public string? description;

	[ObservableProperty]
	public decimal ordered;

	[ObservableProperty]
	public DateOnly receivedDate;

	public string? receivedDateString;

	[ObservableProperty]
	public decimal received;

	[ObservableProperty]
	public int isOrderLine;

	[ObservableProperty]
	public int rowClosed;

	[ObservableProperty]
	public bool rowClosedCheck;

	[ObservableProperty]
	public string rowClosedDae;

	private ObservableCollection<ReceiptsReportModel>? _receiptsReport;

	public ObservableCollection<ReceiptsReportModel> ReceiptsReport
	{
		get => _receiptsReport;
		set
		{
			if ( _receiptsReport != value )
			{
				_receiptsReport = value;
				OnPropertyChanged( nameof( ReceiptsReport ) );
			}
		}
	}

	public ReceiptsReportViewModel()
	{
		Refresh();
	}

	public void Refresh()
	{
		ReceiptsReport = new ObservableCollection<ReceiptsReportModel>( DBCommands.GetAllReceipts() );
		OnPropertyChanged( nameof( ReceiptsReport ) );
	}
}
