namespace Modelbouwer.ViewModels;
public partial class SupplierContactTypeViewModel : ObservableObject
{
	[ObservableProperty]
	public string? contactTypeName;

	[ObservableProperty]
	public int contactTypeId;

	public ObservableCollection<SupplierContactTypeModel>? SupplierContactType { get; set; }

	private readonly ObservableCollection<SupplierContactTypeModel>? _suppliercontacttype;

	[ObservableProperty]
	private SupplierContactTypeModel? _selectedContactType;

	private readonly ObservableCollection<SupplierContactTypeModel>? _contacttype;

	[ObservableProperty]
	public SupplierContactTypeModel? selectedItem;

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

	public void AddNewItem()
	{
		// Voeg het nieuwe, lege item toe aan de lijst
		SupplierContactTypeModel newContactType = new()
		{
			ContactTypeId = 0,
			ContactTypeName = string.Empty
		};

		SupplierContactType.Add( newContactType );
		SelectedContactType = newContactType;
		IsAddingNew = true;
	}

	public SupplierContactTypeViewModel()
	{
		SupplierContactType = new ObservableCollection<SupplierContactTypeModel>( collection: DBCommands.GetContactTypeList() );

		if ( SupplierContactType != null && SupplierContactType.Any() )
		{
			SelectedContactType = SupplierContactType.First();
		}
	}
}
