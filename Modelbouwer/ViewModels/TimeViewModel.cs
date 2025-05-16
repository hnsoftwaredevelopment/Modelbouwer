using System.ComponentModel;
using System.Windows.Threading;

using CommunityToolkit.Mvvm.Input;

namespace Modelbouwer.ViewModels;
public partial class TimeViewModel : ObservableObject
{
	[ObservableProperty]
	public int timeId;

	[ObservableProperty]
	public int timeProjectId;

	[ObservableProperty]
	public int timeWorktypeId;

	[ObservableProperty]
	public string? timeWorkDate;

	[ObservableProperty]
	public string? timeStartTime;

	[ObservableProperty]
	public string? timeEndTime;

	[ObservableProperty]
	public DateTime dateTimeDate;

	[ObservableProperty]
	public string? timeComment;

	[ObservableProperty]
	public double timeElapsedMinutes;

	[ObservableProperty]
	public string? timeElapsedTime;

	[ObservableProperty]
	public TimeSpan? timeWorkedHours;

	[ObservableProperty]
	public int timeYear;

	[ObservableProperty]
	public int timeMonth;

	[ObservableProperty]
	public int timeWorkday;

	[ObservableProperty]
	public string timeWorkdayName;

	[ObservableProperty]
	public string? timeYearMonth;

	[ObservableProperty]
	public string? timeYearWorkday;

	[ObservableProperty]
	public string? timeSortIndex;

	[ObservableProperty]
	public string? timeProjectName;

	[ObservableProperty]
	public string? timeWorktypeName;

	[ObservableProperty]
	private TimeModel? _selectedTimeEntry;

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
	#endregion

	#region Has Time Entry Changes
	[ObservableProperty]
	private bool _hasChanges;

	private void UpdateHasChanges()
	{
		HasChanges = FilteredTimeEntries?.Any( e => e.State != TimeModel.RecordState.Unchanged ) ?? false;
	}

	private void SetupChangeTracking()
	{
		FilteredTimeEntries.CollectionChanged += ( s, e ) => UpdateHasChanges();

		// Event handler voor PropertyChanged op individuele items
		PropertyChangedEventHandler handler = (s, e) =>
		{
			if (e.PropertyName == nameof(TimeModel.State))
			{
				UpdateHasChanges();
			}
		};

		foreach ( TimeModel item in FilteredTimeEntries )
		{
			item.PropertyChanged += handler;
		}

		// Voor nieuwe items die worden toegevoegd
		FilteredTimeEntries.CollectionChanged += ( s, e ) =>
		{
			if ( e.NewItems != null )
			{
				foreach ( TimeModel item in e.NewItems )
				{
					item.PropertyChanged += handler;
				}
			}
		};
	}

	private void Item_PropertyChanged( object sender, PropertyChangedEventArgs e )
	{
		if ( e.PropertyName == nameof( TimeModel.State ) )
		{
			UpdateHasChanges();
		}
	}
	#endregion


	private ProductModel _selectedUsedProduct;
	public ProductModel? SelectedUsedProduct
	{
		get => _selectedUsedProduct;
		set
		{
			if ( SetProperty( ref _selectedUsedProduct, value ) )
			{
				System.Diagnostics.Debug.WriteLine( $"SelectedUsedProduct verandert naar: {value?.ProductName}" );

				// Je kunt dit gebruiken in je ExecuteProductUsageSave methode
				UpdateHasChanges();
			}
		}
	}

	private string? _comments;
	public string? Comments
	{
		get => _comments;
		set => SetProperty( ref _comments, value );
	}

	private ProductViewModel _productViewModel;
	private bool _hasProductUsageChanges;

	public ProductViewModel ProductViewModel
	{
		get => _productViewModel;
		set => SetProperty( ref _productViewModel, value );
	}

	private ProductUsageViewModel _productUsageViewModel;
	public ProductUsageViewModel ProductUsageViewModel
	{
		get => _productUsageViewModel;
		set => SetProperty( ref _productUsageViewModel, value );
	}

	public void ResetProductUsageFields()
	{
		//SelectedUsedProduct = ProductViewModel.Product.First();
		ProductModel? firstProduct = ProductViewModel.Product.FirstOrDefault();
		if ( firstProduct != null )
		{
			// Eerst null om binding te forceren daarna juiste waarde
			SelectedUsedProduct = null;
			OnPropertyChanged( nameof( SelectedUsedProduct ) );

			// Directe toewijzing na korte vertraging (kan in WPF UI thread nodig zijn)
			System.Windows.Application.Current.Dispatcher.BeginInvoke( DispatcherPriority.Background, new Action( () =>
			{
				SelectedUsedProduct = firstProduct;
				OnPropertyChanged( nameof( SelectedUsedProduct ) );
			} ) );
		}

		AmountUsed = "0,00";
		OnPropertyChanged( nameof( AmountUsed ) );

		SelectedDate = DateTime.Today;
		OnPropertyChanged( nameof( SelectedDate ) );

		Comments = string.Empty;
		OnPropertyChanged( nameof( Comments ) );

		UpdateHasProductUsageChanges();
	}

