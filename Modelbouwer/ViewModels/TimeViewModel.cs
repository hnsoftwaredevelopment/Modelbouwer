using System.Collections.ObjectModel;

namespace Modelbouwer.ViewModels;
public partial class TimeViewModel : ObservableObject
{
	[ObservableProperty]
	public int timeId;

	[ObservableProperty]
	public int timeProjectId;

	[ObservableProperty]
	public int timeWorktypeId;

	[ObservableProperty]
	public DateOnly timeWorkDate;

	[ObservableProperty]
	public TimeOnly timeStartTime;

	[ObservableProperty]
	public TimeOnly timeEndTime;

	[ObservableProperty]
	public string? timeComment;

	public ObservableCollection<TimeModel> Time
	{
		get => _time;
		set
		{
			if ( _time != value )
			{
				_time = value;
				OnPropertyChanged( nameof( Time ) );
			}
		}
	}
	private ObservableCollection<TimeModel>? _time;
}
