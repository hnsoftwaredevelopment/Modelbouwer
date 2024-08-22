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
}
