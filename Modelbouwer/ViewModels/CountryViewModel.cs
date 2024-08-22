namespace Modelbouwer.ViewModels;
public partial class CountryViewModel : ObservableObject
{
	[ObservableProperty]
	public int countryDefaultCurrencyId;

	[ObservableProperty]
	public int countryId;

	[ObservableProperty]
	public string? countryCode;

	[ObservableProperty]
	public string? countryDefaultCurrencySymbol;

	[ObservableProperty]
	public string? countryName;
}
