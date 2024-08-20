using System.Windows.Media.Imaging;

namespace Modelbouwer.View.Dialog;
/// <summary>
/// Interaction logic for ScrollableMessagebox.xaml
/// </summary>
public partial class ScrollableMessagebox : Window
{
	public ScrollableMessagebox( string message, string title = "", string messageType = "default" )
	{
		InitializeComponent();
		SetWindowIcon( messageType.ToLower() );
		MessageTextBox.Text = message;
		if ( title == "" )
		{ this.Title = ( string ) FindResource( "Messagebox.Default.Title" ); }
		else
		{
			{ this.Title = title; }
		}
	}

	private void OkButton_Click( object sender, RoutedEventArgs e )
	{
		this.Close();
	}

	private void SetWindowIcon( string messageType )
	{
		BitmapImage? icon = null;

		switch ( messageType.ToLower() )
		{
			case "info":
				icon = new BitmapImage( new Uri( "pack://application:,,,/Resources/Icons/Info-Message.ico" ) );
				break;
			case "warning":
				icon = new BitmapImage( new Uri( "pack://application:,,,/Resources/Icons/Warning-Message.ico" ) );
				break;
			case "error":
				icon = new BitmapImage( new Uri( "pack://application:,,,/Resources/Icons/Error-Message.ico" ) );
				break;
			default:
				icon = new BitmapImage( new Uri( "pack://application:,,,/Resources/Icons/Default-Message.ico" ) );
				break;
		}

		if ( icon != null )
		{
			this.Icon = icon;
		}
	}

	public static void Show( string message, string title, string messageType )
	{
		ScrollableMessagebox msgBox = new (message, title, messageType);
		msgBox.ShowDialog();
	}
}
