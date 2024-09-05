using System.Collections.ObjectModel;

namespace Modelbouwer.View;

/// <summary>
/// Interaction logic for CountryManagement.xaml
/// </summary>
public partial class CountryManagement : Page
{
	public CountryManagement()
	{
		InitializeComponent();
		DataContext = new CombinedCountryViewModel();
	}

	private void ButtonNew( object sender, RoutedEventArgs e )
	{
		inpCountryId.Text = "0";
		if ( DataContext is CombinedCountryViewModel viewModel )
		{
			viewModel.CountryViewModel.AddNewItem();
		}
	}

	private void ButtonDelete( object sender, RoutedEventArgs e )
	{
		if ( DataContext is CombinedCountryViewModel viewModel )
		{
			// Check if a row is selected (check if there is a known Id)
			if ( inpCountryId != null && !string.IsNullOrWhiteSpace( inpCountryId.Text ) )
			{
				CountryViewModel countryViewModel = viewModel.CountryViewModel;

				string [ , ] _whereFields = new string [ 1, 3 ] { { DBNames.CountryFieldNameId, DBNames.CountryFieldTypeId, inpCountryId.Text } };
				string _result = DBCommands.DeleteRecord( DBNames.CountryTable, _whereFields );

				//Refresh the datagrid
				countryViewModel.Country = new ObservableCollection<CountryModel>( DBCommands.GetCountryList() );

				dispStatusLine.Text = $"{inpCountryName.Text} {_result}";
			}
		}
	}
	private void ButtonSave( object sender, RoutedEventArgs e )
	{
		int _codeExists = 0, _nameExists = 0;

		// Code and Name Check only matter when a new record is added
		if ( inpCountryId.Text == "0" )
		{
			// First cheack if the entered Code and Name not yet excists, only if not then perform save
			_codeExists = DBCommands.CheckForRecords( DBNames.CountryView, new string [ 1, 3 ] { { DBNames.CountryFieldNameCode, DBNames.CountryFieldTypeCode, inpCountryCode.Text } } );
			_nameExists = DBCommands.CheckForRecords( DBNames.CountryView, new string [ 1, 3 ] { { DBNames.CountryFieldNameName, DBNames.CountryFieldTypeName, inpCountryName.Text } } );
		}

		if ( _codeExists == 0 && _nameExists == 0 )
		{
			if ( DataContext is CombinedCountryViewModel viewModel )
			{
				// Save can only be performed if a name is entered
				if ( !string.IsNullOrWhiteSpace( inpCountryName.Text ) )
				{
					CountryViewModel countryViewModel = viewModel.CountryViewModel;

					// Check if item is new or has to be updated
					if ( inpCountryId.Text == "0" )
					{
						// Item is new so add to database table
						string [ , ] _whereFields = new string [ 1, 3 ] { { DBNames.CountryFieldNameName, DBNames.CountryFieldTypeName, inpCountryName.Text.ToLower() } };


						// Check if the entered (to be) new value is already available in the table
						int _checkPresence = DBCommands.CheckForRecords(DBNames.CountryTable, _whereFields);

						if ( _checkPresence == 0 )
						{
							// Add item to the table
							string [ , ] _addFields = new string [ 3, 3 ]
					{
						{ DBNames.CountryFieldNameCode, DBNames.CountryFieldTypeCode, inpCountryCode.Text },
						{ DBNames.CountryFieldNameName, DBNames.CountryFieldTypeName, inpCountryName.Text },
						{ DBNames.CountryFieldNameCurrencyId, DBNames.CountryFieldTypeCurrencyId, inpCountryCurrencyId.Text }
					};
							_ = DBCommands.InsertInTable( DBNames.CountryTable, _addFields );

							// Update the Country collection to reflect the new data
							countryViewModel.Country = new ObservableCollection<CountryModel>( DBCommands.GetCountryList() );

							dispStatusLine.Text = $"{inpCountryName.Text} {( string ) FindResource( "Maintanance.Statusline.NotSaved.Unchanged" )}";
						}
						else
						{
							dispStatusLine.Text = $"{( string ) FindResource( "Maintanance.Statusline.NotSaved.AlreadyExcists" )}";
						}
					}
					else
					{
						string [ , ] _whereFields = new string [ 1, 3 ] { { DBNames.CountryFieldNameId, DBNames.CountryFieldTypeId, inpCountryId.Text } };

						// update item in the table
						string [ , ] _updateFields = new string [ 3, 3 ]
					{
						{ DBNames.CountryFieldNameCode, DBNames.CountryFieldTypeCode, inpCountryCode.Text },
						{ DBNames.CountryFieldNameName, DBNames.CountryFieldTypeName, inpCountryName.Text },
						{ DBNames.CountryFieldNameCurrencyId, DBNames.CountryFieldTypeCurrencyId, inpCountryCurrencyId.Text }
					};
						_ = DBCommands.UpdateInTable( DBNames.CountryTable, _updateFields, _whereFields );

						// Update the Country collection to reflect the new data
						countryViewModel.Country = new ObservableCollection<CountryModel>( DBCommands.GetCountryList() );

						dispStatusLine.Text = $"{( string ) FindResource( "Maintanance.Statusline.NotSaved.DataOf" )} {inpCountryName.Text} {( string ) FindResource( "Maintanance.Statusline.NotSaved.Changed" )}";
					}
				}
				else
				{
					dispStatusLine.Text = $"{( string ) FindResource( "Maintanance.Statusline.NotSaved.Empty" )}";
				}
			}
		}
		else
		{
			// Code, Name or both already excists, no save
			if ( _codeExists != 0 && _nameExists != 0 )
			{
				//Maintanance.Statusline.NotSaved.AlreadyExcistsPrefix
				dispStatusLine.Text = $"{( string ) FindResource( "Maintanance.Statusline.NotSaved.AlreadyExcists.Prefix" )} {inpCountryCode.Text} {( string ) FindResource( "Maintanance.Statusline.NotSaved.AlreadyExcists.Concat" )} {inpCountryName.Text} {( string ) FindResource( "Maintanance.Statusline.NotSaved.AlreadyExcists.Suffix" )}";
			}
			else
			{
				dispStatusLine.Text = _codeExists != 0
					? $"{( string ) FindResource( "Maintanance.Statusline.NotSaved.AlreadyExcists.Prefix" )} {inpCountryCode.Text} {( string ) FindResource( "Maintanance.Statusline.NotSaved.AlreadyExcists.SuffixSingle" )}"
					: $"{( string ) FindResource( "Maintanance.Statusline.NotSaved.AlreadyExcists.Prefix" )} {inpCountryName.Text} {( string ) FindResource( "Maintanance.Statusline.NotSaved.AlreadyExcists.SuffixSingle" )}";
			}
		}
	}

	private void SelectionChanged( object sender, SelectionChangedEventArgs e )
	{
		if ( CountryDataGrid.SelectedItem != null )
		{
			CountryDataGrid.ScrollIntoView( CountryDataGrid.SelectedItem );
		}
	}
}
