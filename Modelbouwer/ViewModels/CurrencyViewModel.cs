using System.Collections.ObjectModel;

namespace Modelbouwer.ViewModels;
public partial class CurrencyViewModel : ObservableObject
{
	[ObservableProperty]
	public double currencyConversionRate;

	[ObservableProperty]
	public int currencyId;

	[ObservableProperty]
	public string? currencyCode;

	[ObservableProperty]
	public string? currencyName;

	[ObservableProperty]
	public string? currencySymbol;

	public ObservableCollection<CurrencyModel> Currency
	{
		get => _currency;
		set
		{
			if ( _currency != value )
			{
				_currency = value;
				OnPropertyChanged( nameof( Currency ) );
			}
		}
	}
	private ObservableCollection<CurrencyModel>? _currency;
}
