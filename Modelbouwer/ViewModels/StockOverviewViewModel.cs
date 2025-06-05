namespace Modelbouwer.ViewModels;
public class StockOverviewViewModel : ObservableObject
{
	private decimal _currentStock;
	public decimal CurrentStock
	{
		get => _currentStock;
		set
		{
			if ( _currentStock != value )
			{
				_currentStock = value;
				OnPropertyChanged( nameof( CurrentStock ) );
			}
		}
	}
	public ObservableCollection<StockMutation> Mutations { get; set; } = [ ];
}

public class StockMutation
{
	public DateOnly Datum { get; set; }
	public string? Mutatie { get; set; }
	public string? Project { get; set; }
	public string? Bestelling { get; set; }
	public string? Leverancier { get; set; }
	public decimal? InAantal { get; set; }
	public decimal? UitAantal { get; set; }

	public static StockMutation FromModel( StockMutationModel model )
	{
		return new StockMutation
		{
			Datum = model.Datum,
			Mutatie = model.Mutatie,
			Project = model.Project,
			Bestelling = model.Bestelling,
			Leverancier = model.Leverancier,
			InAantal = model.InAantal,
			UitAantal = model.UitAantal
		};
	}
}
