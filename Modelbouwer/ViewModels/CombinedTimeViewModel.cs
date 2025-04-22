using System.ComponentModel;

namespace Modelbouwer.ViewModels;
public class CombinedTimeViewModel : ObservableObject
{
	public ProjectViewModel ProjectViewModel { get; set; }
	public TimeViewModel TimeViewModel { get; set; }
	public WorktypeViewModel WorktypeViewModel { get; set; }
	public ProductViewModel ProductViewModel { get; set; }
	public ProductUsageViewModel ProductUsageViewModel { get; set; }

	public CombinedTimeViewModel()
	{
		ProjectViewModel = new();
		TimeViewModel = new();
		ProductViewModel = new();
		ProductUsageViewModel = new();
		WorktypeViewModel = new();

		ProjectViewModel.PropertyChanged += OnTimeViewModelPropertyChanged;
	}

	private void OnTimeViewModelPropertyChanged( object? sender, PropertyChangedEventArgs e )
	{
		if ( e.PropertyName == nameof( ProjectViewModel.SelectedProject ) )
		{
			ProjectModel? selectedProject = ProjectViewModel.SelectedProject;
			if ( selectedProject != null )
			{
				TimeViewModel.SelectedProject = selectedProject.ProjectId;
				ProductUsageViewModel.FilterCostEntriesByProjectId( selectedProject.ProjectId );

				OnPropertyChanged( nameof( SelectedProject ) );
			}
		}
	}


	public ProjectModel SelectedProject
	{
		get => ProjectViewModel.SelectedProject;
		set => ProjectViewModel.SelectedProject = value;
	}

	public void RefreshAll()
	{
		ProjectViewModel.PropertyChanged -= OnTimeViewModelPropertyChanged;

		ProjectViewModel.Refresh();
		TimeViewModel.Refresh();
		ProductViewModel.Refresh();
		ProductUsageViewModel.Refresh();
		WorktypeViewModel.Refresh();

		ProjectViewModel.PropertyChanged += OnTimeViewModelPropertyChanged;

		if ( ProjectViewModel.SelectedProject != null )
		{
			TimeViewModel.LoadTimeEntriesForSelectedProject( ProjectViewModel.SelectedProject.ProjectId );
		}
	}
}
