namespace Modelbouwer.ViewModels;
public class CombinedInventoryViewModel
{
	public InventoryViewModel InventoryViewModel { get; set; }

	public CombinedInventoryViewModel()
	{
		InventoryViewModel = new();
	}
}
