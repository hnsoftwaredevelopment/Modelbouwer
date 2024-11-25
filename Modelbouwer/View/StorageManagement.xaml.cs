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
		var columnName = gridColumn.MappingName;

		var editedRow = (InventoryModel)((SfDataGrid)sender).GetRecordAtRowIndex(e.RowColumnIndex.RowIndex);
		string _whereFieldName = "", _whereFieldType = "", _changeFieldName = "", _changeFieldType = "", _changeTable = "";

		// Check if the row is not null
		if ( editedRow != null )
		{
			// Update the MySQL table
			if ( _originalValue.ToString() != editedRow.ProductInventory.ToString() )
			{
				Debug.WriteLine( $"Updated Inventory: {editedRow.ProductId}, {columnName},Old Value={_originalValue} New Value: {editedRow.ProductInventory}" );
				switch ( columnName.ToLower() )
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
						{ //Add new record}
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
				_originalValue = propertyInfo.GetValue( record ); // Store original value
			}
		}
	}
}
