using System.Collections.ObjectModel;

namespace Modelbouwer.ViewModels;
public partial class UnitViewModel : ObservableObject
{
	[ObservableProperty]
	public int unitId;

	[ObservableProperty]
	public string? unitName;

	public ObservableCollection<UnitModel> Unit { get; set; }

	[ObservableProperty]
	private UnitModel? _selectedUnit;

	private readonly ObservableCollection<UnitModel>? _unit;

	[ObservableProperty]
	public UnitModel? selectedItem;

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
		UnitModel newUnit = new()
		{
			UnitId = 0,
			UnitName = string.Empty
		};

		Unit.Add( newUnit );
		SelectedUnit = newUnit;
		IsAddingNew = true;
	}


	public UnitViewModel()
	{
		_ = new DBCommands();
		Unit = new ObservableCollection<UnitModel>( collection: DBCommands.GetUnitList() );

		if ( Unit != null && Unit.Any() )
		{
			SelectedUnit = Unit.First();
		}
	}

}
