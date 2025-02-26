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

	[ObservableProperty]
	private ProductUsageModel? _selectedCostEntry;

	public ObservableCollection<ProductUsageModel> ProjectCost { get; set; }
	public ObservableCollection<ProductUsageModel> FilteredCostEntries { get; private set; } = [ ];
	public bool HasFilteredCostEntries => FilteredCostEntries != null && FilteredCostEntries.Any();

	#region Notify UI when list changes
	private void NotifyHasFilteredCostEntries()
	{
		OnPropertyChanged( nameof( HasFilteredCostEntries ) );
	}
	#endregion

	public void FilterCostEntriesByProjectId( int projectId )
	{
		FilteredCostEntries.Clear();
		foreach ( ProductUsageModel costEntry in ProjectCost.Where( c => c.ProductUsageProjectId == projectId ) )
		{
			FilteredCostEntries.Add( costEntry );
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
	public ProductUsageViewModel()
	{
		ProjectCost = [ .. DBCommands.GetCostList() ];
	}

	public void Refresh()
	{
		ProjectCost = [ .. DBCommands.GetCostList() ];
		OnPropertyChanged( nameof( ProjectCost ) );
	}
}
