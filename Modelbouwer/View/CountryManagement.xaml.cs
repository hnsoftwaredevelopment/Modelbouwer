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
		DataContext = new CountryViewModel();
	}

	private void ButtonNew( object sender, RoutedEventArgs e )
	{
		inpCountryId.Text = "0";
		if ( DataContext is CountryViewModel viewModel )
		{
			viewModel.AddNewItem();
		}
	}

	private void ButtonDelete( object sender, RoutedEventArgs e )
	{
		CountryViewModel viewModel = (CountryViewModel)DataContext;

		// Check if a row is selected (check if there is a known Id)
		if ( inpCountryId != null )
		{
			string [ , ] _whereFields = new string [ 1, 3 ] { { DBNames.CountryFieldNameId, DBNames.CountryFieldTypeId, inpCountryId.Text } };
			string _result = DBCommands.DeleteRecord( DBNames.CountryTable, _whereFields );

			viewModel.Country = new ObservableCollection<CountryModel>( DBCommands.GetCountryList() );

			dispStatusLine.Text = $"{inpCountryName.Text} {_result}";
		}
	}
	private void ButtonSave( object sender, RoutedEventArgs e )
	{
		// Save can only be performed if a name is entered
		if ( inpCountryName.Text != "" )
		{
			CountryViewModel viewModel = (CountryViewModel)DataContext;

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
					string [ , ] _addFields = new string [ 1, 3 ] { { DBNames.CountryFieldNameName, DBNames.CountryFieldTypeName, inpCountryName.Text } };
					_ = DBCommands.InsertInTable( DBNames.CountryTable, _addFields );

					// Update the Country collection to reflect the new data
					viewModel.Country = new ObservableCollection<CountryModel>( DBCommands.GetCountryList() );

					dispStatusLine.Text = $"{inpCountryName.Text} {( string ) FindResource( "Maintanance.Statusline.NotSaved.Unchanged" )}";
				}
				else
				{
					dispStatusLine.Text = $"{( string ) FindResource( "Maintanance.Statusline.NotSaved.AlreadyExcists" )}";
				}
			}
			else
			{
				// Item is an excisting item that has been changed
				string [ , ] _whereFields = new string [ 1, 3 ] { { DBNames.CountryFieldNameName, DBNames.CountryFieldTypeName, inpCountryName.Text.ToLower() } };


				// Check if the entered (to be) changed value is already available in the table
				int _checkPresence = DBCommands.CheckForRecords(DBNames.CountryTable, _whereFields);

				if ( _checkPresence == 0 )
				{
					_whereFields = new string [ 1, 3 ] { { DBNames.CountryFieldNameId, DBNames.CountryFieldTypeId, inpCountryId.Text } };

					// update item in the table
					string [ , ] _updateFields = new string [ 1, 3 ] { { DBNames.CountryFieldNameName, DBNames.CountryFieldTypeName, inpCountryName.Text } };
					_ = DBCommands.UpdateInTable( DBNames.CountryTable, _updateFields, _whereFields );

					// Update the Country collection to reflect the new data
					viewModel.Country = new ObservableCollection<CountryModel>( DBCommands.GetCountryList() );

					dispStatusLine.Text = $"{( string ) FindResource( "Maintanance.Statusline.NotSaved.DataOf" )} {inpCountryName.Text} {( string ) FindResource( "Maintanance.Statusline.NotSaved.Changed" )}";
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

	private void SelectionChanged( object sender, SelectionChangedEventArgs e )
	{
		if ( CountryDataGrid.SelectedItem != null )
		{
			CountryDataGrid.ScrollIntoView( CountryDataGrid.SelectedItem );
		}
	}
}
