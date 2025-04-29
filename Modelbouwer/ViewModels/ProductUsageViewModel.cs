using System.ComponentModel;
using System.Windows.Threading;

namespace Modelbouwer.ViewModels;
public partial class ProductUsageViewModel : ObservableObject
{
	[ObservableProperty]
	public int productUsageId;

	[ObservableProperty]
	public int productUsageProjectId;

	[ObservableProperty]
	public string? productUsageProjectName;

	[ObservableProperty]
	public int productUsageProductId;

	[ObservableProperty]
	public string? productUsageProductName;

	[ObservableProperty]
	public string? productUsageUsageDate;

	[ObservableProperty]
	public int productUsageCategoryId;

	[ObservableProperty]
	public string? productUsageCategoryName;

	[ObservableProperty]
	public double productUsageAmount;

	[ObservableProperty]
	public double productUsageProductPrice;

	[ObservableProperty]
	public double productUsageCosts;

	[ObservableProperty]
	public string? productUsageComment;

	#region Selected Project
	public event EventHandler<int>? SelectedProjectChanged;

	private bool _isProjectSelected;
	public bool IsProjectSelected
	{
		get => _isProjectSelected;
		set
		{
			if ( _isProjectSelected != value )
			{
				_isProjectSelected = value;
				OnPropertyChanged( nameof( IsProjectSelected ) );
			}
		}
	}

	private int _selectedProjectId;
	public int SelectedProject
	{
		get => _selectedProjectId;
		set
		{
			if ( _selectedProjectId != value )
			{
				_selectedProjectId = value;
				LoadProductUsageForSelectedProject( _selectedProjectId );
				SelectedProjectChanged?.Invoke( this, _selectedProjectId );
			}
			else
			{
				CommandManager.InvalidateRequerySuggested();
			}
		}
	}
	#endregion

	#region Costs
	[ObservableProperty]
	private ProductUsageModel? _selectedCostEntry;

	public ObservableCollection<ProductUsageModel> ProjectCost { get; set; }
	public ObservableCollection<ProductUsageModel> FilteredCostEntries { get; private set; } = [ ];
	public bool HasFilteredCostEntries => FilteredCostEntries != null && FilteredCostEntries.Any();
	#endregion

	#region Usage
	[ObservableProperty]
	private ProductUsageModel? _selectedProductUsage;

	public ObservableCollection<ProductUsageModel> ProductUsage { get; set; }
	public ObservableCollection<ProductUsageModel> FilteredProductUsage { get; private set; } = [ ];
	public bool HasFilteredProductUsage => FilteredProductUsage != null && FilteredProductUsage.Any();
	#endregion

	#region Notify UI when list changes
	public void NotifyHasFilteredCostEntries()
	{
		OnPropertyChanged( nameof( HasFilteredCostEntries ) );
	}

	public void NotifyHasFilteredProductUsage()
	{
		OnPropertyChanged( nameof( HasFilteredProductUsage ) );
	}

	// To be able to use OnPropertyChanged from other viewmodels (just use NotifyPropertyChanged instead)
	public void NotifyPropertyChanged( string propertyName )
	{
		OnPropertyChanged( propertyName );
	}
	#endregion

	public void FilterCostEntriesByProjectId( int projectId )
	{
		if ( FilteredCostEntries == null )
		{
			FilteredCostEntries = [ ];
		}
		else
		{
			FilteredCostEntries.Clear();
		}

		if ( ProjectCost != null && ProjectCost.Any() )
		{
			foreach ( ProductUsageModel costEntry in ProjectCost.Where( c => c.ProductUsageProjectId == projectId ) )
			{
				FilteredCostEntries.Add( costEntry );
			}
		}

		//Select first cost entry in the list if there are cost entries
		if ( FilteredCostEntries.Any() )
		{
			SelectedCostEntry = FilteredCostEntries.First();
		}

		// Force DataGrid to recognize changes
		OnPropertyChanged( nameof( FilteredCostEntries ) );
		NotifyHasFilteredCostEntries();
	}
	public void FilterProductUsageByProjectId( int projectId )
	{
		FilteredProductUsage.Clear();
		foreach ( ProductUsageModel productUsage in ProductUsage.Where( c => c.ProductUsageProjectId == projectId ) )
		{
			FilteredProductUsage.Add( productUsage );
		}

		//Select first cost entry in the list if there are cost entries
		if ( FilteredProductUsage.Any() )
		{
			SelectedProductUsage = FilteredProductUsage.First();
		}

		// Force DataGrid to recognize changes
		OnPropertyChanged( nameof( FilteredProductUsage ) );
		NotifyHasFilteredProductUsage();
	}

