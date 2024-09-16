namespace Modelbouwer.View;

/// <summary>
/// Interaction logic for SupplierManagement.xaml
/// </summary>
public partial class SupplierManagement : Page
{
	public SupplierManagement()
	{
		InitializeComponent();
	}

	private void ButtonWeb( object sender, RoutedEventArgs e )
	{
		ProcessStartInfo? browserwindow = new()
		{
			UseShellExecute = true,
			FileName = SupplierUrl.Text
		};
		Process.Start( browserwindow );

	}

	private void ButtonNew( object sender, RoutedEventArgs e )
	{

	}

	private void ButtonDelete( object sender, RoutedEventArgs e )
	{
	}

	private void ButtonSave( object sender, RoutedEventArgs e )
	{

	}

	private void ButtonNewContact( object sender, RoutedEventArgs e )
	{
		SupplierContactId.Text = "0";
		if ( DataContext is CombinedSupplierViewModel viewModel )
		{
			viewModel.SupplierContactViewModel.AddNewItem();
		}
	}

	private void ButtonDeleteContact( object sender, RoutedEventArgs e )
	{
		var viewModel = DataContext as CombinedSupplierViewModel;

		if ( viewModel != null )
		{
			var selectedContact = viewModel.SupplierContactViewModel.SelectedContact;

			if ( selectedContact != null )
			{
				string [ , ] _whereFields = new string [ 1, 3 ]
				{
					{ DBNames.SupplierContactFieldNameId, DBNames.SupplierContactFieldTypeId, SupplierContactId.Text }
				};

				string _result = DBCommands.DeleteRecord( DBNames.SupplierContactTable, _whereFields );

				viewModel.SupplierContactViewModel.SupplierContact = new ObservableCollection<SupplierContactModel>( DBCommands.GetContactList() );
				var selectedSupplier = viewModel.SupplierViewModel.SelectedSupplier;
				if ( selectedSupplier != null )
				{
					viewModel.SupplierContactViewModel.FilterContactsBySupplierId( selectedSupplier.SupplierId );
				}

				dispStatusLine.Text = $"{_result}";
			}
		}
		else
		{
			dispStatusLine.Text = $"{( string ) FindResource( "Edit.SupplierContact.NoContactSelected.Error" )}";
		}
	}

	private void ButtonSaveContact( object sender, RoutedEventArgs e )
	{
		// Save can only be performed if a name is entered
		if ( SupplierContactName.Text != "" )
		{
			var viewModel = DataContext as CombinedSupplierViewModel;
			//SupplierContactViewModel viewModel = (SupplierContactViewModel)DataContext;

			if ( viewModel != null )
			{
				var supplierContactViewModel = viewModel.SupplierContactViewModel;

				if ( !string.IsNullOrEmpty( SupplierContactName.Text ) )
				{
					// Controleer of het item nieuw is of moet worden bijgewerkt
					if ( SupplierContactId.Text == "0" )
					{
						// Nieuw item, dus voeg toe aan de database
						string[,] _whereFields = new string[1, 3]
			{
				{ DBNames.SupplierContactFieldNameName, DBNames.SupplierContactFieldTypeName, SupplierContactName.Text.ToLower() }
			};

						// Controleer of de waarde al beschikbaar is in de tabel
						int _checkPresence = DBCommands.CheckForRecords(DBNames.SupplierContactTable, _whereFields);

						if ( _checkPresence == 0 )
						{
							// Voeg item toe aan de tabel
							string[,] _addFields = new string[5, 3]
				{
					{ DBNames.SupplierContactFieldNameSupplierId, DBNames.SupplierContactFieldTypeSupplierId, SupplierId.Text },
					{ DBNames.SupplierContactFieldNameName, DBNames.SupplierContactFieldTypeName, SupplierContactName.Text },
					{ DBNames.SupplierContactFieldNameTypeId, DBNames.SupplierContactFieldTypeTypeId, SupplierContactTypeId.Text },
					{ DBNames.SupplierContactFieldNameMail, DBNames.SupplierContactFieldTypeMail, SupplierContactMail.Text },
					{ DBNames.SupplierContactFieldNamePhone, DBNames.SupplierContactFieldTypePhone, SupplierContactPhone.Text }
				};
							_ = DBCommands.InsertInTable( DBNames.SupplierContactTable, _addFields );

							// Werk de SupplierContact-collectie bij om de nieuwe data weer te geven
							supplierContactViewModel.SupplierContact = new ObservableCollection<SupplierContactModel>( DBCommands.GetContactList() );

							dispStatusLine.Text = $"{SupplierContactName.Text} {( string ) FindResource( "Maintanance.Statusline.NotSaved.Added" )}";
						}
						else
						{
							dispStatusLine.Text = $"{( string ) FindResource( "Maintanance.Statusline.NotSaved.AlreadyExcists" )}";
						}
					}
					else
					{
						// Bestaand item dat is gewijzigd
						string[,] _whereFields = new string[5, 3]
			{
				{ DBNames.SupplierContactFieldNameSupplierId, DBNames.SupplierContactFieldTypeSupplierId, SupplierId.Text },
				{ DBNames.SupplierContactFieldNameName, DBNames.SupplierContactFieldTypeName, SupplierContactName.Text },
				{ DBNames.SupplierContactFieldNameTypeId, DBNames.SupplierContactFieldTypeTypeId, SupplierContactTypeId.Text },
				{ DBNames.SupplierContactFieldNameMail, DBNames.SupplierContactFieldTypeMail, SupplierContactMail.Text },
				{ DBNames.SupplierContactFieldNamePhone, DBNames.SupplierContactFieldTypePhone, SupplierContactPhone.Text }
			};

						// Controleer of de gewijzigde waarde al beschikbaar is in de tabel
						int _checkPresence = DBCommands.CheckForRecords(DBNames.SupplierContactTable, _whereFields);

						if ( _checkPresence == 0 )
						{
							_whereFields = new string [ 1, 3 ]
							{
					{ DBNames.SupplierContactFieldNameId, DBNames.SupplierContactFieldTypeId, SupplierContactId.Text }
							};

							// Werk het item in de tabel bij
							string[,] _updateFields = new string[5, 3]
				{
					{ DBNames.SupplierContactFieldNameSupplierId, DBNames.SupplierContactFieldTypeSupplierId, SupplierId.Text },
					{ DBNames.SupplierContactFieldNameName, DBNames.SupplierContactFieldTypeName, SupplierContactName.Text },
					{ DBNames.SupplierContactFieldNameTypeId, DBNames.SupplierContactFieldTypeTypeId, SupplierContactTypeId.Text },
					{ DBNames.SupplierContactFieldNameMail, DBNames.SupplierContactFieldTypeMail, SupplierContactMail.Text },
					{ DBNames.SupplierContactFieldNamePhone, DBNames.SupplierContactFieldTypePhone, SupplierContactPhone.Text }
				};
							_ = DBCommands.UpdateInTable( DBNames.SupplierContactTable, _updateFields, _whereFields );

							// Werk de SupplierContact-collectie bij om de nieuwe data weer te geven
							supplierContactViewModel.SupplierContact = new ObservableCollection<SupplierContactModel>( DBCommands.GetContactList() );

							dispStatusLine.Text = $"{( string ) FindResource( "Maintanance.Statusline.NotSaved.DataOf" )} {SupplierContactName.Text} {( string ) FindResource( "Maintanance.Statusline.NotSaved.Changed" )}";
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
	}
}