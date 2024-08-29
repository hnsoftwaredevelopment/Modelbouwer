using System.Collections.ObjectModel;

namespace Modelbouwer.ViewModels;
public partial class UnitViewModel : ObservableObject
{
	[ObservableProperty]
	public int unitId;

	[ObservableProperty]
	public string? unitName;

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

}
