using System.Windows.Media;

using Syncfusion.UI.Xaml.TreeView.Engine;

using Image = System.Windows.Controls.Image;

namespace Modelbouwer.View;

/// <summary>
/// Interaction logic for ProductManagement.xaml
/// </summary>
public partial class ProductManagement : Page
{
	public ProductManagement()
	{
		InitializeComponent();
	}

	private void ImageAdd( object sender, RoutedEventArgs e )
	{
		//OpenFileDialog ImageDialog = new();
		//ImageDialog.Title = "Selecteer een afbeelding voor dit product";
		//ImageDialog.Filter = "Afbeeldingen (*.jpg;*.jpeg;*.png;*.gif;*.bmp)|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
		//if ( ImageDialog.ShowDialog() == true )
		//{
		//	ProductImage.Source = new BitmapImage( new Uri( ImageDialog.FileName ) );
		//}
	}

	private void ImageDelete( object sender, RoutedEventArgs e )
	{
		if ( DataContext is CombinedProductViewModel viewModel )
		{
			var selectedProduct = viewModel.ProductViewModel.SelectedProduct;

			if ( selectedProduct != null )
			{
				viewModel.ProductViewModel.SelectedProduct.ProductImage = null;
				viewModel.ProductViewModel.SelectedProduct.ProductImageRotationAngle = "0";

				ProductImage.GetBindingExpression( Image.SourceProperty )?.UpdateTarget();
			}
		}
	}

	private void ImageRotate( object sender, RoutedEventArgs e )
	{
		if ( DataContext is CombinedProductViewModel viewModel )
		{
			var selectedProduct = viewModel.ProductViewModel.SelectedProduct;

			if ( selectedProduct != null )
			{
				var _tempValue = int.Parse(viewModel.ProductViewModel.SelectedProduct.ProductImageRotationAngle) + 90;
				if ( _tempValue == 360 )
				{
					_tempValue = 0;
				}

				viewModel.ProductViewModel.SelectedProduct.ProductImageRotationAngle = _tempValue.ToString();

				ProductImage.LayoutTransform = new RotateTransform( _tempValue );
			}
		}
	}

	private void CategoryChange( object sender, RoutedEventArgs e )
	{
		CategoryPopup.IsOpen = !CategoryPopup.IsOpen;
	}

	private void StorageChange( object sender, RoutedEventArgs e )
	{
		StoragePopup.IsOpen = !StoragePopup.IsOpen;
	}

	private void ChangedProduct( object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs e )
	{
		if ( DataContext is CombinedProductViewModel viewModel )
		{
			var selectedProduct = viewModel.ProductViewModel.SelectedProduct;

			#region Select the correct Category
			if ( selectedProduct != null && selectedProduct.ProductCategoryId > 0 )
			{
				var selectedCategory = viewModel.CategoryViewModel.FlatCategory.FirstOrDefault(c => c.CategoryId == selectedProduct.ProductCategoryId);

				if ( selectedCategory != null )
				{
					viewModel.CategoryViewModel.SelectedCategory = selectedCategory;

					ExpandAndSelectCategoryNode( selectedCategory );
					CategoryTreeView.SelectedItem = selectedCategory;
					CategoryTreeView.BringIntoView( selectedCategory );
					CategoryTreeView.Focus();
				}
			}
			#endregion

			#region Select the correct Storage Location
			if ( selectedProduct != null && selectedProduct.ProductStorageId > 0 )
			{
				var selectedStorage = viewModel.StorageViewModel.FlatStorage.FirstOrDefault(c => c.StorageId == selectedProduct.ProductStorageId);

				if ( selectedStorage != null )
				{
					viewModel.StorageViewModel.SelectedStorage = selectedStorage;

					ExpandAndSelectStorageNode( selectedStorage );
					StorageTreeView.SelectedItem = selectedStorage;
					StorageTreeView.BringIntoView( selectedStorage );
					StorageTreeView.Focus();
				}
			}
			#endregion
		}
	}

