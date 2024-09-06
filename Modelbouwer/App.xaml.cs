namespace Modelbouwer;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : System.Windows.Application
{
	public App()
	{
		// Registered SyncFusion License
		Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense( "Ngo9BigBOggjHTQxAR8/V1NCaF1cWWhIfEx1RHxQdld5ZFRHallYTnNWUj0eQnxTdEFjXn1bcH1XQWBfV0J+Xw==" );

		CultureInfo.DefaultThreadCurrentCulture = new CultureInfo( "nl-NL" );
		CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo( "nl-NL" );
	}

}

