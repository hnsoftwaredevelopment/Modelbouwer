namespace Modelbouwer.ViewModels;
public class ChartsWorkedHoursPerYearViewModel : ObservableObject
{
	public ObservableCollection<ChartsWorkedHoursPerYearModel> ChartDataWorkedHoursYear { get; set; }

	public ChartsWorkedHoursPerYearViewModel()
	{
		ChartDataWorkedHoursYear = [ .. DBCommands.GetChartsWorkedHoursPerYear( 1 ) ];
	}

	public void Refresh( int projectId )
	{
		ChartDataWorkedHoursYear = [ .. DBCommands.GetChartsWorkedHoursPerYear( projectId ) ];
		decimal total = ChartDataWorkedHoursYear.Sum(x => x.WorkedHours);
		foreach ( ChartsWorkedHoursPerYearModel item in ChartDataWorkedHoursYear )
		{
			item.Percentage = total == 0 ? 0 : item.WorkedHours / total * 100;
		}
		OnPropertyChanged( nameof( ChartDataWorkedHoursYear ) );
	}
}
