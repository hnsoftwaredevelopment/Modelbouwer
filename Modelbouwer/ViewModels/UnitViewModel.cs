namespace Modelbouwer.ViewModels;
public partial class UnitViewModel : ObservableObject
{
	[ObservableProperty]
	public int unitId;

	[ObservableProperty]
	public string? unitName;

}
