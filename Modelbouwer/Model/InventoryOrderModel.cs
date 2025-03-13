using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Modelbouwer.Model;
public class InventoryOrderModel : INotifyPropertyChanged
{
	public event PropertyChangedEventHandler? PropertyChanged;

	protected void OnPropertyChanged( [CallerMemberName] string? propertyName = null ) =>
		PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );

	public int ProductId { get; set; }
	public string? ProductCode { get; set; }
	public string? ProductName { get; set; }
	public string? ProductDisplayPrice { get; set; }
	public decimal ProductPrice { get; set; }

	public double ProductMinimalStock { get; set; }
	public double ProductOrderPer { get; set; }
	public string? ProductCategory { get; set; }
	public double ProductInventory { get; set; }
	public double ProductInOrder { get; set; }
	public double ProductShortInventory { get; set; }
	public string? SupplierProductNumber { get; set; }

	private decimal _supplierPrice;
	public decimal SupplierPrice
	{
		get => _supplierPrice;
		set
		{
			if ( _supplierPrice != value )
			{
				_supplierPrice = value;
				OnPropertyChanged();
				OnPropertyChanged( nameof( CalculatedTotal ) ); // Herberekenen
			}
		}
	}

	private decimal _productToOrder;
	public decimal ProductToOrder
	{
		get => _productToOrder;
		set
		{
			if ( _productToOrder != value )
			{
				_productToOrder = value;
				OnPropertyChanged();
				OnPropertyChanged( nameof( CalculatedTotal ) ); // Herberekenen
			}
		}
	}

	public decimal CalculatedTotal => SupplierPrice * ProductToOrder;

	public int SupplierCurrencyId { get; set; }
	public string? SupplierCurrencySymbol { get; set; }
	public string? SupplierProductName { get; set; }
	public string? ProductFromSupplier { get; set; }
	private bool isSelected;
	public bool IsSelected
	{
		get => isSelected;
		set
		{
			if ( isSelected != value )
			{
				isSelected = value;
				OnPropertyChanged();
			}
		}
	}

	// Define the property that you want to use in TLists (for example in the errorList
	public string Name => ProductName;

	// Mapping dictionary for mapping Database Header to Property name
	public static readonly Dictionary<string, string> HeaderToPropertyMap = new()
	{
		{ DBNames.ProductInventoryOrderProcedureFieldNameProductId, "Product_Id" },
		{ DBNames.ProductInventoryOrderProcedureFieldNameProductCode, "Code" },
		{ DBNames.ProductInventoryOrderProcedureFieldNameProductName, "Name" },
		{ DBNames.ProductInventoryOrderProcedureFieldNameProductPrice, "Price" },
		{ DBNames.ProductInventoryOrderProcedureFieldNameProductMinimalStock, "MinimalStock" },
		{ DBNames.ProductInventoryOrderProcedureFieldNameProductCategory, "Category" },
		{ DBNames.ProductInventoryOrderProcedureFieldNameProductMinimalInventory, "InventoryOrder" },
		{ DBNames.ProductInventoryOrderProcedureFieldNameProductMinimalValue, "Value" },
		{ DBNames.ProductInventoryOrderProcedureFieldNameProductMinimalOrder, "InOrder" },
		{ DBNames.ProductInventoryOrderProcedureFieldNameProductOrderPer, "OrderPer" },
		{ DBNames.ProductInventoryOrderProcedureFieldNameProductToOrder, "ToOrder" },
		{ DBNames.ProductInventoryOrderProcedureFieldNameProductShort, "Short" },
		{ DBNames.ProductInventoryOrderProcedureFieldNameSupplierProductNumber, "SupplierProductNumber" },
		{ DBNames.ProductInventoryOrderProcedureFieldNameSupplierProductName, "SupplierProductName" },
		{ DBNames.ProductInventoryOrderProcedureFieldNameSupplierPrice, "SupplierPrice" },
		{ DBNames.ProductInventoryOrderProcedureFieldNameSupplierCurrencyId, "SupplierCurrencyId" },
		{ DBNames.ProductInventoryOrderProcedureFieldNameSupplierCurrencySymbol, "SupplierCurrencySymbol" },
		{ DBNames.ProductInventoryOrderProcedureFieldNameProductFromSupplier, "ProductFromSupplier" }
	};
}
