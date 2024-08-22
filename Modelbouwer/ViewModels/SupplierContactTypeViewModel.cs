namespace Modelbouwer.ViewModels;
public partial class SupplierContactTypeViewModel : ObservableObject
{
	[ObservableProperty]
	public string? contactTypeName;

	[ObservableProperty]
	public int contactTypeId;
}
