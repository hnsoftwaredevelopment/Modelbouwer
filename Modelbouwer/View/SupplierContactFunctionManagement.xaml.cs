namespace Modelbouwer.View;

/// <summary>
/// Interaction logic for SupplierContactFunctionManagement.xaml
/// </summary>
public partial class SupplierContactFunctionManagement : Page
{
	public SupplierContactFunctionManagement()
	{
		InitializeComponent();
		DataContext = new SupplierContactTypeViewModel();
		this.Loaded += Data_Loaded;
		dataGrid.Loaded += DataGrid_Loaded;
	}

	private void Data_Loaded( object sender, RoutedEventArgs e )
	{
		if ( DataContext is SupplierContactTypeViewModel viewModel )
		{
			viewModel.Refresh();
		}
	}

	private void ButtonNew( object sender, RoutedEventArgs e )
	{
		inpId.Text = "0";
		if ( DataContext is SupplierContactTypeViewModel viewModel )
		{
			viewModel.AddNewItem();
		}
	}

	private void DataGrid_Loaded( object sender, RoutedEventArgs e )
	{
		if ( dataGrid.View != null && dataGrid.View.Records != null )
		{
			int _rowCount = dataGrid.View.Records.Count;
			dispStatusLine.Text = _rowCount == 0
				? $"{( string ) FindResource( "Maintanance.Statusline.Read.None" )}"
				: _rowCount == 1
				? $"{_rowCount} {( string ) FindResource( "Maintanance.Statusline.Read.None" )}"
				: $"{_rowCount} {( string ) FindResource( "Maintanance.Statusline.Read" )}";
		}
		else
		{
			dispStatusLine.Text = $"{( string ) FindResource( "Maintanance.Statusline.Read.None" )}";
		}
	}

	private void ButtonDelete( object sender, RoutedEventArgs e )
	{
		_ = ( SupplierContactTypeViewModel ) DataContext;

		// Check if a row is selected (check if there is a known Id)
		if ( inpId.Text != null )
		{
			string [ , ] _whereFields = new string [ 1, 3 ] { { DBNames.ContactTypeFieldNameId, DBNames.ContactTypeFieldTypeId, inpId.Text } };
			string _result = DBCommands.DeleteRecord( DBNames.ContactTypeTable, _whereFields );

			UpdateDataGrid();

			dispStatusLine.Text = $"{_result}";
		}

	}

	private void ButtonSave( object sender, RoutedEventArgs e )
	{
		// Save can only be performed if a name is entered
		if ( inpContactTypeName.Text != "" )
		{
			_ = ( SupplierContactTypeViewModel ) DataContext;

			// Check if item is new or has to be updated
			if ( inpId.Text == "0" )
			{
				// Item is new so add to database table
				string [ , ] _whereFields = new string [ 1, 3 ] { { DBNames.ContactTypeFieldNameName, DBNames.ContactTypeFieldTypeName, inpContactTypeName.Text.ToLower() } };


				// Check if the entered (to be) new value is already available in the table
				int _checkPresence = DBCommands.CheckForRecords(DBNames.ContactTypeTable, _whereFields);

				if ( _checkPresence == 0 )
				{
					// Add item to the table
					string [ , ] _addFields = new string [ 1, 3 ]
					{
						{ DBNames.ContactTypeFieldNameName, DBNames.ContactTypeFieldTypeName, inpContactTypeName.Text }
					};
					_ = DBCommands.InsertInTable( DBNames.ContactTypeTable, _addFields );

					dispStatusLine.Text = $"{inpContactTypeName.Text} {( string ) FindResource( "Maintanance.Statusline.NotSaved.Added" )}";
				}
				else
				{
					dispStatusLine.Text = $"{( string ) FindResource( "Maintanance.Statusline.NotSaved.AlreadyExcists" )}";
				}
			}
			else
			{
				// Item is an excisting item that has been changed
				string [ , ] _whereFields = new string [ 1, 3 ]
					{
						{ DBNames.ContactTypeFieldNameName, DBNames.ContactTypeFieldTypeName, inpContactTypeName.Text }
					};


				// Check if the entered (to be) changed value is already available in the table
				int _checkPresence = DBCommands.CheckForRecords(DBNames.ContactTypeTable, _whereFields);

				if ( _checkPresence == 0 )
				{
					_whereFields = new string [ 1, 3 ] { { DBNames.ContactTypeFieldNameId, DBNames.ContactTypeFieldTypeId, inpId.Text } };

					string [ , ] _updateFields = new string [ 1, 3 ]
					{
						{ DBNames.ContactTypeFieldNameName, DBNames.ContactTypeFieldTypeName, inpContactTypeName.Text }
					};
					_ = DBCommands.UpdateInTable( DBNames.ContactTypeTable, _updateFields, _whereFields );

					dispStatusLine.Text = $"{( string ) FindResource( "Maintanance.Statusline.NotSaved.DataOf" )} {inpContactTypeName.Text} {( string ) FindResource( "Maintanance.Statusline.NotSaved.Changed" )}";
				}
				else
				{
					dispStatusLine.Text = $"{( string ) FindResource( "Maintanance.Statusline.NotSaved.Unchanged" )}";
				}
			}
		}
		else
		{
			dispStatusLine.Text = $"{( string ) FindResource( "Maintanance.Statusline.NotSaved.Empty" )}";
		}
		UpdateDataGrid();
	}

	private void UpdateDataGrid()
	{
		if ( DataContext is SupplierContactTypeViewModel viewModel )
		{
			SupplierContactTypeViewModel supplierContactTypeViewModel = viewModel;
			object temp = dataGrid.ItemsSource;

			dataGrid.ItemsSource = null;
			dataGrid.ItemsSource = temp;

			ObservableCollection<SupplierContactTypeModel> newContactTypeList = DBCommands.GetContactTypeList();
			supplierContactTypeViewModel.SupplierContactType.Clear();
			foreach ( SupplierContactTypeModel contacttype in newContactTypeList )
			{
				supplierContactTypeViewModel.SupplierContactType.Add( contacttype );
			}

		}

	}
}
