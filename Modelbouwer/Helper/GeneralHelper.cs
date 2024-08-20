using Modelbouwer.View.Dialog;

namespace Modelbouwer.Helper;
public class GeneralHelper
{
	public GeneralHelper()
	{
		// no action needed
	}

	#region Add zeros to a timestring
	public static string AddZeros( string TempString, int TotalLength )
	{
		var _temp = new string('0', TotalLength);
		var NewString = (_temp + TempString.Trim()).Substring((_temp + TempString.Trim()).Length - TotalLength, TotalLength);

		return NewString;
	}
	#endregion

	#region Get Timestamp as Filename prefix
	/// <summary>
	/// Get file prefix. The filename for an csv exportfile wil start with a timestamp, this function generates the timestamp for the current export action
	/// </summary>
	/// <returns>A string containing the current timestamp</returns>
	public static string GetFilePrefix()
	{
		var _tempMonth = "0" + DateTime.Now.Month.ToString();
		var _tempDay = "0" + DateTime.Now.Day.ToString();
		var _tempHour = "0" + DateTime.Now.Hour.ToString();
		var _tempMinute = "0" + DateTime.Now.Minute.ToString();
		var _tempSecond = "0" + DateTime.Now.Second.ToString();

		var Prefix = DateTime.Now.Year.ToString() +
			_tempMonth.Substring(_tempMonth.Length - 2, 2) +
			_tempDay.Substring(_tempDay.Length - 2, 2) +
			_tempHour.Substring(_tempHour.Length - 2, 2) +
			_tempMinute.Substring(_tempMinute.Length - 2, 2) +
			_tempSecond.Substring(_tempSecond.Length - 2, 2) +
			" - ";

		return Prefix;
	}
	#endregion

	#region Get Im-/Export file headers
	/// <summary>
	/// All CSV export files will have different named columns to export. Therefore each file will also have <see langword="abstract"/>different header
	/// </summary>
	/// <param name="ExportFile This represents the type of fily user wants to export"></param>
	/// <returns>_header <see langword="abstract"/>string contining the file header</returns>
	public static string [ ] GetHeaders( string ExportFile )
	{
		string[]? _header = null;

		switch ( ExportFile.ToLower() )
		{
			case "brand":
				_header = [ DBNames.BrandFieldNameName ];
				break;
			case "category":
				_header =
				[
					DBNames.CategoryFieldNameId,
					DBNames.CategoryFieldNameParentId,
					DBNames.CategoryFieldNameName,
					DBNames.CategoryFieldNameFullpath
				];
				break;
			case "contacttype":
				_header = [ DBNames.ContactTypeFieldNameName ];
				break;
			case "country":
				_header =
				[
					DBNames.CountryFieldNameCode,
					DBNames.CountryFieldNameName,
					DBNames.CountryFieldNameDefCurrencySymbol,
					DBNames.CountryFieldNameDefCurrencyId
				];
				break;
			case "currency":
				_header =
				[
					DBNames.CurrencyFieldNameCode,
					DBNames.CurrencyFieldNameName,
					DBNames.CurrencyFieldNameSymbol,
					DBNames.CurrencyFieldNameRate
				];
				break;
			case "product":
				_header =
				[
					DBNames.ProductFieldNameCode,
					DBNames.ProductFieldNameName,
					DBNames.ProductFieldNameDimensions,
					DBNames.ProductFieldNamePrice,
					DBNames.ProductFieldNameMinimalStock,
					DBNames.ProductFieldNameStandardOrderQuantity,
					DBNames.ProductFieldNameProjectCosts,
					DBNames.ProductFieldNameUnitId,
					DBNames.ProductFieldNameBrandId,
					DBNames.ProductFieldNameCategoryId,
					DBNames.ProductFieldNameStorageId
				];
				break;
			case "productsupplier":
				_header =
				[
					DBNames.ProductSupplierFieldNameProductId,
					DBNames.ProductSupplierFieldNameSupplierId,
					DBNames.ProductSupplierFieldNameCurrencyId,
					DBNames.ProductSupplierFieldNameProductNumber,
					DBNames.ProductSupplierFieldNameProductName,
					DBNames.ProductSupplierFieldNamePrice,
					DBNames.ProductSupplierFieldNameProductUrl
				];
				break;
			case "project":
				_header =
				[
					DBNames.ProjectFieldNameCode,
					DBNames.ProjectFieldNameName,
					DBNames.ProjectFieldNameStartDate,
					DBNames.ProjectFieldNameEndDate,
					DBNames.ProjectFieldNameExpectedTime,
					DBNames.ProjectFieldNameClosed
				];
				break;
			case "storagelocation":
				_header =
				[
					DBNames.StorageFieldNameId,
					DBNames.StorageFieldNameParentId,
					DBNames.StorageFieldNameName,
					DBNames.StorageFieldNameFullpath
				];
				break;
			case "supplier":
				_header =
				[
					DBNames.SupplierFieldNameCode,
					DBNames.SupplierFieldNameName,
					DBNames.SupplierFieldNameAddress1,
					DBNames.SupplierFieldNameAddress2,
					DBNames.SupplierFieldNameZip,
					DBNames.SupplierFieldNameCity,
					DBNames.SupplierFieldNameUrl,
					DBNames.SupplierFieldNameShippingCosts,
					DBNames.SupplierFieldNameMinShippingCosts,
					DBNames.SupplierFieldNameOrderCosts,
					DBNames.SupplierFieldNameCurrencyId,
					DBNames.SupplierFieldNameCountryId
				];
				break;
			case "suppliercontact":
				_header =
				[
					DBNames.SupplierContactFieldNameSupplierId,
					DBNames.SupplierContactFieldNameName,
					DBNames.SupplierContactFieldNameTypeId,
					DBNames.SupplierContactFieldNameMail,
					DBNames.SupplierContactFieldNamePhone
				];
				break;
			case "suppliercontactfunction":
				_header =
				[
					DBNames.ContactTypeFieldNameName
				];
				break;
			case "time":
				_header =
				[
					DBNames.TimeFieldNameProjectId,
					DBNames.TimeFieldNameWorktypeId,
					DBNames.TimeFieldNameWorkDate,
					DBNames.TimeFieldNameStartTime,
					DBNames.TimeFieldNameEndTime,
					DBNames.TimeFieldNameComment
				];
				break;
			case "unit":
				_header =
				[ DBNames.UnitFieldNameUnitName ];
				break;
			case "worktype":
				_header =
				[
					DBNames.WorktypeFieldNameId,
					DBNames.WorktypeFieldNameParentId,
					DBNames.WorktypeFieldNameName,
					DBNames.WorktypeFieldNameFullpath
				];
				break;
			default:
				_header = [ "" ];
				break;
		}

		return _header;
	}
	#endregion

