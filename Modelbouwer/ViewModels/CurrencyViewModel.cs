namespace Modelbouwer.ViewModels;
public partial class CurrencyViewModel : ObservableObject
{
	[ObservableProperty]
	public int currencyId;

	[ObservableProperty]
	public double currencyConversionRate;

	[ObservableProperty]
	public string? currencyCode;

	[ObservableProperty]
	public string? currencySymbol;

	[ObservableProperty]
	public string? currencyName;

	[ObservableProperty]
	private CurrencyModel? _selectedCurrency;

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

	[ObservableProperty]
	public CurrencyModel? selectedItem;

	[ObservableProperty]
	private bool isTextChanged;

	private CurrencyModel? _temporaryCurrency;
	private string? _originalCurrencyName;
	private int _originalCurrencyId;

	public string? SelectedCurrencyName
	{
		get => _temporaryCurrency?.CurrencyName;
		set
		{
			if ( _temporaryCurrency != null && _temporaryCurrency.CurrencyName != value )
			{
				_temporaryCurrency.CurrencyName = value;
				IsTextChanged = _temporaryCurrency.CurrencyName != _originalCurrencyName;
				OnPropertyChanged( nameof( SelectedCurrencyName ) );
			}
		}
	}

	public int SelectedCurrencyId
	{
		get => _temporaryCurrency?.CurrencyId ?? 0;
		set
		{
			if ( _temporaryCurrency != null )
			{
				_temporaryCurrency.CurrencyId = value;
				OnPropertyChanged( nameof( SelectedCurrencyId ) );
			}
		}
	}

	partial void OnSelectedCurrencyChanged( CurrencyModel? value )
	{
		if ( value != null )
		{
			// Copy the selected item for changes
			_temporaryCurrency = new() { CurrencyId = value.CurrencyId, CurrencyName = value.CurrencyName };
			_originalCurrencyName = value.CurrencyName;
			_originalCurrencyId = value.CurrencyId;
		}
		else
		{
			_temporaryCurrency = new();
		}

		OnPropertyChanged( nameof( SelectedCurrencyName ) );
		OnPropertyChanged( nameof( SelectedCurrencyId ) );
		IsTextChanged = false;
	}

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
		CurrencyModel newCurrency = new()
		{
			CurrencyId = 0,
			CurrencyCode = string.Empty,
			CurrencyName = string.Empty,
			CurrencySymbol = string.Empty,
			CurrencyConversionRate = 0.0000
		};

		Currency.Add( newCurrency );
		SelectedCurrency = newCurrency;
		IsAddingNew = true;
	}

	public CurrencyViewModel()
	{
		DBCommands dbCommands = new();
		Currency = [ .. dbCommands.GetCurrencyList() ];
		_temporaryCurrency = new();

		if ( Currency != null && Currency.Any() )
		{
			SelectedCurrency = Currency.First();
		}
	}

	public void Refresh()
	{
		DBCommands dbCommands = new();
		Currency = [ .. dbCommands.GetCurrencyList() ];
		OnPropertyChanged( nameof( Currency ) );
	}
}