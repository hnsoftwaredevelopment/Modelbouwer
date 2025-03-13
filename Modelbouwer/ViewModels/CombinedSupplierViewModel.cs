using System.ComponentModel;

namespace Modelbouwer.ViewModels;
public class CombinedSupplierViewModel
{
	public SupplierViewModel SupplierViewModel { get; set; }

	public SupplierContactViewModel SupplierContactViewModel { get; set; }

	public SupplierContactTypeViewModel SupplierContactTypeViewModel { get; set; }

	public CurrencyViewModel CurrencyViewModel { get; set; }

	public CountryViewModel CountryViewModel { get; set; }

	public CombinedSupplierViewModel()
	{
		SupplierViewModel = new SupplierViewModel();
		SupplierContactViewModel = new SupplierContactViewModel();
		SupplierContactTypeViewModel = new SupplierContactTypeViewModel();
		CurrencyViewModel = new CurrencyViewModel();
		CountryViewModel = new CountryViewModel();

		// Subscribe to changes in the SupplierViewModel
		SupplierViewModel.PropertyChanged += OnSupplierViewModelPropertyChanged;
	}

	private void OnSupplierViewModelPropertyChanged( object? sender, PropertyChangedEventArgs e )
	{
		if ( e.PropertyName == nameof( SupplierViewModel.SelectedSupplier ) )
		{
			// Update SupplierContactViewModel with the selected supplier
			SupplierModel? selectedSupplier = SupplierViewModel.SelectedSupplier;
			if ( selectedSupplier != null )
			{
				SupplierContactViewModel.FilterContactsBySupplierId( selectedSupplier.SupplierId );

				if ( selectedSupplier.SupplierCountryId > 0 )
				{
					CountryModel? country = CountryViewModel.Country.FirstOrDefault( c => c.CountryId == selectedSupplier.SupplierCountryId );
					if ( country != null )
					{
						CountryViewModel.SelectedCountry = country;
					}
				}

				if ( selectedSupplier.SupplierCurrencyId > 0 )
				{
					CurrencyModel? currency = CurrencyViewModel.Currency.FirstOrDefault( c => c.CurrencyId == selectedSupplier.SupplierCurrencyId );
					if ( currency != null )
					{
						CurrencyViewModel.SelectedCurrency = currency;
					}
				}
			}
		}
	}

	public void RefreshAll()
	{
		SupplierViewModel.Refresh();
		SupplierContactViewModel.Refresh();
		SupplierContactTypeViewModel.Refresh();
		CurrencyViewModel.Refresh();
		CountryViewModel.Refresh();

		SupplierViewModel.PropertyChanged += OnSupplierViewModelPropertyChanged;
	}
}
