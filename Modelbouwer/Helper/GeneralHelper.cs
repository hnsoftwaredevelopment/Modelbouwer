using System.Data;
using System.IO;

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

	#region Get Export file headers
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
					DBNames.ProductSupplierFieldNameProductCode,
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
			case "storage":
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
					DBNames.SupplierContactFieldNameTypeId,
					DBNames.SupplierContactFieldNameName,
					DBNames.SupplierContactFieldNameMail,
					DBNames.SupplierContactFieldNamePhone
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
	public static void PrepareCsv( string FileName, string [ ] Columns )
	{
		int _column = 0;
		StreamWriter sw = new(FileName, false);

		for ( int i = 0; i < Columns.Length; i++ )
		{
			_column++;
			sw.Write( Columns [ i ] );
			// Check if the Column is the last column that has to be written, if not we need a ;
			if ( _column < Columns.Length )
			{
				if ( i < Columns.Length - 1 )
				{
					sw.Write( ";" );
				}
			}
		}
		sw.Write( sw.NewLine );
		sw.Close();
	}
	#endregion

	#region Export data to CSV file
	/// <summary>
	/// Export converts selected table to a csv file.
	/// </summary>
	/// <param name="_dt">The datatable containing the data to export</param>
	/// <param name="_filename">The file name for the csv file.</param>
	/// <param name="_header">The header on the first line of the csv file.</param>
	/// <param name="_needsHeader">when it containes "header" a header is needed on the first file of the csv file, <see langword="if"/>different the no header will be written..</param>
	public static void ExportToCsv( DataTable _dt, string _filename, string [ ] _header, string _needsHeader )
	{
		int _column = 0;
		StreamWriter sw = new(_filename, false);

		// Check if a _needsHeader is wanted in the CSV File
		if ( _needsHeader.ToLower() != "header" )
		{
			for ( int i = 0; i < _dt.Columns.Count; i++ )
			{
				// Check if the Column name is Selected for export
				if ( _header.Contains( _dt.Columns [ i ].ToString() ) )
				{
					_column++;
					sw.Write( _dt.Columns [ i ] );
					// Check if the Column is the last column that has to be written, if not we need a ;
					if ( _column < _header.Length )
					{
						if ( i < _dt.Columns.Count - 1 )
						{
							sw.Write( ";" );
						}
					}
				}
			}
			sw.Write( sw.NewLine );
		}

		_column = 0;
		int _rowCount = 0;
		foreach ( DataRow dr in _dt.Rows )
		{
			_rowCount++;
			for ( int i = 0; i < _dt.Columns.Count; i++ )
			{
				_column++;
				if ( !Convert.IsDBNull( dr [ i ] ) )
				{
					string value = dr[i].ToString();
					// Check if the vaule belongs to a selected Column
					if ( _header.Contains( _dt.Columns [ i ].ToString() ) )
					{
						// Check if the string contains a ; what is also the value separator
						if ( value.Contains( ';' ) )
						{
							value = String.Format( "\"{0}\"", value );
							sw.Write( value );
						}
						else
						{
							sw.Write( dr [ i ].ToString() );
						}
					}
				}
				// Check if the Column is the last column that has to be written, if not we need a ;
				if ( _column < _header.Length )
				{
					if ( i < _dt.Columns.Count - 1 )
					{
						sw.Write( ";" );
					}
				}
			}
			_column = 0;
			sw.Write( sw.NewLine );
		}
		sw.Close();
	}
	#endregion
}
