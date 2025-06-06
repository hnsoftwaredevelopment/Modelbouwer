using System.ComponentModel;

namespace Modelbouwer.ViewModels;
public class CombinedProjectViewModel
{
	public ProjectViewModel ProjectViewModel { get; set; }
	public TimeViewModel TimeViewModel { get; set; }
	public ProductViewModel ProductViewModel { get; set; }
	public ProductUsageViewModel ProductUsageViewModel { get; set; }
	public ChartsWorkedHoursPerDayViewModel ChartsWorkedHoursPerDayViewModel { get; set; }
	public ChartsWorkedHoursPerMonthViewModel ChartsWorkedHoursPerMonthViewModel { get; set; }
	public ChartsWorkedHoursPerYearViewModel ChartsWorkedHoursPerYearViewModel { get; set; }
	public ChartsWorkedHoursPerWorktypeViewModel ChartsWorkedHoursPerWorktypeViewModel { get; set; }

	public CombinedProjectViewModel()
	{
		ProjectViewModel = new();
		TimeViewModel = new();
		ProductViewModel = new();
		ProductUsageViewModel = new();
		ChartsWorkedHoursPerDayViewModel = new();
		ChartsWorkedHoursPerMonthViewModel = new();
		ChartsWorkedHoursPerYearViewModel = new();
		ChartsWorkedHoursPerWorktypeViewModel = new();

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
				ChartsWorkedHoursPerDayViewModel.Refresh( selectedProject.ProjectId );
				ChartsWorkedHoursPerMonthViewModel.Refresh( selectedProject.ProjectId );
				ChartsWorkedHoursPerYearViewModel.Refresh( selectedProject.ProjectId );
				ChartsWorkedHoursPerWorktypeViewModel.Refresh( selectedProject.ProjectId );
			}
		}
	}

	public void RefreshAll()
	{
		ProjectViewModel.Refresh();
		TimeViewModel.Refresh();
		ProductViewModel.Refresh();
		ProductUsageViewModel.Refresh();
		//ChartsWorkedHoursPerMonthViewModel.Refresh( ProjectViewModel.SelectedProject.ProjectId );

		ProjectViewModel.PropertyChanged += OnTimeViewModelPropertyChanged;
	}
}