	public bool HasProductUsageChanges
	{
		get => _hasProductUsageChanges;
		private set => SetProperty( ref _hasProductUsageChanges, value );
	}

	private string _amountUsed;
	private DateTime? _selectedDate;

	public string AmountUsed
	{
		get => _amountUsed;
		set
		{
			if ( SetProperty( ref _amountUsed, value ) )
			{
				UpdateHasProductUsageChanges();
			}
		}
	}

	public DateTime? SelectedDate
	{
		get => _selectedDate;
		set
		{
			if ( SetProperty( ref _selectedDate, value ) )
			{
				UpdateHasProductUsageChanges();
			}
		}
	}

	private void UpdateHasProductUsageChanges()
	{
		HasProductUsageChanges =
			ProductViewModel?.SelectedProduct != null &&
			!string.IsNullOrWhiteSpace( AmountUsed ) &&
			AmountUsed != "0,00" &&
			SelectedDate.HasValue;
	}

	private bool _hasFilteredTimeEntries;
	//public bool HasFilteredTimeEntries => FilteredTimeEntries != null && FilteredTimeEntries.Any();
	public bool HasFilteredTimeEntries
	{
		get => _hasFilteredTimeEntries;
		private set => SetProperty( ref _hasFilteredTimeEntries, value );
	}

	#region FilteredFrom Time Entries
	private ObservableCollection<TimeModel> _filteredTimeEntries = [];
	public ObservableCollection<TimeModel> FilteredTimeEntries
	{
		get => _filteredTimeEntries;
		set
		{
			if ( _filteredTimeEntries != value )
			{
				_filteredTimeEntries = value;
				OnPropertyChanged( nameof( FilteredTimeEntries ) );
				UpdateHasFilteredTimeEntries();
			}
		}
	}
	private void UpdateHasFilteredTimeEntries()
	{
		HasFilteredTimeEntries = FilteredTimeEntries != null && FilteredTimeEntries.Any();
		Console.WriteLine( HasFilteredTimeEntries );
	}
	#endregion

	#region Selected Project
	private int _selectedProjectId;
	public int SelectedProject
	{
		get => _selectedProjectId;
		set
		{
			if ( _selectedProjectId != value )
			{
				_selectedProjectId = value;

				if ( value != 0 )
				{
					LoadTimeEntriesForSelectedProject( value );
				}
				else
				{
					FilteredTimeEntries.Clear();
				}

				SelectedProjectChanged?.Invoke( this, _selectedProjectId );
				OnPropertyChanged( nameof( FilteredTimeEntries ) );
			}
		}
	}
	#endregion

	#region Toolbar commands
	public ICommand AddNewRowCommand { get; }
	public ICommand SaveCommand { get; }
	public ICommand SaveProductUsageCommand { get; }

	#region Add new row to the TimeEntries
	private void ExectuteAddNewRow()
	{
		TimeModel newEntry = new()
		{
			DateTimeDate = DateTime.Today,
			DateTimeStart = DateTime.Now.AddHours(-1),
			DateTimeEnd = DateTime.Now,
			State = TimeModel.RecordState.Added
		};

		FilteredTimeEntries.Insert( 0, newEntry );

		UpdateHasChanges();
		UpdateHasFilteredTimeEntries();

		SelectedTimeEntry = newEntry;
	}
	#endregion

	#region Save command for Product Usage
	private void ExecuteProductUsageSave()
	{
		int _productId = SelectedUsedProduct?.ProductId ?? 0;
		string _amount = AmountUsed;
		string _date = SelectedDate?.ToString( "yyyy-MM-dd" ) ?? string.Empty;
		string _comments = Comments ?? "";

		string[ , ] saveFields = new string [5,3]
			{
				{DBNames.ProductUsageFieldNameProjectId, DBNames.ProductUsageFieldTypeProjectId, SelectedProject.ToString()},
				{DBNames.ProductUsageFieldNameProductId, DBNames.ProductUsageFieldTypeProductId, _productId.ToString()},
				{DBNames.ProductUsageFieldNameAmountUsed, DBNames.ProductUsageFieldTypeAmountUsed, AmountUsed},
				{DBNames.ProductUsageFieldNameUsageDate, DBNames.ProductUsageFieldTypeUsageDate, _date},
				{DBNames.ProductUsageFieldNameComment, DBNames.ProductUsageFieldTypeComment, _comments}
			};

		DBCommands.InsertInTable( DBNames.ProductUsageTable, saveFields );

		ResetProductUsageFields();

		if ( ProductUsageViewModel != null )
		{
			ProductUsageViewModel.Refresh();
		}

		OnPropertyChanged( nameof( SelectedUsedProduct ) );

		UpdateHasChanges();
	}
	#endregion

