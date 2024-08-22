namespace Modelbouwer.View;

/// <summary>
/// Interaction logic for CategoryImport.xaml
/// </summary>
public partial class CategoryImport : Page
{
	public CategoryImport()
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

		var _filename = $"{GeneralHelper.GetFilePrefix()}{(string)FindResource("Import.Category.Filename")}.csv";
		string[] _header = GeneralHelper.GetHeaders("Category");

		GeneralHelper.PrepareCsv( $"{folderDialog.SelectedPath}\\{_filename}", _header );

		dispStatusLine.Text = $"{( string ) FindResource( "Import.Statusline.Completed.Prepare" )} {folderDialog.SelectedPath}\\{_filename}";
	}
	#endregion

	#region Select file to import
	private void SelectFile( object sender, RoutedEventArgs e )
	{
		OpenFileDialog fileDialog = new ()
		{
			Title = (string)FindResource("Import.Category.FileDialog.Description"),
			DefaultExt = ".csv",
			Filter = $"{(string)FindResource("Import.Category.FileDialog.FilterTest")}  (*.csv)|*.csv",
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
		var errorIdentifier = 5; //See Error Class for meaning of this number
		string[] checkField = [DBNames.CategoryFieldNameName, DBNames.CategoryFieldTypeName, "0"]; // The field name to check if record is unique, number is the header item that contains thye data for the check field
		string[] idFields = [DBNames.CategoryFieldNameId, DBNames.CategoryFieldNameParentId]; // The field name to check if record is unique, number is the header item that contains thye data for the check field
		string metadataType = "Category"; // Used for getting the headers for import

		dispStatusLine.Text = GeneralHelper.ProcessCsvFile( DBNames.CategoryTable, errorIdentifier, dispFileName.Text, checkField, idFields, metadataType, () => DBCommands.GetCategoryList() );
	}
	#endregion
}
