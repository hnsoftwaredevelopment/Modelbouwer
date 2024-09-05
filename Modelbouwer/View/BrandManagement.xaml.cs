using System.Collections.ObjectModel;

namespace Modelbouwer.View;
/// <summary>
/// Interaction logic for BrandManagement.xaml
/// </summary>
public partial class BrandManagement : Page
{
	public BrandManagement()
	{
		InitializeComponent();
		DataContext = new BrandViewModel();
	}

	private void ButtonNew( object sender, RoutedEventArgs e )
	{
		inpBrandId.Text = "0";
		if ( DataContext is BrandViewModel viewModel )
		{
			viewModel.AddNewItem();
		}
	}

	private void ButtonDelete( object sender, RoutedEventArgs e )
	{
		BrandViewModel viewModel = (BrandViewModel)DataContext;

		// Check if a row is selected (check if there is a known Id)
		if ( inpBrandId != null )
		{
			string [ , ] _whereFields = new string [ 1, 3 ] { { DBNames.BrandFieldNameId, DBNames.BrandFieldTypeId, inpBrandId.Text } };
			string _result = DBCommands.DeleteRecord( DBNames.BrandTable, _whereFields );

			viewModel.Brand = new ObservableCollection<BrandModel>( DBCommands.GetBrandList() );

			dispStatusLine.Text = $"{inpBrandName.Text} {_result}";
		}
	}
	private void ButtonSave( object sender, RoutedEventArgs e )
	{
		// Save can only be performed if a name is entered
		if ( inpBrandName.Text != "" )
		{
			BrandViewModel viewModel = (BrandViewModel)DataContext;

			// Check if item is new or has to be updated
			if ( inpBrandId.Text == "0" )
			{
				// Item is new so add to database table
				string [ , ] _whereFields = new string [ 1, 3 ] { { DBNames.BrandFieldNameName, DBNames.BrandFieldTypeName, inpBrandName.Text.ToLower() } };


				// Check if the entered (to be) new value is already available in the table
				int _checkPresence = DBCommands.CheckForRecords(DBNames.BrandTable, _whereFields);

				if ( _checkPresence == 0 )
				{
					// Add item to the table
					string [ , ] _addFields = new string [ 1, 3 ] { { DBNames.BrandFieldNameName, DBNames.BrandFieldTypeName, inpBrandName.Text } };
					_ = DBCommands.InsertInTable( DBNames.BrandTable, _addFields );

					// Update the Brand collection to reflect the new data
					viewModel.Brand = new ObservableCollection<BrandModel>( DBCommands.GetBrandList() );

					dispStatusLine.Text = $"{inpBrandName.Text} {( string ) FindResource( "Maintanance.Statusline.NotSaved.Unchanged" )}";
				}
				else
				{
					dispStatusLine.Text = $"{( string ) FindResource( "Maintanance.Statusline.NotSaved.AlreadyExcists" )}";
				}
			}
			else
			{
				// Item is an excisting item that has been changed
				string [ , ] _whereFields = new string [ 1, 3 ] { { DBNames.BrandFieldNameName, DBNames.BrandFieldTypeName, inpBrandName.Text.ToLower() } };


				// Check if the entered (to be) changed value is already available in the table
				int _checkPresence = DBCommands.CheckForRecords(DBNames.BrandTable, _whereFields);

				if ( _checkPresence == 0 )
				{
					_whereFields = new string [ 1, 3 ] { { DBNames.BrandFieldNameId, DBNames.BrandFieldTypeId, inpBrandId.Text } };

					// update item in the table
					string [ , ] _updateFields = new string [ 1, 3 ] { { DBNames.BrandFieldNameName, DBNames.BrandFieldTypeName, inpBrandName.Text } };
					_ = DBCommands.UpdateInTable( DBNames.BrandTable, _updateFields, _whereFields );

					// Update the Brand collection to reflect the new data
					viewModel.Brand = new ObservableCollection<BrandModel>( DBCommands.GetBrandList() );

					dispStatusLine.Text = $"{( string ) FindResource( "Maintanance.Statusline.NotSaved.DataOf" )} {inpBrandName.Text} {( string ) FindResource( "Maintanance.Statusline.NotSaved.Changed" )}";
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
		if ( BrandDataGrid.SelectedItem != null )
		{
			BrandDataGrid.ScrollIntoView( BrandDataGrid.SelectedItem );
		}
	}
}
