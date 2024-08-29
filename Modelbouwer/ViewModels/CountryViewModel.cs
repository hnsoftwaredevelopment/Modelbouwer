using System.Collections.ObjectModel;

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

	public ObservableCollection<CountryModel> Country
	{
		get => _country;
		set
		{
			if ( _country != value )
			{
				_country = value;
				OnPropertyChanged( nameof( Country ) );
			}
		}
	}
	private ObservableCollection<CountryModel>? _country;
}
