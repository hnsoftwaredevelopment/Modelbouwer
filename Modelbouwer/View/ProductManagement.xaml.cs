using DataFormats = System.Windows.DataFormats;

namespace Modelbouwer.View;

public partial class ProductManagement : Page
{
	private bool _isUpdatingFields;

	public ProductManagement()
	{
		InitializeComponent();
		this.Loaded += Data_Loaded;
	}

	private void Data_Loaded( object sender, RoutedEventArgs e )
	{
		if ( DataContext is CombinedProductViewModel viewModel )
		{
			viewModel.RefreshAll();
		}
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
			ProductModel? selectedProduct = viewModel.ProductViewModel.SelectedProduct;

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
			ProductModel? selectedProduct = viewModel.ProductViewModel.SelectedProduct;

			if ( selectedProduct != null )
			{
				int _tempValue = int.Parse(viewModel.ProductViewModel.SelectedProduct.ProductImageRotationAngle) + 90;
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
		if ( !string.IsNullOrEmpty( rtfContent ) )
		{
			using MemoryStream stream = new(Encoding.UTF8.GetBytes(rtfContent));
			TextRange textRange = new(ProductMemo.Document.ContentStart, ProductMemo.Document.ContentEnd);
			textRange.Load( stream, DataFormats.Rtf );
		}
		else
		{
			ProductMemo.Document.Blocks.Clear();
		}
	}
	#endregion

	#region Selected product changed
	private void ChangedProduct( object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs? e )
	{
		if ( DataContext is CombinedProductViewModel viewModel )
		{
			ProductModel? selectedProduct = viewModel.ProductViewModel.SelectedProduct;

			//Make sure the SafeChanges warning will not be dislayed when changing products
			_isUpdatingFields = true;


			if ( selectedProduct != null )
			{
				SetRtfContent( selectedProduct.ProductMemo );
			}

			ObservableCollection<ProductSupplierModel> Supliers = viewModel.ProductSupplierViewModel.ProductSupplier;

			#region Select the correct Category
			if ( selectedProduct != null && selectedProduct.ProductCategoryId > 0 )
			{
				CategoryModel? selectedCategory = viewModel.CategoryViewModel.FlatCategory.FirstOrDefault( c => c.CategoryId == selectedProduct.ProductCategoryId );

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
				StorageModel? selectedStorage = viewModel.StorageViewModel.FlatStorage.FirstOrDefault( c => c.StorageId == selectedProduct.ProductStorageId );

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

			#region Select the Brand of the selected Product
			if ( selectedProduct != null && selectedProduct.ProductBrandId > 0 )
			{
				BrandModel? selectedBrand = viewModel.BrandViewModel.Brand.FirstOrDefault( c => c.BrandId == selectedProduct.ProductBrandId );

				if ( selectedBrand != null )
				{
					viewModel.BrandViewModel.SelectedBrand = selectedBrand;
				}
			}
			#endregion

			#region Select the Unit of the selected Product
			if ( selectedProduct != null && selectedProduct.ProductUnitId > 0 )
			{
				UnitModel? selectedUnit = viewModel.UnitViewModel.Unit.FirstOrDefault( c => c.UnitId == selectedProduct.ProductUnitId );

				if ( selectedUnit != null )
				{
					viewModel.UnitViewModel.SelectedUnit = selectedUnit;
				}
			}
			#endregion

			#region Select the Correct (first) Supplier
			if ( selectedProduct != null )
			{
				// Make sure ProductSupplierViewModel uses the selected product
				viewModel.ProductSupplierViewModel.SelectedProduct = selectedProduct;
				viewModel.ProductSupplierViewModel.FilterSuppliersByProductId( selectedProduct.ProductId );
			}
			#endregion

			#region Set the correct label for the Price per [quantity] [unit]
			viewModel.ProductViewModel.ProductOrderQuantity = ProductOrderQuantity.Text ?? "1";
			viewModel.ProductViewModel.ProductPackagingUnit = ProductPackagingUnit.Text ?? "stuk";
			#endregion

			//From here changes will activate the SafeChanges Warning 
			_isUpdatingFields = false;
		}
	}
	#endregion

	#region Product DataGrid Loaded
	private void ProductDataGridLoaded( object sender, RoutedEventArgs e )
	{
		ObservableCollection<ProductModel>? dataSource = dataGrid.ItemsSource as ObservableCollection<ProductModel>;
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
			CategoryModel? selectedCategory = viewModel.CategoryViewModel.SelectedCategory;
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
			StorageModel? selectedStorage = viewModel.StorageViewModel.SelectedStorage;
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
			ProductModel? selectedProduct = viewModel.ProductViewModel.SelectedProduct;

			if ( selectedProduct != null && selectedProduct.ProductCategoryId > 0 )
			{
				CategoryModel? selectedCategory = viewModel.CategoryViewModel.FlatCategory
				.FirstOrDefault( c => c.CategoryId == selectedProduct.ProductCategoryId );

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
			ProductModel? selectedProduct = viewModel.ProductViewModel.SelectedProduct;

			if ( selectedProduct != null && selectedProduct.ProductStorageId > 0 )
			{
				StorageModel? selectedStorage = viewModel.StorageViewModel.FlatStorage
			.FirstOrDefault( c => c.StorageId == selectedProduct.ProductStorageId );

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
		TreeViewNode node = FindCategoryNode(CategoryTreeView.Nodes, category);

		if ( node != null )
		{
			ExpandParentNodes( node );

			CategoryTreeView.SelectedItem = node.Content;
			CategoryTreeView.Focus();
		}
	}

	private void ExpandAndSelectStorageNode( StorageModel storage )
	{
		TreeViewNode node = FindStorageNode(StorageTreeView.Nodes, storage);

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
		foreach ( TreeViewNode? node in nodes )
		{
			if ( node.Content is CategoryModel categoryModel && categoryModel.CategoryId == category.CategoryId )
			{
				return node;
			}

			// Zoek recursief in de subnodes
			TreeViewNode foundNode = FindCategoryNode(node.ChildNodes, category);
			if ( foundNode != null )
			{
				return foundNode;
			}
		}

		return null;
	}

	private TreeViewNode FindStorageNode( TreeViewNodeCollection nodes, StorageModel storage )
	{
		foreach ( TreeViewNode? node in nodes )
		{
			if ( node.Content is StorageModel storageModel && storageModel.StorageId == storage.StorageId )
			{
				return node;
			}

			// Search recursifly in the subnodes
			TreeViewNode foundNode = FindStorageNode(node.ChildNodes, storage);
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
		TreeViewNode parentNode = node.ParentNode;
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
			ProductModel? selectedProduct = viewModel.ProductViewModel.SelectedProduct;
			string? _deleteName = selectedProduct.ProductName;

			int result = DBCommands.DeleteProduct( selectedProduct.ProductId );
			viewModel.ProductViewModel.Product.Remove( selectedProduct );

			if ( result == 1 )
			{ dispStatusLine.Text = $"{_deleteName} {( string ) FindResource( "Edit.Product.Tab.Statusline.Hidden" )}"; }
			else
			{ dispStatusLine.Text = $"{_deleteName} {( string ) FindResource( "Edit.Product.Tab.Statusline.Deleted" )}"; }
		}
	}
	#endregion

	#region Product Save
	private void ButtonSave( object sender, RoutedEventArgs e )
	{
		CombinedProductViewModel? viewModel = DataContext as CombinedProductViewModel;
		ProductModel? selectedProduct = viewModel.ProductViewModel.SelectedProduct;

		byte [ ]? productImage = selectedProduct.ProductImage;
		string ProjCost = ProductProjectCosts.IsChecked == true ? "1" : "0";

		ProductSupplierModel? selectedSupplier = viewModel.ProductSupplierViewModel.SelectedSupplier;

		string[ , ] productFields = new string [12,3]
			{
				{DBNames.ProductFieldNameBrandId, DBNames.ProductFieldTypeBrandId, viewModel.BrandViewModel.SelectedBrandId.ToString()},
				{DBNames.ProductFieldNameCategoryId, DBNames.ProductFieldTypeCategoryId, viewModel.CategoryViewModel.SelectedCategory.CategoryId.ToString()},
				{DBNames.ProductFieldNameCode, DBNames.ProductFieldTypeCode, ProductCode.Text},
				{DBNames.ProductFieldNameDimensions, DBNames.ProductFieldTypeDimensions, ProductDimensions.Text},
				{DBNames.ProductFieldNameImageRotationAngle, DBNames.ProductFieldTypeImageRotationAngle, ProductImageRotationAngle.Text},
				{DBNames.ProductFieldNameMinimalStock, DBNames.ProductFieldTypeMinimalStock, ProductMinStock.Text},
				{DBNames.ProductFieldNameName, DBNames.ProductFieldTypeName, ProductName.Text},
				{DBNames.ProductFieldNameStandardOrderQuantity, DBNames.ProductFieldTypeStandardOrderQuantity, ProductOrderQuantity.Text},
				{DBNames.ProductFieldNamePrice, DBNames.ProductFieldTypePrice, Double.Parse(ProductPrice.Text).ToString()},
				{DBNames.ProductFieldNameProjectCosts, DBNames.ProductFieldTypeProjectCosts, ProjCost},
				{DBNames.ProductFieldNameStorageId, DBNames.ProductFieldTypeStorageId, viewModel.StorageViewModel.SelectedStorage.StorageId.ToString()},
				{DBNames.ProductFieldNameUnitId, DBNames.ProductFieldTypeUnitId, ((Modelbouwer.Model.UnitModel)ProductPackagingUnit.SelectedValue).UnitId.ToString()}
			};

		if ( selectedProduct.ProductId == 0 )
		{
			// Save a new product including the image
			DBCommands.InsertInTable( DBNames.ProductTable, productFields, selectedProduct.ProductImage, DBNames.ProductFieldNameImage );

			// Get the Id of the just saved product and use it to create the where field for the update memo action
			string productId = DBCommands.GetLatestIdFromTable( DBNames.ProductTable );
			string[,] whereFields = new string[1, 3]
			{
				{ DBNames.ProductFieldNameId, DBNames.ProductFieldTypeId,  productId},
			};

			//Save the memo in de product table
			DBCommands.UpdateMemoFieldInTable( DBNames.ProductTable, whereFields, DBNames.ProductFieldNameMemo, GeneralHelper.GetRichTextFromFlowDocument( ProductMemo.Document ) );

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
			DBCommands.UpdateMemoFieldInTable( DBNames.ProductTable, whereFields, DBNames.ProductFieldNameMemo, GeneralHelper.GetRichTextFromFlowDocument( ProductMemo.Document ) );

			//Update Image
			DBCommands.UpdateImageInTable( DBNames.ProductTable, whereFields, productImage, DBNames.ProductFieldNameImage );

		}

		//Update the datgrid and reselect the saved product
		viewModel.ProductViewModel.RefreshProductList( selectedProduct.ProductId );
		viewModel.ProductSupplierViewModel.RefreshProductSupplierList( selectedProduct.ProductId, selectedSupplier?.ProductSupplierId );

		// When saving a product, the product per supplier tab should be enabled
		SupplierTab.IsEnabled = true;
		InventoryTab.IsEnabled = true;

	}
	#endregion

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

			int selectedProductId = viewModel.ProductViewModel.SelectedProduct.ProductId;

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
			ProductSupplierModel? selectedSupplier = viewModel.ProductSupplierViewModel.SelectedSupplier;

			//Check if there is a supplier selected
			if ( selectedSupplier != null )
			{
				int selectedSupplierId = selectedSupplier.ProductSupplierSupplierId;
				int selectedProductId = viewModel.ProductViewModel.SelectedProduct.ProductId;

				string [ , ] _whereFields = new string [ 2, 3 ]
				{
					{ DBNames.ProductSupplierFieldNameProductId, DBNames.ProductSupplierFieldTypeProductId, selectedProductId.ToString() },
					{ DBNames.ProductSupplierFieldNameSupplierId, DBNames.ProductSupplierFieldTypeSupplierId, selectedSupplierId.ToString() }
				};

				string _result = DBCommands.DeleteRecord( DBNames.ProductSupplierTable, _whereFields );
				viewModel.ProductSupplierViewModel.ProductSupplier.Remove( selectedSupplier );

				viewModel.ProductSupplierViewModel.FilterSuppliersByProductId( selectedProductId );

				dispStatusLine.Text = $"{_result}";
			}
			;
		}
	}
	#endregion

	#region Save product supplier
	private void SupplierToolbarButtonSave( object sender, RoutedEventArgs e )
	{
		CombinedProductViewModel? viewModel = DataContext as CombinedProductViewModel;
		ProductSupplierModel? selectedSupplier = viewModel.ProductSupplierViewModel.SelectedSupplier;
		int selectedProductId = viewModel.ProductViewModel.SelectedProduct.ProductId;
		//var selectedProductId = viewModel.ProductViewModel.selectedProduct.ProductId.ToString();
		string selectedId = viewModel.ProductSupplierViewModel.SelectedSupplier.ProductSupplierId.ToString();
		string selectedSupplierId = ((int)(SupplierComboBox.SelectedValue ?? 0)).ToString();
		string selectedSupplierName = ((SupplierModel)SupplierComboBox.SelectedItem).Name.ToString();
		string currencyId = viewModel.ProductSupplierViewModel.SelectedSupplier.ProductSupplierCurrencyId.ToString();

		bool isNew = viewModel.ProductSupplierViewModel.IsAddingNew;

		if ( isNew )
		{
			// New item, so add to the table
			// First check if item already excists
			string[,] _whereFields = new string[2, 3]
			{
				{ DBNames.ProductSupplierFieldNameId, DBNames.ProductSupplierFieldTypeId, selectedSupplierId },
				{ DBNames.ProductSupplierFieldNameProductId, DBNames.ProductSupplierFieldTypeProductId, selectedProductId.ToString() }
			};

			int _checkPresence = DBCommands.CheckForRecords(DBNames.ProductSupplierTable, _whereFields);

			if ( _checkPresence == 0 )
			{
				string[,] _addFields = new string[8, 3]
				{
					{ DBNames.ProductSupplierFieldNameSupplierId, DBNames.ProductSupplierFieldTypeSupplierId, selectedSupplierId },
					{ DBNames.ProductSupplierFieldNameProductId, DBNames.ProductSupplierFieldTypeProductId,selectedProductId.ToString() },
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

				viewModel.ProductSupplierViewModel.FilterSuppliersByProductId( int.Parse( selectedProductId.ToString() ) );
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
				{ DBNames.ProductSupplierFieldNameProductId, DBNames.ProductSupplierFieldTypeProductId, selectedProductId.ToString() }
			};

			string[,] _updateFields = new string[7, 3]
			{
					{ DBNames.ProductSupplierFieldNameSupplierId, DBNames.ProductSupplierFieldTypeSupplierId, selectedSupplierId.ToString() },
					{ DBNames.ProductSupplierFieldNameCurrencyId, DBNames.ProductSupplierFieldTypeCurrencyId,currencyId },
					{ DBNames.ProductSupplierFieldNameProductNumber, DBNames.ProductSupplierFieldTypeProductNumber, SupplierProductNumber.Text },
					{ DBNames.ProductSupplierFieldNameProductName, DBNames.ProductSupplierFieldTypeProductName, SupplierProductName.Text },
					{ DBNames.ProductSupplierFieldNamePrice, DBNames.ProductSupplierFieldTypePrice, double.Parse(SupplierProductPrice.Text).ToString() },
					{ DBNames.ProductSupplierFieldNameProductUrl, DBNames.ProductSupplierFieldTypeProductUrl, SupplierProductUrl.Text },
					{ DBNames.ProductSupplierFieldNameDefaultSupplier, DBNames.ProductSupplierFieldTypeDefaultSupplier, SupplierDefault.IsChecked == true ? "*" : "" }
			};

			_ = DBCommands.UpdateInTable( DBNames.ProductSupplierTable, _updateFields, _whereFields );


			viewModel.ProductSupplierViewModel.IsAddingNew = false;

			viewModel.ProductSupplierViewModel.FilterSuppliersByProductId( int.Parse( selectedProductId.ToString() ) );
			_ = viewModel.SupplierViewModel.SelectedSupplier;

			dispStatusLine.Text = $"{( string ) FindResource( "Maintanance.Statusline.NotSaved.DataOf" )} {SupplierProductNumber.Text} {( string ) FindResource( "Maintanance.Statusline.NotSaved.Changed" )}";

			// If the DefaultSupplier checkbox is checked, then the DefautltSupplier in the database table should be updated
			if ( SupplierDefault.IsChecked == true )
			{
				DBCommands.SetDefaultSupplier( selectedProductId.ToString(), selectedSupplierId, "Set" );
			}
			else { DBCommands.SetDefaultSupplier( selectedProductId.ToString(), selectedSupplierId, "Reset" ); }
		}

		// Refresh the DataGrid to show the updated supplier
		viewModel.ProductSupplierViewModel.RefreshProductSupplierList(
		selectedProductId,
		selectedSupplier?.ProductSupplierId
	);
	}
	#endregion

	#region Selected supplier changed
	private void SupplierChanged( object sender, SelectionChangedEventArgs e )
	{
		if ( !_isUpdatingFields )
		{
			CombinedProductViewModel? _viewModel = DataContext as CombinedProductViewModel;
			if ( _viewModel != null )
			{
				_viewModel.ProductSupplierViewModel.HasUnsavedChanges = false;
			}
		}
		if ( DataContext is CombinedProductViewModel viewModel )
		{
			ProductSupplierModel? selectedSupplier = viewModel.ProductSupplierViewModel.SelectedSupplier;
			int? selectedSupplierId = ( int? ) SupplierComboBox.SelectedValue;

			if ( selectedSupplier != null && selectedSupplierId.HasValue )
			{
				SupplierModel? supplier = viewModel.ProductSupplierViewModel.SupplierList
				.FirstOrDefault( s => s.SupplierId == selectedSupplierId.Value );

				if ( supplier != null )
				{
					selectedSupplier.ProductSupplierCurrencyId = supplier.SupplierCurrencyId;
					selectedSupplier.ProductSupplierCurrencySymbol = supplier.SupplierCurrency;
					selectedSupplier.ProductSupplierSupplierName = supplier.SupplierName;
					// Ensure PropertyChanged event is raised
					viewModel.ProductSupplierViewModel.RaisePropertyChanged( nameof( viewModel.ProductSupplierViewModel.SelectedSupplier.ProductSupplierSupplierName ) );
					viewModel.ProductSupplierViewModel.RaisePropertyChanged( nameof( viewModel.ProductSupplierViewModel.SelectedSupplier.ProductSupplierCurrencyId ) );
					viewModel.ProductSupplierViewModel.RaisePropertyChanged( nameof( viewModel.ProductSupplierViewModel.SelectedSupplier.ProductSupplierCurrencySymbol ) );
					// Force DataGrid refresh
					ProductSupplierDataGrid.Items.Refresh();
				}
			}
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

	private void FieldChanged( object sender, RoutedEventArgs e )
	{
		if ( !_isUpdatingFields )
		{
			CombinedProductViewModel? _viewModel = DataContext as CombinedProductViewModel;
			if ( _viewModel != null )
			{
				_viewModel.ProductSupplierViewModel.HasUnsavedChanges = true;
			}
		}
	}
}
