namespace Modelbouwer.View;

/// <summary>
/// Interaction logic for ProjectManagement.xaml
/// </summary>
public partial class ProjectManagement : Page
{
	public ProjectManagement()
	{
		InitializeComponent();
		this.Loaded += Data_Loaded;
	}

	private void Data_Loaded( object sender, RoutedEventArgs e )
	{
		if ( DataContext is CombinedProjectViewModel viewModel )
		{
			viewModel.RefreshAll();
		}
	}

	#region Checkbox for project completion is checked/unchecked
	private void ProjectStatus( object sender, RoutedEventArgs e )
	{
		if ( ProjectClosed.IsChecked == true )
		{
			valueShow.IsChecked = false;
		}
		else
		{
			valueShow.IsChecked = true;
		}
	}
	#endregion

	#region Rotate project image 90degrees
	private void ImageRotate( object sender, RoutedEventArgs e )
	{
		if ( DataContext is CombinedProjectViewModel viewModel )
		{
			ProjectModel? selectedProject = viewModel.ProjectViewModel.SelectedProject;

			if ( selectedProject != null )
			{
				int _tempValue = int.Parse(viewModel.ProjectViewModel.SelectedProject.ProjectImageRotationAngle) + 90;
				if ( _tempValue == 360 )
				{
					_tempValue = 0;
				}

				viewModel.ProjectViewModel.SelectedProject.ProjectImageRotationAngle = _tempValue.ToString();

				ProjectImage.LayoutTransform = new RotateTransform( _tempValue );
			}
		}
	}
	#endregion

	#region Delete Project Image
	private void ImageDelete( object sender, RoutedEventArgs e )
	{
		if ( DataContext is CombinedProjectViewModel viewModel )
		{
			ProjectModel? selectedProject = viewModel.ProjectViewModel.SelectedProject;

			if ( selectedProject != null )
			{
				viewModel.ProjectViewModel.SelectedProject.ProjectImage = null;
				viewModel.ProjectViewModel.SelectedProject.ProjectImageRotationAngle = "0";

				ProjectImage.GetBindingExpression( Image.SourceProperty )?.UpdateTarget();
			}
		}
	}
	#endregion

	#region Add/Replace project image
	private void ImageAdd( object sender, RoutedEventArgs e )
	{
		OpenFileDialog ImageDialog = new();
		ImageDialog.Title = "Selecteer een afbeelding voor dit project";
		ImageDialog.Filter = "Afbeeldingen (*.jpg;*.jpeg;*.png;*.gif;*.bmp)|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
		if ( ImageDialog.ShowDialog() == DialogResult.OK )
		{
			ProjectImage.Source = new BitmapImage( new Uri( ImageDialog.FileName ) );
		}
	}
	#endregion

	#region Load the memo field
	private void SetRtfContent( string rtfContent )
	{
		if ( rtfContent != null && rtfContent != "" )
		{
			using MemoryStream stream = new( Encoding.UTF8.GetBytes( rtfContent ) );
			TextRange textRange = new(ProjectMemo.Document.ContentStart, ProjectMemo.Document.ContentEnd);
			textRange.Load( stream, System.Windows.DataFormats.Rtf );
		}
		else { ProjectMemo.Document.Blocks.Clear(); }
	}
	#endregion

	private void ProjectDataGridLoaded( object sender, RoutedEventArgs e )
	{
		if ( dataGrid.ItemsSource is IEnumerable<ProjectModel> projects && projects.Any() )
		{
			dataGrid.SelectedIndex = 0;
		}
	}

	#region Changed Project in dataGrid
	private void ChangedProject( object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs e )
	{
		if ( DataContext is CombinedProjectViewModel viewModel )
		{
			ProjectModel? selectedProject = viewModel.ProjectViewModel.SelectedProject;

			if ( selectedProject != null )
			{
				// Load the memo field for the selected project
				SetRtfContent( selectedProject.ProjectMemo );

				// Load time entries and update HasFilteredTimeEntries
				viewModel.TimeViewModel.LoadTimeEntriesForSelectedProject( selectedProject.ProjectId );

				// Force a UI update for the HasFilteredTimeEntries property
				CommandManager.InvalidateRequerySuggested();

				//viewModel.ChartsWorkedHoursPerMonthViewModel.LoadChartData( selectedProject.ProjectId );
				viewModel.ChartsWorkedHoursPerMonthViewModel.Refresh( selectedProject.ProjectId );
			}

			if ( ProjectClosed.IsChecked == false && selectedProject != null && double.Parse( viewModel.ProjectViewModel.SelectedProject.ProjectExpectedTime ) > 0 )
			{
				ProjectExpEnddate.Text = DBCommands.GetProjectEndDate( viewModel.ProjectViewModel.SelectedProject.ProjectId ).ToString();
			}
		}

		object originalSource = ProjectTimeEntriesDataGrid.ItemsSource;
		ProjectTimeEntriesDataGrid.ItemsSource = null;
		ProjectTimeEntriesDataGrid.ItemsSource = originalSource;
	}
	#endregion

