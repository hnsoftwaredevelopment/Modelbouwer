namespace Modelbouwer.ViewModels;
public partial class supplierContactViewModel : ObservableObject
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
	public string? supplierContactMail;

	[ObservableProperty]
	public string? supplierContactPhone;
}
