namespace Modelbouwer.View;

/// <summary>
/// Interaction logic for StorageOrder.xaml
/// </summary>
public partial class StorageOrder : Page
{
	/// <summary>
	/// Initializes a new instance of the <see cref="StorageOrder"/> class.
	/// </summary>
	/// 
	double minShippingCosts= 0.00;
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

		double totalOrderCost = 0.00;
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
				totalOrderCost += inventoryOrder.ProductShortInventory * inventoryOrder.SupplierPrice;
			}

			if ( totalOrderCost < minShippingCosts )
			{
				shippingCosts = selectedSupplier.SupplierShippingCosts;
			}
		}

		TotalOrder.Text = totalOrderCost.ToString( "N", CultureInfo.CreateSpecificCulture( "nl-NL" ) );
		SupplierShippingCosts.Text = shippingCosts.ToString( "N", CultureInfo.CreateSpecificCulture( "nl-NL" ) );
		SupplierOrderCosts.Text = orderCosts.ToString( "N", CultureInfo.CreateSpecificCulture( "nl-NL" ) );

		var grandTotal = totalOrderCost + shippingCosts + orderCosts;
		GrandTotal.Text = grandTotal.ToString( "N", CultureInfo.CreateSpecificCulture( "nl-NL" ) );
	}

	private void SupplierSelectionChanged( object sender, SelectionChangedEventArgs e )
	{
		var viewModel = (CombinedInventoryOrderViewModel)this.DataContext;
		var selectedSupplier = viewModel.SupplyOrderViewModel.SupplierList.FirstOrDefault(s => s.SupplierId == viewModel.SupplyOrderViewModel.SelectedSupplier);
		if ( selectedSupplier != null )
		{
			var shippingCosts = selectedSupplier.SupplierShippingCosts;
			minShippingCosts = selectedSupplier.SupplierMinShippingCosts;
			var orderCosts = selectedSupplier.SupplierOrderCosts;

			SupplierShippingCosts.Text = shippingCosts.ToString( "N", CultureInfo.CreateSpecificCulture( "nl-NL" ) );
			SupplierOrderCosts.Text = orderCosts.ToString( "N", CultureInfo.CreateSpecificCulture( "nl-NL" ) );

		}
	}
}
