namespace Modelbouwer.ViewModels;
public class CombinedInventoryOrderViewModel
{
	public SupplyOrderViewModel SupplyOrderViewModel { get; set; }
	public SupplyReceiptViewModel SupplyReceiptViewModel { get; set; }
	public ProductViewModel ProductViewModel { get; set; }

	public CombinedInventoryOrderViewModel()
	{
		SupplyOrderViewModel = new();
		SupplyReceiptViewModel = new();
		ProductViewModel = new();
	}

	public void RefreshAll()
	{
		SupplyOrderViewModel.Refresh();
		SupplyReceiptViewModel.Refresh();
		ProductViewModel.Refresh();
	}
}
