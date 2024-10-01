namespace Modelbouwer.ViewModels;
public partial class SupplierContactViewModel : ObservableObject
{
	[ObservableProperty]
	public int supplierContactId;

	[ObservableProperty]
	public int supplierContactSuppplierId;

	[ObservableProperty]
	public string? supplierContactName;

	[ObservableProperty]
	public int supplierContactContactTypeId;

	[ObservableProperty]
	public string? supplierContactContactType;

	[ObservableProperty]
	public string? supplierContactMail;

	[ObservableProperty]
	public string? supplierContactPhone;

	public ObservableCollection<SupplierContactModel> SupplierContact { get; set; }
	public ObservableCollection<SupplierContactModel> FilteredContacts { get; private set; } = new ObservableCollection<SupplierContactModel>();

	private SupplierModel? _selectedSupplier;

	[ObservableProperty]
	private SupplierContactModel? _selectedContact;

	private bool _isAddingNew;

	public bool IsAddingNew
	{
		get => _isAddingNew;
		set
		{
			if ( _isAddingNew != value )
			{
				_isAddingNew = value;
				OnPropertyChanged( nameof( IsAddingNew ) );
			}
		}
	}

	public void AddNewItem( string supplierId )
	{
		SupplierContactModel newContact = new()
		{
			SupplierContactId = 0,
			SupplierContactSuppplierId = int.Parse(supplierId),
			SupplierContactContactTypeId = 1,
			SupplierContactName = string.Empty,
			SupplierContactContactType = string.Empty,
			SupplierContactMail = string.Empty,
			SupplierContactPhone = string.Empty
		};

		SupplierContact.Add( newContact );
		SelectedContact = newContact;
		IsAddingNew = true;

		// Refresh the list to display
		OnPropertyChanged( nameof( FilteredContacts ) );
		FilterContactsBySupplierId( int.Parse( supplierId ) );
	}

	public SupplierModel? SelectedSupplier
	{
		get => _selectedSupplier;
		set
		{
			if ( SetProperty( ref _selectedSupplier, value ) )
			{
				if ( _selectedSupplier != null )
				{
					FilterContactsBySupplierId( _selectedSupplier.SupplierId );
				}
			}
		}
	}

	public void FilterContactsBySupplierId( int supplierId )
	{
		FilteredContacts.Clear();
		foreach ( SupplierContactModel contact in SupplierContact.Where( c => c.SupplierContactSuppplierId == supplierId ) )
		{
			FilteredContacts.Add( contact );
		}

		//Select first cntact in the list if there are contacts
		if ( FilteredContacts.Any() )
		{
			SelectedContact = FilteredContacts.First();
		}
	}

	public SupplierContactViewModel()
	{
		SupplierContact = new ObservableCollection<SupplierContactModel>( DBCommands.GetContactList() );
	}
}
