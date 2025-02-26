namespace Modelbouwer.View;

/// <summary>
/// Interaction logic for WorktypeManagement.xaml
/// </summary>
public partial class WorktypeManagement : Page
{
	public WorktypeManagement()
	{
		InitializeComponent();
		this.Loaded += Data_Loaded;
	}

	private void Data_Loaded( object sender, RoutedEventArgs e )
	{
		if ( DataContext is WorktypeViewModel viewModel )
		{
			viewModel.Refresh();
		}
	}

	private void ButtonDelete( object sender, RoutedEventArgs e )
	{
		if ( DataContext is WorktypeViewModel )
		{
			// Check if a row is selected (check if there is a known Id)
			if ( inpWorktypeId != null && !string.IsNullOrWhiteSpace( inpWorktypeId.Text ) )
			{
				DBCommands.DeleteRecordTree( DBNames.WorktypeTable, DBNames.WorktypeFieldNameId, DBNames.WorktypeFieldNameParentId, int.Parse( inpWorktypeId.Text ) );

				UpdateDataGrid();
				dispStatusLine.Text = $"{inpWorktypeName.Text}";
			}
		}
	}
	private void ButtonSave( object sender, RoutedEventArgs e )
	{
		if ( DataContext is WorktypeViewModel )
		{
			// Save can only be performed if a name is entered
			if ( !string.IsNullOrWhiteSpace( inpWorktypeName.Text ) )
			{

				// Check if item is new or has to be updated
				if ( inpWorktypeId.Text is "0" or "" )
				{

					string [ , ] _addFields = new string [ 2, 3 ]
			{
				{ DBNames.WorktypeFieldNameParentId, DBNames.WorktypeFieldTypeParentId, inpWorktypeParentId.Text },
				{ DBNames.WorktypeFieldNameName, DBNames.WorktypeFieldTypeName, inpWorktypeName.Text }
			};
					_ = DBCommands.InsertInTable( DBNames.WorktypeTable, _addFields );

					UpdateDataGrid();
					dispStatusLine.Text = $"{inpWorktypeName.Text} {( string ) FindResource( "Maintanance.Statusline.NotSaved.Unchanged" )}";
				}
				else
				{
					string [ , ] _whereFields = new string [ 1, 3 ] { { DBNames.WorktypeFieldNameId, DBNames.WorktypeFieldTypeId, inpWorktypeId.Text } };

					_ = new string [ 1, 3 ]
				{
				{ DBNames.WorktypeFieldNameName, DBNames.WorktypeFieldTypeName, inpWorktypeName.Text }
				};

					int arrayLength = string.IsNullOrEmpty(inpWorktypeParentId.Text) ? 1 : 2;
					string [ , ] _updateFields = new string [ arrayLength, 3 ];

					if ( arrayLength == 1 )
					{
						_updateFields [ 0, 0 ] = DBNames.WorktypeFieldNameName;
						_updateFields [ 0, 1 ] = DBNames.WorktypeFieldTypeName;
						_updateFields [ 0, 2 ] = inpWorktypeName.Text;
					}
					else
					{
						_updateFields [ 0, 0 ] = DBNames.WorktypeFieldNameParentId;
						_updateFields [ 0, 1 ] = DBNames.WorktypeFieldTypeParentId;
						_updateFields [ 0, 2 ] = inpWorktypeParentId.Text;

						_updateFields [ 1, 0 ] = DBNames.WorktypeFieldNameName;
						_updateFields [ 1, 1 ] = DBNames.WorktypeFieldTypeName;
						_updateFields [ 1, 2 ] = inpWorktypeName.Text;
					}

					_ = DBCommands.UpdateInTable( DBNames.WorktypeTable, _updateFields, _whereFields );

					dispStatusLine.Text = $"{( string ) FindResource( "Maintanance.Statusline.NotSaved.DataOf" )} {inpWorktypeName.Text} {( string ) FindResource( "Maintanance.Statusline.NotSaved.Changed" )}";
					UpdateDataGrid();
				}

			}
		}
		else
		{
			dispStatusLine.Text = $"{( string ) FindResource( "Maintanance.Statusline.NotSaved.Empty" )}";
		}
	}

	private void CollapseAll( object sender, RoutedEventArgs e )
	{
		treeView?.CollapseAll();
	}

	private void ExpandAll( object sender, RoutedEventArgs e )
	{
		treeView?.ExpandAll();
	}

	private void AddSubTask( object sender, RoutedEventArgs e )
	{
		MenuItem? menuItem = sender as MenuItem;

		if ( menuItem?.DataContext is Syncfusion.UI.Xaml.TreeView.Engine.TreeViewNode node &&
	node.Content is WorktypeModel selectedWorktype )
		{
			int? _parentId = selectedWorktype.WorktypeId;

			AddNewWorktype( _parentId );
		}
	}

	private void AddTask( object sender, RoutedEventArgs e )
	{
		MenuItem? menuItem = sender as MenuItem;

		if ( menuItem?.DataContext is Syncfusion.UI.Xaml.TreeView.Engine.TreeViewNode node &&
				node.Content is WorktypeModel selectedWorktype )
		{
			int? _parentId = selectedWorktype.WorktypeParentId;

			AddNewWorktype( _parentId );
		}
	}

	private void ButtonNewWorktype( object sender, RoutedEventArgs e )
	{
		if ( int.TryParse( inpWorktypeParentId.Text, out int parentId ) )
		{
			AddNewWorktype( parentId );
		}
		else
		{
			AddNewWorktype( null );
		}
	}

	private void ButtonNewSubWorktype( object sender, RoutedEventArgs e )
	{
		if ( int.TryParse( inpWorktypeId.Text, out int parentId ) )
		{
			AddNewWorktype( parentId );
		}
		else
		{
			AddNewWorktype( null );
		}
	}
	private void AddNewWorktype( int? _parentId = 0 )
	{
		inpWorktypeId.Text = "0";
		inpWorktypeName.Text = string.Empty;
		inpWorktypeParentId.Text = _parentId?.ToString();
	}

	private void UpdateDataGrid()
	{
		if ( DataContext is WorktypeViewModel viewModel )
		{
			WorktypeViewModel worktypeViewModel = viewModel;
			object temp = treeView.ItemsSource;
			DBCommands dbCommands = new();

			treeView.ItemsSource = null;
			treeView.ItemsSource = temp;

			ObservableCollection<WorktypeModel> newWorktypeList = dbCommands.GetWorktypeList();
			worktypeViewModel.Worktype.Clear();
			foreach ( WorktypeModel worktype in newWorktypeList )
			{
				worktypeViewModel.Worktype.Add( worktype );
			}
		}
	}
}
