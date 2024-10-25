using System.Windows.Documents;

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
		DataContext = new CombinedProductViewModel();
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
			if ( !textRange.IsEmpty )
			{ textRange.Load( stream, System.Windows.DataFormats.Rtf ); }
		}
	}
	#endregion

	#region Selected product changed
	private void ChangedProduct( object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs? e )
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

			#region Select the Barnsd of the selected Product
			if ( selectedProduct != null && selectedProduct.ProductBrandId > 0 )
			{
				var selectedBrand = viewModel.BrandViewModel.Brand.FirstOrDefault(c => c.BrandId == selectedProduct.ProductBrandId);

				if ( selectedBrand != null )
				{
					viewModel.BrandViewModel.SelectedBrand = selectedBrand;
				}
			}
			#endregion

			#region Select the Correct (first) Supplier
			if ( selectedProduct != null )
			{
				// Make sure ProductSupplierViewModel uses the selected product
				viewModel.ProductSupplierViewModel.SelectedProduct = selectedProduct;
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
		//Create a new empty supplier, emtpy memo, empty image
		if ( DataContext is CombinedProductViewModel viewModel )
		{
			// When adding a new product, the product per supplier tab should disabled, until the new product is saved and a productId is available
			SupplierTab.IsEnabled = false;
			InventoryTab.IsEnabled = false;

			// Add new product
			viewModel.ProductViewModel.AddNewItem();

			viewModel.ProductViewModel.SelectedProduct = viewModel.ProductViewModel.Product.Last();

			ProductSupplierDataGrid.Items.Refresh();
		}
	}
	#endregion

	#region Delete Product after checking if it is used in history (if yes, hide product)
	private void ButtonDelete( object sender, RoutedEventArgs e )
	{
		if ( DataContext is CombinedProductViewModel viewModel )
		{
			var selectedProduct = viewModel.ProductViewModel.SelectedProduct;
			var _deleteName = selectedProduct.ProductName;

			var result = DBCommands.DeleteProduct( selectedProduct.ProductId );
			viewModel.ProductViewModel.Product.Remove( selectedProduct );

			if ( result == 1 )
			{ dispStatusLine.Text = $"{_deleteName} {( string ) FindResource( "Edit.Product.Tab.Statusline.Hidden" )}"; }
			else
			{ dispStatusLine.Text = $"{_deleteName} {( string ) FindResource( "Edit.Product.Tab.Statusline.Deleted" )}"; }
		}
	}
	#endregion

	private void ButtonSave( object sender, RoutedEventArgs e )
	{
		var viewModel = DataContext as CombinedProductViewModel;
		var selectedProduct = viewModel.ProductViewModel.SelectedProduct;

		var productImage = selectedProduct.ProductImage;
		var productMemo = selectedProduct.ProductMemo;

		string[ , ] productFields = new string [12,3]
			{
				{DBNames.ProductFieldNameBrandId, DBNames.ProductFieldTypeBrandId, selectedProduct.ProductBrandId.ToString()},
				{DBNames.ProductFieldNameCategoryId, DBNames.ProductFieldTypeCategoryId, selectedProduct.ProductCategoryId.ToString()},
				{DBNames.ProductFieldNameCode, DBNames.ProductFieldTypeCode, selectedProduct.ProductCode.ToString()},
				{DBNames.ProductFieldNameDimensions, DBNames.ProductFieldTypeDimensions, selectedProduct.ProductDimensions.ToString()},
				{DBNames.ProductFieldNameImageRotationAngle, DBNames.ProductFieldTypeImageRotationAngle, selectedProduct.ProductImageRotationAngle.ToString()},
				{DBNames.ProductFieldNameMinimalStock, DBNames.ProductFieldTypeMinimalStock, selectedProduct.ProductMinimalStock.ToString()},
				{DBNames.ProductFieldNameName, DBNames.ProductFieldTypeName, selectedProduct.ProductName.ToString()},
				{DBNames.ProductFieldNameStandardOrderQuantity, DBNames.ProductFieldTypeStandardOrderQuantity, selectedProduct.ProductStandardQuantity.ToString()},
				{DBNames.ProductFieldNamePrice, DBNames.ProductFieldTypePrice, selectedProduct.ProductPrice.ToString()},
				{DBNames.ProductFieldNameProjectCosts, DBNames.ProductFieldTypeProjectCosts, selectedProduct.ProductProjectCosts.ToString()},
				{DBNames.ProductFieldNameStorageId, DBNames.ProductFieldTypeStorageId, selectedProduct.ProductStorageId.ToString()},
				{DBNames.ProductFieldNameUnitId, DBNames.ProductFieldTypeUnitId, selectedProduct.ProductUnitId.ToString()}
			};

		if ( selectedProduct.ProductId == 0 )
		{
			// Save a new product including the image
			DBCommands.InsertInTable( DBNames.ProductTable, productFields, selectedProduct.ProductImage, DBNames.ProductFieldNameImage );

			// Get the Id of the just saved product and use it to create the where field for the update memo action
			var productId = DBCommands.GetLatestIdFromTable( DBNames.ProductTable );
			string[,] whereFields = new string[1, 3]
			{
				{ DBNames.ProductFieldNameId, DBNames.ProductFieldTypeId,  productId},
			};

			//Save the memo in de product table
			DBCommands.UpdateMemoFieldInTable( DBNames.ProductTable, whereFields, DBNames.ProductFieldNameMemo, productMemo );

			//Save Image
			DBCommands.UpdateImageInTable( DBNames.ProductTable, whereFields, productImage, DBNames.ProductFieldNameImage );

			//Set the pnew productId for the selected Product
			selectedProduct.ProductId = int.Parse( productId );

			// Product is no longer new, so reset the IsNew flag
			viewModel.ProductViewModel.IsAddingNew = false;
		}
		else
		{
			// Update an excisting product
			string[,] whereFields = new string[1, 3]
			{
				{ DBNames.ProductFieldNameId, DBNames.ProductFieldTypeId, selectedProduct.ProductId.ToString() },
			};

			DBCommands.UpdateInTable( DBNames.ProductTable, productFields, whereFields );

			//Update the memo in de product table
			DBCommands.UpdateMemoFieldInTable( DBNames.ProductTable, whereFields, DBNames.ProductFieldNameMemo, productMemo );

			//Update Image
			DBCommands.UpdateImageInTable( DBNames.ProductTable, whereFields, productImage, DBNames.ProductFieldNameImage );

		}

		//TODO Update the datgrid and reselect the saved product
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

	#region Save product supplier
	private void SupplierToolbarButtonSave( object sender, RoutedEventArgs e )
	{
		var viewModel = DataContext as CombinedProductViewModel;
		var selectedSupplier = viewModel.ProductSupplierViewModel.SelectedSupplier;

		var selectedProductId = viewModel.ProductViewModel.SelectedProduct.ProductId.ToString();
		var selectedId = viewModel.ProductSupplierViewModel.SelectedSupplier.ProductSupplierId.ToString();
		var selectedSupplierId = ((int)(SupplierComboBox.SelectedValue ?? 0)).ToString();
		var selectedSupplierName = ((SupplierModel)SupplierComboBox.SelectedItem).Name.ToString();
		var currencyId = viewModel.ProductSupplierViewModel.SelectedSupplier.ProductSupplierCurrencyId.ToString();

		var isNew = viewModel.ProductSupplierViewModel.IsAddingNew;

		if ( isNew )
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
				string[,] _addFields = new string[8, 3]
				{
					{ DBNames.ProductSupplierFieldNameSupplierId, DBNames.ProductSupplierFieldTypeSupplierId, selectedSupplierId },
					{ DBNames.ProductSupplierFieldNameProductId, DBNames.ProductSupplierFieldTypeProductId,selectedProductId },
					{ DBNames.ProductSupplierFieldNameCurrencyId, DBNames.ProductSupplierFieldTypeCurrencyId,currencyId },
					{ DBNames.ProductSupplierFieldNameProductNumber, DBNames.ProductSupplierFieldTypeProductNumber, SupplierProductNumber.Text },
					{ DBNames.ProductSupplierFieldNameProductName, DBNames.ProductSupplierFieldTypeProductName, SupplierProductName.Text },
					{ DBNames.ProductSupplierFieldNamePrice, DBNames.ProductSupplierFieldTypePrice, double.Parse(SupplierProductPrice.Text).ToString() },
					{ DBNames.ProductSupplierFieldNameProductUrl, DBNames.ProductSupplierFieldTypeProductUrl, SupplierProductUrl.Text },
					{ DBNames.ProductSupplierFieldNameDefaultSupplier, DBNames.ProductSupplierFieldTypeDefaultSupplier, SupplierDefault.IsChecked == true ? "*" : "" }
				};
				_ = DBCommands.InsertInTable( DBNames.ProductSupplierTable, _addFields );

				viewModel.ProductSupplierViewModel.IsAddingNew = false;

				// Refresh the ProductSupplier-collection

				viewModel.ProductSupplierViewModel.FilterSuppliersByProductId( int.Parse( selectedProductId ) );
				_ = viewModel.SupplierViewModel.SelectedSupplier;


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

			string[,] _updateFields = new string[7, 3]
			{
					{ DBNames.ProductSupplierFieldNameSupplierId, DBNames.ProductSupplierFieldTypeSupplierId, selectedSupplierId },
					{ DBNames.ProductSupplierFieldNameCurrencyId, DBNames.ProductSupplierFieldTypeCurrencyId,currencyId },
					{ DBNames.ProductSupplierFieldNameProductNumber, DBNames.ProductSupplierFieldTypeProductNumber, SupplierProductNumber.Text },
					{ DBNames.ProductSupplierFieldNameProductName, DBNames.ProductSupplierFieldTypeProductName, SupplierProductName.Text },
					{ DBNames.ProductSupplierFieldNamePrice, DBNames.ProductSupplierFieldTypePrice, double.Parse(SupplierProductPrice.Text).ToString() },
					{ DBNames.ProductSupplierFieldNameProductUrl, DBNames.ProductSupplierFieldTypeProductUrl, SupplierProductUrl.Text },
					{ DBNames.ProductSupplierFieldNameDefaultSupplier, DBNames.ProductSupplierFieldTypeDefaultSupplier, SupplierDefault.IsChecked == true ? "*" : "" }
			};

			_ = DBCommands.UpdateInTable( DBNames.SupplierContactTable, _updateFields, _whereFields );


			viewModel.ProductSupplierViewModel.IsAddingNew = false;

			viewModel.ProductSupplierViewModel.FilterSuppliersByProductId( int.Parse( selectedProductId ) );
			_ = viewModel.SupplierViewModel.SelectedSupplier;

			dispStatusLine.Text = $"{( string ) FindResource( "Maintanance.Statusline.NotSaved.DataOf" )} {SupplierProductNumber.Text} {( string ) FindResource( "Maintanance.Statusline.NotSaved.Changed" )}";

			// If the DefaultSupplier checkbox is checked, then the DefautltSupplier in the database table should be updated
			if ( SupplierDefault.IsChecked == true )
			{
				DBCommands.SetDefaultSupplier( selectedProductId, selectedSupplierId, "Set" );
			}
			else { DBCommands.SetDefaultSupplier( selectedProductId, selectedSupplierId, "Reset" ); }
		}
	}
	#endregion

	#region Selected supplier changed
	private void SupplierChanged( object sender, SelectionChangedEventArgs e )
	{
		//if ( DataContext is CombinedProductViewModel viewModel )
		//{
		//	var selectedSupplier = viewModel.ProductSupplierViewModel.SelectedSupplier;
		//	if ( selectedSupplier != null )
		//	{
		//		SupplierComboBox.SelectedValue = selectedSupplier.ProductSupplierSupplierId;
		//	}
		//	else
		//	{
		//		//SupplierComboBox.SelectedValue = null;
		//	}
		//}
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
