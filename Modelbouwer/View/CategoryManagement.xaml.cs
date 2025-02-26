namespace Modelbouwer.View;

/// <summary>
/// Interaction logic for CatagoryManagement.xaml
/// </summary>
public partial class CategoryManagement : Page
{
	public CategoryManagement()
	{
		InitializeComponent();
		this.Loaded += Data_Loaded;
	}

	private void Data_Loaded( object sender, RoutedEventArgs e )
	{
		if ( DataContext is BrandViewModel viewModel )
		{
			viewModel.Refresh();
		}
	}

	private void ButtonDelete( object sender, RoutedEventArgs e )
	{
		if ( DataContext is CategoryViewModel )
		{
			// Check if a row is selected (check if there is a known Id)
			if ( inpCategoryId != null && !string.IsNullOrWhiteSpace( inpCategoryId.Text ) )
			{
				//CategoryViewModel categoryViewModel = viewModel;

				DBCommands.DeleteRecordTree( DBNames.CategoryTable, DBNames.CategoryFieldNameId, DBNames.CategoryFieldNameParentId, int.Parse( inpCategoryId.Text ) );
				//string [ , ] _whereFields = new string [ 1, 3 ] { { DBNames.CategoryFieldNameId, DBNames.CategoryFieldTypeId, inpCategoryId.Text } };
				//string _result = DBCommands.DeleteRecord( DBNames.CategoryTable, _whereFields );

				UpdateDataGrid();
				dispStatusLine.Text = $"{inpCategoryName.Text}";
			}
		}
	}
	private void ButtonSave( object sender, RoutedEventArgs e )
	{
		if ( DataContext is CategoryViewModel )
		{
			// Save can only be performed if a name is entered
			if ( !string.IsNullOrWhiteSpace( inpCategoryName.Text ) )
			{

				// Check if item is new or has to be updated
				if ( inpCategoryId.Text is "0" or "" )
				{

					string [ , ] _addFields = new string [ 2, 3 ]
				{
					{ DBNames.CategoryFieldNameParentId, DBNames.CategoryFieldTypeParentId, inpCategoryParentId.Text },
					{ DBNames.CategoryFieldNameName, DBNames.CategoryFieldTypeName, inpCategoryName.Text }
				};
					_ = DBCommands.InsertInTable( DBNames.CategoryTable, _addFields );

					UpdateDataGrid();
					dispStatusLine.Text = $"{inpCategoryName.Text} {( string ) FindResource( "Maintanance.Statusline.NotSaved.Unchanged" )}";
				}
				else
				{
					string [ , ] _whereFields = new string [ 1, 3 ] { { DBNames.CategoryFieldNameId, DBNames.CategoryFieldTypeId, inpCategoryId.Text } };

					_ = new string [ 1, 3 ]
				{
					{ DBNames.CategoryFieldNameName, DBNames.CategoryFieldTypeName, inpCategoryName.Text }
				};

					int arrayLength = string.IsNullOrEmpty(inpCategoryParentId.Text) ? 1 : 2;
					string [ , ] _updateFields = new string [ arrayLength, 3 ];

					if ( arrayLength == 1 )
					{
						_updateFields [ 0, 0 ] = DBNames.CategoryFieldNameName;
						_updateFields [ 0, 1 ] = DBNames.CategoryFieldTypeName;
						_updateFields [ 0, 2 ] = inpCategoryName.Text;
					}
					else
					{
						_updateFields [ 0, 0 ] = DBNames.CategoryFieldNameParentId;
						_updateFields [ 0, 1 ] = DBNames.CategoryFieldTypeParentId;
						_updateFields [ 0, 2 ] = inpCategoryParentId.Text;

						_updateFields [ 1, 0 ] = DBNames.CategoryFieldNameName;
						_updateFields [ 1, 1 ] = DBNames.CategoryFieldTypeName;
						_updateFields [ 1, 2 ] = inpCategoryName.Text;
					}

					_ = DBCommands.UpdateInTable( DBNames.CategoryTable, _updateFields, _whereFields );

					dispStatusLine.Text = $"{( string ) FindResource( "Maintanance.Statusline.NotSaved.DataOf" )} {inpCategoryName.Text} {( string ) FindResource( "Maintanance.Statusline.NotSaved.Changed" )}";
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
		CategoryTreeView?.CollapseAll();
	}

	private void ExpandAll( object sender, RoutedEventArgs e )
	{
		CategoryTreeView?.ExpandAll();
	}

	private void AddSubTask( object sender, RoutedEventArgs e )
	{
		MenuItem? menuItem = sender as MenuItem;

		if ( menuItem?.DataContext is Syncfusion.UI.Xaml.TreeView.Engine.TreeViewNode node &&
	node.Content is Modelbouwer.Model.CategoryModel selectedCategory )
		{
			int? _parentId = selectedCategory.CategoryId;

			AddNewCategory( _parentId );
		}
	}

	private void AddTask( object sender, RoutedEventArgs e )
	{
		MenuItem? menuItem = sender as MenuItem;

		if ( menuItem?.DataContext is Syncfusion.UI.Xaml.TreeView.Engine.TreeViewNode node &&
				node.Content is Modelbouwer.Model.CategoryModel selectedCategory )
		{
			int? _parentId = selectedCategory.CategoryParentId;

			AddNewCategory( _parentId );
		}
	}

	private void ButtonNewCategory( object sender, RoutedEventArgs e )
	{
		if ( int.TryParse( inpCategoryParentId.Text, out int parentId ) )
		{
			AddNewCategory( parentId );
		}
		else
		{
			AddNewCategory( null );
		}
	}

	private void ButtonNewSubCategory( object sender, RoutedEventArgs e )
	{
		if ( int.TryParse( inpCategoryId.Text, out int parentId ) )
		{
			AddNewCategory( parentId );
		}
		else
		{
			AddNewCategory( null );
		}
	}
	private void AddNewCategory( int? _parentId = 0 )
	{
		inpCategoryId.Text = "0";
		inpCategoryName.Text = string.Empty;
		inpCategoryParentId.Text = _parentId?.ToString();
	}

	private void UpdateDataGrid()
	{
		if ( DataContext is CategoryViewModel viewModel )
		{
			CategoryViewModel categoryViewModel = viewModel;
			object temp = CategoryTreeView.ItemsSource;
			DBCommands dbCommands = new();

			CategoryTreeView.ItemsSource = null;
			CategoryTreeView.ItemsSource = temp;

			ObservableCollection<CategoryModel> newCategoryList = dbCommands.GetCategoryList();
			categoryViewModel.Category.Clear();
			foreach ( CategoryModel category in newCategoryList )
			{
				categoryViewModel.Category.Add( category );
			}

		}

	}
}
