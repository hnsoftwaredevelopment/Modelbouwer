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

	private ObservableCollection<InventoryOrderModel>? _selectedProducts;

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
		InventoryOrder = [ .. DBCommands.GetInventoryOrder() ];
		SelectedProducts = [ ];

		foreach ( var product in InventoryOrder )
		{
			product.PropertyChanged += SelectedProduct_PropertyChanged;
		}
	}

	private void SelectedProduct_PropertyChanged( object sender, PropertyChangedEventArgs e )
	{
		if ( e.PropertyName == nameof( InventoryOrderModel.IsSelected ) )
		{
			var selectedProduct = sender as InventoryOrderModel;
			if ( selectedProduct != null )
			{
				if ( selectedProduct.IsSelected && !SelectedProducts.Contains( selectedProduct ) )
					SelectedProducts.Add( selectedProduct );
				else if ( !selectedProduct.IsSelected && SelectedProducts.Contains( selectedProduct ) )
					SelectedProducts.Remove( selectedProduct );
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;
	protected void OnPropertyChanged( string name ) =>
		PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( name ) );
}
