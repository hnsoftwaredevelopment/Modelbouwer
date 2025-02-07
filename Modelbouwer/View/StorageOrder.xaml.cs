namespace Modelbouwer.View;

public partial class StorageOrder : Page
{
	double _minShippingCosts= 0.00;

	public StorageOrder()
	{
		InitializeComponent();
		DataContext = new CombinedInventoryOrderViewModel();
	}

	#region Switch between search and filter button
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
	private void ClearText( object sender, RoutedEventArgs e )
	{
		FilterSearchText.Clear();
	}
	#endregion

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

	private void SelectedProductsDataGrid_SelectionChanged( object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs e )
	{
		CalculateTotalOrderCost();
	}

	private void SelectedProductsDataGrid_CellEditEnd( object sender, Syncfusion.UI.Xaml.Grid.CurrentCellEndEditEventArgs e )
	{
		CalculateTotalOrderCost();
	}

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

			if ( subTotalOrder < _minShippingCosts )
			{
				shippingCosts = selectedSupplier.SupplierShippingCosts;
			}
		}

		viewModel.SupplyOrderViewModel.UpdateOrderTotals( subTotalOrder, shippingCosts, orderCosts );
	}

	private void SupplierSelectionChanged( object sender, SelectionChangedEventArgs e )
	{
		var viewModel = (CombinedInventoryOrderViewModel)this.DataContext;
		var selectedSupplier = viewModel.SupplyOrderViewModel.SupplierList.FirstOrDefault(s => s.SupplierId == viewModel.SupplyOrderViewModel.SelectedSupplier);
		if ( selectedSupplier != null )
		{
			var shippingCosts = selectedSupplier.SupplierShippingCosts;
			_minShippingCosts = selectedSupplier.SupplierMinShippingCosts;
			var orderCosts = selectedSupplier.SupplierOrderCosts;

			SupplierShippingCosts.Text = shippingCosts.ToString( "N", CultureInfo.CreateSpecificCulture( "nl-NL" ) );
			SupplierOrderCosts.Text = orderCosts.ToString( "N", CultureInfo.CreateSpecificCulture( "nl-NL" ) );
			viewModel.SupplyOrderViewModel.UpdateOrderTotals( 0, shippingCosts, orderCosts );
		}
	}
}