	#region Add a new project
	private void ButtonNew( object sender, RoutedEventArgs e )
	{
		//Create a new empty project, emtpy memo, empty image
		if ( DataContext is CombinedProjectViewModel viewModel )
		{
			// When adding a new Project, the used Material and registered Time registration should disabled, until the new Project is saved and a ProjectId is available
			ProjectCostsTab.IsEnabled = false;
			ProjectTimeTab.IsEnabled = false;

			// Add new Project
			viewModel.ProjectViewModel.AddNewItem();

			viewModel.ProjectViewModel.SelectedProject = viewModel.ProjectViewModel.Project.Last();

			//dataGrid.Items.Refresh();
		}
	}
	#endregion

	#region Delete an existing project
	private void ButtonDelete( object sender, RoutedEventArgs e )
	{
		if ( DataContext is CombinedProjectViewModel viewModel )
		{
			ProjectModel? selectedProject = viewModel.ProjectViewModel.SelectedProject;
			string? _deleteName = selectedProject.ProjectName;

			int result = DBCommands.DeleteProduct( selectedProject.ProjectId );
			viewModel.ProjectViewModel.Project.Remove( selectedProject );

			//if ( result == 1 )
			//{ dispStatusLine.Text = $"{_deleteName} {( string ) FindResource( "Edit.Product.Tab.Statusline.Hidden" )}"; }
			//else
			//{ dispStatusLine.Text = $"{_deleteName} {( string ) FindResource( "Edit.Product.Tab.Statusline.Deleted" )}"; }
		}
		;
	}
	#endregion

	#region Save a new or changed project
	private void ButtonSave( object sender, RoutedEventArgs e )
	{
		CombinedProjectViewModel? viewModel = DataContext as CombinedProjectViewModel;
		ProjectModel? selectedProject = viewModel.ProjectViewModel.SelectedProject;

		byte [ ]? ProjectImage = selectedProject.ProjectImage;
		//var ProjectMemo = selectedProject.ProjectMemo;

		string[ , ] ProjectFields = new string [7,3]
			{
				{ DBNames.ProjectFieldNameName, DBNames.ProjectFieldTypeName, ProjectName.Text },
				{ DBNames.ProjectFieldNameCode, DBNames.ProjectFieldTypeCode, ProjectCode.Text },
				{ DBNames.ProjectFieldNameStartDate, DBNames.ProjectFieldTypeStartDate, ProjectStartDate.Text },
				{ DBNames.ProjectFieldNameEndDate, DBNames.ProjectFieldTypeEndDate, ProjectEnddate.Text },
				{ DBNames.ProjectFieldNameClosed, DBNames.ProjectFieldTypeClosed, ProjectClosed.IsChecked == true ? "1" : "0" },
				{ DBNames.ProjectFieldNameExpectedTime, DBNames.ProjectFieldTypeExpectedTime, ProjectExpectedBuildTime.Text },
				{ DBNames.ProjectFieldNameImageRotationAngle, DBNames.ProjectFieldTypeImageRotationAngle, selectedProject.ProjectImageRotationAngle.ToString() }
			};

		if ( selectedProject.ProjectId == 0 )
		{
			// Save a new Project including the image
			DBCommands.InsertInTable( DBNames.ProjectTable, ProjectFields, ProjectImage, DBNames.ProjectFieldNameImage );

			// Get the Id of the just saved Project and use it to create the where field for the update memo action
			string ProjectId = DBCommands.GetLatestIdFromTable( DBNames.ProjectTable );
			string[,] whereFields = new string[1, 3]
			{
				{ DBNames.ProjectFieldNameId, DBNames.ProjectFieldTypeId,  ProjectId},
			};

			//Save the memo in de Project table
			DBCommands.UpdateMemoFieldInTable( DBNames.ProjectTable, whereFields, DBNames.ProjectFieldNameMemo, GeneralHelper.GetRichTextFromFlowDocument( ProjectMemo.Document ) );

			//Save Image
			DBCommands.UpdateImageInTable( DBNames.ProjectTable, whereFields, ProjectImage, DBNames.ProjectFieldNameImage );

			//Set the pnew ProjectId for the selected Project
			selectedProject.ProjectId = int.Parse( ProjectId );

			// Project is no longer new, so reset the IsNew flag
			viewModel.ProjectViewModel.IsAddingNew = false;
		}
		else
		{
			// Update an excisting Project
			string[,] whereFields = new string[1, 3]
			{
				{ DBNames.ProjectFieldNameId, DBNames.ProjectFieldTypeId, selectedProject.ProjectId.ToString() },
			};

			DBCommands.UpdateInTable( DBNames.ProjectTable, ProjectFields, whereFields );

			//Update the memo in de Project table
			DBCommands.UpdateMemoFieldInTable( DBNames.ProjectTable, whereFields, DBNames.ProjectFieldNameMemo, GeneralHelper.GetRichTextFromFlowDocument( ProjectMemo.Document ) );

			//Update Image
			DBCommands.UpdateImageInTable( DBNames.ProjectTable, whereFields, ProjectImage, DBNames.ProjectFieldNameImage );

		}

		// Update the ObservableCollection
		viewModel.ProjectViewModel.Project.Clear();

		ObservableCollection<ProjectModel> updatedProjects = DBCommands.GetProjectList();
		foreach ( ProjectModel project in updatedProjects )
		{
			viewModel.ProjectViewModel.Project.Add( project );
		}

		// Reselect the saved project in the DataGrid
		viewModel.ProjectViewModel.SelectedProject = viewModel.ProjectViewModel.Project
			.FirstOrDefault( p => p.ProjectId == selectedProject.ProjectId );
	}

	#endregion
}
