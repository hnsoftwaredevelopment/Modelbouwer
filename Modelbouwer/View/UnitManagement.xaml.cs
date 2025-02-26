namespace Modelbouwer.View;
/// <summary>
/// Interaction logic for UnitManagement.xaml
/// </summary>
public partial class UnitManagement : Page
{
	public UnitManagement()
	{
		InitializeComponent();
		DataContext = new UnitViewModel();
		this.Loaded += Data_Loaded;
		dataGrid.Loaded += DataGrid_Loaded;
	}

	private void Data_Loaded( object sender, RoutedEventArgs e )
	{
		if ( DataContext is UnitViewModel viewModel )
		{
			viewModel.Refresh();
		}
	}

	private void ButtonNew( object sender, RoutedEventArgs e )
	{
		inpId.Text = "0";
		if ( DataContext is UnitViewModel viewModel )
		{
			viewModel.AddNewItem();
		}
	}

	private void DataGrid_Loaded( object sender, RoutedEventArgs e )
	{
		if ( dataGrid.View != null )
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
			dispStatusLine.Text = "DataGrid view is not initialized.";
		}
	}

	private void ButtonDelete( object sender, RoutedEventArgs e )
	{
		_ = ( UnitViewModel ) DataContext;

		// Check if a row is selected (check if there is a known Id)
		if ( inpId.Text != null )
		{
			string [ , ] _whereFields = new string [ 1, 3 ] { { DBNames.UnitFieldNameUnitId, DBNames.UnitFieldTypeUnitId, inpId.Text } };
			string _result = DBCommands.DeleteRecord( DBNames.UnitTable, _whereFields );

			UpdateDataGrid();

			dispStatusLine.Text = $"{_result}";
		}

	}

	private void ButtonSave( object sender, RoutedEventArgs e )
	{
		// Save can only be performed if a name is entered
		if ( inpUnitName.Text != "" )
		{
			_ = ( UnitViewModel ) DataContext;

			// Check if item is new or has to be updated
			if ( inpId.Text == "0" )
			{
				// Item is new so add to database table
				string [ , ] _whereFields = new string [ 1, 3 ] { { DBNames.UnitFieldNameUnitName, DBNames.UnitFieldTypeUnitName, inpUnitName.Text.ToLower() } };


				// Check if the entered (to be) new value is already available in the table
				int _checkPresence = DBCommands.CheckForRecords(DBNames.UnitTable, _whereFields);

				if ( _checkPresence == 0 )
				{
					// Add item to the table
					string [ , ] _addFields = new string [ 1, 3 ]
					{
						{ DBNames.UnitFieldNameUnitName, DBNames.UnitFieldTypeUnitName, inpUnitName.Text }
					};
					_ = DBCommands.InsertInTable( DBNames.UnitTable, _addFields );

					dispStatusLine.Text = $"{inpUnitName.Text} {( string ) FindResource( "Maintanance.Statusline.NotSaved.Added" )}";
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
						{ DBNames.UnitFieldNameUnitName, DBNames.UnitFieldTypeUnitName, inpUnitName.Text }
					};


				// Check if the entered (to be) changed value is already available in the table
				int _checkPresence = DBCommands.CheckForRecords(DBNames.UnitTable, _whereFields);

				if ( _checkPresence == 0 )
				{
					_whereFields = new string [ 1, 3 ] { { DBNames.UnitFieldNameUnitId, DBNames.UnitFieldTypeUnitId, inpId.Text } };

					string [ , ] _updateFields = new string [ 1, 3 ]
					{
						{ DBNames.UnitFieldNameUnitName, DBNames.UnitFieldTypeUnitName, inpUnitName.Text }
					};
					_ = DBCommands.UpdateInTable( DBNames.UnitTable, _updateFields, _whereFields );

					dispStatusLine.Text = $"{( string ) FindResource( "Maintanance.Statusline.NotSaved.DataOf" )} {inpUnitName.Text} {( string ) FindResource( "Maintanance.Statusline.NotSaved.Changed" )}";
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
		if ( DataContext is UnitViewModel viewModel )
		{
			UnitViewModel unitViewModel = viewModel;
			object temp = dataGrid.ItemsSource;

			dataGrid.ItemsSource = null;
			dataGrid.ItemsSource = temp;

			ObservableCollection<UnitModel> newUnitList = DBCommands.GetUnitList();
			unitViewModel.Unit.Clear();
			foreach ( UnitModel unit in newUnitList )
			{
				unitViewModel.Unit.Add( unit );
			}

		}

	}
}
