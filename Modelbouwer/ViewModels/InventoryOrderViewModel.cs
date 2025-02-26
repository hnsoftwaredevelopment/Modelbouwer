using System.ComponentModel;

namespace Modelbouwer.ViewModels;
public partial class InventoryOrderViewModel : ObservableObject
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
	public double productInventory;

	[ObservableProperty]
	public double productInOrder;

	[ObservableProperty]
	public double productShortInventory;

	[ObservableProperty]
	public string? supplierProductNumber;

	[ObservableProperty]
	public string? supplierProductName;

	[ObservableProperty]
	public double supplierPrice;

	[ObservableProperty]
	public int supplierCurrencyId ;

	[ObservableProperty]
	public string? supplierCurrencySymbol;

	[ObservableProperty]
	public string? productFromSupplier;

	[ObservableProperty]
	private bool isSelected;

	private ObservableCollection<InventoryOrderModel>? _inventoryOrder;

	public ObservableCollection<InventoryOrderModel> SelectedProducts { get; set; }

	public ObservableCollection<InventoryOrderModel> InventoryOrder
	{
		get => _inventoryOrder;
		set
		{
			if ( _inventoryOrder != value )
			{
				_inventoryOrder = value;
				OnPropertyChanged( nameof( InventoryOrder ) );
			}
		}
	}

	public InventoryOrderViewModel()
	{
		InventoryOrder = new ObservableCollection<InventoryOrderModel>( DBCommands.GetInventoryOrder() );
		SelectedProducts = new ObservableCollection<InventoryOrderModel>();

		foreach ( InventoryOrderModel product in InventoryOrder )
		{
			product.PropertyChanged += SelectedProduct_PropertyChanged;
		}
	}

	public void Refresh()
	{
		InventoryOrder = new ObservableCollection<InventoryOrderModel>( DBCommands.GetInventoryOrder() );
		SelectedProducts = new ObservableCollection<InventoryOrderModel>();
		OnPropertyChanged( nameof( InventoryOrder ) );
		OnPropertyChanged( nameof( SelectedProducts ) );
	}

	private void SelectedProduct_PropertyChanged( object? sender, PropertyChangedEventArgs e )
	{
		if ( e.PropertyName == nameof( InventoryOrderModel.IsSelected ) )
		{
			InventoryOrderModel? selectedProduct = sender as InventoryOrderModel;
			if ( selectedProduct != null )
			{
				if ( selectedProduct.IsSelected && !SelectedProducts.Contains( selectedProduct ) )
				{
					SelectedProducts.Add( selectedProduct );
				}
				else if ( !selectedProduct.IsSelected && SelectedProducts.Contains( selectedProduct ) )
				{
					SelectedProducts.Remove( selectedProduct );
				}
			}
		}
	}

	public new event PropertyChangedEventHandler? PropertyChanged;
	protected new void OnPropertyChanged( string name ) =>
		PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( name ) );
}
