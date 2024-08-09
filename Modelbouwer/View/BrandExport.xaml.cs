using System.Data;

using Modelbouwer.Helper;

namespace Modelbouwer.View;

/// <summary>
/// Interaction logic for BrandExport.xaml
/// </summary>
public partial class BrandExport : Page
{
	private DataTable? _dt;

	public BrandExport()
	{
		InitializeComponent();
		GetData();
	}

	#region Get the Data
	private void GetData()
	{
		_dt = DBCommands.GetData( DBNames.BrandTable, "nosort" );
	}
	#endregion

	#region Perform Export after selecting folder
	private void SelectFolder( object sender, RoutedEventArgs e )
	{
		if ( _dt != null )
		{
			FolderBrowserDialog folderDialog = new ();
			DialogResult result = folderDialog.ShowDialog();

			var _filename= $"{GeneralHelper.GetFilePrefix()}{(string)FindResource("Export.Brands.Filename")}.csv";
			string[] _header = GeneralHelper.GetHeaders("Brand");

			dispFolderName.Text = $"{folderDialog.SelectedPath}\\{_filename}";
			GeneralHelper.ExportToCsv( _dt, $"{folderDialog.SelectedPath}\\{_filename}", _header, "Header" );

			btnBrowseFolder.IsEnabled = false;

			dispStatusLine.Text = $"{_dt.Rows.Count} {( string ) FindResource( "Export.Statusline.Completed" )}";
		}
		else
		{
			dispFolderName.Text = TryFindResource( "Export.Brands.Empty" ) as string;
			dispStatusLine.Text = TryFindResource( "Export.Brands.Empty" ) as string;
		}
	}
	#endregion
}
