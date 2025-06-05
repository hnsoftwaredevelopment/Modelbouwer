namespace Modelbouwer.Model;
public class StockMutationModel
{
	public DateOnly Datum { get; set; }
	public string Mutatie { get; set; } = string.Empty;
	public string? Project { get; set; }
	public string? Bestelling { get; set; }
	public string? Leverancier { get; set; }
	public decimal? InAantal { get; set; }
	public decimal? UitAantal { get; set; }
}
