namespace Modelbouwer.View;

/// <summary>
/// Interaction logic for LocationManagement.xaml
/// </summary>
public partial class LocationManagement : Page
{
	public LocationManagement()
	{
		InitializeComponent();
		this.Loaded += Data_Loaded;
	}

	private void Data_Loaded( object sender, RoutedEventArgs e )
	{
		if ( DataContext is StorageViewModel viewModel )
		{
			viewModel.Refresh();
		}
	}

	private void ButtonDelete( object sender, RoutedEventArgs e )
	{
		if ( DataContext is StorageViewModel )
		{
			// Check if a row is selected (check if there is a known Id)
			if ( inpStorageId != null && !string.IsNullOrWhiteSpace( inpStorageId.Text ) )
			{
				//StorageViewModel storageViewModel = viewModel;

				DBCommands.DeleteRecordTree( DBNames.StorageTable, DBNames.StorageFieldNameId, DBNames.StorageFieldNameParentId, int.Parse( inpStorageId.Text ) );
				//string [ , ] _whereFields = new string [ 1, 3 ] { { DBNames.StorageFieldNameId, DBNames.StorageFieldTypeId, inpStorageId.Text } };
				//string _result = DBCommands.DeleteRecord( DBNames.StorageTable, _whereFields );

				UpdateDataGrid();
				dispStatusLine.Text = $"{inpStorageName.Text}";
			}
		}
	}
	private void ButtonSave( object sender, RoutedEventArgs e )
	{
		if ( DataContext is StorageViewModel )
		{
			// Save can only be performed if a name is entered
			if ( !string.IsNullOrWhiteSpace( inpStorageName.Text ) )
			{

				// Check if item is new or has to be updated
				if ( inpStorageId.Text is "0" or "" )
				{

					string [ , ] _addFields = new string [ 2, 3 ]
			{
				{ DBNames.StorageFieldNameParentId, DBNames.StorageFieldTypeParentId, inpStorageParentId.Text },
				{ DBNames.StorageFieldNameName, DBNames.StorageFieldTypeName, inpStorageName.Text }
			};
					_ = DBCommands.InsertInTable( DBNames.StorageTable, _addFields );

					UpdateDataGrid();
					dispStatusLine.Text = $"{inpStorageName.Text} {( string ) FindResource( "Maintanance.Statusline.NotSaved.Unchanged" )}";
				}
				else
				{
					string [ , ] _whereFields = new string [ 1, 3 ] { { DBNames.StorageFieldNameId, DBNames.StorageFieldTypeId, inpStorageId.Text } };

					_ = new string [ 1, 3 ]
				{
				{ DBNames.StorageFieldNameName, DBNames.StorageFieldTypeName, inpStorageName.Text }
				};

					int arrayLength = string.IsNullOrEmpty(inpStorageParentId.Text) ? 1 : 2;
					string [ , ] _updateFields = new string [ arrayLength, 3 ];

					if ( arrayLength == 1 )
					{
						_updateFields [ 0, 0 ] = DBNames.StorageFieldNameName;
						_updateFields [ 0, 1 ] = DBNames.StorageFieldTypeName;
						_updateFields [ 0, 2 ] = inpStorageName.Text;
					}
					else
					{
						_updateFields [ 0, 0 ] = DBNames.StorageFieldNameParentId;
						_updateFields [ 0, 1 ] = DBNames.StorageFieldTypeParentId;
						_updateFields [ 0, 2 ] = inpStorageParentId.Text;

						_updateFields [ 1, 0 ] = DBNames.StorageFieldNameName;
						_updateFields [ 1, 1 ] = DBNames.StorageFieldTypeName;
						_updateFields [ 1, 2 ] = inpStorageName.Text;
					}

					_ = DBCommands.UpdateInTable( DBNames.StorageTable, _updateFields, _whereFields );

					dispStatusLine.Text = $"{( string ) FindResource( "Maintanance.Statusline.NotSaved.DataOf" )} {inpStorageName.Text} {( string ) FindResource( "Maintanance.Statusline.NotSaved.Changed" )}";
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
	node.Content is StorageModel selectedStorage )
		{
			int? _parentId = selectedStorage.StorageId;

			AddNewStorage( _parentId );
		}
	}

	private void AddTask( object sender, RoutedEventArgs e )
	{
		MenuItem? menuItem = sender as MenuItem;

		if ( menuItem?.DataContext is Syncfusion.UI.Xaml.TreeView.Engine.TreeViewNode node &&
				node.Content is StorageModel selectedStorage )
		{
			int? _parentId = selectedStorage.StorageParentId;

			AddNewStorage( _parentId );
		}
	}

	private void ButtonNewStorage( object sender, RoutedEventArgs e )
	{
		if ( int.TryParse( inpStorageParentId.Text, out int parentId ) )
		{
			AddNewStorage( parentId );
		}
		else
		{
			AddNewStorage( null );
		}
	}

	private void ButtonNewSubStorage( object sender, RoutedEventArgs e )
	{
		if ( int.TryParse( inpStorageId.Text, out int parentId ) )
		{
			AddNewStorage( parentId );
		}
		else
		{
			AddNewStorage( null );
		}
	}
	private void AddNewStorage( int? _parentId = 0 )
	{
		inpStorageId.Text = "0";
		inpStorageName.Text = string.Empty;
		inpStorageParentId.Text = _parentId?.ToString();
	}

	private void UpdateDataGrid()
	{
		if ( DataContext is StorageViewModel viewModel )
		{
			StorageViewModel storageViewModel = viewModel;
			object temp = treeView.ItemsSource;
			DBCommands dbCommands = new();

			treeView.ItemsSource = null;
			treeView.ItemsSource = temp;

			ObservableCollection<StorageModel> newStorageList = dbCommands.GetStorageList();
			storageViewModel.Storage.Clear();
			foreach ( StorageModel storage in newStorageList )
			{
				storageViewModel.Storage.Add( storage );
			}
		}
	}
}
