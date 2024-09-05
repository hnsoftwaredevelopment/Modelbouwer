using System.Collections.ObjectModel;

namespace Modelbouwer.ViewModels;
public partial class BrandViewModel : ObservableObject
{
	[ObservableProperty]
	public string? brandName;

	[ObservableProperty]
	public int brandId;

	[ObservableProperty]
	public BrandModel? selectedItem;

	public ObservableCollection<BrandModel> Brand
	{
		get => _brand;
		set
		{
			if ( _brand != value )
			{
				_brand = value;
				OnPropertyChanged( nameof( Brand ) );
			}
		}
	}
	private ObservableCollection<BrandModel> _brand;

	[ObservableProperty]
	private bool isTextChanged;

	[ObservableProperty]
	private BrandModel _selectedBrand;

	private BrandModel? _temporaryBrand;
	private string? _originalBrandName;
	private int _originalBrandId;

	public string? SelectedBrandName
	{
		get => _temporaryBrand?.BrandName;
		set
		{
			if ( _temporaryBrand != null && _temporaryBrand.BrandName != value )
			{
				_temporaryBrand.BrandName = value;
				IsTextChanged = _temporaryBrand.BrandName != _originalBrandName;
				OnPropertyChanged( nameof( SelectedBrandName ) );
			}
		}
	}

	public int SelectedBrandId
	{
		get => _temporaryBrand?.BrandId ?? 0;
		set
		{
			if ( _temporaryBrand != null )
			{
				_temporaryBrand.BrandId = value;
				OnPropertyChanged( nameof( SelectedBrandId ) );
			}
		}
	}

	partial void OnSelectedBrandChanged( BrandModel value )
	{
		if ( value != null )
		{
			// Copy the selected item for changes
			_temporaryBrand = new() { BrandId = value.BrandId, BrandName = value.BrandName };
			_originalBrandName = value.BrandName;
			_originalBrandId = value.BrandId;
		}
		else
		{
			_temporaryBrand = new();
		}

		OnPropertyChanged( nameof( SelectedBrandName ) );
		OnPropertyChanged( nameof( SelectedBrandId ) );
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
			// Als er wijzigingen zijn aangebracht in het geselecteerde item, voeg die wijzigingen dan als een nieuw item toe.
			if ( _temporaryBrand != null )
			{
				BrandModel newBrand = new()
				{
					BrandId = _temporaryBrand.BrandId,
					BrandName = _temporaryBrand.BrandName
				};
				Brand.Add( newBrand );
				SelectedBrand = newBrand; // Stel de nieuwe regel in als de geselecteerde regel
			}
		}
		else
		{
			// inpBrandName has not been changed, so create a new empty brand
			_temporaryBrand = new BrandModel
			{
				BrandId = 0,
				BrandName = string.Empty
			};

			// Voeg het nieuwe, lege item toe aan de lijst
			Brand.Add( _temporaryBrand );
			SelectedBrand = _temporaryBrand;
		}

		// Stel de IsAddingNew flag in om aan te geven dat we in de "add new" modus zijn
		IsAddingNew = true;

		// Stel de Save knop in om zichtbaar te zijn
		IsTextChanged = true;
	}

	public BrandViewModel()
	{
		Brand = new ObservableCollection<BrandModel>( DBCommands.GetBrandList() );
		_temporaryBrand = new();
	}

}
