namespace Modelbouwer.View;

/// <summary>
/// Interaction logic for CurrencyManagement.xaml
/// </summary>
public partial class CurrencyManagement : Page
{
	public CurrencyManagement()
	{
		System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo( "nl" );
		InitializeComponent();
		DataContext = new CurrencyViewModel();
		this.Loaded += Data_Loaded;
		dataGrid.Loaded += DataGrid_Loaded;
	}

	private void Data_Loaded( object sender, RoutedEventArgs e )
	{
		if ( DataContext is CurrencyViewModel viewModel )
		{
			viewModel.Refresh();
		}
	}

	private void ButtonNew( object sender, RoutedEventArgs e )
	{
		inpId.Text = "0";
		if ( DataContext is CurrencyViewModel viewModel )
		{
			viewModel.AddNewItem();
		}
	}

	private void DataGrid_Loaded( object sender, RoutedEventArgs e )
	{
		if ( dataGrid.View != null && dataGrid.View.Records != null )
		{
			int _rowCount = dataGrid.View.Records.Count;
			if ( _rowCount == 0 )
			{ dispStatusLine.Text = $"{( string ) FindResource( "Maintanance.Statusline.Read.None" )}"; }
			else if ( _rowCount == 1 )
			{ dispStatusLine.Text = $"{_rowCount} {( string ) FindResource( "Maintanance.Statusline.Read.None" )}"; }
			else { dispStatusLine.Text = $"{_rowCount} {( string ) FindResource( "Maintanance.Statusline.Read" )}"; }
		}
		else
		{
			dispStatusLine.Text = $"{( string ) FindResource( "Maintanance.Statusline.Read.None" )}";
		}
	}

	private void ButtonDelete( object sender, RoutedEventArgs e )
	{
		CurrencyViewModel viewModel = (CurrencyViewModel)DataContext;

		// Check if a row is selected (check if there is a known Id)
		if ( inpId.Text != null )
		{
			string [ , ] _whereFields = new string [ 1, 3 ] { { DBNames.CurrencyFieldNameId, DBNames.CurrencyFieldTypeId, inpId.Text } };
			string _result = DBCommands.DeleteRecord( DBNames.CurrencyTable, _whereFields );

			DBCommands dbCommands = new();

			viewModel.Currency = new ObservableCollection<CurrencyModel>( dbCommands.GetCurrencyList() );

			dispStatusLine.Text = $"{_result}";
		}

	}

	private void ButtonSave( object sender, RoutedEventArgs e )
	{
		// Save can only be performed if a name is entered
		if ( inpCurrencyName.Text != "" )
		{
			CurrencyViewModel viewModel = (CurrencyViewModel)DataContext;

			// Check if item is new or has to be updated
			if ( inpId.Text == "0" )
			{
				// Item is new so add to database table
				string [ , ] _whereFields = new string [ 1, 3 ] { { DBNames.CurrencyFieldNameName, DBNames.CurrencyFieldTypeName, inpCurrencyName.Text.ToLower() } };


				// Check if the entered (to be) new value is already available in the table
				int _checkPresence = DBCommands.CheckForRecords(DBNames.CurrencyTable, _whereFields);

				if ( _checkPresence == 0 )
				{
					// Add item to the table
					string [ , ] _addFields = new string [ 4, 3 ]
					{
						{ DBNames.CurrencyFieldNameCode, DBNames.CurrencyFieldTypeCode, inpCurrenyCode.Text },
						{ DBNames.CurrencyFieldNameName, DBNames.CurrencyFieldTypeName, inpCurrencyName.Text },
						{ DBNames.CurrencyFieldNameSymbol, DBNames.CurrencyFieldTypeSymbol, inpCurrencySymbol.Text },
						{ DBNames.CurrencyFieldNameRate, DBNames.CurrencyFieldTypeRate, inpCurrencyConversionRate.Text }
					};
					_ = DBCommands.InsertInTable( DBNames.CurrencyTable, _addFields );

					// Update the Currency collection to reflect the new data
					DBCommands dbCommands = new();
					viewModel.Currency = new ObservableCollection<CurrencyModel>( dbCommands.GetCurrencyList() );

					dispStatusLine.Text = $"{inpCurrencyName.Text} {( string ) FindResource( "Maintanance.Statusline.NotSaved.Added" )}";
				}
				else
				{
					dispStatusLine.Text = $"{( string ) FindResource( "Maintanance.Statusline.NotSaved.AlreadyExcists" )}";
				}
			}
			else
			{
				// Item is an excisting item that has been changed
				string [ , ] _whereFields = new string [ 4, 3 ]
					{
						{ DBNames.CurrencyFieldNameCode, DBNames.CurrencyFieldTypeCode, inpCurrenyCode.Text },
						{ DBNames.CurrencyFieldNameName, DBNames.CurrencyFieldTypeName, inpCurrencyName.Text },
						{ DBNames.CurrencyFieldNameSymbol, DBNames.CurrencyFieldTypeSymbol, inpCurrencySymbol.Text },
						{ DBNames.CurrencyFieldNameRate, DBNames.CurrencyFieldTypeRate, inpCurrencyConversionRate.Text }
					};


				// Check if the entered (to be) changed value is already available in the table
				int _checkPresence = DBCommands.CheckForRecords(DBNames.CurrencyTable, _whereFields);

				if ( _checkPresence == 0 )
				{
					_whereFields = new string [ 1, 3 ] { { DBNames.CurrencyFieldNameId, DBNames.CurrencyFieldTypeId, inpId.Text } };

					// update item in the table
					string [ , ] _updateFields = new string [ 4, 3 ]
					{
						{ DBNames.CurrencyFieldNameCode, DBNames.CurrencyFieldTypeCode, inpCurrenyCode.Text },
						{ DBNames.CurrencyFieldNameName, DBNames.CurrencyFieldTypeName, inpCurrencyName.Text },
						{ DBNames.CurrencyFieldNameSymbol, DBNames.CurrencyFieldTypeSymbol, inpCurrencySymbol.Text },
						{ DBNames.CurrencyFieldNameRate, DBNames.CurrencyFieldTypeRate, inpCurrencyConversionRate.Text }
					};
					_ = DBCommands.UpdateInTable( DBNames.CurrencyTable, _updateFields, _whereFields );

					// Update the Currency collection to reflect the new data
					DBCommands dbCommands = new();
					viewModel.Currency = new ObservableCollection<CurrencyModel>( dbCommands.GetCurrencyList() );

					dispStatusLine.Text = $"{( string ) FindResource( "Maintanance.Statusline.NotSaved.DataOf" )} {inpCurrencyName.Text} {( string ) FindResource( "Maintanance.Statusline.NotSaved.Changed" )}";
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

	}
}
