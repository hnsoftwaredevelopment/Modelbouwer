namespace Modelbouwer.View;

/// <summary>
/// Interaction logic for ProjectManagement.xaml
/// </summary>
public partial class ProjectManagement : Page
{
	public ProjectManagement()
	{
		InitializeComponent();
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

	private void ImageRotate( object sender, RoutedEventArgs e )
	{

	}

	private void ImageDelete( object sender, RoutedEventArgs e )
	{

	}

	private void ImageAdd( object sender, RoutedEventArgs e )
	{

	}
}
