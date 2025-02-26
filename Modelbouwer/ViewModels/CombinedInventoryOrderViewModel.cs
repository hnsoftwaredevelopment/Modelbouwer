namespace Modelbouwer.ViewModels;
public class CombinedInventoryOrderViewModel
{
	public SupplyOrderViewModel SupplyOrderViewModel { get; set; }
	public ProductViewModel ProductViewModel { get; set; }

	public CombinedInventoryOrderViewModel()
	{
		SupplyOrderViewModel = new();
		ProductViewModel = new();
	}

	public void RefreshAll()
	{
		SupplyOrderViewModel.Refresh();
		ProductViewModel.Refresh();
	}
}
