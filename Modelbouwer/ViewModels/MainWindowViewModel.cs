using Modelbouwer.Commands;

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
	public WorktypeManagement WorktypeManagementPage = new();
	public WorktypeImport WorktypeImportPage = new();
	public WorktypeExport WorktypeExportPage = new();

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
			string heritage = parameter.ToString().ToLower();
			switch ( heritage )
			{
				case "project":
					_ = mainWindow.MainFrame.Navigate( projectManagementPage );
					break;
				case "storage":
					_ = mainWindow.MainFrame.Navigate( storageManagementPage );
					break;
				case "time":
					_ = mainWindow.MainFrame.Navigate( timeManagementPage );
					break;
				case "timeimport":
					_ = mainWindow.MainFrame.Navigate( timeImportPage );
					break;
				case "timeexport":
					_ = mainWindow.MainFrame.Navigate( timeExportPage );
					break;
				case "timereport":
					_ = mainWindow.MainFrame.Navigate( timeReportPage );
					break;
				case "projectimport":
					_ = mainWindow.MainFrame.Navigate( projectImportPage );
					break;
				case "projectexport":
					_ = mainWindow.MainFrame.Navigate( projectExportPage );
					break;
				case "projectreport":
					_ = mainWindow.MainFrame.Navigate( projectReportPage );
					break;
				case "storageorder":
					_ = mainWindow.MainFrame.Navigate( storageOrderPage );
					break;
				case "storagereceipt":
					_ = mainWindow.MainFrame.Navigate( storageReceiptPage );
					break;
				case "storagereportmanagement":
					_ = mainWindow.MainFrame.Navigate( storageReportManagementPage );
					break;
				case "storagereportorder":
					_ = mainWindow.MainFrame.Navigate( storageReportOrderPage );
					break;
				case "storagereportreceipt":
					_ = mainWindow.MainFrame.Navigate( storageReportReceiptPage );
					break;
				case "product":
					_ = mainWindow.MainFrame.Navigate( productManagementPage );
					break;
				case "productimport":
					_ = mainWindow.MainFrame.Navigate( productImportPage );
					break;
				case "productexport":
					_ = mainWindow.MainFrame.Navigate( productExportPage );
					break;
				case "productsupplierimport":
					_ = mainWindow.MainFrame.Navigate( productSupplierImportPage );
					break;
				case "productsupplierexport":
					_ = mainWindow.MainFrame.Navigate( productSupplierExportPage );
					break;
				case "category":
					_ = mainWindow.MainFrame.Navigate( categoryManagementPage );
					break;
				case "categoryimport":
					_ = mainWindow.MainFrame.Navigate( categoryImportPage );
					break;
				case "categoryexport":
					_ = mainWindow.MainFrame.Navigate( categoryExportPage );
					break;
				case "unit":
					_ = mainWindow.MainFrame.Navigate( unitManagement );
					break;
				case "unitimport":
					_ = mainWindow.MainFrame.Navigate( unitImportPage );
					break;
				case "unitexport":
					_ = mainWindow.MainFrame.Navigate( unitExportPage );
					break;
				case "country":
					_ = mainWindow.MainFrame.Navigate( countryManagementPage );
					break;
				case "countryimport":
					_ = mainWindow.MainFrame.Navigate( countryImportPage );
					break;
				case "countryexport":
					_ = mainWindow.MainFrame.Navigate( countryExportPage );
					break;
				case "supplier":
					_ = mainWindow.MainFrame.Navigate( supplierManagementPage );
					break;
				case "supplierimport":
					_ = mainWindow.MainFrame.Navigate( supplierImportPage );
					break;
				case "supplierexport":
					_ = mainWindow.MainFrame.Navigate( supplierExportPage );
					break;
				case "suppliercontact":
					_ = mainWindow.MainFrame.Navigate( supplierContactManagementPage );
					break;
				case "suppliercontactimport":
					_ = mainWindow.MainFrame.Navigate( supplierContactImportPage );
					break;
				case "suppliercontactexport":
					_ = mainWindow.MainFrame.Navigate( supplierContactExportPage );
					break;
				case "suppliercontactfunction":
					_ = mainWindow.MainFrame.Navigate( supplierContactFunctionManagementPage );
					break;
				case "suppliercontactfunctionimport":
					_ = mainWindow.MainFrame.Navigate( supplierContactFunctionImportPage );
					break;
				case "suppliercontactfunctionexport":
					_ = mainWindow.MainFrame.Navigate( supplierContactFunctionExportPage );
					break;
				case "location":
					_ = mainWindow.MainFrame.Navigate( locationManagementPage );
					break;
				case "locationimport":
					_ = mainWindow.MainFrame.Navigate( locationImportPage );
					break;
				case "locationexport":
					_ = mainWindow.MainFrame.Navigate( locationExportPage );
					break;
				case "brand":
					_ = mainWindow.MainFrame.Navigate( brandManagementPage );
					break;
				case "brandimport":
					_ = mainWindow.MainFrame.Navigate( brandImportPage );
					break;
				case "brandexport":
					_ = mainWindow.MainFrame.Navigate( brandExportPage );
					break;
				case "currency":
					_ = mainWindow.MainFrame.Navigate( currencyManagementPage );
					break;
				case "currencyimport":
					_ = mainWindow.MainFrame.Navigate( currencyImportPage );
					break;
				case "currencyexport":
					_ = mainWindow.MainFrame.Navigate( currencyExportPage );
					break;
				case "worktype":
					_ = mainWindow.MainFrame.Navigate( WorktypeManagementPage );
					break;
				case "worktypeimport":
					_ = mainWindow.MainFrame.Navigate( WorktypeImportPage );
					break;
				case "worktypeexport":
					_ = mainWindow.MainFrame.Navigate( WorktypeExportPage );
					break;
			}
		}
	}
}