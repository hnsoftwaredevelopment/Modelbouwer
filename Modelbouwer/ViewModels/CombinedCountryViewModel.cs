namespace Modelbouwer.ViewModels;
public class CombinedCountryViewModel
{
	public CountryViewModel CountryViewModel { get; set; }
	public CurrencyViewModel CurrencyViewModel { get; set; }

	public CombinedCountryViewModel()
	{
		CountryViewModel = new();
		CurrencyViewModel = new();
	}
}
