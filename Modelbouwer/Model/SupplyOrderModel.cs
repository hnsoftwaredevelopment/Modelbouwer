using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Modelbouwer.Model;

public class SupplyOrderModel : ObservableObject
{
	public new event PropertyChangedEventHandler? PropertyChanged;

	protected new void OnPropertyChanged( [CallerMemberName] string? propertyName = null ) =>
	PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );

	public int SupplyOrderId { get; set; }
	public int SupplyOrderSupplierId { get; set; }
	public int SupplyOrderCurrencyId { get; set; }
	//public string? SupplyOrderNumber { get; set; }
	//public string? SupplyOrderDate { get; set; }
	private string _supplyOrderNumber = string.Empty;
	public string SupplyOrderNumber
	{
		get => _supplyOrderNumber;
		set => SetProperty( ref _supplyOrderNumber, value );
	}

	private DateOnly? _supplyOrderDate;
	public DateOnly? SupplyOrderDate
	{
		get => _supplyOrderDate;
		set => SetProperty( ref _supplyOrderDate, value );
	}
	public string? SupplyOrderCurrencySymbol { get; set; }
	public double SupplyOrderCurrencyRate { get; set; }
	public double SupplyOrderShippingCosts { get; set; }
	public double SupplyOrderOrderCosts { get; set; }
	public string? SupplyOrderMemo { get; set; }
	public int SupplyOrderClosed { get; set; }
	public string? SupplyOrderClosedDate { get; set; }

	// Define the property that you want to use in TLists (for example in the errorList
	public string Name => SupplyOrderNumber;

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

	// Mapping dictionary for mapping Database Header to Property name
	public static readonly Dictionary<string, string> HeaderToPropertyMap = new()
					{
									{ DBNames.OrderFieldNameId, "SupplyOrderId" },
									{ DBNames.OrderFieldNameClosed, "SupplyOrderClosed"},
									{ DBNames.OrderFieldNameClosedDate, "SupplyOrderClosedDate"},
									{ DBNames.OrderFieldNameSupplierId, "SupplyOrderSupplierId"},
									{ DBNames.OrderFieldNameCurrencyId, "SupplyOrderCurrencyId"},
									{ DBNames.OrderFieldNameOrderNumber, "SupplyOrderNumber"},
									{ DBNames.OrderFieldNameOrderDate, "SupplyOrderDate"},
									{ DBNames.OrderFieldNameCurrencySymbol, "SupplyOrderCurrencySymbol"},
									{ DBNames.OrderFieldNameConversionRate, "SupplyOrderCurrencyRate"},
									{ DBNames.OrderFieldNameShippingCosts, "SupplyOrderShippingCosts"},
									{ DBNames.OrderFieldNameOrderCosts, "SupplyOrderOrderCosts"},
									{ DBNames.OrderFieldNameOrderMemo, "SupplyOrderMemo"},
					};
}