	public void LoadProductUsageForSelectedProject( int projectId )
	{
		try
		{
			ObservableCollection<ProductUsageModel> ProductUsageModelLines = DBCommands.GetProductUsageByProjectList(projectId);


			// Set all entrie on Status: Unchanged by default
			foreach ( ProductUsageModel entry in ProductUsageModelLines )
			{
				entry.State = ProductUsageModel.RecordState.Unchanged;
			}

			FilteredProductUsage = new ObservableCollection<ProductUsageModel>( ProductUsageModelLines );

			// Setup change tracking for new items
			foreach ( ProductUsageModel item in FilteredProductUsage )
			{
				item.PropertyChanged += Item_PropertyProductUsageChanged;
			}

			UpdateHasProductUsageChanges();

			if ( FilteredProductUsage.Any() )
			{
				SelectedProductUsage = FilteredProductUsage.First();
			}
		}
		catch ( Exception ex )
		{
			Console.WriteLine( $"Error loading product usage entries: {ex.Message}" );
			throw;
		}
	}

	#region Has Product Usage Changes
	[ObservableProperty]
	private bool _hasProductUsageChanges;

	private void UpdateHasProductUsageChanges()
	{
		HasProductUsageChanges = FilteredProductUsage?.Any( e => e.State != ProductUsageModel.RecordState.Unchanged ) ?? false;
	}

	private void Item_PropertyProductUsageChanged( object sender, PropertyChangedEventArgs e )
	{
		if ( e.PropertyName == nameof( TimeModel.State ) )
		{
			UpdateHasProductUsageChanges();
		}
	}
	#endregion

	public ProductUsageViewModel()
	{
		ProjectCost = [ ];
		ProductUsage = [ ];

		try
		{
			ObservableCollection<ProductUsageModel> costList = DBCommands.GetCostList();
			if ( costList != null && costList.Any() )
			{
				ProjectCost = new ObservableCollection<ProductUsageModel>( costList );
			}

			ProductUsage = DBCommands.GetAllProductUsageList();
		}
		catch ( Exception ex )
		{
			// Hier zou je een betere foutafhandeling kunnen implementeren
		}
	}

	public void Refresh()
	{
		try
		{
			ProductUsageModel? previousSelectedCost = SelectedCostEntry;
			ProductUsageModel? previousSelectedUsage = SelectedProductUsage;
			int currentProjectId = SelectedProject;

			System.Diagnostics.Debug.WriteLine( $"ProductUsageViewModel.Refresh() gestart voor project {currentProjectId}" );

			ObservableCollection<ProductUsageModel> newCostList = DBCommands.GetCostList();
			ObservableCollection<ProductUsageModel> newProductUsage = DBCommands.GetAllProductUsageList();

			ProjectCost = new ObservableCollection<ProductUsageModel>( newCostList );
			ProductUsage = new ObservableCollection<ProductUsageModel>( newProductUsage );

			// Notificeer de UI
			OnPropertyChanged( nameof( ProjectCost ) );
			OnPropertyChanged( nameof( ProductUsage ) );

			FilteredCostEntries.Clear();

			if ( SelectedProject > 0 )
			{
				FilterCostEntriesByProjectId( SelectedProject );
				FilterProductUsageByProjectId( SelectedProject );
			}

			OnPropertyChanged( nameof( FilteredCostEntries ) );
			OnPropertyChanged( nameof( FilteredProductUsage ) );

			NotifyHasFilteredCostEntries();
			NotifyHasFilteredProductUsage();
			UpdateHasProductUsageChanges();

			// Gebruik Dispatcher voor thread-safety bij UI updates
			System.Windows.Application.Current.Dispatcher.BeginInvoke( DispatcherPriority.Background, new Action( () =>
			{
				if ( FilteredCostEntries.Any() )
				{
					SelectedCostEntry = FilteredCostEntries.FirstOrDefault( c =>
						previousSelectedCost != null && c.ProductUsageId == previousSelectedCost.ProductUsageId )
						?? FilteredCostEntries.FirstOrDefault();

					OnPropertyChanged( nameof( SelectedCostEntry ) );
				}

				if ( FilteredProductUsage.Any() )
				{
					SelectedProductUsage = FilteredProductUsage.FirstOrDefault( p =>
						previousSelectedUsage != null && p.ProductUsageId == previousSelectedUsage.ProductUsageId )
						?? FilteredProductUsage.FirstOrDefault();

					OnPropertyChanged( nameof( SelectedProductUsage ) );
				}
			} ) );
		}
		catch ( Exception ex )
		{
			Console.WriteLine( $"Error refreshing product usage data: {ex.Message}" );
		}
	}
}
