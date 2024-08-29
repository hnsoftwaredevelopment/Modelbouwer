using System.Collections.ObjectModel;

namespace Modelbouwer.ViewModels;
public partial class SupplierContactTypeViewModel : ObservableObject
{
	[ObservableProperty]
	public string? contactTypeName;

	[ObservableProperty]
	public int contactTypeId;

	public ObservableCollection<SupplierContactTypeModel> SupplierContactType
	{
		get => _suppliercontacttype;
		set
		{
			if ( _suppliercontacttype != value )
			{
				_suppliercontacttype = value;
				OnPropertyChanged( nameof( SupplierContactType ) );
			}
		}
	}
	private ObservableCollection<SupplierContactTypeModel>? _suppliercontacttype;
}
