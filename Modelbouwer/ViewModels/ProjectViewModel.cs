namespace Modelbouwer.ViewModels;
public partial class ProjectViewModel : ObservableObject
{
	[ObservableProperty]
	public int projectId;

	[ObservableProperty]
	public string? projectCode;

	[ObservableProperty]
	public string? projectName;

	[ObservableProperty]
	public string? projectStartDate;

	[ObservableProperty]
	public string? projectStartDateStr;

	[ObservableProperty]
	public string? projectEndDate;

	[ObservableProperty]
	public string? projectEndDateStr;

	[ObservableProperty]
	public string? projectExpectedTime;

	//[ObservableProperty]
	public byte[]? projectImage;

	[ObservableProperty]
	public int projectImageRotationAngle;

	[ObservableProperty]
	public bool projectClosed;

	[ObservableProperty]
	public string? projectMemo;

	[ObservableProperty]
	public string? projectExpectedWorkdays;

	[ObservableProperty]
	public string? projectExpectedWorkdaysText;

	[ObservableProperty]
	public string? projectTodoTime;

	[ObservableProperty]
	public string? projectTodoWorkdays;

	[ObservableProperty]
	public string? projectTodoWorkdaysText;

	[ObservableProperty]
	public string? projectCreated;

	[ObservableProperty]
	public string? projectModified;

	[ObservableProperty]
	public string? projectSearchField;

	[ObservableProperty]
	public string? projectTotalTimeInHours;

	[ObservableProperty]
	public string? projectTotalTimeInText;

	[ObservableProperty]
	public string? projectShortestWorkday;

	[ObservableProperty]
	public string? projectShortestWorkdayHours;

	[ObservableProperty]
	public string? projectLongestWorkday;

	[ObservableProperty]
	public string? projectLongestWorkdayHours;

	[ObservableProperty]
	public string? projectAverageHoursPerDay;

	[ObservableProperty]
	public string? projectAverageHoursPerDayLong;

	[ObservableProperty]
	public string? projectBuildDays;

	[ObservableProperty]
	public string? projectMaterialCosts;

	[ObservableProperty]
	public string? projectTimeCosts;

	[ObservableProperty]
	public string? projectTotalCosts;

	public ImageSource? _projectImage;
	public ImageSource? ProjectImage
	{
		get => _projectImage;
		set
		{
			_projectImage = value ?? ( System.Windows.Application.Current.TryFindResource( "noimage" ) as ImageSource )
				?? new BitmapImage( new Uri( "pack://application:,,,/YourAssemblyName;component/Resources/noimage.png" ) );

			OnPropertyChanged( nameof( ProjectImage ) );
		}
	}

	[ObservableProperty]
	private ProjectModel? _selectedProject;

	private bool _isAddingNew;

	public bool IsAddingNew
	{
		get => _isAddingNew;
		set
		{
			if ( _isAddingNew != value )
			{
				_isAddingNew = value;
				OnPropertyChanged( nameof( IsAddingNew ) );
			}
		}
	}

	public ObservableCollection<ProjectModel> Project
	{
		get => _project;
		set
		{
			if ( _project != value )
			{
				_project = value;
				OnPropertyChanged( nameof( Project ) );
			}
		}
	}
	private ObservableCollection<ProjectModel>? _project;

	public void AddNewItem()
	{
		ProjectModel newProject = new()
		{
			ProjectCode = string.Empty,
			ProjectName = string.Empty,
			ProjectStartDate = string.Empty,
			ProjectEndDate = string.Empty,
			ProjectExpectedTime = "0",
			ProjectImage = null,
			ProjectImageRotationAngle = "0",
			ProjectMemo = string.Empty,
			ProjectTotalTimeInHours = "0",
			ProjectShortestWorkday = string.Empty,
			ProjectShortestWorkdayHours = "0",
			ProjectLongestWorkday = string.Empty,
			ProjectLongestWorkdayHours = "0",
			ProjectBuildDays = "0",
			ProjectClosed = false,
			ProjectAverageHoursPerDay = "0",
			ProjectTodoTime = "0",
			ProjectExpectedWorkdays = "0",
			ProjectMaterialCosts = "0",
			ProjectTimeCosts = "0",
			ProjectTotalCosts = "0"
		};

		Project.Add( newProject );
		SelectedProject = newProject;
		IsAddingNew = true;
	}

	public ProjectViewModel()
	{
		Project = new ObservableCollection<ProjectModel>( DBCommands.GetProjectList() );
		SelectedProject = Project [ 0 ];
	}
}
