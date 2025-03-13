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
	private bool isTextChanged;

	[ObservableProperty]
	public CountryModel? selectedItem;

	[ObservableProperty]
	private CountryModel? _selectedCountry;

	private bool _isAddingNew;

	private CountryModel? _temporaryCountry;
	private string? _originalCountryName;
	private int _originalCountryId;

	public string? SelectedCountryName
	{
		get => _temporaryCountry?.CountryName;
		set
		{
			if ( _temporaryCountry != null && _temporaryCountry.CountryName != value )
			{
				_temporaryCountry.CountryName = value;
				IsTextChanged = _temporaryCountry.CountryName != _originalCountryName;
				OnPropertyChanged( nameof( SelectedCountryName ) );
			}
		}
	}

	public int SelectedCountryId
	{
		get => _temporaryCountry?.CountryId ?? 0;
		set
		{
			if ( _temporaryCountry != null )
			{
				_temporaryCountry.CountryId = value;
				OnPropertyChanged( nameof( SelectedCountryId ) );
			}
		}
	}

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

	partial void OnSelectedCountryChanged( CountryModel? value )
	{
		if ( value != null )
		{
			// Copy the selected item for changes
			_temporaryCountry = new() { CountryId = value.CountryId, CountryName = value.CountryName };
			_originalCountryName = value.CountryName;
			_originalCountryId = value.CountryId;
		}
		else
		{
			_temporaryCountry = new();
		}

		OnPropertyChanged( nameof( SelectedCountryName ) );
		OnPropertyChanged( nameof( SelectedCountryId ) );
		IsTextChanged = false;
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

	public CountryViewModel()
	{
		Country = new ObservableCollection<CountryModel>( DBCommands.GetCountryList() );
		_temporaryCountry = new();
	}

	public void Refresh()
	{
		Country = [ .. DBCommands.GetCountryList() ];
		OnPropertyChanged( nameof( Country ) );
	}
}
