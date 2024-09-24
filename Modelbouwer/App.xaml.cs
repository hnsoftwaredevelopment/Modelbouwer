namespace Modelbouwer;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : System.Windows.Application
{
	public App()
	{
		// Registered SyncFusion License

		//MzQ5MDE4M0AzMjM3MmUzMDJlMzBVNndvL3YzckZsL0ZkZlZhV2t1bjlvb1Zxd3hHMHhPdHIvRHJpNXdHK3IwPQ==;MzQ5MDE4NEAzMjM3MmUzMDJlMzBQdkhranlZaElhNGJTZWJObzhNSDdJMEhpQlNPRjErRXhVYmFpaWpXNDlnPQ==;MzQ5MDE4NUAzMjM3MmUzMDJlMzBLK1dGbUVsdkljTVVjTW45ajhnMEJFRXc2MFU3VW1odVdjK1BwMFRWQ2RjPQ==
		Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense( "MzQ5MDE4M0AzMjM3MmUzMDJlMzBVNndvL3YzckZsL0ZkZlZhV2t1bjlvb1Zxd3hHMHhPdHIvRHJpNXdHK3IwPQ==;MzQ5MDE4NEAzMjM3MmUzMDJlMzBQdkhranlZaElhNGJTZWJObzhNSDdJMEhpQlNPRjErRXhVYmFpaWpXNDlnPQ==;MzQ5MDE4NUAzMjM3MmUzMDJlMzBLK1dGbUVsdkljTVVjTW45ajhnMEJFRXc2MFU3VW1odVdjK1BwMFRWQ2RjPQ==" );

		CultureInfo.DefaultThreadCurrentCulture = new CultureInfo( "nl-NL" );
		CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo( "nl-NL" );
	}

}

