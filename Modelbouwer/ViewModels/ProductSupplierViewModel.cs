using System.Collections.ObjectModel;

namespace Modelbouwer.ViewModels;
public partial class productSupplierViewModel : ObservableObject
{
	[ObservableProperty]
	public int productSupplierId;

	[ObservableProperty]
	public int productSupplierProductId;

	[ObservableProperty]
	public int productSupplierSupplierId;

	[ObservableProperty]
	public int productSupplierCurrencyId;

	[ObservableProperty]
	public string? productSupplierProductNumber;

	[ObservableProperty]
	public string? productSupplierProductName;

	[ObservableProperty]
	public double productSupplierPriceId;

	[ObservableProperty]
	public string? productSupplierURLId;

	[ObservableProperty]
	public string? productSupplierDefaultSupplier;

	public ObservableCollection<ProductSupplierModel> ProductSupplier
	{
		get => _productsupplier;
		set
		{
			if ( _productsupplier != value )
			{
				_productsupplier = value;
				OnPropertyChanged( nameof( ProductSupplier ) );
			}
		}
	}
	private ObservableCollection<ProductSupplierModel>? _productsupplier;
}
