using System.ComponentModel;

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

	#region Check if Order is completely received
	private bool CheckReceiptLinesComplete()
	{
		bool isComplete = true;

		CombinedInventoryOrderViewModel? viewModel = DataContext as CombinedInventoryOrderViewModel;

		if ( viewModel?.SupplyReceiptViewModel?.FilteredReceiptLines != null )
		{
			ObservableCollection<SupplyReceiptModel> receiptLines = viewModel.SupplyReceiptViewModel.FilteredReceiptLines;

			foreach ( SupplyReceiptModel line in receiptLines )
			{
				if ( line.Received + line.WaitFor != line.Ordered )
				{
					isComplete = false;
					break;
				}
			}

			if ( viewModel.SupplyReceiptViewModel is INotifyPropertyChanged viewModelWithNotify )
			{
				viewModel.SupplyReceiptViewModel.IsComplete = isComplete;
			}
		}
		return isComplete;
	}

	#endregion

	private void ReceiptLinesDataGrid_CellEditEnd( object sender, Syncfusion.UI.Xaml.Grid.CurrentCellEndEditEventArgs e )
	{
		// done with editing the line 
		//Update the orderComplete status
		CheckReceiptLinesComplete();
		SupplyReceiptViewModel viewModel = new();
	}

	private void ReceiptLinesDataGrid_SelectionChanged( object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs e )
	{
		// Row changed in datagrid
	}

	private void SupplierSelectionChanged( object sender, SelectionChangedEventArgs e )
	{
		CombinedInventoryOrderViewModel? viewModel = DataContext as CombinedInventoryOrderViewModel;
		viewModel.SupplyReceiptViewModel.IsOrderSelected = false;
	}

	private void OrderSelected( object sender, SelectionChangedEventArgs e )
	{
		CheckReceiptLinesComplete();

		//CombinedInventoryOrderViewModel? viewModel = DataContext as CombinedInventoryOrderViewModel;
		//if ( viewModel != null && viewModel.SupplyReceiptViewModel.SelectedOrder != null )
		//{
		//	int orderId = viewModel.SupplyReceiptViewModel.SelectedOrder.SupplyOrderId;
		//	viewModel.SupplyReceiptViewModel.LoadLinesForSelectedOrder( orderId );
		//	viewModel.SupplyReceiptViewModel.IsOrderSelected = true;
		//	CheckReceiptLinesComplete();
		//}
	}

	private void OrderStatus( object sender, RoutedEventArgs e )
	{
		if ( OrderClosed.IsChecked == true )
		{
			valueShow.IsChecked = true;
		}
		else
		{
			valueShow.IsChecked = false;
		}
	}
}
