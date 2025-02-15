﻿namespace Modelbouwer.ViewModels;
public partial class InventoryOrderSupplierViewModel : ObservableObject
{
	[ObservableProperty]
	public int productSupplierId;

	[ObservableProperty]
	public int productSupplierProductId;

	[ObservableProperty]
	public int productSupplierSupplierId;

	[ObservableProperty]
	public int orderSupplierSupplierId;

	[ObservableProperty]
	public string? productSupplierSupplierName;

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

	[ObservableProperty]
	public bool? productSupplierDefaultSupplierCheck;

	//public ObservableCollection<InventoryOrderModel> SelectedProducts { get; set; } = new ObservableCollection<InventoryOrderModel>();

	//private ObservableCollection<InventoryOrderModel> _productList;
	//public ObservableCollection<InventoryOrderModel> ProductList
	//{
	//	get => _productList;
	//	set
	//	{
	//		if ( _productList != value )
	//		{
	//			if ( _productList != null )
	//			{
	//				foreach ( var product in _productList )
	//				{
	//					product.PropertyChanged -= SelectedProduct_PropertyChanged;
	//				}
	//			}

	//			_productList = value;

	//			if ( _productList != null )
	//			{
	//				foreach ( var product in _productList )
	//				{
	//					product.PropertyChanged += SelectedProduct_PropertyChanged;
	//				}
	//			}

	//			OnPropertyChanged( nameof( ProductList ) );
	//		}
	//	}
	//}

	//private void SelectedProduct_PropertyChanged( object sender, PropertyChangedEventArgs e )
	//{
	//	if ( e.PropertyName == nameof( InventoryOrderModel.IsSelected ) )
	//	{
	//		var selectedProduct = sender as InventoryOrderModel;
	//		if ( selectedProduct != null )
	//		{
	//			if ( selectedProduct.IsSelected && !SelectedProducts.Contains( selectedProduct ) )
	//				SelectedProducts.Add( selectedProduct );
	//			else if ( !selectedProduct.IsSelected && SelectedProducts.Contains( selectedProduct ) )
	//				SelectedProducts.Remove( selectedProduct );
	//		}
	//	}
	//}

	public ObservableCollection<SupplierModel> SupplierList { get; set; } = [ ];
	public ObservableCollection<SupplyOrderModel> SupplierOrderList { get; set; } = [ ];

	public event EventHandler<int>? SelectedSupplierChanged;

	public ObservableCollection<InventoryOrderModel> ProductList { get; set; } = new();

	//private void LoadProductsForSelectedSupplier( int supplierId )
	//{
	//	// Stel hier je DB-oproep in om de productenlijst te halen op basis van supplierId
	//	ProductList = [ .. DBCommands.GetInventoryOrder( supplierId ) ];
	//	// Zorg ervoor dat de UI wordt geïnformeerd over de wijziging
	//	OnPropertyChanged( nameof( ProductList ) );
	//}

	//private int _selectedSupplierId;
	//public int SelectedSupplier
	//{
	//	get => _selectedSupplierId;
	//	set
	//	{
	//		if ( _selectedSupplierId != value )
	//		{
	//			_selectedSupplierId = value;
	//			OnPropertyChanged( nameof( SelectedSupplier ) );

	//			LoadProductsForSelectedSupplier( _selectedSupplierId );

	//			SelectedSupplierChanged?.Invoke( this, _selectedSupplierId );
	//			//SupplierOrderList = new ObservableCollection<SupplyOrderModel>( DBCommands.GetSupplierOrderList( _selectedSupplierId.ToString() ) );
	//		}
	//	}
	//}

	public InventoryOrderSupplierViewModel()
	{
		SupplierList = new ObservableCollection<SupplierModel>( DBCommands.GetSupplierList() );
	}
}
