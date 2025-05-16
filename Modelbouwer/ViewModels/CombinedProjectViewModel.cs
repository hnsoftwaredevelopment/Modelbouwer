using System.ComponentModel;

namespace Modelbouwer.ViewModels;
public class CombinedProjectViewModel
{
	public ProjectViewModel ProjectViewModel { get; set; }
	public TimeViewModel TimeViewModel { get; set; }
	public ProductViewModel ProductViewModel { get; set; }
	public ProductUsageViewModel ProductUsageViewModel { get; set; }

	public CombinedProjectViewModel()
	{
		ProjectViewModel = new();
		TimeViewModel = new();
		ProductViewModel = new();
		ProductUsageViewModel = new();

		// Subscribe to changes in the TimeViewModel
		ProjectViewModel.PropertyChanged += OnTimeViewModelPropertyChanged;
	}

	private void OnTimeViewModelPropertyChanged( object? sender, PropertyChangedEventArgs e )
	{
		if ( e.PropertyName == nameof( ProjectViewModel.SelectedProject ) )
		{
			// Update TimeContactViewModel with the selected Project
			ProjectModel? selectedProject = ProjectViewModel.SelectedProject;
			if ( selectedProject != null )
			{
				ProductUsageViewModel.FilterCostEntriesByProjectId( selectedProject.ProjectId );
				ProductUsageViewModel.FilterProductUsageByProjectId( selectedProject.ProjectId );
			}
		}
	}

	public void RefreshAll()
	{
		ProjectViewModel.Refresh();
		TimeViewModel.Refresh();
		ProductViewModel.Refresh();
		ProductUsageViewModel.Refresh();

		ProjectViewModel.PropertyChanged += OnTimeViewModelPropertyChanged;
	}
}