	#region Save command for Time Entries
	private void ExecuteSave()
	{
		//Execuyte the save command
		List<TimeModel> added = FilteredTimeEntries.Where(e => e.State == TimeModel.RecordState.Added).ToList();
		List<TimeModel> modified = FilteredTimeEntries.Where(e => e.State == TimeModel.RecordState.Modified).ToList();

		//Add new records to the TimeTable
		foreach ( TimeModel? entry in added )
		{
			entry.State = TimeModel.RecordState.Unchanged;
		}

		//Change existing records in TimeTable
		foreach ( TimeModel entry in modified )
		{
			entry.State = TimeModel.RecordState.Unchanged;
		}

		UpdateHasChanges();
	}
	#endregion
	#endregion

	public void LoadTimeEntriesForSelectedProject( int projectId )
	{
		try
		{
			ObservableCollection<TimeModel> TimeModelLines = DBCommands.GetTimeList(projectId);


			// Set all entrie on Status: Unchanged by default
			foreach ( TimeModel entry in TimeModelLines )
			{
				entry.State = TimeModel.RecordState.Unchanged;
			}

			FilteredTimeEntries = new ObservableCollection<TimeModel>( TimeModelLines );
			OnPropertyChanged( nameof( FilteredTimeEntries ) );

			// Setup change tracking for new items
			foreach ( TimeModel item in FilteredTimeEntries )
			{
				item.PropertyChanged += Item_PropertyChanged;
			}

			UpdateHasChanges();
			UpdateHasFilteredTimeEntries();
			OnPropertyChanged( nameof( HasFilteredTimeEntries ) );

			if ( FilteredTimeEntries.Any() )
			{
				SelectedTimeEntry = FilteredTimeEntries.First();
				OnPropertyChanged( nameof( SelectedTimeEntry ) );
			}
		}
		catch ( Exception ex )
		{
			Console.WriteLine( $"Error loading time entries: {ex.Message}" );
			throw;
		}
	}

	private void TimeViewModel_PropertyChanged( object sender, PropertyChangedEventArgs e )
	{
		if ( e.PropertyName == nameof( FilteredTimeEntries ) )
		{
			//NotifyHasFilteredTimeEntries();
			UpdateHasFilteredTimeEntries();
		}
	}

	public TimeViewModel( ProductUsageViewModel productUsageViewModel = null )
	{
		_filteredTimeEntries = [ ];
		UpdateHasFilteredTimeEntries();

		// Save the parameter in the property
		ProductUsageViewModel = productUsageViewModel;

		PropertyChanged += TimeViewModel_PropertyChanged;

		_filteredTimeEntries.CollectionChanged += ( s, e ) =>
		{
			UpdateHasFilteredTimeEntries();
		};

		SetupChangeTracking();

		// When there is an initial projectID, load the timeentries
		if ( SelectedProject != 0 )
		{
			LoadTimeEntriesForSelectedProject( SelectedProject );
		}

		ProductViewModel = new ProductViewModel();
		ProductViewModel.PropertyChanged += ( s, e ) =>
		{
			if ( e.PropertyName == nameof( ProductViewModel.SelectedProduct ) )
			{
				UpdateHasChanges();
			}
		};

		AddNewRowCommand = new RelayCommand( ExectuteAddNewRow );
		SaveCommand = new RelayCommand( ExecuteSave );
		SaveProductUsageCommand = new RelayCommand( ExecuteProductUsageSave );

		//FilteredTimeEntries.CollectionChanged += ( sender, e ) => UpdateCanExecuteSave();
		AmountUsed = "0,00";
		SelectedDate = DateTime.Today;
	}

	public void Refresh()
	{

		if ( SelectedProject != 0 )
		{
			LoadTimeEntriesForSelectedProject( SelectedProject );
		}
		OnPropertyChanged( nameof( FilteredTimeEntries ) );
		OnPropertyChanged( nameof( HasFilteredTimeEntries ) );
	}
}