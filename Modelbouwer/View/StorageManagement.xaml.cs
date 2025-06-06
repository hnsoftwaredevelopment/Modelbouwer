﻿using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.Grid.Helpers;

namespace Modelbouwer.View;

/// <summary>
/// Interaction logic for StorageManagement.xaml
/// </summary>
public partial class StorageManagement : Page
{
	private object? _originalValue; // Temporary variable to store the original value
	public StorageManagement()
	{
		InitializeComponent();

		// Attach the CurrentCellEndEdit event handler
		dataGrid.CurrentCellBeginEdit += OriginalInventory;
		dataGrid.CurrentCellEndEdit += ChangedInventory;

		//Refresh the datagrid when the page is loaded
		this.Loaded += StorageManagement_Loaded;

	}

	private void StorageManagement_Loaded( object sender, RoutedEventArgs e )
	{
		RefreshDataGrid();
	}

	private void RefreshDataGrid()
	{
		// Haal de bijgewerkte data op vanuit de database
		ObservableCollection<InventoryModel> updatedInventory = DBCommands.GetInventory();

		// Stel de nieuwe ItemsSource in
		dataGrid.ItemsSource = updatedInventory;

		// Forceer de grid om de wijzigingen weer te geven
		dataGrid.View.Refresh();
	}
	private void ChangedInventory( object? sender, CurrentCellEndEditEventArgs e )
	{
		SfDataGrid dataGrid = (SfDataGrid)sender!;

		int columnIndex = e.RowColumnIndex.ColumnIndex;
		int rowIndex = e.RowColumnIndex.RowIndex;
		GridColumn gridColumn = dataGrid.Columns[columnIndex];
		string columnName = gridColumn.MappingName.ToString().ToLower();

		InventoryModel editedRow = (InventoryModel)dataGrid.GetRecordAtRowIndex(e.RowColumnIndex.RowIndex);
		string _whereFieldName = "", _whereFieldType = "", _changeFieldName = "", _changeFieldType = "", _changeTable = "", _changeValue="";

		// Check if the row is not null
		if ( editedRow != null )
		{
			// Update the MySQL table
			if ( _originalValue?.ToString() != editedRow.ProductInventory.ToString() )
			{
				switch ( columnName )
				{
					case "productinventory":
						_changeValue = editedRow.ProductInventory.ToString();
						_changeFieldName = DBNames.StocklogFieldNameAmountCorrection;
						_changeFieldType = DBNames.StocklogFieldTypeAmountCorrection;
						_changeTable = DBNames.StocklogTable;
						_whereFieldName = DBNames.StocklogFieldNameProductId;
						_whereFieldType = DBNames.StocklogFieldTypeProductId;
						string _checkTable = DBNames.ProductsInStockView;
						string _checkWhereFieldName = DBNames.ProductsInStockFieldNameProduct_Id;
						string _checkWhereFieldType = DBNames.ProductsInStockFieldTypeProduct_Id;

						// Check here if the Selected Product_Id already exists in the productinventory table, if exists do nothing otherwise add _productId, with value 0 
						int _exists = DBCommands.CheckForRecords(_checkTable, new string[1, 3] { { _checkWhereFieldName, _checkWhereFieldType, editedRow.ProductId.ToString() } });
						if ( _exists == 0 )
						{
							// There is no record available in table, add first with minimal data to be able to find the record to save changes
							DBCommands.InsertInTable( _changeTable, new string [ 1, 3 ] { { _whereFieldName, _whereFieldType, editedRow.ProductId.ToString() } } );
						}
						else
						{
							//There is already a record for the product, now the new ammount should be entered as an correction
							// First get the current ammount to determine the cnage amount
							double _currentAmount = double.Parse(DBCommands.GetData(DBNames.ProductsInStockView, new string [ 1, 3 ] { { _whereFieldName, _whereFieldType, editedRow.ProductId.ToString() } }, new string [ 1, 2] { { DBNames.ProductsInStockFieldNameAmount, DBNames.ProductsInStockFieldTypeAmount } } ));
							double _newAmount = double.Parse(_changeValue);
							_changeValue = ( _newAmount - _currentAmount ).ToString();
						}
						break;
					case "productminimalstock":
						_changeValue = editedRow.ProductInventory.ToString();
						_changeFieldName = DBNames.ProductFieldNameMinimalStock;
						_changeFieldType = DBNames.ProductFieldTypeMinimalStock;
						_changeTable = DBNames.ProductTable;
						_whereFieldName = DBNames.ProductFieldNameId;
						_whereFieldType = DBNames.ProductFieldTypeId;
						break;
					case "productprice":
						_changeValue = editedRow.ProductInventory.ToString();
						_changeFieldName = DBNames.ProductFieldNamePrice;
						_changeFieldType = DBNames.ProductFieldTypePrice;
						_changeTable = DBNames.ProductTable;
						_whereFieldName = DBNames.ProductFieldNameId;
						_whereFieldType = DBNames.ProductFieldTypeId;
						break;
				}

				DBCommands.UpdateInTable( _changeTable, new string [ 1, 3 ] { { _changeFieldName, _changeFieldType, _changeValue } }, new string [ 1, 3 ] { { _whereFieldName, _whereFieldType, editedRow.ProductId.ToString() } } );
				RefreshDataGrid();

				// UpdateDatabase(editedRow);
			}
		}
	}

	private void OriginalInventory( object? sender, CurrentCellBeginEditEventArgs e )
	{
		SfDataGrid dataGrid = (SfDataGrid)sender!;
		int rowIndex = e.RowColumnIndex.RowIndex;
		string columnName = e.Column.MappingName;

		// Retrieve the record and original value
		object record = dataGrid.GetRecordAtRowIndex(rowIndex);
		if ( record != null )
		{
			System.Reflection.PropertyInfo? propertyInfo = record.GetType().GetProperty( columnName );
			if ( propertyInfo != null )
			{
				_originalValue = propertyInfo.GetValue( record ) ?? new object(); // Store original value
			}
		}
	}

	#region Switch between search and filter button
	private void ToggleButton( object sender, RoutedEventArgs e )
	{
		string name = ((FrameworkElement)sender).Name;
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

		string action = SearchButton.Visibility == Visibility.Visible
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