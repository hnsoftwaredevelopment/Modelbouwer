#pragma warning disable CS8602 // Dereference of a possibly null reference.
using System.Windows.Input;

using Modelbouwer.Commands;
using Modelbouwer.View;

namespace Modelbouwer.ViewModels;

public class MainWindowViewModel
{
	#region variable declarations
	public ICommand OpenManagementCommand { get; }

	// Declare variables for loading the pages
	public TimeManagement timeManagementPage = new();
	public TimeImport timeImportPage = new();
	public TimeExport timeExportPage = new();
	public TimeReport timeReportPage = new();
	public ProjectManagement projectManagementPage = new();
	public ProjectImport projectImportPage = new();
	public ProjectExport projectExportPage = new();
	public ProjectReport projectReportPage = new();
	public StorageManagement storageManagementPage = new();
	public StorageOrder storageOrderPage = new();
	public StorageReceipt storageReceiptPage = new();
	public StorageReportManagement  storageReportManagementPage = new();
	public StorageReportOrder  storageReportOrderPage = new();
	public StorageReportReceipt storageReportReceiptPage = new();
	public ProductManagement productManagementPage = new();
	public ProductImport productImportPage = new();
	public ProductExport productExportPage = new();
	public ProductSupplierImport productSupplierImportPage = new();
	public ProductSupplierExport productSupplierExportPage = new();
	public CategoryManagement categoryManagementPage = new();
	public CategoryImport categoryImportPage = new();
	public CategoryExport categoryExportPage = new();
	public UnitManagement unitManagement = new();
	public UnitImport unitImportPage = new();
	public UnitExport unitExportPage = new();
	public CountryManagement countryManagementPage = new();
	public CountryImport countryImportPage = new();
	public CountryExport countryExportPage = new();
	public SupplierManagement supplierManagementPage = new();
	public SupplierImport supplierImportPage = new();
	public SupplierExport supplierExportPage = new();
	public SupplierContactManagement supplierContactManagementPage = new();
	public SupplierContactImport supplierContactImportPage = new();
	public SupplierContactExport supplierContactExportPage = new();
	public SupplierContactFunctionManagement supplierContactFunctionManagementPage = new();
	public SupplierContactFunctionImport supplierContactFunctionImportPage = new();
	public SupplierContactFunctionExport supplierContactFunctionExportPage = new();
	public LocationManagement locationManagementPage = new();
	public LocationImport locationImportPage = new();
	public LocationExport locationExportPage = new();
	public BrandManagement brandManagementPage = new();
	public BrandImport brandImportPage = new();
	public BrandExport brandExportPage = new();
	public CurrencyManagement currencyManagementPage = new();
	public CurrencyImport currencyImportPage = new();
	public CurrencyExport currencyExportPage = new();
	public WorktypeManagement worktypeManagementPage = new();
	public WorktypeImport worktypeImportPage = new();
	public WorktypeExport worktypeExportPage = new();

	public MainWindow mainWindow = ( MainWindow ) System.Windows.Application.Current.MainWindow;
	#endregion

	public MainWindowViewModel()
	{
		OpenManagementCommand = new MenuRelayCommand( OpenManagementPage );
	}

