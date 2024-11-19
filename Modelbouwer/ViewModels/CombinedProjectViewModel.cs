using System.ComponentModel;

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

		// Subscribe to changes in the TimeViewModel
		ProjectViewModel.PropertyChanged += OnTimeViewModelPropertyChanged;
	}

	private void OnTimeViewModelPropertyChanged( object? sender, PropertyChangedEventArgs e )
	{
		if ( e.PropertyName == nameof( ProjectViewModel.SelectedProject ) )
		{
			// Update TimeContactViewModel with the selected Project
			var selectedProject = ProjectViewModel.SelectedProject;
			if ( selectedProject != null )
			{
				TimeViewModel.FilterTimeEntriesByProjectId( selectedProject.ProjectId );
			}
		}
	}
}
