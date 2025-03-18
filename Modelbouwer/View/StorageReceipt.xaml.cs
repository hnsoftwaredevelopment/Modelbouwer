namespace Modelbouwer.View;

/// <summary>
/// Interaction logic for StorageReceipt.xaml
/// </summary>
public partial class StorageReceipt : Page
{
	public StorageReceipt()
	{
		InitializeComponent();
	}

	private void ReceiptLinesDataGrid_CellEditEnd( object sender, Syncfusion.UI.Xaml.Grid.CurrentCellEndEditEventArgs e )
	{
		// done with editing the line 
		//Recalculate totals
	}

	private void ReceiptLinesDataGrid_SelectionChanged( object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs e )
	{
		// Row changed in datagrid
	}

	private void SupplierSelectionChanged( object sender, SelectionChangedEventArgs e )
	{
		CombinedInventoryOrderViewModel? viewModel = DataContext as CombinedInventoryOrderViewModel;
		viewModel.SupplyReceiptViewModel.IsOrderSelected = false;
		//SupplierModel? selectedSupplier = viewModel.SupplyReceiptViewModel.SupplierList.FirstOrDefault( s => s.SupplierId == viewModel.SupplyReceiptViewModel.SelectedSupplier );
	}

	private void OrderSelected( object sender, SelectionChangedEventArgs e )
	{
		CombinedInventoryOrderViewModel? viewModel = DataContext as CombinedInventoryOrderViewModel;
		if ( viewModel != null && viewModel.SupplyReceiptViewModel.SelectedOrder != null )
		{
			int orderId = viewModel.SupplyReceiptViewModel.SelectedOrder.SupplyOrderId;
			viewModel.SupplyReceiptViewModel.LoadLinesForSelectedOrder( orderId );
			viewModel.SupplyReceiptViewModel.IsOrderSelected = true;
		}
	}
}
