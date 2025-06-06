﻿namespace Modelbouwer.View;

/// <summary>
/// Interaction logic for CountryExport.xaml
/// </summary>
public partial class CountryExport : Page
{
	private DataTable? _dt;
	public CountryExport()
	{
		InitializeComponent();
		GetData();

		// Since by default there is no export folder selected, hide the column with the export button
		columnExportButton.Width = new GridLength( 0 );
	}

	#region Get the Data
	private void GetData()
	{
		_dt = DBCommands.GetData( DBNames.CountryTable, "nosort" );

		if ( _dt == null )
		{
			dispFolderName.Text = TryFindResource( "Export.Country.Empty" ) as string;
			dispStatusLine.Text = TryFindResource( "Export.Country.Empty" ) as string;

			columnExportButton.Width = new GridLength( 0 );
			columnSelectFolderButton.Width = new GridLength( 0 );
		}
		else
		{
			columnSelectFolderButton.Width = ( GridLength ) FindResource( "Column.SelectFolder.Button.Width" );
		}
	}
	#endregion

	#region Select folder
	private void SelectFolder( object sender, RoutedEventArgs e )
	{
		FolderBrowserDialog folderDialog = new();
		DialogResult result = folderDialog.ShowDialog();

		dispFolderName.Text = $"{folderDialog.SelectedPath}";

		columnExportButton.Width = ( GridLength ) FindResource( "Column.Export.Button.Width" );

	}
	#endregion

	#region Perform Export after selecting folder
	private void Export( object sender, RoutedEventArgs e )
	{
		string _filename = $"{GeneralHelper.GetFilePrefix()}{(string)FindResource("Export.Country.Filename")}.csv";
		string[] _header = GeneralHelper.GetHeaders("Country");

		DBCommands.ExportToCsv( _dt, $"{dispFolderName.Text}\\{_filename}", _header, "Header" );

		columnExportButton.Width = new GridLength( 0 );
		dispStatusLine.Text = $"{_dt.Rows.Count} {( string ) FindResource( "Export.Statusline.Completed" )}";
	}
	#endregion
}
