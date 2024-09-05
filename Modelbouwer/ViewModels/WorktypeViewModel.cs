using System.Collections.ObjectModel;

namespace Modelbouwer.ViewModels;
public partial class WorktypeViewModel : ObservableObject
{
	[ObservableProperty]
	public string? categoryName;

	[ObservableProperty]
	public string? categoryFullpath;

	[ObservableProperty]
	public int categoryId;

	[ObservableProperty]
	public int categoryParentId;

	public ObservableCollection<WorktypeModel> Worktype
	{
		get => _worktype;
		set
		{
			if ( _worktype != value )
			{
				_worktype = value;
				OnPropertyChanged( nameof( Worktype ) );
			}
		}
	}
	private ObservableCollection<WorktypeModel>? _worktype;
}
