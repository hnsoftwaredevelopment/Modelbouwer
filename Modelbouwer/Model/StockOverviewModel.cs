namespace Modelbouwer.Model;
public class StockOverviewModel
{
	public decimal CurrentStock { get; set; }
	public ObservableCollection<StockMutationModel> Mutations { get; set; } = [ ];
}
