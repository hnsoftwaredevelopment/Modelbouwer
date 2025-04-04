﻿namespace Modelbouwer.View;

/// <summary>
/// Interaction logic for ProjectExport.xaml
/// </summary>
public partial class ProjectExport : Page
{
	private DataTable? _dt;

	public ProjectExport()
	{
		InitializeComponent();
		GetData();

		// Since by default there is no export folder selected, hide the column with the export button
		columnExportButton.Width = new GridLength( 0 );
	}

	#region Get the Data
	private void GetData()
	{
		_dt = DBCommands.GetData( DBNames.ProjectTable, "nosort" );

		if ( _dt == null )
		{
			dispFolderName.Text = TryFindResource( "Export.Project.Empty" ) as string;
			dispStatusLine.Text = TryFindResource( "Export.Project.Empty" ) as string;

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
		string _filename = $"{GeneralHelper.GetFilePrefix()}{(string)FindResource("Export.Project.Filename")}.csv";
		string[] _header = GeneralHelper.GetHeaders("Project");

		DBCommands.ExportToCsv( _dt!, $"{dispFolderName.Text}\\{_filename}", _header, "Header" );

		columnExportButton.Width = new GridLength( 0 );

		dispStatusLine.Text = $"{_dt!.Rows.Count} {( string ) FindResource( "Export.Statusline.Completed" )}";

	}
	#endregion
}
