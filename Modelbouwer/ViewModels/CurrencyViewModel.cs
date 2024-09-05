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

	[ObservableProperty]
	public CurrencyModel? selectedItem;

	[ObservableProperty]
	private bool isTextChanged;

	[ObservableProperty]
	private CurrencyModel _selectedCurrency;

	private CurrencyModel? _temporaryCurrency;
	private string? _originalCurrencyCode;
	private string? _originalCurrencyName;
	private string? _originalCurrencySymbol;
	private int _originalCurrencyId;
	private double _originalCurrencyConversionRate;

	public string? SelectedCurrencyCode
	{
		get => _temporaryCurrency?.CurrencyCode;
		set
		{
			if ( _temporaryCurrency != null && _temporaryCurrency.CurrencyCode != value )
			{
				_temporaryCurrency.CurrencyCode = value;
				IsTextChanged = _temporaryCurrency.CurrencyCode != _originalCurrencyCode;
				OnPropertyChanged( nameof( SelectedCurrencyCode ) );
			}
		}
	}

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

	public string? SelectedCurrencySymbol
	{
		get => _temporaryCurrency?.CurrencySymbol;
		set
		{
			if ( _temporaryCurrency != null && _temporaryCurrency.CurrencySymbol != value )
			{
				_temporaryCurrency.CurrencySymbol = value;
				IsTextChanged = _temporaryCurrency.CurrencySymbol != _originalCurrencySymbol;
				OnPropertyChanged( nameof( SelectedCurrencySymbol ) );
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
				IsTextChanged = _temporaryCurrency.CurrencyId != _originalCurrencyId;
				OnPropertyChanged( nameof( SelectedCurrencyId ) );
			}
		}
	}

	public double SelectedCurrencyConversionRate
	{
		get => _temporaryCurrency?.CurrencyConversionRate ?? 0;
		set
		{
			if ( _temporaryCurrency != null )
			{
				_temporaryCurrency.CurrencyConversionRate = value;
				IsTextChanged = _temporaryCurrency.CurrencyConversionRate != _originalCurrencyConversionRate;
				OnPropertyChanged( nameof( SelectedCurrencyConversionRate ) );
			}
		}
	}

	partial void OnSelectedCurrencyChanged( CurrencyModel value )
	{
		if ( value != null )
		{
			// Copy the selected item for changes
			_temporaryCurrency = new() { CurrencyId = value.CurrencyId, CurrencyName = value.CurrencyName, CurrencyCode = value.CurrencyName, CurrencyConversionRate = value.CurrencyConversionRate, CurrencySymbol = value.CurrencySymbol };
			_originalCurrencyCode = value.CurrencyCode;
			_originalCurrencySymbol = value.CurrencySymbol;
			_originalCurrencyName = value.CurrencyName;
			_originalCurrencyId = value.CurrencyId;
			_originalCurrencyConversionRate = value.CurrencyConversionRate;
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
		if ( IsTextChanged )
		{
			if ( _temporaryCurrency != null )
			{
				CurrencyModel newCurrency = new()
				{
					CurrencyId = _temporaryCurrency.CurrencyId,
					CurrencyName = _temporaryCurrency.CurrencyName,
					CurrencySymbol = _temporaryCurrency.CurrencySymbol,
					CurrencyConversionRate = _temporaryCurrency.CurrencyConversionRate,
					CurrencyCode = _temporaryCurrency.CurrencyCode
				};
				Currency.Add( newCurrency );
				SelectedCurrency = newCurrency;
			}
		}
		else
		{
			// inpCurrencyName has not been changed, so create a new empty currency
			_temporaryCurrency = new CurrencyModel
			{
				CurrencyId = 0,
				CurrencyCode = string.Empty,
				CurrencyName = string.Empty,
				CurrencySymbol = string.Empty,
				CurrencyConversionRate = 0
			};

			// Voeg het nieuwe, lege item toe aan de lijst
			Currency.Add( _temporaryCurrency );
			SelectedCurrency = _temporaryCurrency;
		}

		IsAddingNew = true;

		IsTextChanged = true;
	}

	public CurrencyViewModel()
	{
		Currency = new ObservableCollection<CurrencyModel>( DBCommands.GetCurrencyList() );
		_temporaryCurrency = new();
	}
}
