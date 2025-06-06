namespace Modelbouwer.ViewModels;
public class ChartsWorkedHoursPerWorktypeViewModel : ObservableObject
{
	public ObservableCollection<ChartsWorkedHoursPerWorktypeModel> ChartDataWorkedHoursWorktype { get; set; }

	public ChartsWorkedHoursPerWorktypeViewModel()
	{
		ChartDataWorkedHoursWorktype = [ .. DBCommands.GetChartsWorkedHoursPerWorktype( 1 ) ];
	}

	public void Refresh( int projectId )
	{
		ChartDataWorkedHoursWorktype = [ .. DBCommands.GetChartsWorkedHoursPerWorktype( projectId ) ];
		decimal total = ChartDataWorkedHoursWorktype.Sum(x => x.WorkedHours);
		foreach ( ChartsWorkedHoursPerWorktypeModel item in ChartDataWorkedHoursWorktype )
		{
			item.Percentage = total == 0 ? 0 : item.WorkedHours / total * 100;
		}
		OnPropertyChanged( nameof( ChartDataWorkedHoursWorktype ) );
	}

}
