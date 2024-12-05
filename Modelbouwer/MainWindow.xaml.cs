global using System.Windows;
global using System.Windows.Controls;

namespace Modelbouwer;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
	public MainWindow()
	{
		InitializeComponent();
		DataContext = new MainWindowViewModel();
	}

	#region Move the window on the screen by dragging it into possition
	private void Window_MouseLeftButtonDown( object sender, MouseButtonEventArgs e )
	{
		DragMove();
	}
	#endregion

}