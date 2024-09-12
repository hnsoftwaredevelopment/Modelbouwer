namespace Modelbouwer.ViewModels;
public class CombinedSupplierViewModel
{
	public SupplierViewModel SupplierViewModel { get; set; }
	public SupplierContactViewModel SupplierContactViewModel { get; set; }
	public SupplierContactTypeViewModel SupplierContactTypeViewModel { get; set; }
	public CurrencyViewModel CurrencyViewModel { get; set; }

	public CombinedSupplierViewModel()
	{
		SupplierViewModel = new();
		SupplierContactViewModel = new();
		SupplierContactTypeViewModel = new();
		CurrencyViewModel = new();
	}
}
