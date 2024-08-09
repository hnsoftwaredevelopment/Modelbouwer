global using System.Windows;
global using System.Windows.Controls;

using Modelbouwer.ViewModels;

namespace Modelbouwer;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
	public MainWindow()
	{
		InitializeComponent();
		DataContext = new MainWindowViewModel();
	}

	#region Time Management Split Button Actions
	#region Show Time report
	private void TimeReportPage( object sender, RoutedEventArgs e )
	{

	}
	#endregion

	#region Import Time entries
	private void TimeImportPage( object sender, RoutedEventArgs e )
	{

	}
	#endregion

	#region Export time entries
	private void TimeExportPage( object sender, RoutedEventArgs e )
	{

	}
	#endregion
	#endregion

	#region Storage Management Split Button Actions
	#region Order products
	private void StorageOrderPage( object sender, RoutedEventArgs e )
	{

	}
	#endregion

	#region Add Recieved orders
	private void StorageReceiptPage( object sender, RoutedEventArgs e )
	{

	}
	#endregion
	#endregion

	#region Project Management
	private void ProjectImportPage( object sender, RoutedEventArgs e )
	{

	}

	private void ProjectExportPage( object sender, RoutedEventArgs e )
	{

	}

	private void ProjectReportPage( object sender, RoutedEventArgs e )
	{

	}
	#endregion

	#region Product Management Split Button Actions
	#region Enter Product registration
	private void ProductManagementPage( object sender, RoutedEventArgs e )
	{

	}
	#endregion

	#region Import Product entries
	private void ProductImportPage( object sender, RoutedEventArgs e )
	{

	}
	#endregion

	#region Export Product entries
	private void ProductExportPage( object sender, RoutedEventArgs e )
	{

	}
	#endregion

	#region Import Product per Supplier entries
	private void ProductSupplierImportPage( object sender, RoutedEventArgs e )
	{

	}
	#endregion

	#region Export Product per Supplier entries
	private void ProductSupplierExportPage( object sender, RoutedEventArgs e )
	{

	}
	#endregion

	#endregion

	#region Category Management Split Button Actions
	#region Enter Product registration
	private void CategoryManagementPage( object sender, RoutedEventArgs e )
	{

	}
	#endregion

	#region Import Category entries
	private void CategoryImportPage( object sender, RoutedEventArgs e )
	{

	}
	#endregion

	#region Export Category entries
	private void CategoryExportPage( object sender, RoutedEventArgs e )
	{

	}
	#endregion
	#endregion

	#region Unit Management Split Button Actions
	#region Enter Unit registration
	private void UnitManagementPage( object sender, RoutedEventArgs e )
	{

	}
	#endregion

	#region Import Unit entries
	private void UnitImportPage( object sender, RoutedEventArgs e )
	{

	}
	#endregion

	#region Export Unit entries
	private void UnitExportPage( object sender, RoutedEventArgs e )
	{

	}
	#endregion
	#endregion

	#region Country Management Split Button Actions
	#region Enter Country registration
	private void CountryManagementPage( object sender, RoutedEventArgs e )
	{

	}
	#endregion

	#region Import Country entries
	private void CountryImportPage( object sender, RoutedEventArgs e )
	{

	}
	#endregion

	#region Export Country entries
	private void CountryExportPage( object sender, RoutedEventArgs e )
	{

	}
	#endregion
	#endregion

	#region Supplier Management Split Button Actions
	#region Enter Supplier registration
	private void SupplierManagementPage( object sender, RoutedEventArgs e )
	{

	}
	#endregion

	#region Import Supplier entries
	private void SupplierImportPage( object sender, RoutedEventArgs e )
	{

	}
	#endregion

	#region Export Supplier entries
	private void SupplierExportPage( object sender, RoutedEventArgs e )
	{

	}
	#endregion
	#endregion

	#region SupplierContact Management Split Button Actions
	#region Enter SupplierContact registration
	private void SupplierContactManagementPage( object sender, RoutedEventArgs e )
	{

	}
	#endregion

	#region Import SupplierContact entries
	private void SupplierContactImportPage( object sender, RoutedEventArgs e )
	{

	}
	#endregion

	#region Export SupplierContact entries
	private void SupplierContactExportPage( object sender, RoutedEventArgs e )
	{

	}
	#endregion
	#endregion

	#region SupplierContactType Management Split Button Actions
	#region Enter SupplierContactType registration
	private void SupplierContactTypeManagementPage( object sender, RoutedEventArgs e )
	{

	}
	#endregion

	#region Import SupplierContactType entries
	private void SupplierContactTypeImportPage( object sender, RoutedEventArgs e )
	{

	}
	#endregion

	#region Export SupplierContactType entries
	private void SupplierContactTypeExportPage( object sender, RoutedEventArgs e )
	{

	}
	#endregion
	#endregion

	#region StorageLocation Management Split Button Actions
	#region Enter StorageLocation registration
	private void StorageLocationManagementPage( object sender, RoutedEventArgs e )
	{

	}
	#endregion

	#region Import StorageLocation entries
	private void StorageLocationImportPage( object sender, RoutedEventArgs e )
	{

	}
	#endregion

	#region Export StorageLocation entries
	private void StorageLocationExportPage( object sender, RoutedEventArgs e )
	{

	}
	#endregion
	#endregion

	#region Brand Management Split Button Actions
	#region Enter Brand registration
	private void BrandManagementPage( object sender, RoutedEventArgs e )
	{

	}
	#endregion

	#region Import Brand entries
	private void BrandImportPage( object sender, RoutedEventArgs e )
	{

	}
	#endregion

	#region Export Brand entries
	private void BrandExportPage( object sender, RoutedEventArgs e )
	{

	}
	#endregion
	#endregion

	#region Currency Management Split Button Actions
	#region Enter Currency registration
	private void CurrencyManagementPage( object sender, RoutedEventArgs e )
	{

	}
	#endregion

	#region Import Currency entries
	private void CurrencyImportPage( object sender, RoutedEventArgs e )
	{

	}
	#endregion

	#region Export Currency entries
	private void CurrencyExportPage( object sender, RoutedEventArgs e )
	{

	}
	#endregion
	#endregion

	#region Worktype Management Split Button Actions
	#region Enter Worktype registration
	private void WorktypeManagementPage( object sender, RoutedEventArgs e )
	{

	}
	#endregion

	#region Import Worktype entries
	private void WorktypeImportPage( object sender, RoutedEventArgs e )
	{

	}
	#endregion

	#region Export Worktype entries
	private void WorktypeExportPage( object sender, RoutedEventArgs e )
	{

	}
	#endregion

	#endregion

}