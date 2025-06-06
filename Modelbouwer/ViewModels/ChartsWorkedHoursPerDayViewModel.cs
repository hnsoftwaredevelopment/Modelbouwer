namespace Modelbouwer.ViewModels;
public class ChartsWorkedHoursPerDayViewModel : ObservableObject
{
	public ObservableCollection<ChartsWorkedHoursPerDayModel> ChartDataWorkedHoursDay { get; set; }

	public ChartsWorkedHoursPerDayViewModel()
	{
		ChartDataWorkedHoursDay = [ .. DBCommands.GetChartsWorkedHoursPerDay( 1 ) ];
	}

	public void Refresh( int projectId )
	{
		ChartDataWorkedHoursDay = [ .. DBCommands.GetChartsWorkedHoursPerDay( projectId ) ];
		decimal total = ChartDataWorkedHoursDay.Sum(x => x.WorkedHours);
		foreach ( ChartsWorkedHoursPerDayModel item in ChartDataWorkedHoursDay )
		{
			item.Percentage = total == 0 ? 0 : item.WorkedHours / total * 100;
		}
		OnPropertyChanged( nameof( ChartDataWorkedHoursDay ) );
	}
}
