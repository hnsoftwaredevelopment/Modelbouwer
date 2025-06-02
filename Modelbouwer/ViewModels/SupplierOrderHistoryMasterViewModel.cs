namespace Modelbouwer.ViewModels;
public partial class SupplierOrderHistoryMasterViewModel : ObservableObject
{
	[ObservableProperty]
	private ObservableCollection<OrderHeaderModel> _groupedOrderHistory;

	public SupplierOrderHistoryMasterViewModel( int supplierId = 0 )
	{
		GroupedOrderHistory = DBCommands.GetOrderHistoryHierarchical( supplierId );
	}

	public void Refresh( int supplierId )
	{
		GroupedOrderHistory = DBCommands.GetOrderHistoryHierarchical( supplierId );
		OnPropertyChanged( nameof( GroupedOrderHistory ) );
	}
}
