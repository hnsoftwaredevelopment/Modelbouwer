namespace Modelbouwer.ViewModels;
public class ChartsWorkedHoursPerMonthViewModel : ObservableObject
{
	public ObservableCollection<ChartsWorkedHoursPerMonthModel> ChartDataWorkedHoursMonth { get; set; }

	public ChartsWorkedHoursPerMonthViewModel()
	{
		ChartDataWorkedHoursMonth = [ .. DBCommands.GetChartsWorkedHoursPerMonth( 1 ) ];
	}

	public void Refresh( int projectId )
	{
		ChartDataWorkedHoursMonth = [ .. DBCommands.GetChartsWorkedHoursPerMonth( projectId ) ];
		decimal total = ChartDataWorkedHoursMonth.Sum(x => x.WorkedHours);
		foreach ( ChartsWorkedHoursPerMonthModel item in ChartDataWorkedHoursMonth )
		{
			item.Percentage = total == 0 ? 0 : item.WorkedHours / total * 100;
		}
		OnPropertyChanged( nameof( ChartDataWorkedHoursMonth ) );
	}
}
