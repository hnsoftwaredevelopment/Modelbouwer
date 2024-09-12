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

	private readonly ObservableCollection<SupplierContactModel>? _supplierContact;

	[ObservableProperty]
	private SupplierContactModel? _selectedContact;

	public SupplierContactViewModel()
	{
		SupplierContact = new ObservableCollection<SupplierContactModel>( DBCommands.GetContactList() );
	}
}
