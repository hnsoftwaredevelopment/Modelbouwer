namespace Modelbouwer.ViewModels;
public partial class InventoryViewModel : ObservableObject
{
	[ObservableProperty]
	public int productId;

	[ObservableProperty]
	public string? productCode;

	[ObservableProperty]
	public string? productName;

	[ObservableProperty]
	public double productPrice;

	[ObservableProperty]
	public double productMinimalStock;

	[ObservableProperty]
	public string? productCategory;

	[ObservableProperty]
	public string? productStorageLocation;

	[ObservableProperty]
	public double productInventory;

	[ObservableProperty]
	public double productInventoryValue;

	[ObservableProperty]
	public double productInOrder;

	[ObservableProperty]
	public double productVirtualInventory;

	[ObservableProperty]
	public double productVirtualInventoryValue;

	[ObservableProperty]
	public double productShortInventory;

	[ObservableProperty]
	public double productTempShortInventory;

	private ObservableCollection<InventoryModel>? _inventory;

	public ObservableCollection<InventoryModel> Inventory
	{
		get => _inventory;
		set
		{
			if ( _inventory != value )
			{
				_inventory = value;
				OnPropertyChanged( nameof( Inventory ) );
			}
		}
	}

	public InventoryViewModel()
	{
		Inventory = [ .. DBCommands.GetInventory() ];
	}
}
