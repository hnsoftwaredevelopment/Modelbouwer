namespace Modelbouwer.ViewModels;
public class CombinedProjectViewModel
{
	public ProjectViewModel ProjectViewModel { get; set; }
	public TimeViewModel TimeViewModel { get; set; }
	public ProductViewModel ProductViewModel { get; set; }

	public CombinedProjectViewModel()
	{
		ProjectViewModel = new();
		TimeViewModel = new();
		ProductViewModel = new();
	}
}
