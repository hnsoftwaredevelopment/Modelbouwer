﻿namespace Modelbouwer.ViewModels;
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

	partial void OnSelectedCurrencyChanged( CurrencyModel? value )
	{
		if ( value != null )
		{
			SelectedCurrency = value;
		}
	}

	public CurrencyViewModel()
	{
		DBCommands dbCommands = new();
		Currency = new ObservableCollection<CurrencyModel>( dbCommands.GetCurrencyList() );

		if ( Currency != null && Currency.Any() )
		{
			SelectedCurrency = Currency.First();
		}
	}

	public void Refresh()
	{
		DBCommands dbCommands = new();
		Currency = new ObservableCollection<CurrencyModel>( dbCommands.GetCurrencyList() );
		OnPropertyChanged( nameof( Currency ) );
	}
}