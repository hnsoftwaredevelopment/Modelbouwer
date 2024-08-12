global using System.Windows;
global using System.Windows.Controls;

using Modelbouwer.ViewModels;

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

}