	#region Write header to empty CSV file
	public static void PrepareCsv( string _filename, string [ ] _header )
	{
		StreamWriter sw = new(_filename, false);
		sw.WriteLine( string.Join( ";", _header ) );
		sw.Close();
	}
	#endregion

	#region Format Date in string to correct notation
	public static string FormatDate( string _date )
	{
		string _result = "";
		// Input date looks like "d-m-yyyy 00:00:00"

		//Remove Time from string and split it up in seperate elements
		var _temp = _date.Replace(" 00:00:00", "" ).Split( '-' );
		_result = $"{_temp [ 2 ]}-{AddZeros( _temp [ 1 ], 2 )}-{AddZeros( _temp [ 0 ], 2 )}";

		return _result;
	}
	#endregion

	#region Prepare data for Import
	public static string ProcessCsvFile( string table, int errorIdentifier, string dispFileName, string [ ] checkField, string metadataType )
	{
		int lineCount = 0, errorCount = 0, headerCount = 0;
		string completed = "", completedError = "", completedOk = "", linesRead = "", linesError = "";
		string insertPart, valuesPart, prefix;
		var errorList = new List<ErrorList>();

		//Get the headers
		var headers = GetHeaders(metadataType);

		//Read the lines of the CSV file into a list
		var lines = File.ReadLines(dispFileName);

		var _errorClass = new ErrorClass();

		foreach ( var line in lines )
		{
			headerCount = 0;
			insertPart = $"{DBNames.SqlInsert}{DBNames.Database}.{table} (";
			valuesPart = $"{DBNames.SqlValues}(";
			prefix = "";
			string[] fields = line.Split(';');

			// Sla de rij met de header over als er een header is
			if ( !line.Contains( headers [ 0 ] ) )
			{
				lineCount++;
				// Sla de rij over als het record al bestaat
				string [,] _wherefields = null;
				if ( checkField.Length == 3 )
				{
					_wherefields = new string [ 1, 3 ] { { checkField [ 0 ], checkField [ 1 ], fields [ int.Parse( checkField [ 2 ] ) ] } };
				}

				if ( checkField.Length == 6 )
				{
					_wherefields = new string [ 2, 3 ]
					{
						{ checkField[0], checkField[1], fields [ int.Parse( checkField [2] )] },
						{ checkField[3], checkField[4], fields [ int.Parse( checkField [5] )] }
					};
				}

				if ( checkField.Length == 9 )
				{
					_wherefields = new string [ 3, 3 ]
					{
						{ checkField[0], checkField[1], fields [ int.Parse( checkField [2] )] },
						{ checkField[3], checkField[4], fields [ int.Parse( checkField [5] )] },
						{ checkField[6], checkField[7], fields [ int.Parse( checkField [8] )] }
					};
				}

				if ( checkField.Length == 12 )
				{
					_wherefields = new string [ 4, 3 ]
					{
						{ checkField[0], checkField[1], fields [ int.Parse( checkField [2] )] },
						{ checkField[3], checkField[4], fields [ int.Parse( checkField [5] )] },
						{ checkField[6], checkField[7], fields [ int.Parse( checkField [8] )] },
						{ checkField[9], checkField[10], fields [ int.Parse( checkField [11] )] }
					};
				}

				if ( checkField.Length == 15 )
				{
					_wherefields = new string [ 5, 3 ]
					{
						{ checkField[0], checkField[1], fields [ int.Parse( checkField [2] )] },
						{ checkField[3], checkField[4], fields [ int.Parse( checkField [5] )] },
						{ checkField[6], checkField[7], fields [ int.Parse( checkField [8] )] },
						{ checkField[9], checkField[10], fields [ int.Parse( checkField [11] )] },
						{ checkField[12], checkField[13], fields [ int.Parse( checkField [14] )] }
					};
				}

				// Controleer of er al een record beschikbaar is voor deze regel
				if ( _wherefields != null )
				{
					var existingDatatype = DBCommands.CheckForRecords(table, _wherefields);

					if ( existingDatatype == 0 )
					{
						foreach ( var header in headers )
						{
							if ( fields [ headerCount ] != "" )
							{
								insertPart += $"{prefix}{header}";

								string _valueToWrite="";

								// Checks value
								if ( int.TryParse( fields [ headerCount ], out int intValue ) )
								{ _valueToWrite = $"{prefix}{fields [ headerCount ]}"; }
								else if ( double.TryParse( fields [ headerCount ].Replace( ',', '.' ), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double doubleValue ) )
								{ _valueToWrite = $"{prefix}{doubleValue.ToString( System.Globalization.CultureInfo.InvariantCulture )}"; }

								if ( fields [ headerCount ].Contains( " 00:00:00" ) )
								{ _valueToWrite = $"{prefix}'{FormatDate( fields [ headerCount ] )}'"; }

								if ( _valueToWrite == "" )
								{ valuesPart += $"{prefix}'{fields [ headerCount ]}'"; }
								else
								{ valuesPart += _valueToWrite; }
							}
							headerCount++;
							prefix = ", ";
						}

						var sqlQuery = $"{insertPart}) {valuesPart});";

						DBCommands.InsertInTable( sqlQuery );
					}
					else
					{
						errorCount++;
						var error = _errorClass.GetErrorMessages(errorIdentifier);
						errorList.Add( new ErrorList
						{
							LineNumber = lineCount,
							ErrorCode = errorIdentifier,
							Line = $"{error.Label}: {line} - {error.ErrorMessageLong}"
						} );
					}
				}
			}

		}

		#region Status message
		if ( lineCount != 0 )
		{
			if ( lineCount == 1 )
			{ completed = ( string ) System.Windows.Application.Current.FindResource( "Import.Statusline.Completed.Single" ); }
			else
			{ completed = ( string ) System.Windows.Application.Current.FindResource( "Import.Statusline.Completed" ); }
		}

		if ( errorCount != 0 )
		{
			if ( errorCount == 1 )
			{ completedError = ( string ) System.Windows.Application.Current.FindResource( "Import.Statusline.Completed.Error.Single" ); }
			else
			{ completedError = ( string ) System.Windows.Application.Current.FindResource( "Import.Statusline.Completed.Error" ); }
		}

		if ( lineCount == 0 ) { linesRead = ( string ) System.Windows.Application.Current.FindResource( "Import.Statusline.None" ) + " "; } else { linesRead = lineCount.ToString() + " "; }
		if ( lineCount - errorCount == 1 )
		{ completedOk = ( string ) System.Windows.Application.Current.FindResource( "Import.Statusline.Completed.Ok.Single" ); }
		else
		{ completedOk = ( string ) System.Windows.Application.Current.FindResource( "Import.Statusline.Completed.Ok" ); }

		if ( lineCount - errorCount != 0 ) { linesRead = ( lineCount - errorCount ).ToString() + " "; }
		if ( errorCount != 0 ) { linesError = ", " + errorCount.ToString() + " "; }

		var statusMessage = $"{linesRead}{completed}{linesError}{completedError}.";
		#endregion

		#region Display Scrollable error message
		if ( errorList.Count > 0 )
		{
			var errorMessage = $"{statusMessage}{System.Environment.NewLine}{System.Environment.NewLine}";

			for ( int i = 0; i < errorList.Count; i++ )
			{
				var error = _errorClass.GetErrorMessages(errorList [ i ].ErrorCode);
				errorMessage += $"{( string ) System.Windows.Application.Current.FindResource( "Import.Messagebox.Error.Line" )} {errorList [ i ].LineNumber} - {errorList [ i ].Line}{System.Environment.NewLine}";
			}

			ScrollableMessagebox.Show( errorMessage, ( string ) System.Windows.Application.Current.FindResource( "Import.Messagebox.Error.Message" ), "Warning" );
		}
		#endregion

		return statusMessage;
	}
	#endregion

	#region Get the Level of an item in a list
	private int GetLevel( CategoryModel category, List<CategoryModel> allCategories )
	{
		int level = 0;
		while ( category.ParentId != null )
		{
			category = allCategories.FirstOrDefault( c => c.Id == category.ParentId );
			level++;
		}
		return level;
	}
	#endregion
}
