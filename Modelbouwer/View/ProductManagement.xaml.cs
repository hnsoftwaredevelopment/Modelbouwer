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

	}

	private void ImageDelete( object sender, RoutedEventArgs e )
	{

	}

	private void ImageRotate( object sender, RoutedEventArgs e )
	{

	}
	//private void CategoryTextBox_PreviewMouseDown( object sender, MouseButtonEventArgs e )
	//{
	//	// Open the Popup when the TextBox is clicked
	//	CategoryPopup.IsOpen = true;
	//	e.Handled = true;
	//}

	private void CategoryTreeView_SelectedItemChanged( object sender, RoutedPropertyChangedEventArgs<object> e )
	{
		if ( e.NewValue is CategoryModel selectedCategory )
		{
			// Update the selected category in the ViewModel
			var viewModel = (CombinedProductViewModel)DataContext;
			viewModel.CategoryViewModel.SelectedCategory = selectedCategory;

			// Close the popup
			CategoryPopup.IsOpen = false;
		}
	}

	private void CategoryChange( object sender, RoutedEventArgs e )
	{
		CategoryPopup.IsOpen = !CategoryPopup.IsOpen;
	}

	private void ChangedProduct( object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs e )
	{
		if ( DataContext is CombinedProductViewModel viewModel )
		{
			var selectedProduct = viewModel.ProductViewModel.SelectedProduct;

			if ( selectedProduct != null && selectedProduct.ProductCategoryId > 0 )
			{
				var selectedCategory = viewModel.CategoryViewModel.Category
				.FirstOrDefault(c => c.CategoryId == selectedProduct.ProductCategoryId);

				if ( selectedCategory != null )
				{
					viewModel.CategoryViewModel.SelectedCategory = selectedCategory;

					CategoryTreeView.SelectedItem = selectedCategory;
					CategoryTreeView.BringIntoView( selectedCategory );
				}
			}
		}
	}
}