	private void CategoryTreeViewLoaded( object sender, RoutedEventArgs e )
	{
		if ( DataContext is CombinedProductViewModel viewModel )
		{
			var selectedCategory = viewModel.CategoryViewModel.SelectedCategory;
			if ( selectedCategory != null )
			{
				ExpandAndSelectCategoryNode( selectedCategory );
			}
		}
	}
	private void StorageTreeViewLoaded( object sender, RoutedEventArgs e )
	{
		if ( DataContext is CombinedProductViewModel viewModel )
		{
			var selectedStorage = viewModel.StorageViewModel.SelectedStorage;
			if ( selectedStorage != null )
			{
				ExpandAndSelectStorageNode( selectedStorage );
			}
		}
	}

	#region Open Alternative Popub
	private void CategoryPopupOpened( object sender, EventArgs e )
	{

		if ( DataContext is CombinedProductViewModel viewModel )
		{
			var selectedProduct = viewModel.ProductViewModel.SelectedProduct;

			if ( selectedProduct != null && selectedProduct.ProductCategoryId > 0 )
			{
				var selectedCategory = viewModel.CategoryViewModel.FlatCategory
				.FirstOrDefault(c => c.CategoryId == selectedProduct.ProductCategoryId);

				if ( selectedCategory != null )
				{
					ExpandAndSelectCategoryNode( selectedCategory );
				}
			}
		}
	}

	private void StoragePopupOpened( object sender, EventArgs e )
	{

		if ( DataContext is CombinedProductViewModel viewModel )
		{
			var selectedProduct = viewModel.ProductViewModel.SelectedProduct;

			if ( selectedProduct != null && selectedProduct.ProductStorageId > 0 )
			{
				var selectedStorage = viewModel.StorageViewModel.FlatStorage
			.FirstOrDefault(c => c.StorageId == selectedProduct.ProductStorageId);

				if ( selectedStorage != null )
				{
					ExpandAndSelectStorageNode( selectedStorage );
				}
			}
		}
	}
	#endregion

	#region Expand and select the node from the selected product
	private void ExpandAndSelectCategoryNode( CategoryModel category )
	{
		var node = FindCategoryNode(CategoryTreeView.Nodes, category);

		if ( node != null )
		{
			ExpandParentNodes( node );

			CategoryTreeView.SelectedItem = node.Content;
			CategoryTreeView.Focus();
		}
	}

	private void ExpandAndSelectStorageNode( StorageModel storage )
	{
		var node = FindStorageNode(StorageTreeView.Nodes, storage);

		if ( node != null )
		{
			ExpandParentNodes( node );

			StorageTreeView.SelectedItem = node.Content;
			StorageTreeView.Focus();
		}
	}
	#endregion

	#region TreeViewNode: Method to find the correct node in the tree
	private TreeViewNode FindCategoryNode( TreeViewNodeCollection nodes, CategoryModel category )
	{
		foreach ( var node in nodes )
		{
			if ( node.Content is CategoryModel categoryModel && categoryModel.CategoryId == category.CategoryId )
			{
				return node;
			}

			// Zoek recursief in de subnodes
			var foundNode = FindCategoryNode(node.ChildNodes, category);
			if ( foundNode != null )
			{
				return foundNode;
			}
		}

		return null;
	}

	private TreeViewNode FindStorageNode( TreeViewNodeCollection nodes, StorageModel storage )
	{
		foreach ( var node in nodes )
		{
			if ( node.Content is StorageModel storageModel && storageModel.StorageId == storage.StorageId )
			{
				return node;
			}

			// Search recursifly in the subnodes
			var foundNode = FindStorageNode(node.ChildNodes, storage);
			if ( foundNode != null )
			{
				return foundNode;
			}
		}

		return null;
	}
	#endregion

	#region ExpandParentNode: // Method to open the parrent nodes of the selected node
	private void ExpandParentNodes( TreeViewNode node )
	{
		var parentNode = node.ParentNode;
		while ( parentNode != null )
		{
			parentNode.IsExpanded = true;
			parentNode = parentNode.ParentNode;
		}
	}
	#endregion
}
