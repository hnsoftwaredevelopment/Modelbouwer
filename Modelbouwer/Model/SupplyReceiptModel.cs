using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Modelbouwer.Model;
public class SupplyReceiptModel : ObservableObject
{
	public new event PropertyChangedEventHandler? PropertyChanged;

	protected new void OnPropertyChanged( [CallerMemberName] string? propertyName = null ) =>
	PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );

	public int SupplyOrderId { get; set; }
	public int SupplyOrderSupplierId { get; set; }
	public int OrderNumber { get; set; }
	public int OrderLineId { get; set; }
	public int ProductId { get; set; }
	public string? SupplierNumber { get; set; }
	public string? SupplierDescription { get; set; }
	public decimal Ordered { get; set; }
	public decimal Received { get; set; }
	public decimal WaitFor { get; set; }
	public decimal StockLogReceived { get; set; }
	public decimal InStock { get; set; }
	public int SupplyOrderClosed { get; set; }
	public string? SupplyOrderClosedDate { get; set; }
	public int SupplyOrderHasStackLog { get; set; }


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
}
