namespace Modelbouwer.ViewModels;
public class CombinedInventoryOrderViewModel
{
	public SupplyOrderViewModel SupplyOrderViewModel { get; set; }

	public CombinedInventoryOrderViewModel()
	{
		SupplyOrderViewModel = new();
	}
}
