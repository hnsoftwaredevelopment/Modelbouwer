namespace Modelbouwer.Model;

public class ErrorList
{
	public int LineNumber { get; set; }
	public int ErrorCode { get; set; }
	public string? Line { get; set; }
}