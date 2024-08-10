using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Modelbouwer.View
{
    /// <summary>
    /// Interaction logic for CategoryExport.xaml
    /// </summary>
    public partial class CategoryExport : Page
    {
        private DataTable? _dt;
        public CategoryExport()
        {
            InitializeComponent();
            GetData();
            btnExport.IsEnabled = false;
        }

        #region Get the Data
        private void GetData()
        {
            _dt = DBCommands.GetData(DBNames.CategoryTable, "nosort");
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
                dispFolderName.Text = TryFindResource("Export.Category.Empty") as string;
                dispStatusLine.Text = TryFindResource("Export.Category.Empty") as string;

                btnExport.IsEnabled = false;
            }
        }
        #endregion

        #region Perform Export after selecting folder
        private void Export(object sender, RoutedEventArgs e)
        {
            if (_dt != null)
            {
                var _filename = $"{GeneralHelper.GetFilePrefix()}{(string)FindResource("Export.Category.Filename")}.csv";
                string[] _header = GeneralHelper.GetHeaders("Category");

                GeneralHelper.ExportToCsv(_dt, $"{dispFolderName.Text}\\{_filename}", _header, "Header");

                btnExport.IsEnabled = false;

                dispStatusLine.Text = $"{_dt.Rows.Count} {(string)FindResource("Export.Statusline.Completed")}";
            }
            else
            {
                dispFolderName.Text = TryFindResource("Export.Category.Empty") as string;
                dispStatusLine.Text = TryFindResource("Export.Category.Empty") as string;
            }
        }
        #endregion

    }
}
