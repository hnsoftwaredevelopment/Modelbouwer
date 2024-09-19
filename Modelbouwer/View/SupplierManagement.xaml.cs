#pragma warning disable CS8601 // Possible null reference assignment.
using System.Windows.Documents;

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

	#region Open Supplier Manitenance page on specific Tab page
	public void SelectTab( string tabName )
	{
		switch ( tabName.ToLower() )
		{
			case "contacts":
				TabControl.SelectedItem = ContactsTab;
				break;
		}
	}
	#endregion

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
			viewModel.SupplierViewModel.Supplier = new ObservableCollection<SupplierModel>( DBCommands.GetSupplierList() );
		}
		SupplierCountryId.Text = string.IsNullOrEmpty( SupplierCountryId.Text ) ? "1" : SupplierCountryId.Text;
		SupplierCurrencyId.Text = string.IsNullOrEmpty( SupplierCurrencyId.Text ) ? "1" : SupplierCurrencyId.Text;
		SupplierContactTypeId.Text = string.IsNullOrEmpty( SupplierContactTypeId.Text ) ? "1" : SupplierContactTypeId.Text;
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

				//viewModel.SupplierViewModel.Supplier = new ObservableCollection<SupplierModel>( DBCommands.GetSupplierList() );

				viewModel.SupplierViewModel.Supplier.Remove( selectedSupplier );

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

		// Cost fields should not be empty
		SendCosts.Text = string.IsNullOrEmpty( SendCosts.Text ) ? "0.00" : SendCosts.Text;
		SendCostsMax.Text = string.IsNullOrEmpty( SendCostsMax.Text ) ? "0.00" : SendCostsMax.Text;
		OrderCosts.Text = string.IsNullOrEmpty( OrderCosts.Text ) ? "0.00" : OrderCosts.Text;

		// Country and Currency should be added. When empty, first Country and Currency will be selected
		SupplierCountryId.Text = string.IsNullOrEmpty( SupplierCountryId.Text ) ? "1" : SupplierCountryId.Text;
		SupplierCurrencyId.Text = string.IsNullOrEmpty( SupplierCurrencyId.Text ) ? "1" : SupplierCurrencyId.Text;
		SupplierContactTypeId.Text = string.IsNullOrEmpty( SupplierContactTypeId.Text ) ? "1" : SupplierContactTypeId.Text;

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
						SupplierId.Text = _newId;

						// Update memo field for the newly saved supplier
						string[,] _whereFields = new string[1, 3]
						{
							{ DBNames.SupplierFieldNameId, DBNames.SupplierFieldTypeId, _newId }
						};

						string _memo = GeneralHelper.GetRichTextFromFlowDocument(SupplierMemo.Document);
						DBCommands.UpdateMemoFieldInTable( DBNames.SupplierTable, _whereFields, DBNames.SupplierFieldNameMemo, _memo );

						viewModel.SupplierViewModel.Supplier = new ObservableCollection<SupplierModel>( DBCommands.GetSupplierList() );

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

					viewModel.SupplierViewModel.Supplier = new ObservableCollection<SupplierModel>( DBCommands.GetSupplierList() );

					_result = $"{( string ) FindResource( "Edit.Supplier.Changed" )} ({SupplierName.Text})";
				}

				// Refresh the suppliers list
				viewModel.SupplierViewModel.Supplier = new ObservableCollection<SupplierModel>( DBCommands.GetSupplierList() );
				var selectedSupplier = viewModel.SupplierViewModel.SelectedSupplier;
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
		// Clear the input fields for the new contact
		SupplierContactId.Text = "0";
		SupplierContactName.Text = string.Empty;
		SupplierContactMail.Text = string.Empty;
		SupplierContactPhone.Text = string.Empty;
		SupplierContactTypeId.Text = "1";

		if ( DataContext is CombinedSupplierViewModel viewModel )
		{
			// Add a new contact
			viewModel.SupplierContactViewModel.AddNewItem( SupplierId.Text );

			// Ensure FilteredContacts is updated
			viewModel.SupplierContactViewModel.FilterContactsBySupplierId( viewModel.SupplierViewModel.SelectedSupplier.SupplierId );


			// Select the newly added contact if it exists
			if ( viewModel.SupplierContactViewModel.FilteredContacts.Any() )
			{
				viewModel.SupplierContactViewModel.SelectedContact = viewModel.SupplierContactViewModel.FilteredContacts.Last();
			}

			// Refresh the DataGrid to show the new contact
			ContactsDataGrid.Items.Refresh();

			//viewModel.SupplierContactViewModel.SupplierContact = new ObservableCollection<SupplierContactModel>( DBCommands.GetContactList() );
			//var selectedSupplier = viewModel.SupplierViewModel.SelectedSupplier;
		}
		SupplierContactTypeId.Text = string.IsNullOrEmpty( SupplierContactTypeId.Text ) ? "1" : SupplierContactTypeId.Text;
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
					if ( SupplierContactId.Text == "0" || SupplierContactId.Text == "" )
					{
						// Nieuw item, dus voeg toe aan de database
						string[,] _whereFields = new string[2, 3]
						{
							{ DBNames.SupplierFieldNameId, DBNames.SupplierFieldTypeId, SupplierId.Text.ToLower() },
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
								{ DBNames.SupplierContactFieldNameTypeId, DBNames.SupplierContactFieldTypeTypeId, SupplierContactType.SelectedValue.ToString() },
								{ DBNames.SupplierContactFieldNameMail, DBNames.SupplierContactFieldTypeMail, SupplierContactMail.Text },
								{ DBNames.SupplierContactFieldNamePhone, DBNames.SupplierContactFieldTypePhone, SupplierContactPhone.Text }
							};
							_ = DBCommands.InsertInTable( DBNames.SupplierContactTable, _addFields );

							// Werk de SupplierContact-collectie bij om de nieuwe data weer te geven
							//viewModel.SupplierContactViewModel.SupplierContact = new ObservableCollection<SupplierContactModel>( DBCommands.GetContactList );
							//viewModel.SupplierContactViewModel.SupplierContact = new ObservableCollection<SupplierContactModel>( DBCommands.GetContactList() );
							viewModel.SupplierContactViewModel.SupplierContact = new ObservableCollection<SupplierContactModel>( DBCommands.GetContactList() );
							var selectedSupplier = viewModel.SupplierViewModel.SelectedSupplier;


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
						string[,] _whereFields = new string [ 1, 3 ]
							{
								{ DBNames.SupplierContactFieldNameId, DBNames.SupplierContactFieldTypeId, SupplierContactId.Text }
							};

						// Werk het item in de tabel bij
						string[,] _updateFields = new string[4, 3]
							{
								{ DBNames.SupplierContactFieldNameName, DBNames.SupplierContactFieldTypeName, SupplierContactName.Text },
								{ DBNames.SupplierContactFieldNameTypeId, DBNames.SupplierContactFieldTypeTypeId, SupplierContactType.SelectedValue.ToString() },
								{ DBNames.SupplierContactFieldNameMail, DBNames.SupplierContactFieldTypeMail, SupplierContactMail.Text },
								{ DBNames.SupplierContactFieldNamePhone, DBNames.SupplierContactFieldTypePhone, SupplierContactPhone.Text }
							};
						_ = DBCommands.UpdateInTable( DBNames.SupplierContactTable, _updateFields, _whereFields );

						viewModel.SupplierContactViewModel.SupplierContact = new ObservableCollection<SupplierContactModel>( DBCommands.GetContactList() );
						var selectedSupplier = viewModel.SupplierViewModel.SelectedSupplier;

						dispStatusLine.Text = $"{( string ) FindResource( "Maintanance.Statusline.NotSaved.DataOf" )} {SupplierContactName.Text} {( string ) FindResource( "Maintanance.Statusline.NotSaved.Changed" )}";
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

	#region Open the mail client to send a mail to the selected contact
	private void SendMail( object sender, RoutedEventArgs e )
	{
		if ( DataContext is CombinedSupplierViewModel viewModel )
		{
			var selectedContact = (SupplierContactModel)((System.Windows.Controls.Button)sender).Tag;

			if ( selectedContact != null )
			{
				//Determine what salution to use
				int _hour = DateTime.Now.Hour;
				string _salution = "";

				if ( _hour >= 5 && _hour < 12 ) { _salution = $"{( string ) FindResource( "Maintanance.Mail.Salution.Morning" )}"; }
				else if ( _hour >= 12 && _hour < 18 ) { _salution = $"{( string ) FindResource( "Maintanance.Mail.Salution.Afternoon" )}"; }
				else if ( _hour >= 18 && _hour < 24 ) { _salution = $"{( string ) FindResource( "Maintanance.Mail.Salution.Evening" )}"; }
				else { _salution = $"{( string ) FindResource( "Maintanance.Mail.Salution.Night" )}"; }
				var emailAddress = selectedContact.SupplierContactMail;
				var contactName = selectedContact.SupplierContactName;
				var subject = $"{( string ) FindResource( "Maintanance.Mail.Subject" )}";
				var body = $"{_salution} {contactName},\n\n";

				// Construct the mailto URL
				var mailtoUrl = $"mailto:{emailAddress}?subject={Uri.EscapeDataString(subject)}&body={Uri.EscapeDataString(body)}";

				// Open the default mail client
				Process.Start( new ProcessStartInfo
				{
					FileName = mailtoUrl,
					UseShellExecute = true
				} );
			}
		}
	}
	#endregion

	#region Update the MemoField
	private void UpdateRichTextBox( string memoText )
	{
		// Create a new FlowDocument and Paragraph
		FlowDocument flowDoc = new FlowDocument();
		Paragraph paragraph = new Paragraph();

		// Add the memo text as a Run in the Paragraph
		paragraph.Inlines.Add( new Run( memoText ) );

		// Set the FlowDocument to the RichTextBox
		SupplierMemo.Document = flowDoc;
		flowDoc.Blocks.Add( paragraph );
	}
	#endregion

	private void SetRtfContent( string rtfContent )
	{
		using ( var stream = new MemoryStream( Encoding.UTF8.GetBytes( rtfContent ) ) )
		{
			TextRange textRange = new TextRange(SupplierMemo.Document.ContentStart, SupplierMemo.Document.ContentEnd);
			textRange.Load( stream, System.Windows.DataFormats.Rtf );
		}
	}

	private void DataGrid_SelectionChanged( object sender, SelectionChangedEventArgs e )
	{
		if ( DataContext is CombinedSupplierViewModel viewModel )
		{
			var selectedSupplier = viewModel.SupplierViewModel.SelectedSupplier;
			if ( selectedSupplier != null )
			{
				//UpdateRichTextBox( selectedSupplier.SupplierMemo );
				SetRtfContent( selectedSupplier.SupplierMemo );
			}
		}
	}
}
#pragma warning restore CS8601 // Possible null reference assignment.