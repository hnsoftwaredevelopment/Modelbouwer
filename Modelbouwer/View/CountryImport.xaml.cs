namespace Modelbouwer.View;

/// <summary>
/// Interaction logic for CountryImport.xaml
/// </summary>
public partial class CountryImport : Page
{
	public CountryImport()
	{
		InitializeComponent();
		// Since by default there is no export folder selected, hide the column with the export button
		columnImportButton.Width = new GridLength( 0 );
	}

	#region Prepare an empty CSV file, with only headers
	private void Prepare( object sender, RoutedEventArgs e )
	{
		var folderDialog = new FolderBrowserDialog()
		{
			Description = (string)FindResource("Import.FileDialog.Description")
		};

		DialogResult result = folderDialog.ShowDialog();

		var _filename = $"{GeneralHelper.GetFilePrefix()}{(string)FindResource("Import.Country.Filename")}.csv";
		string[] _header = GeneralHelper.GetHeaders("Country");

		GeneralHelper.PrepareCsv( $"{folderDialog.SelectedPath}\\{_filename}", _header );

		dispStatusLine.Text = $"{( string ) FindResource( "Import.Statusline.Completed.Prepare" )} {folderDialog.SelectedPath}\\{_filename}";
	}
	#endregion

	#region Select file to import
	private void SelectFile( object sender, RoutedEventArgs e )
	{
		OpenFileDialog fileDialog = new ()
		{
			Title = (string)FindResource("Import.Country.FileDialog.Description"),
			DefaultExt = ".csv",
			Filter = $"{(string)FindResource("Import.Country.FileDialog.FilterTest")}  (*.csv)|*.csv",
			FilterIndex = 1
		};

		DialogResult result = fileDialog.ShowDialog();

		if ( result == DialogResult.OK )
		{
			dispFileName.Text = fileDialog.FileName;
			columnImportButton.Width = ( GridLength ) FindResource( "Column.Import.Button.Width" );
		}
	}
	#endregion

	#region Import selected CSV file
	private void Import( object sender, RoutedEventArgs e )
	{
		var errorIdentifier = 11; //See Error Class for meaning of this number

		string[] checkField = [DBNames.CountryFieldNameCode, DBNames.CountryFieldTypeCode, "0"]; // The field name to check if record is unique
		string metadataType = "Country"; // Used for getting the headers for import

		dispStatusLine.Text = GeneralHelper.ProcessCsvFile( DBNames.CountryTable, errorIdentifier, dispFileName.Text, checkField, metadataType );
	}
	#endregion
}