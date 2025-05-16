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

		ProjectViewModel.PropertyChanged += OnProjectViewModelPropertyChanged;
		TimeViewModel.PropertyChanged += OnTimeViewModelPropertyChanged;
	}

	private void OnProjectViewModelPropertyChanged( object? sender, PropertyChangedEventArgs e )
	{
		if ( e.PropertyName == nameof( ProjectViewModel.SelectedProject ) )
		{
			ProjectModel? selectedProject = ProjectViewModel.SelectedProject;
			if ( selectedProject != null )
			{
				TimeViewModel.SelectedProject = selectedProject.ProjectId;
				TimeViewModel.LoadTimeEntriesForSelectedProject( selectedProject.ProjectId );
				ProductUsageViewModel.FilterCostEntriesByProjectId( selectedProject.ProjectId );

				OnPropertyChanged( nameof( SelectedProject ) );

				// Trigger een update van HasFilteredTimeEntries na laden van nieuwe time entries
				OnPropertyChanged( nameof( TimeViewModel.FilteredTimeEntries ) );
				OnPropertyChanged( nameof( TimeViewModel.HasFilteredTimeEntries ) );
			}
		}

		if ( e.PropertyName == nameof( TimeViewModel.HasFilteredTimeEntries ) )
		{
			// Forward de property changed naar de UI
			OnPropertyChanged( nameof( TimeViewModel ) );
		}
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
		ProjectViewModel.PropertyChanged -= OnProjectViewModelPropertyChanged;
		TimeViewModel.PropertyChanged -= OnTimeViewModelPropertyChanged;

		ProjectViewModel.Refresh();
		TimeViewModel.Refresh();
		ProductViewModel.Refresh();
		ProductUsageViewModel.Refresh();
		WorktypeViewModel.Refresh();

		ProjectViewModel.PropertyChanged += OnProjectViewModelPropertyChanged;
		TimeViewModel.PropertyChanged += OnTimeViewModelPropertyChanged;

		if ( ProjectViewModel.SelectedProject != null )
		{
			TimeViewModel.LoadTimeEntriesForSelectedProject( ProjectViewModel.SelectedProject.ProjectId );

			OnPropertyChanged( nameof( TimeViewModel ) );
		}
	}
}
