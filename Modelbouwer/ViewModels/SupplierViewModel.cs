using System.Collections.ObjectModel;

namespace Modelbouwer.ViewModels;
public partial class supplierViewModel : ObservableObject
{
	[ObservableProperty]
	public int supplierId;

	[ObservableProperty]
	public string? supplierCode;

	[ObservableProperty]
	public string? supplierName;

	[ObservableProperty]
	public string? supplierAddress1;

	[ObservableProperty]
	public string? supplierAddress2;

	[ObservableProperty]
	public string? supplierZip;

	[ObservableProperty]
	public string? supplierCity;

	[ObservableProperty]
	public string? supplierURL;

	[ObservableProperty]
	public double supplierShippingCosts;

	[ObservableProperty]
	public double supplierMinShippingCosts;

	[ObservableProperty]
	public double supplierOrderCosts;

	[ObservableProperty]
	public string? supplierMemo;

	[ObservableProperty]
	public int supplierCurrencyId;

	[ObservableProperty]
	public int supplierCountryId;

	public ObservableCollection<SupplierModel> Supplier
	{
		get => _supplier;
		set
		{
			if ( _supplier != value )
			{
				_supplier = value;
				OnPropertyChanged( nameof( Supplier ) );
			}
		}
	}
	private ObservableCollection<SupplierModel>? _supplier;
}
