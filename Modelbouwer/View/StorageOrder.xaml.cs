using TextBox = System.Windows.Controls.TextBox;

namespace Modelbouwer.View;

public partial class StorageOrder : Page
{
	double minShippingCosts= 0.00;

	public StorageOrder()
	{
		InitializeComponent();
		this.Loaded += Data_Loaded;
	}

	private void Data_Loaded( object sender, RoutedEventArgs e )
	{
		if ( DataContext is CombinedInventoryOrderViewModel viewModel )
		{
			viewModel.RefreshAll();
		}
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
			foreach ( var record in dataGrid.View.Records )
			{
				var inventoryOrderModel = record.Data as Modelbouwer.Model.InventoryOrderModel;

				// Zoek het bijbehorende product in de ProductViewModel
				var correspondingProduct = viewModel.ProductViewModel.Product
				.FirstOrDefault(p => p.ProductId == line.SupplyOrderlineShortProductId);

				if ( inventoryOrderModel != null &&
					line.SupplyOrderlineShortProductId == inventoryOrderModel.ProductId )
				{
					inventoryOrderModel.IsSelected = true;

					// Bereken de bestelhoeveelheid op basis van ProductStandardQuantity
					double standardQuantity = correspondingProduct?.ProductStandardQuantity ?? 1;
					double orderAmount = line.SupplyOrderlineShortAmount;

					// Bereken de juiste bestelhoeveelheid
					double calculatedOrderQuantity = CalculateOrderQuantity(orderAmount, standardQuantity);

					inventoryOrderModel.ProductShortInventory = calculatedOrderQuantity;
				}
			}
		}
		CalculateTotalOrderCost();
	}

	private double CalculateOrderQuantity( double requestedAmount, double standardQuantity )
	{
		// Als de standaard bestel hoeveelheid 0 of 1 is, gebruik dan de oorspronkelijke hoeveelheid
		if ( standardQuantity <= 1 )
			return requestedAmount;

		// Bereken hoeveel volledige standaard hoeveelheden nodig zijn
		double fullQuantities = Math.Ceiling(requestedAmount / standardQuantity);

		// Bereken de finale bestelhoeveelheid
		return fullQuantities * standardQuantity;
	}

	private void SelectedProductsDataGrid_SelectionChanged( object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs e )
	{
		CalculateTotalOrderCost();
	}

	private void SelectedProductsDataGrid_CellEditEnd( object sender, Syncfusion.UI.Xaml.Grid.CurrentCellEndEditEventArgs e )
	{
		CalculateTotalOrderCost();
	}

	#region calculate subtotal, shipping costs, order costs and grand total for each order and update the view
	private void CalculateTotalOrderCost()
	{
		var viewModel = (CombinedInventoryOrderViewModel)this.DataContext;
		viewModel?.SupplyOrderViewModel.CalculateTotalOrderCost();
	}
	#endregion

	#region The selected supplier is changed
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
			viewModel.SupplyOrderViewModel.UpdateOrderTotals( 0, shippingCosts, orderCosts );
		}
	}
	#endregion

	#region Clear selected supplier, all selected rows, order number and Order date
	private void ButtonClear( object sender, RoutedEventArgs e )
	{

	}

	#endregion

	#region Save the order to the table, if products are new or changed for the selected supplier  update this table
	private void ButtonSave( object sender, RoutedEventArgs e )
	{
	}
	#endregion

	private void OrderNumberChanged( object sender, TextChangedEventArgs e )
	{
		var textBox = sender as TextBox;
		if ( textBox != null )
		{
			var viewModel = (CombinedInventoryOrderViewModel)this.DataContext;
			if ( viewModel?.SupplyOrderViewModel?.SelectedOrder != null )
			{
				viewModel.SupplyOrderViewModel.SelectedOrder.SupplyOrderNumber = textBox.Text;
			}
		}
	}
}