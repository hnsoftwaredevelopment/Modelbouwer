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
		{ Title = ( string ) FindResource( "Messagebox.Default.Title" ); }
		else
		{
			{ Title = title; }
		}
	}

	private void OkButton_Click( object sender, RoutedEventArgs e )
	{
		Close();
	}

	private void SetWindowIcon( string messageType )
	{
		BitmapImage? icon = messageType.ToLower() switch
		{
			"info" => new BitmapImage( new Uri( "pack://application:,,,/Resources/Icons/Info-Message.ico" ) ),
			"warning" => new BitmapImage( new Uri( "pack://application:,,,/Resources/Icons/Warning-Message.ico" ) ),
			"error" => new BitmapImage( new Uri( "pack://application:,,,/Resources/Icons/Error-Message.ico" ) ),
			_ => new BitmapImage( new Uri( "pack://application:,,,/Resources/Icons/Default-Message.ico" ) ),
		};
		if ( icon != null )
		{
			Icon = icon;
		}
	}

	public static void Show( string message, string title, string messageType )
	{
		ScrollableMessagebox msgBox = new (message, title, messageType);
		_ = msgBox.ShowDialog();
	}
}