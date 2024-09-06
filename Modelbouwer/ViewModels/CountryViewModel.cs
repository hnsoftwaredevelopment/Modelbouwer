namespace Modelbouwer.ViewModels;
public partial class CountryViewModel : ObservableObject
{
	[ObservableProperty]
	public int countryCurrencyId;

	[ObservableProperty]
	public int countryId;

	[ObservableProperty]
	public string? countryCode;

	[ObservableProperty]
	public string? countryCurrencySymbol;

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

	[ObservableProperty]
	public CountryModel? selectedItem;

	[ObservableProperty]
	private CountryModel? _selectedCountry;

	private bool _isAddingNew;

	public bool IsAddingNew
	{
		get => _isAddingNew;
		set
		{
			if ( _isAddingNew != value )
			{
				_isAddingNew = value;
				OnPropertyChanged( nameof( IsAddingNew ) );
			}
		}
	}

	public void AddNewItem()
	{
		// Voeg het nieuwe, lege item toe aan de lijst
		CountryModel newCountry = new()
		{
			CountryId = 0,
			CountryCode = string.Empty,
			CountryName = string.Empty,
			CountryCurrencySymbol = string.Empty,
			CountryCurrencyId = 0
		};

		Country.Add( newCountry );
		SelectedCountry = newCountry;
		IsAddingNew = true;
	}

	partial void OnSelectedCountryChanged( CountryModel value )
	{
		if ( value != null )
		{
			// Zet de geselecteerde Country
			SelectedCountry = value;
		}
	}

	public CountryViewModel()
	{
		Country = new ObservableCollection<CountryModel>( DBCommands.GetCountryList() );
	}
}