	private void OpenManagementPage( object parameter )
	{
		if ( parameter != null )
		{
			var heritage = parameter.ToString().ToLower();
			switch ( heritage )
			{
				case "project":
					mainWindow.MainFrame.Navigate( projectManagementPage );
					break;
				case "storage":
					mainWindow.MainFrame.Navigate( storageManagementPage );
					break;
				case "time":
					mainWindow.MainFrame.Navigate( timeManagementPage );
					break;
				case "timeimport":
					mainWindow.MainFrame.Navigate( timeImportPage );
					break;
				case "timeexport":
					mainWindow.MainFrame.Navigate( timeExportPage );
					break;
				case "timereport":
					mainWindow.MainFrame.Navigate( timeReportPage );
					break;
				case "projectimport":
					mainWindow.MainFrame.Navigate( projectImportPage );
					break;
				case "projectexport":
					mainWindow.MainFrame.Navigate( projectExportPage );
					break;
				case "projectreport":
					mainWindow.MainFrame.Navigate( projectReportPage );
					break;
				case "storageorder":
					mainWindow.MainFrame.Navigate( storageOrderPage );
					break;
				case "storagereceipt":
					mainWindow.MainFrame.Navigate( storageReceiptPage );
					break;
				case "storagereportmanagement":
					mainWindow.MainFrame.Navigate( storageReportManagementPage );
					break;
				case "storagereportorder":
					mainWindow.MainFrame.Navigate( storageReportOrderPage );
					break;
				case "storagereportreceipt":
					mainWindow.MainFrame.Navigate( storageReportReceiptPage );
					break;
				case "product":
					mainWindow.MainFrame.Navigate( productManagementPage );
					break;
				case "productimport":
					mainWindow.MainFrame.Navigate( productImportPage );
					break;
				case "productexport":
					mainWindow.MainFrame.Navigate( productExportPage );
					break;
				case "productsupplierimport":
					mainWindow.MainFrame.Navigate( productSupplierImportPage );
					break;
				case "productsupplierexport":
					mainWindow.MainFrame.Navigate( productSupplierExportPage );
					break;
				case "category":
					mainWindow.MainFrame.Navigate( categoryManagementPage );
					break;
				case "categoryimport":
					mainWindow.MainFrame.Navigate( categoryImportPage );
					break;
				case "categoryexport":
					mainWindow.MainFrame.Navigate( categoryExportPage );
					break;
				case "unit":
					mainWindow.MainFrame.Navigate( unitManagement );
					break;
				case "unitimport":
					mainWindow.MainFrame.Navigate( unitImportPage );
					break;
				case "unitexport":
					mainWindow.MainFrame.Navigate( unitExportPage );
					break;
				case "country":
					mainWindow.MainFrame.Navigate( countryManagementPage );
					break;
				case "countryimport":
					mainWindow.MainFrame.Navigate( countryImportPage );
					break;
				case "countryexport":
					mainWindow.MainFrame.Navigate( countryExportPage );
					break;
				case "supplier":
					mainWindow.MainFrame.Navigate( supplierManagementPage );
					break;
				case "supplierimport":
					mainWindow.MainFrame.Navigate( supplierImportPage );
					break;
				case "supplierexport":
					mainWindow.MainFrame.Navigate( supplierExportPage );
					break;
				case "suppliercontact":
					mainWindow.MainFrame.Navigate( supplierContactManagementPage );
					break;
				case "suppliercontactimport":
					mainWindow.MainFrame.Navigate( supplierContactImportPage );
					break;
				case "suppliercontactexport":
					mainWindow.MainFrame.Navigate( supplierContactExportPage );
					break;
				case "suppliercontactfunction":
					mainWindow.MainFrame.Navigate( supplierContactFunctionManagementPage );
					break;
				case "suppliercontactfunctionimport":
					mainWindow.MainFrame.Navigate( supplierContactFunctionImportPage );
					break;
				case "suppliercontactfunctionexport":
					mainWindow.MainFrame.Navigate( supplierContactFunctionExportPage );
					break;
				case "location":
					mainWindow.MainFrame.Navigate( locationManagementPage );
					break;
				case "locationimport":
					mainWindow.MainFrame.Navigate( locationImportPage );
					break;
				case "locationexport":
					mainWindow.MainFrame.Navigate( locationExportPage );
					break;
				case "brand":
					mainWindow.MainFrame.Navigate( brandManagementPage );
					break;
				case "brandimport":
					mainWindow.MainFrame.Navigate( brandImportPage );
					break;
				case "brandexport":
					mainWindow.MainFrame.Navigate( brandExportPage );
					break;
				case "currency":
					mainWindow.MainFrame.Navigate( currencyManagementPage );
					break;
				case "currencyimport":
					mainWindow.MainFrame.Navigate( currencyImportPage );
					break;
				case "currencyexport":
					mainWindow.MainFrame.Navigate( currencyExportPage );
					break;
				case "worktype":
					mainWindow.MainFrame.Navigate( worktypeManagementPage );
					break;
				case "worktypeimport":
					mainWindow.MainFrame.Navigate( worktypeImportPage );
					break;
				case "worktypeexport":
					mainWindow.MainFrame.Navigate( worktypeExportPage );
					break;
			}
		}
	}
}
#pragma warning restore CS8602 // Dereference of a possibly null reference.
