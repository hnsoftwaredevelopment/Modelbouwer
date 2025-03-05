namespace Modelbouwer.ViewModels;
public partial class UnitViewModel : ObservableObject
{
	[ObservableProperty]
	public string? unitName;

	[ObservableProperty]
	public int unitId;

	[ObservableProperty]
	public UnitModel? selectedItem;

	public ObservableCollection<UnitModel> Unit
	{
		get => _unit;
		set
		{
			if ( _unit != value )
			{
				_unit = value;
				OnPropertyChanged( nameof( Unit ) );
			}
		}
	}

	private ObservableCollection<UnitModel>? _unit;

	[ObservableProperty]
	private bool isTextChanged;

	[ObservableProperty]
	private UnitModel? _selectedUnit;

	private UnitModel? _temporaryUnit;
	private string? _originalUnitName;
	private int _originalUnitId;

	public string? SelectedUnitName
	{
		get => _temporaryUnit?.UnitName;
		set
		{
			if ( _temporaryUnit != null && _temporaryUnit.UnitName != value )
			{
				_temporaryUnit.UnitName = value;
				IsTextChanged = _temporaryUnit.UnitName != _originalUnitName;
				OnPropertyChanged( nameof( SelectedUnitName ) );
			}
		}
	}

	public int SelectedUnitId
	{
		get => _temporaryUnit?.UnitId ?? 0;
		set
		{
			if ( _temporaryUnit != null )
			{
				_temporaryUnit.UnitId = value;
				OnPropertyChanged( nameof( SelectedUnitId ) );
			}
		}
	}

	partial void OnSelectedUnitChanged( UnitModel? value )
	{
		if ( value != null )
		{
			// Copy the selected item for changes
			_temporaryUnit = new() { UnitId = value.UnitId, UnitName = value.UnitName };
			_originalUnitName = value.UnitName;
			_originalUnitId = value.UnitId;
		}
		else
		{
			_temporaryUnit = new();
		}

		OnPropertyChanged( nameof( SelectedUnitName ) );
		OnPropertyChanged( nameof( SelectedUnitId ) );
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
			if ( _temporaryUnit != null )
			{
				UnitModel newUnit = new()
				{
					UnitId = _temporaryUnit.UnitId,
					UnitName = _temporaryUnit.UnitName
				};
				Unit.Add( newUnit );
				SelectedUnit = newUnit; // Stel de nieuwe regel in als de geselecteerde regel
			}
		}
		else
		{
			_temporaryUnit = new UnitModel
			{
				UnitId = 0,
				UnitName = string.Empty
			};

			// Voeg het nieuwe, lege item toe aan de lijst
			Unit.Add( _temporaryUnit );
			SelectedUnit = _temporaryUnit;
		}

		IsAddingNew = true;

		IsTextChanged = true;
	}

	public UnitViewModel()
	{
		Unit = new ObservableCollection<UnitModel>( DBCommands.GetUnitList() );
		_temporaryUnit = new();
	}

	public void Refresh()
	{
		Unit = [ .. DBCommands.GetUnitList() ];
		OnPropertyChanged( nameof( Unit ) );
	}
}
