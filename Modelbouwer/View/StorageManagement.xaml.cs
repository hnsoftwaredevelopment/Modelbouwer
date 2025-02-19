using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.Grid.Helpers;

namespace Modelbouwer.View;

/// <summary>
/// Interaction logic for StorageManagement.xaml
/// </summary>
public partial class StorageManagement : Page
{
	private object _originalValue; // Temporary variable to store the original value
	public StorageManagement()
	{
		InitializeComponent();

		// Attach the CurrentCellEndEdit event handler
		dataGrid.CurrentCellBeginEdit += OriginalInventory;
		dataGrid.CurrentCellEndEdit += ChangedInventory;

	}
	private void RefreshDataGrid()
	{
		// Haal de bijgewerkte data op vanuit de database
		var updatedInventory = DBCommands.GetInventory();

		// Stel de nieuwe ItemsSource in
		dataGrid.ItemsSource = updatedInventory;

		// Forceer de grid om de wijzigingen weer te geven
		dataGrid.View.Refresh();
	}
	private void ChangedInventory( object sender, CurrentCellEndEditEventArgs e )
	{
		var dataGrid = (SfDataGrid)sender;

		var columnIndex = e.RowColumnIndex.ColumnIndex;
		var rowIndex = e.RowColumnIndex.RowIndex;
		var gridColumn = dataGrid.Columns[columnIndex];
		var columnName = gridColumn.MappingName.ToString().ToLower();

		var editedRow = (InventoryModel)((SfDataGrid)sender).GetRecordAtRowIndex(e.RowColumnIndex.RowIndex);
		string _whereFieldName = "", _whereFieldType = "", _changeFieldName = "", _changeFieldType = "", _changeTable = "";

		// Check if the row is not null
		if ( editedRow != null )
		{
			// Update the MySQL table
			if ( _originalValue.ToString() != editedRow.ProductInventory.ToString() )
			{
				switch ( columnName )
				{
					case "productinventory":
						_changeFieldName = DBNames.ProductInventoryFieldNameAmount;
						_changeFieldType = DBNames.ProductInventoryFieldTypeAmount;
						_changeTable = DBNames.ProductInventoryTable;
						_whereFieldName = DBNames.ProductInventoryFieldNameProduct_Id;
						_whereFieldType = DBNames.ProductInventoryFieldTypeProduct_Id;

						// Check here if the Selected Product_Id already excists in the productinventory table, if excists do nothing otherwise add ProductId, with value 0 
						var _excists = DBCommands.CheckForRecords( _changeTable, new string [ 1, 3 ] { { _whereFieldName, _whereFieldType, editedRow.ProductId.ToString() } } );
						if ( _excists == 0 )
						{
							//There is no record available in table, add first with minimal data to be able to find the record to save changes
							DBCommands.InsertInTable( _changeTable, new string [ 1, 3 ] { { _whereFieldName, _whereFieldType, editedRow.ProductId.ToString() } } );
						}
						break;
					case "productminimalstock":
						_changeFieldName = DBNames.ProductFieldNameMinimalStock;
						_changeFieldType = DBNames.ProductFieldTypeMinimalStock;
						_changeTable = DBNames.ProductTable;
						_whereFieldName = DBNames.ProductFieldNameId;
						_whereFieldType = DBNames.ProductFieldTypeId;
						break;
					case "productprice":
						_changeFieldName = DBNames.ProductFieldNamePrice;
						_changeFieldType = DBNames.ProductFieldTypePrice;
						_changeTable = DBNames.ProductTable;
						_whereFieldName = DBNames.ProductFieldNameId;
						_whereFieldType = DBNames.ProductFieldTypeId;
						break;
				}

				DBCommands.UpdateInTable( _changeTable, new string [ 1, 3 ] { { _changeFieldName, _changeFieldType, editedRow.ProductInventory.ToString() } }, new string [ 1, 3 ] { { _whereFieldName, _whereFieldType, editedRow.ProductId.ToString() } } );
				RefreshDataGrid();

				//UpdateDatabase( editedRow );
			}
		}
	}
	private void OriginalInventory( object sender, CurrentCellBeginEditEventArgs e )
	{
		var dataGrid = (SfDataGrid)sender;
		var rowIndex = e.RowColumnIndex.RowIndex;
		var columnName = e.Column.MappingName;

		// Retrieve the record and original value
		var record = dataGrid.GetRecordAtRowIndex(rowIndex);
		if ( record != null )
		{
			var propertyInfo = record.GetType().GetProperty(columnName);
			if ( propertyInfo != null )
			{
				_originalValue = propertyInfo.GetValue( record ) ?? new object(); // Store original value
			}
		}
	}

	#region Switch between search and filter button
	private void ToggleButton( object sender, RoutedEventArgs e )
	{
		var name = ((FrameworkElement)sender).Name;
		//var viewModel = (InventoryViewModel)this.DataContext;

		if ( !string.IsNullOrEmpty( name ) )
		{
			switch ( name )
			{
				case "FilterButton":
					//Current button is FilterButton, toggle to SearchButton
					FilterButton.Visibility = Visibility.Collapsed;
					SearchButton.Visibility = Visibility.Visible;
					FilterSearchText.Tag = ( string ) FindResource( "Edit.InventoryOrder.DataGrid.FilterSearch.Search.Tag" );
					FilterSearchText.ToolTip = ( string ) FindResource( "Edit.InventoryOrder.DataGrid.FilterSearch.Search.Tooltip" );
					ClearFilterSearch.ToolTip = ( string ) FindResource( "Edit.InventoryOrder.DataGrid.FilterSearch.Search.Clear.Tooltip" );
					//Toggle filtering to show result of toggle directly in datagrid
					dataGrid.SearchHelper.AllowFiltering = false;
					break;
				case "SearchButton":
					//Current button is SearchButton, toggle to FilterButton
					SearchButton.Visibility = Visibility.Collapsed;
					FilterButton.Visibility = Visibility.Visible;
					FilterSearchText.Tag = ( string ) FindResource( "Edit.InventoryOrder.DataGrid.FilterSearch.Filter.Tag" );
					FilterSearchText.ToolTip = ( string ) FindResource( "Edit.InventoryOrder.DataGrid.FilterSearch.Filter.Tooltip" );
					ClearFilterSearch.ToolTip = ( string ) FindResource( "Edit.InventoryOrder.DataGrid.FilterSearch.Filter.Clear.Tooltip" );
					//Toggle filtering to show result of toggle directly in datagrid
					dataGrid.SearchHelper.AllowFiltering = true;
					break;
			}
			//Show result of toggle directly in datagrid
			dataGrid.SearchHelper.Search( FilterSearchText.Text );
		}
	}
	#endregion

	private void FilterSearch( object sender, TextChangedEventArgs e )
	{
		#region when text is entered in the search/filter box it should be possible to clear it, threfore the clear icon has to becom visible
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

	private void ClearText( object sender, RoutedEventArgs e )
	{
		FilterSearchText.Clear();
	}
}