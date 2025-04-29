using System.ComponentModel;
using System.Windows.Threading;

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
		ProductUsageViewModel = new();
		ProductViewModel = new();
		ProjectViewModel = new();
		WorktypeViewModel = new();

		TimeViewModel = new( ProductUsageViewModel )
		{
			// Set references so TimeViewModel uses the same instances
			ProductViewModel = ProductViewModel
		};

		ProjectViewModel.PropertyChanged += OnSelectedProjectChanged;

		// Add handlers for ProductUsage synchroization
		TimeViewModel.SaveProductUsageCommand.CanExecuteChanged += ( s, e ) => RefreshProductUsageView();
	}

	private void OnSelectedProjectChanged( object? sender, PropertyChangedEventArgs e )
	{
		if ( e.PropertyName == nameof( ProjectViewModel.SelectedProject ) )
		{
			ProjectModel? selectedProject = ProjectViewModel.SelectedProject;

			if ( selectedProject != null )
			{
				int projectId = selectedProject.ProjectId;

				// Synchroniseer project-ID naar onderliggende viewmodels
				TimeViewModel.SelectedProject = projectId;
				TimeViewModel.IsProjectSelected = true;

				ProductUsageViewModel.SelectedProject = projectId;
				ProductUsageViewModel.IsProjectSelected = true;

				// Laad gegevens
				TimeViewModel.LoadTimeEntriesForSelectedProject( projectId );
				ProductUsageViewModel.LoadProductUsageForSelectedProject( projectId );
				ProductUsageViewModel.FilterCostEntriesByProjectId( projectId );
				ProductUsageViewModel.FilterProductUsageByProjectId( projectId );

				OnPropertyChanged( nameof( SelectedProject ) );
			}
			else
			{
				TimeViewModel.IsProjectSelected = false;
				ProductUsageViewModel.IsProjectSelected = false;
			}
		}
	}

	public ProjectModel SelectedProject
	{
		get => ProjectViewModel.SelectedProject;
		set => ProjectViewModel.SelectedProject = value;
	}

	private void RefreshProductUsageView()
	{
		if ( ProductUsageViewModel != null && ProjectViewModel.SelectedProject != null )
		{
			System.Windows.Application.Current.Dispatcher.BeginInvoke( DispatcherPriority.Background, new Action( () =>
			{
				int projectId = ProjectViewModel.SelectedProject.ProjectId;

				// Vernieuw gegevens
				ProductUsageViewModel.Refresh();

				// Pas filters opnieuw toe
				ProductUsageViewModel.FilterCostEntriesByProjectId( projectId );
				ProductUsageViewModel.FilterProductUsageByProjectId( projectId );

				// Forceer UI updates
				ProductUsageViewModel.NotifyPropertyChanged( nameof( ProductUsageViewModel.FilteredCostEntries ) );
				ProductUsageViewModel.NotifyPropertyChanged( nameof( ProductUsageViewModel.FilteredProductUsage ) );

				CommandManager.InvalidateRequerySuggested();
				//ProductUsageViewModel.NotifyHasFilteredCostEntries();
				//ProductUsageViewModel.NotifyHasFilteredProductUsage();
			} ) );
		}
	}

	public void RefreshAll()
	{
		// Tijdelijk event handlers verwijderen om cascade van events te voorkomen
		ProjectViewModel.PropertyChanged -= OnSelectedProjectChanged;

		// Vernieuw alle viewmodels
		ProjectViewModel.Refresh();
		ProductViewModel.Refresh();
		WorktypeViewModel.Refresh();

		// Reset specifieke eigenschappen
		TimeViewModel.ResetProductUsageFields();

		// Vernieuw gegevens die project-afhankelijk zijn
		if ( ProjectViewModel.SelectedProject != null )
		{
			int projectId = ProjectViewModel.SelectedProject.ProjectId;

			// Vernieuw productgebruik viewmodel
			ProductUsageViewModel.Refresh();
			ProductUsageViewModel.FilterCostEntriesByProjectId( projectId );
			ProductUsageViewModel.FilterProductUsageByProjectId( projectId );

			// Vernieuw tijdregistraties
			TimeViewModel.LoadTimeEntriesForSelectedProject( projectId );
		}

		// Vernieuw TimeViewModel 
		TimeViewModel.Refresh();

		// Event handlers weer activeren
		ProjectViewModel.PropertyChanged += OnSelectedProjectChanged;
	}
}
