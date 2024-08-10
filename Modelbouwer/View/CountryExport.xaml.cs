namespace Modelbouwer.View;

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
        btnExport.IsEnabled = false;
    }

    #region Get the Data
    private void GetData()
    {
        _dt = DBCommands.GetData(DBNames.CountryTable, "nosort");
    }
    #endregion

    #region Select folder
    private void SelectFolder(object sender, RoutedEventArgs e)
    {
        if (_dt != null)
        {
            FolderBrowserDialog folderDialog = new();
            DialogResult result = folderDialog.ShowDialog();

            dispFolderName.Text = $"{folderDialog.SelectedPath}";

            btnExport.IsEnabled = true;
        }
        else
        {
            dispFolderName.Text = TryFindResource("Export.Country.Empty") as string;
            dispStatusLine.Text = TryFindResource("Export.Country.Empty") as string;

            btnExport.IsEnabled = false;
        }
    }
    #endregion

    #region Perform Export after selecting folder
    private void Export(object sender, RoutedEventArgs e)
    {
        if (_dt != null)
        {
            var _filename = $"{GeneralHelper.GetFilePrefix()}{(string)FindResource("Export.Country.Filename")}.csv";
            string[] _header = GeneralHelper.GetHeaders("Country");

            GeneralHelper.ExportToCsv(_dt, $"{dispFolderName.Text}\\{_filename}", _header, "Header");

            btnExport.IsEnabled = false;

            dispStatusLine.Text = $"{_dt.Rows.Count} {(string)FindResource("Export.Statusline.Completed")}";
        }
        else
        {
            dispFolderName.Text = TryFindResource("Export.Country.Empty") as string;
            dispStatusLine.Text = TryFindResource("Export.Country.Empty") as string;
        }
    }
    #endregion
}
