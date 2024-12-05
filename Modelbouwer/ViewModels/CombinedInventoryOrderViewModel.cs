namespace Modelbouwer.ViewModels;
public class CombinedInventoryOrderViewModel
{
	public InventoryOrderViewModel InventoryOrderViewModel { get; set; }
	public InventoryOrderSupplierViewModel InventoryOrderSupplierViewModel { get; set; }

	public CombinedInventoryOrderViewModel()
	{
		InventoryOrderViewModel = new();
		InventoryOrderSupplierViewModel = new();
	}
}
