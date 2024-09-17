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

	#region Open browser with Supplier URL
	private void ButtonWeb( object sender, RoutedEventArgs e )
	{
		ProcessStartInfo? browserwindow = new()
		{
			UseShellExecute = true,
			FileName = SupplierUrl.Text
		};
		Process.Start( browserwindow );

	}
	#endregion

	#region Create new supplier
	private void ButtonNew( object sender, RoutedEventArgs e )
	{
		SupplierId.Text = "0";
		if ( DataContext is CombinedSupplierViewModel viewModel )
		{
			viewModel.SupplierViewModel.AddNewItem();
		}
	}
	#endregion

	#region Delete selected supplier and all related contacts
	private void ButtonDelete( object sender, RoutedEventArgs e )
	{
		var viewModel = DataContext as CombinedSupplierViewModel;

		if ( viewModel != null )
		{
			var selectedSupplier = viewModel.SupplierViewModel.SelectedSupplier;

			if ( selectedSupplier != null )
			{
				string [ , ] _whereFields = new string [ 1, 3 ]
				{
					{ DBNames.SupplierFieldNameId, DBNames.SupplierFieldTypeId, SupplierId.Text }
				};

				string _result = DBCommands.DeleteRecord( DBNames.SupplierTable, _whereFields );

				string [ , ] _whereContactFields = new string [ 1, 3 ]
				{
					{ DBNames.SupplierContactFieldNameSupplierId, DBNames.SupplierContactFieldTypeSupplierId, SupplierId.Text }
				};

				string _result2 = DBCommands.DeleteRecord( DBNames.SupplierContactTable, _whereContactFields );

				viewModel.SupplierViewModel.Supplier = new ObservableCollection<SupplierModel>( DBCommands.GetSupplierList() );

				if ( _result2 == "" )
				{ dispStatusLine.Text = $"{_result}"; }
				else { dispStatusLine.Text = $"{_result}, {_result2}"; }
			}
		}
		else
		{
			dispStatusLine.Text = $"{( string ) FindResource( "Edit.Supplier.NoSupplierSelected.Error" )}";
		}
	}
	#endregion

	#region Save changes of the (new) Supplier to the database table
	private void ButtonSave( object sender, RoutedEventArgs e )
	{
		// Check if Supplier code and or name are filled if yes statusline variable will be filled later, ottherwise its filled here
		string _result = (SupplierCode.Text, SupplierName.Text) switch
		{
			("", "") => $"{( string ) FindResource( "Edit.Supplier.SupplierCodeAndName.Empty" )}",
			("", _) => $"{( string ) FindResource( "Edit.Supplier.SupplierCode.Empty" )}",
			(_, "") => $"{( string ) FindResource( "Edit.Supplier.SupplierNamee.Empty" )}",
			_ => ""
		};

		// Country and Currency should be added. When empty, first Country and Currency will be selected
		SupplierCountryId.Text = string.IsNullOrEmpty( SupplierCountryId.Text ) ? "1" : SupplierCountryId.Text;
		SupplierCurrencyId.Text = string.IsNullOrEmpty( SupplierCurrencyId.Text ) ? "1" : SupplierCurrencyId.Text;

		if ( SupplierCode.Text != "" && SupplierName.Text != "" )
		{
			if ( DataContext is CombinedSupplierViewModel viewModel )
			{
				// If supplier is new, add to databse otherwise change the existing Supplier
				if ( SupplierId.Text == "0" )
				{
					// New Supplier
					// Check if Code or Name already excist
					string[,] _whereCodeFields = new string[1, 3]
					{
						{ DBNames.SupplierFieldNameCode, DBNames.SupplierFieldTypeCode, SupplierCode.Text.ToLower() }
					};

					int _checkCodePresence = DBCommands.CheckForRecords(DBNames.SupplierTable, _whereCodeFields);

					string[,] _whereNameFields = new string[1, 3]
					{
						{ DBNames.SupplierFieldNameName, DBNames.SupplierFieldTypeName, SupplierName.Text.ToLower() }
					};

					int _checkNamePresence = DBCommands.CheckForRecords(DBNames.SupplierTable, _whereNameFields);

					if ( _checkCodePresence + _checkNamePresence == 0 )
					{
						// Save new supplier to the table, memo has to be handeled different.
						string[,] _addFields = new string[12, 3]
						{
							{ DBNames.SupplierFieldNameCode, DBNames.SupplierFieldTypeCode, SupplierCode.Text },
							{ DBNames.SupplierFieldNameName, DBNames.SupplierFieldTypeName, SupplierName.Text },
							{ DBNames.SupplierFieldNameAddress1, DBNames.SupplierFieldTypeAddress1, SupplierAddress1.Text },
							{ DBNames.SupplierFieldNameAddress2, DBNames.SupplierFieldTypeAddress2, SupplierAddress2.Text },
							{ DBNames.SupplierFieldNameZip, DBNames.SupplierFieldTypeZip, SupplierZip.Text },
							{ DBNames.SupplierFieldNameCity, DBNames.SupplierFieldTypeCity, SupplierCity.Text },
							{ DBNames.SupplierFieldNameUrl, DBNames.SupplierFieldTypeUrl, SupplierUrl.Text },
							{ DBNames.SupplierFieldNameCountryId, DBNames.SupplierFieldTypeCountryId, SupplierCountryId.Text },
							{ DBNames.SupplierFieldNameCurrencyId, DBNames.SupplierFieldTypeCurrencyId, SupplierCurrencyId.Text },
							{ DBNames.SupplierFieldNameShippingCosts, DBNames.SupplierFieldTypeShippingCosts, SendCosts.Text },
							{ DBNames.SupplierFieldNameMinShippingCosts, DBNames.SupplierFieldTypeMinShippingCosts, SendCostsMax.Text },
							{ DBNames.SupplierFieldNameOrderCosts, DBNames.SupplierFieldTypeOrderCosts, OrderCosts.Text }
						};
						_ = DBCommands.InsertInTable( DBNames.SupplierTable, _addFields );

						// Get the Id of the newly added supplier
						var _newId = DBCommands.GetLatestIdFromTable( DBNames.SupplierTable );

						// Update memo field for the newly saved supplier
						string[,] _whereFields = new string[1, 3]
						{
							{ DBNames.SupplierFieldNameId, DBNames.SupplierFieldTypeId, _newId }
						};

						string _memo = GeneralHelper.GetRichTextFromFlowDocument(SupplierMemo.Document);
						DBCommands.UpdateMemoFieldInTable( DBNames.SupplierTable, _whereFields, DBNames.SupplierFieldNameMemo, _memo );

						_result = $"{( string ) FindResource( "Edit.Supplier.Added" )} ({SupplierName.Text})";
					}
					else
					{
						// Not able to save new supplier because code and/or name already excist
						_result = (_checkCodePresence, _checkNamePresence) switch
						{
							(0, 0 ) => "",
							(_, 0 ) => $"{( string ) FindResource( "Edit.Supplier.SupplierCode.Excist" )}",
							(0, _ ) => $"{( string ) FindResource( "Edit.Supplier.SupplierName.Excist" )}",
							(_, _ ) => $"{( string ) FindResource( "Edit.Supplier.SupplierCodeAndName.Excist" )}"
						};
					}
				}
				else
				{
					// Change excisting supplier
					string[,] _whereFields = new string[1, 3]
					{
						{ DBNames.SupplierFieldNameId, DBNames.SupplierFieldTypeId, SupplierId.Text }
					};

					string[,] _updateFields = new string[12, 3]
					{
						{ DBNames.SupplierFieldNameCode, DBNames.SupplierFieldTypeCode, SupplierCode.Text },
						{ DBNames.SupplierFieldNameName, DBNames.SupplierFieldTypeName, SupplierName.Text },
						{ DBNames.SupplierFieldNameAddress1, DBNames.SupplierFieldTypeAddress1, SupplierAddress1.Text },
						{ DBNames.SupplierFieldNameAddress2, DBNames.SupplierFieldTypeAddress2, SupplierAddress2.Text },
						{ DBNames.SupplierFieldNameZip, DBNames.SupplierFieldTypeZip, SupplierZip.Text },
						{ DBNames.SupplierFieldNameCity, DBNames.SupplierFieldTypeCity, SupplierCity.Text },
						{ DBNames.SupplierFieldNameUrl, DBNames.SupplierFieldTypeUrl, SupplierUrl.Text },
						{ DBNames.SupplierFieldNameCountryId, DBNames.SupplierFieldTypeCountryId, SupplierCountryId.Text },
						{ DBNames.SupplierFieldNameCurrencyId, DBNames.SupplierFieldTypeCurrencyId, SupplierCurrencyId.Text },
						{ DBNames.SupplierFieldNameShippingCosts, DBNames.SupplierFieldTypeShippingCosts, SendCosts.Text },
						{ DBNames.SupplierFieldNameMinShippingCosts, DBNames.SupplierFieldTypeMinShippingCosts, SendCostsMax.Text },
						{ DBNames.SupplierFieldNameOrderCosts, DBNames.SupplierFieldTypeOrderCosts, OrderCosts.Text }
					};
					_ = DBCommands.UpdateInTable( DBNames.SupplierTable, _updateFields, _whereFields );

					// Update memo field
					string _memo = GeneralHelper.GetRichTextFromFlowDocument(SupplierMemo.Document);
					DBCommands.UpdateMemoFieldInTable( DBNames.SupplierTable, _whereFields, DBNames.SupplierFieldNameMemo, _memo );

					_result = $"{( string ) FindResource( "Edit.Supplier.Changed" )} ({SupplierName.Text})";
				}

				// Refresh the suppliers list
				viewModel.SupplierViewModel.Supplier = new ObservableCollection<SupplierModel>( DBCommands.GetSupplierList() );
			}
			else
			{
				dispStatusLine.Text = $"{( string ) FindResource( "Maintanance.Statusline.NotSaved.Empty" )}";
			}
		}

		dispStatusLine.Text = $"{_result}";
	}
	#endregion

	#region Add new contact for the selected supplier
	private void ButtonNewContact( object sender, RoutedEventArgs e )
	{
		SupplierContactId.Text = "0";
		if ( DataContext is CombinedSupplierViewModel viewModel )
		{
			viewModel.SupplierContactViewModel.AddNewItem();
		}
	}
	#endregion

	#region Delete selected contact
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
	#endregion

	#region Save changes of the (new) contact for the selected supplier
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
	#endregion
}