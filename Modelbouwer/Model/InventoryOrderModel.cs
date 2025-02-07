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
	public double ProductPrice { get; set; }
	public double ProductMinimalStock { get; set; }
	public double ProductOrderPer { get; set; }
	public double ProductToOrder { get; set; }
	public string? ProductCategory { get; set; }
	public double ProductInventory { get; set; }
	public double ProductInOrder { get; set; }
	public double ProductShortInventory { get; set; }
	public string? SupplierProductNumber { get; set; }
	public double SupplierPrice { get; set; }
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
