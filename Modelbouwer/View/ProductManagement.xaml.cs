using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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

	#region Add/Replace product image
	private void ImageAdd( object sender, RoutedEventArgs e )
	{
		OpenFileDialog ImageDialog = new();
		ImageDialog.Title = "Selecteer een afbeelding voor dit product";
		ImageDialog.Filter = "Afbeeldingen (*.jpg;*.jpeg;*.png;*.gif;*.bmp)|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
		if ( ImageDialog.ShowDialog() == DialogResult.OK )
		{
			ProductImage.Source = new BitmapImage( new Uri( ImageDialog.FileName ) );
		}
	}
	#endregion

	#region Delete Product Image
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
	#endregion

	#region Rotate product image 90degrees
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
	#endregion

	#region Open Category treeview popup when Category button is clicked
	private void CategoryChange( object sender, RoutedEventArgs e )
	{
		CategoryPopup.IsOpen = !CategoryPopup.IsOpen;
	}
	#endregion

	#region Open Storage treeview popup when Storage button is clicked
	private void StorageChange( object sender, RoutedEventArgs e )
	{
		StoragePopup.IsOpen = !StoragePopup.IsOpen;
	}
	#endregion

	#region Load the memo field
	private void SetRtfContent( string rtfContent )
	{
		using ( var stream = new MemoryStream( Encoding.UTF8.GetBytes( rtfContent ) ) )
		{
			TextRange textRange = new TextRange(ProductMemo.Document.ContentStart, ProductMemo.Document.ContentEnd);
			textRange.Load( stream, System.Windows.DataFormats.Rtf );
		}
	}
	#endregion

	#region Selected product changed
	private void ChangedProduct( object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs e )
	{
		if ( DataContext is CombinedProductViewModel viewModel )
		{
			var selectedProduct = viewModel.ProductViewModel.SelectedProduct;

			if ( selectedProduct != null )
			{
				SetRtfContent( selectedProduct.ProductMemo );
			}

			var Supliers = viewModel.ProductSupplierViewModel.ProductSupplier;

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

			#region Select the Correct (first) Supplier
			if ( dataGrid.SelectedItem is ProductModel SelectedProduct )
			{
				viewModel.ProductSupplierViewModel.FilterSuppliersByProductId( SelectedProduct.ProductId );
			}
			#endregion
		}
	}
	#endregion

	#region Product DataGrid Loaded
	private void ProductDataGridLoaded( object sender, RoutedEventArgs e )
	{
		var dataSource = dataGrid.ItemsSource as ObservableCollection<ProductModel>;
		if ( dataGrid.SelectedIndex == 0 && dataSource.Count > 0 )
		{
			// Forceer de selectie om de SelectionChanged logica aan te roepen
			ChangedProduct( dataGrid, null );
		}
	}
	#endregion

	#region Load Category treeview in popup
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
	#endregion

	#region Load Storage treeview in popup
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
	#endregion

	#region Open Alternative Popup
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

	#region Create new Product
	private void ButtonNew( object sender, RoutedEventArgs e )
	{

	}
	#endregion

	private void ButtonDelete( object sender, RoutedEventArgs e )
	{

	}

	private void ButtonSave( object sender, RoutedEventArgs e )
	{

	}

	#region Add new supplier for this product
	private void supplierToolbarButtonNew( object sender, RoutedEventArgs e )
	{
		if ( DataContext is CombinedProductViewModel viewModel )
		{
			SupplierComboBox.SelectedIndex = 0;
			SupplierProductNumber.Text = "";
			SupplierProductName.Text = "";
			SupplierProductPrice.Text = "0,00";
			SupplierProductUrl.Text = "";
			SupplierDefault.IsChecked = false;

			var selectedProductId = viewModel.ProductViewModel.SelectedProduct.ProductId;

			// Add a new contact
			viewModel.ProductSupplierViewModel.AddNewItem( selectedProductId.ToString() );

			// Ensure FilteredSupplierss is updated
			viewModel.ProductSupplierViewModel.FilterSuppliersByProductId( selectedProductId );


			// Select the newly added contact if it exists
			if ( viewModel.ProductSupplierViewModel.FilteredSuppliers.Any() )
			{
				viewModel.ProductSupplierViewModel.SelectedSupplier = viewModel.ProductSupplierViewModel.FilteredSuppliers.Last();
			}

			// Refresh the DataGrid to show the new supplier
			ProductSupplierDataGrid.Items.Refresh();
		}
	}
	#endregion

	#region Delete the selected Supplier from the ProductSuppiler table
	private void SupplierToolbarButtonDelete( object sender, RoutedEventArgs e )
	{
		if ( DataContext is CombinedProductViewModel viewModel )
		{
			var selectedSupplier = viewModel.ProductSupplierViewModel.SelectedSupplier;

			//Check if there is a supplier selected
			if ( selectedSupplier != null )
			{
				var selectedSupplierId = selectedSupplier.ProductSupplierSupplierId;
				var selectedProductId = viewModel.ProductViewModel.SelectedProduct.ProductId;

				string [ , ] _whereFields = new string [ 2, 3 ]
				{
					{ DBNames.ProductSupplierFieldNameProductId, DBNames.ProductSupplierFieldTypeProductId, selectedProductId.ToString() },
					{ DBNames.ProductSupplierFieldNameSupplierId, DBNames.ProductSupplierFieldTypeSupplierId, selectedSupplierId.ToString() }
				};

				string _result = DBCommands.DeleteRecord( DBNames.ProductSupplierTable, _whereFields );
				viewModel.ProductSupplierViewModel.ProductSupplier.Remove( selectedSupplier );

				viewModel.ProductSupplierViewModel.FilterSuppliersByProductId( selectedProductId );

				dispStatusLine.Text = $"{_result}";
			};
		}
	}
	#endregion

	private void SupplierToolbarButtonSave( object sender, RoutedEventArgs e )
	{
		var viewModel = DataContext as CombinedProductViewModel;
		var selectedSupplier = viewModel.ProductSupplierViewModel.SelectedSupplier;

		var selectedProductId = viewModel.ProductViewModel.SelectedProduct.ProductId.ToString();
		var selectedId = viewModel.ProductSupplierViewModel.SelectedSupplier.ProductSupplierId.ToString();
		var selectedSupplierId = SupplierComboBox.SelectedValue.ToString();

		if ( selectedId == "0" || selectedId == "" )
		{
			// New item, so add to the table
			// First check if item already excists
			string[,] _whereFields = new string[2, 3]
			{
				{ DBNames.ProductSupplierFieldNameId, DBNames.ProductSupplierFieldTypeId, selectedSupplierId },
				{ DBNames.ProductSupplierFieldNameProductId, DBNames.ProductSupplierFieldTypeProductId, selectedProductId }
			};

			int _checkPresence = DBCommands.CheckForRecords(DBNames.ProductSupplierTable, _whereFields);

			if ( _checkPresence == 0 )
			{
				string[,] _addFields = new string[7, 3]
				{
					{ DBNames.ProductSupplierFieldNameSupplierId, DBNames.ProductSupplierFieldTypeSupplierId, selectedSupplierId },
					{ DBNames.ProductSupplierFieldNameProductId, DBNames.ProductSupplierFieldTypeProductId,selectedProductId },
					{ DBNames.ProductSupplierFieldNameProductNumber, DBNames.ProductSupplierFieldTypeProductNumber, SupplierProductNumber.Text },
					{ DBNames.ProductSupplierFieldNameProductName, DBNames.ProductSupplierFieldTypeProductName, SupplierProductName.Text },
					{ DBNames.ProductSupplierFieldNamePrice, DBNames.ProductSupplierFieldTypePrice, SupplierProductPrice.Text },
					{ DBNames.ProductSupplierFieldNameProductUrl, DBNames.ProductSupplierFieldTypeProductUrl, SupplierProductUrl.Text },
					{ DBNames.ProductSupplierFieldNameDefaultSupplier, DBNames.ProductSupplierFieldTypeDefaultSupplier, SupplierDefault.IsChecked == true ? "1" : "0" }
				};
				_ = DBCommands.InsertInTable( DBNames.ProductSupplierTable, _addFields );

				// Reefresh the ProductSupplier-collection

				viewModel.ProductSupplierViewModel.FilterSuppliersByProductId( int.Parse( selectedProductId ) );
				var SelectedSupplier = viewModel.SupplierViewModel.SelectedSupplier;


				dispStatusLine.Text = $"{SupplierProductNumber.Text} {( string ) FindResource( "Maintanance.Statusline.NotSaved.Added" )}";
			}
			else
			{
				dispStatusLine.Text = $"{( string ) FindResource( "Maintanance.Statusline.NotSaved.AlreadyExcists" )}";
			}
		}
		else
		{
			// An excisting item has been changed, save changes
			string[,] _whereFields = new string [ 1, 3 ]
			{
				{ DBNames.ProductSupplierFieldNameProductId, DBNames.ProductSupplierFieldTypeProductId, selectedProductId }
			};

			string[,] _updateFields = new string[6, 3]
			{
					{ DBNames.ProductSupplierFieldNameSupplierId, DBNames.ProductSupplierFieldTypeSupplierId, selectedSupplierId },
					{ DBNames.ProductSupplierFieldNameProductNumber, DBNames.ProductSupplierFieldTypeProductNumber, SupplierProductNumber.Text },
					{ DBNames.ProductSupplierFieldNameProductName, DBNames.ProductSupplierFieldTypeProductName, SupplierProductName.Text },
					{ DBNames.ProductSupplierFieldNamePrice, DBNames.ProductSupplierFieldTypePrice, SupplierProductPrice.Text },
					{ DBNames.ProductSupplierFieldNameProductUrl, DBNames.ProductSupplierFieldTypeProductUrl, SupplierProductUrl.Text },
					{ DBNames.ProductSupplierFieldNameDefaultSupplier, DBNames.ProductSupplierFieldTypeDefaultSupplier, SupplierDefault.IsChecked == true ? "1" : "0" }
			};

			_ = DBCommands.UpdateInTable( DBNames.SupplierContactTable, _updateFields, _whereFields );

			viewModel.ProductSupplierViewModel.FilterSuppliersByProductId( int.Parse( selectedProductId ) );
			var SelectedSupplier = viewModel.SupplierViewModel.SelectedSupplier;

			dispStatusLine.Text = $"{( string ) FindResource( "Maintanance.Statusline.NotSaved.DataOf" )} {SupplierProductNumber.Text} {( string ) FindResource( "Maintanance.Statusline.NotSaved.Changed" )}";
		}
	}

	#region Selected supplier changed
	private void SupplierChanged( object sender, SelectionChangedEventArgs e )
	{
		if ( DataContext is CombinedProductViewModel viewModel )
		{
			var selectedSupplier = viewModel.ProductSupplierViewModel.SelectedSupplier;
			if ( selectedSupplier != null )
			{
				var selectedSupplierId = selectedSupplier.ProductSupplierSupplierId;

				SupplierComboBox.SelectedValue = selectedSupplierId;
			}
			else { SupplierComboBox.SelectedValue = null; }
		}
	}
	#endregion

	#region Open Product webpage
	private void ButtonWeb( object sender, RoutedEventArgs e )
	{
		if ( SupplierProductUrl.Text != "" )
		{
			ProcessStartInfo? browserwindow = new()
			{
				UseShellExecute = true,
				FileName = SupplierProductUrl.Text
			};
			Process.Start( browserwindow );
		}
	}
	#endregion

}
