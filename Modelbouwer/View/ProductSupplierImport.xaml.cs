namespace Modelbouwer.View;

/// <summary>
/// Interaction logic for ProductSupplierImport.xaml
/// </summary>
public partial class ProductSupplierImport : Page
{
	public ProductSupplierImport()
	{
		InitializeComponent();

		// Since by default there is no export folder selected, hide the column with the export button
		columnImportButton.Width = new GridLength( 0 );
	}

	#region Prepare an empty CSV file, with only headers
	private void Prepare( object sender, RoutedEventArgs e )
	{
		FolderBrowserDialog folderDialog = new()
		{
			Description = (string)FindResource("Import.FileDialog.Description")
		};

		DialogResult result = folderDialog.ShowDialog();

		string _filename = $"{GeneralHelper.GetFilePrefix()}{(string)FindResource("Import.ProductSupplier.Filename")}.csv";
		string[] _header = GeneralHelper.GetHeaders("ProductSupplier");

		GeneralHelper.PrepareCsv( $"{folderDialog.SelectedPath}\\{_filename}", _header );

		dispStatusLine.Text = $"{( string ) FindResource( "Import.Statusline.Completed.Prepare" )} {folderDialog.SelectedPath}\\{_filename}";
	}
	#endregion

	#region Select file to import
	private void SelectFile( object sender, RoutedEventArgs e )
	{
		OpenFileDialog fileDialog = new ()
		{
			Title = (string)FindResource("Import.ProductSupplier.FileDialog.Description"),
			DefaultExt = ".csv",
			Filter = $"{(string)FindResource("Import.ProductSupplier.FileDialog.FilterText")}  (*.csv)|*.csv",
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
		int errorIdentifier = 14; //See Error Class for meaning of this number
		string[] checkField = [DBNames.ProductSupplierFieldNameProductId, DBNames.ProductSupplierFieldTypeProductId, "0", DBNames.ProductSupplierFieldNameSupplierId, DBNames.ProductSupplierFieldTypeSupplierId, "1"]; // The field name to check if record is unique, number is the header item that contains thye data for the check field
		string metadataType = "ProductSupplier"; // Used for getting the headers for import

		dispStatusLine.Text = GeneralHelper.ProcessCsvFile( DBNames.ProductSupplierTable, errorIdentifier, dispFileName.Text, checkField, metadataType );
	}
	#endregion

}
