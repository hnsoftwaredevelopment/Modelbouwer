using System.Reflection;
using System.Windows.Documents;

using Modelbouwer.View.Dialog;

using Application = System.Windows.Application;

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
		string _temp = new('0', TotalLength);
		string NewString = (_temp + TempString.Trim()).Substring((_temp + TempString.Trim()).Length - TotalLength, TotalLength);

		return NewString;
	}

	#endregion Add zeros to a timestring

	#region Get Timestamp as Filename prefix

	/// <summary>
	/// Get file prefix. The filename for an csv exportfile wil start with a timestamp, this function generates the timestamp for the current export action
	/// </summary>
	/// <returns>A string containing the current timestamp</returns>
	public static string GetFilePrefix()
	{
		string _tempMonth = "0" + DateTime.Now.Month.ToString();
		string _tempDay = "0" + DateTime.Now.Day.ToString();
		string _tempHour = "0" + DateTime.Now.Hour.ToString();
		string _tempMinute = "0" + DateTime.Now.Minute.ToString();
		string _tempSecond = "0" + DateTime.Now.Second.ToString();

		string Prefix = DateTime.Now.Year.ToString() +
			_tempMonth.Substring(_tempMonth.Length - 2, 2) +
			_tempDay.Substring(_tempDay.Length - 2, 2) +
			_tempHour.Substring(_tempHour.Length - 2, 2) +
			_tempMinute.Substring(_tempMinute.Length - 2, 2) +
			_tempSecond.Substring(_tempSecond.Length - 2, 2) +
			" - ";

		return Prefix;
	}

	#endregion Get Timestamp as Filename prefix

	#region Get Im-/Export file headers

	/// <summary>
	/// All CSV export files will have different named columns to export. Therefore each file will also have <see langword="abstract"/>different header
	/// </summary>
	/// <param name="ExportFile This represents the type of fily user wants to export"></param>
	/// <returns>_header <see langword="abstract"/>string contining the file header</returns>
	public static string [ ] GetHeaders( string ExportFile )
	{
		string [ ]? _header = ExportFile.ToLower() switch
		{
			"brand" => [ DBNames.BrandFieldNameName ],
			"category" => [
								DBNames.CategoryFieldNameId,
					DBNames.CategoryFieldNameParentId,
					DBNames.CategoryFieldNameName,
					DBNames.CategoryFieldNameFullpath
							],
			"contacttype" => [ DBNames.ContactTypeFieldNameName ],
			"country" => [
								DBNames.CountryFieldNameCode,
					DBNames.CountryFieldNameName,
					DBNames.CountryFieldNameCurrencySymbol,
					DBNames.CountryFieldNameCurrencyId
							],
			"currency" => [
								DBNames.CurrencyFieldNameCode,
					DBNames.CurrencyFieldNameName,
					DBNames.CurrencyFieldNameSymbol,
					DBNames.CurrencyFieldNameRate
							],
			"product" => [
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
							],
			"productsupplier" => [
								DBNames.ProductSupplierFieldNameProductId,
					DBNames.ProductSupplierFieldNameSupplierId,
					DBNames.ProductSupplierFieldNameCurrencyId,
					DBNames.ProductSupplierFieldNameProductNumber,
					DBNames.ProductSupplierFieldNameProductName,
					DBNames.ProductSupplierFieldNamePrice,
					DBNames.ProductSupplierFieldNameProductUrl
							],
			"project" => [
								DBNames.ProjectFieldNameCode,
					DBNames.ProjectFieldNameName,
					DBNames.ProjectFieldNameStartDate,
					DBNames.ProjectFieldNameEndDate,
					DBNames.ProjectFieldNameExpectedTime,
					DBNames.ProjectFieldNameClosed
							],
			"storagelocation" => [
								DBNames.StorageFieldNameId,
					DBNames.StorageFieldNameParentId,
					DBNames.StorageFieldNameName,
					DBNames.StorageFieldNameFullpath
							],
			"supplier" => [
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
							],
			"suppliercontact" => [
								DBNames.SupplierContactFieldNameSupplierId,
					DBNames.SupplierContactFieldNameName,
					DBNames.SupplierContactFieldNameTypeId,
					DBNames.SupplierContactFieldNameMail,
					DBNames.SupplierContactFieldNamePhone
							],
			"suppliercontactfunction" => [
								DBNames.ContactTypeFieldNameName
							],
			"time" => [
								DBNames.TimeFieldNameProjectId,
					DBNames.TimeFieldNameWorktypeId,
					DBNames.TimeFieldNameWorkDate,
					DBNames.TimeFieldNameStartTime,
					DBNames.TimeFieldNameEndTime,
					DBNames.TimeFieldNameComment
							],
			"unit" => [ DBNames.UnitFieldNameUnitName ],
			"worktype" => [
								DBNames.WorktypeFieldNameId,
					DBNames.WorktypeFieldNameParentId,
					DBNames.WorktypeFieldNameName,
					DBNames.WorktypeFieldNameFullpath
							],
			_ => [ "" ],
		};
		return _header;
	}

	#endregion Get Im-/Export file headers

	#region Write header to empty CSV file

	public static void PrepareCsv( string _filename, string [ ] _header )
	{
		StreamWriter sw = new(_filename, false);
		sw.WriteLine( string.Join( ";", _header ) );
		sw.Close();
	}

	#endregion Write header to empty CSV file

	#region Format Date in string to correct notation

	public static string FormatDate( string _date )
	{
		// Input date looks like "d-m-yyyy 00:00:00"

		//Remove Time from string and split it up in seperate elements
		string [ ] _temp = _date.Replace(" 00:00:00", "" ).Split( '-' );
		string _result = $"{_temp [ 2 ]}-{AddZeros( _temp [ 1 ], 2 )}-{AddZeros( _temp [ 0 ], 2 )}";

		return _result;
	}

	#endregion Format Date in string to correct notation

	#region Get Table Name for generic methods (FindMissingItems)

	public string GetTableNameForModel<T>()
	{
		Type modelType = typeof(T);
		return ModelTableHelper.ModelTableMappings.TryGetValue( modelType, out string? tableName )
			? tableName
			: throw new InvalidOperationException( $"No table name mapping found for type {modelType.Name}" );
	}

	#endregion Get Table Name for generic methods (FindMissingItems)

	#region Prepare data for Import

	#region Check what new lines are in the list that should be added to the existing list

	public (List<T> MissingItems, List<(string Name, int ErrorCode, int LineNumber)> SkippedItems) FindMissingItems<T>( ObservableCollection<T> observableCollection, List<T> itemList, string [ ] headers, Dictionary<string, string> headerToPropertyMap, int errorIdentifier ) where T : INameable
	{
		List<T> missingItems = [];
		List<(string Name, int ErrorCode, int LineNumber)> skippedItems = [];

		for ( int i = 0; i < itemList.Count; i++ )
		{
			T itemInList = itemList[i];
			bool existsInObservable = observableCollection.Any(itemInObservable =>
			headers.All(header =>
			{
				string propertyName = headerToPropertyMap.ContainsKey(header) ? headerToPropertyMap[header] : header;
				object? propertyInObservable = typeof(T).GetProperty(propertyName)?.GetValue(itemInObservable);
				object? propertyInList = typeof(T).GetProperty(propertyName)?.GetValue(itemInList);
				return Equals(propertyInObservable, propertyInList);
			})
		);

			if ( existsInObservable )
			{
				skippedItems.Add( (itemInList.Name, errorIdentifier, i + 1) );
			}
			else
			{
				missingItems.Add( itemInList );
			}
		}

		return (MissingItems: missingItems, SkippedItems: skippedItems);
	}

	#endregion Check what new lines are in the list that should be added to the existing list

	#region import flat csv file (no levels in the data)

	public static string ProcessCsvFile( string table, int errorIdentifier, string dispFileName, string [ ] checkField, string metadataType )
	{
		int lineCount = 0, errorCount = 0;
		string completed = "", completedError = "", linesError = "";
		string insertPart, valuesPart, prefix;
		List<ErrorList> errorList = [];

		//Get the headers
		string [ ] headers = GetHeaders(metadataType);

		//Read the lines of the CSV file into a list
		IEnumerable<string> lines = File.ReadLines( dispFileName );

		ErrorClass _errorClass = new();

		foreach ( string line in lines )
		{
			int headerCount = 0;
			insertPart = $"{DBNames.SqlInsert}{DBNames.Database}.{table} (";
			valuesPart = $"{DBNames.SqlValues}(";
			prefix = "";
			string[] fields = line.Split(';');

			// Sla de rij met de header over als er een header is
			if ( !line.Contains( headers [ 0 ] ) )
			{
				lineCount++;
				// Sla de rij over als het record al bestaat
				string [,]? _wherefields = null;
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
					int existingDatatype = DBCommands.CheckForRecords(table, _wherefields);

					if ( existingDatatype == 0 )
					{
						foreach ( string header in headers )
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

						string sqlQuery = $"{insertPart}) {valuesPart});";

						_ = DBCommands.InsertInTable( sqlQuery );
					}
					else
					{
						errorCount++;
						(string Label, string ErrorMessageShort, string ErrorMessageLong) error = _errorClass.GetErrorMessages( errorIdentifier );
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
			completed = lineCount == 1
				? ( string ) System.Windows.Application.Current.FindResource( "Import.Statusline.Completed.Single" )
				: ( string ) System.Windows.Application.Current.FindResource( "Import.Statusline.Completed" );
		}

		if ( errorCount != 0 )
		{
			completedError = errorCount == 1
				? ( string ) System.Windows.Application.Current.FindResource( "Import.Statusline.Completed.Error.Single" )
				: ( string ) System.Windows.Application.Current.FindResource( "Import.Statusline.Completed.Error" );
		}

		string linesRead = lineCount == 0
	? ( string ) Application.Current.FindResource( "Import.Statusline.None" ) + " "
	: lineCount.ToString() + " ";
		_ = lineCount - errorCount == 1
			? ( string ) Application.Current.FindResource( "Import.Statusline.Completed.Ok.Single" )
			: ( string ) Application.Current.FindResource( "Import.Statusline.Completed.Ok" );

		if ( lineCount - errorCount != 0 ) { linesRead = ( lineCount - errorCount ).ToString() + " "; }
		if ( errorCount != 0 ) { linesError = ", " + errorCount.ToString() + " "; }

		string statusMessage = $"{linesRead}{completed}{linesError}{completedError}.";

		#endregion Status message

		#region Display Scrollable error message

		if ( errorList.Count > 0 )
		{
			string errorMessage = $"{statusMessage}{System.Environment.NewLine}{System.Environment.NewLine}";

			for ( int i = 0; i < errorList.Count; i++ )
			{
				_ = _errorClass.GetErrorMessages( errorList [ i ].ErrorCode );
				errorMessage += $"{( string ) System.Windows.Application.Current.FindResource( "Import.Messagebox.Error.Line" )} {errorList [ i ].LineNumber} - {errorList [ i ].Line}{System.Environment.NewLine}";
			}

			ScrollableMessagebox.Show( errorMessage, ( string ) System.Windows.Application.Current.FindResource( "Import.Messagebox.Error.Message" ), "Warning" );
		}

		#endregion Display Scrollable error message

		return statusMessage;
	}

	#endregion import flat csv file (no levels in the data)

	#region import leveled csv file (containing parent Id)

	public static string ProcessCsvFile<T>( string table, int errorIdentifier, string dispFileName, string [ ] checkField, string [ ] idFields, string metadataType, Func<ObservableCollection<T>> getExistingDataList ) where T : INameable, new()
	{
		int lineCount = 0;
		string completed = "", completedError = "", linesError = "";
		List<T> itemList = [];

		ErrorClass? _errorClass = new ();
		GeneralHelper? generalHelper = new ();

		ObservableCollection<T> existingDataList = getExistingDataList();

		string[]? headers = GetHeaders(metadataType);

		IEnumerable<string>? lines = File.ReadLines(dispFileName);

		PropertyInfo[]? properties = typeof(T).GetProperties();

		// Create item list from csv file, without header and root items wil get 0 as parentId
		foreach ( string line in lines )
		{
			if ( !( line.Contains( headers [ 0 ] ) && line.Contains( headers [ 1 ] ) && line.Contains( headers [ 2 ] ) ) )
			{
				string [ ] _splitLine = line.Split(";");
				int _parentId = 0;
				lineCount++;
				T item = new();

				if ( _splitLine [ 1 ] != "" ) { _parentId = int.Parse( _splitLine [ 1 ] ); }

				// Assign values from CSV to the model properties
				for ( int i = 0; i < properties.Length; i++ )
				{
					PropertyInfo property = properties[i];
					if ( i < _splitLine.Length )
					{
						string value = _splitLine[i];

						// Controleer of de waarde leeg is en het type int is
						if ( string.IsNullOrEmpty( value ) && property.PropertyType == typeof( int ) )
						{
							// Stel een standaardwaarde in, zoals 0, voor een int property
							property.SetValue( item, 0 );
						}
						else if ( string.IsNullOrEmpty( value ) && property.PropertyType == typeof( int? ) )
						{
							// Voor nullable int properties, stel je null in als de waarde leeg is
							property.SetValue( item, null );
						}
						else
						{
							try
							{
								// Convert the value to the correct type
								object convertedValue = Convert.ChangeType(value, property.PropertyType);
								property.SetValue( item, convertedValue );
							}
							catch ( Exception ex )
							{
								Console.WriteLine( $"Error converting value '{value}' for property '{property.Name}': {ex.Message}" );
							}
						}
					}
				}
				itemList.Add( item );
			}
		}

		(List<T> _importItems, List<(string Name, int ErrorCode, int LineNumber)> _errorList) = generalHelper.FindMissingItems( existingDataList, itemList, headers, CategoryModel.HeaderToPropertyMap, errorIdentifier );

		// Write importItems to Database
		// First Get the tablename
		_ = generalHelper.GetTableNameForModel<T>();

		DBCommands.InsertInTable( _importItems, generalHelper.GetTableNameForModel<T>(), headers, CategoryModel.HeaderToPropertyMap );

		// Determine the numbers
		_ = itemList.Count();
		lineCount = _importItems.Count();
		int errorCount = _errorList.Count();

		#region Status message

		if ( lineCount != 0 )
		{
			completed = lineCount == 1
				? ( string ) System.Windows.Application.Current.FindResource( "Import.Statusline.Completed.Single" )
				: ( string ) System.Windows.Application.Current.FindResource( "Import.Statusline.Completed" );
		}

		if ( errorCount != 0 )
		{
			completedError = errorCount == 1
				? ( string ) System.Windows.Application.Current.FindResource( "Import.Statusline.Completed.Error.Single" )
				: ( string ) System.Windows.Application.Current.FindResource( "Import.Statusline.Completed.Error" );
		}

		string linesRead = lineCount == 0
	? ( string ) Application.Current.FindResource( "Import.Statusline.None" ) + " "
	: lineCount.ToString() + " ";
		_ = lineCount - errorCount == 1
			? ( string ) Application.Current.FindResource( "Import.Statusline.Completed.Ok.Single" )
			: ( string ) Application.Current.FindResource( "Import.Statusline.Completed.Ok" );

		//if ( lineCount - errorCount != 0 ) { linesRead = ( lineCount - errorCount ).ToString() + " "; } // Line gives wrong value Why is it used
		if ( errorCount != 0 ) { linesError = ", " + errorCount.ToString() + " "; }

		string statusMessage = $"{linesRead}{completed}{linesError}{completedError}.";

		#endregion Status message

		#region Display Scrollable error message

		if ( _errorList.Count > 0 )
		{
			string errorMessage = $"{statusMessage}{System.Environment.NewLine}{System.Environment.NewLine}";

			for ( int i = 0; i < _errorList.Count; i++ )
			{
				_ = _errorClass.GetErrorMessages( _errorList [ i ].ErrorCode );
				errorMessage += $"{( string ) System.Windows.Application.Current.FindResource( "Import.Messagebox.Error.Line" )} {_errorList [ i ].LineNumber} - {_errorList [ i ].Name}{System.Environment.NewLine}";
			}

			ScrollableMessagebox.Show( errorMessage, ( string ) System.Windows.Application.Current.FindResource( "Import.Messagebox.Error.Message" ), "Warning" );
		}

		#endregion Display Scrollable error message

		return statusMessage;
	}

	#endregion import leveled csv file (containing parent Id)

	#endregion Prepare data for Import

	#region Get the Level of an item in a list

	private int GetCategoryLevel( CategoryModel category, ObservableCollection<CategoryModel> allCategories )
	{
		int level = 0;
		while ( category.CategoryParentId != null )
		{
			category = allCategories.FirstOrDefault( c => c.CategoryId == category.CategoryParentId );
			level++;
		}
		return level;
	}

	#endregion Get the Level of an item in a list

	#region Check if Catergory Exists

	private bool CategoryExists( ObservableCollection<CategoryModel> gridCategories, CategoryModel category, ObservableCollection<CategoryModel> allCategories )
	{
		int categoryLevel = GetCategoryLevel(category, allCategories);

		foreach ( CategoryModel gridCategory in gridCategories )
		{
			int gridCategoryLevel = GetCategoryLevel(gridCategory, allCategories);

			if ( gridCategory.CategoryName == category.CategoryName && gridCategoryLevel == categoryLevel )
			{
				return true; // category already exists in DataGrid
			}
		}
		return false;
	}

	#endregion Check if Catergory Exists

	#region Check if CategoryName has been changed and FullPath should be updated

	public static string [ , ] CheckCategoryNameChange( string _categoryName, string _categoryFullpath )
	{
		string [ , ] _result = new string[1,4];
		string [ ] _temp = _categoryFullpath.Split( "\\" );

		// The CategoryName should always be the last CategoryFullPath item
		if ( _temp [ ^1 ] == _categoryName ) // ^1 means _temp.Length - 1
		{
			_result [ 0, 0 ] = "0";
			_result [ 0, 1 ] = _categoryName;
			_result [ 0, 2 ] = _categoryFullpath;
			_result [ 0, 3 ] = _categoryFullpath; //Unchanged Path
		}
		else
		{
			_temp [ ^1 ] = _categoryName;
			_result [ 0, 0 ] = "1";
			_result [ 0, 1 ] = _categoryName;
			_result [ 0, 2 ] = _categoryFullpath;
			_result [ 0, 3 ] = string.Join( "\\", _temp );
		};

		return _result;
	}

	#endregion Check if CategoryName has been changed and FullPath should be updated

	#region Remove header from list if available

	#region Check if the first line contains the header

	private bool IsHeaderLine<T>( T firstLine, string [ ] headers )
	{
		PropertyInfo [ ] properties = typeof(T).GetProperties();

		for ( int i = 0; i < headers.Length; i++ )
		{
			PropertyInfo? property = properties.FirstOrDefault( p => p.Name.Equals( headers [ i ], StringComparison.OrdinalIgnoreCase ) );

			if ( property == null )
			{
				return false; // When header does not correspont with a property, it is no headerline
			}

			// get the vaule of the property
			string? value = property.GetValue( firstLine )?.ToString();

			if ( value != headers [ i ] )
			{
				return false; // When header does not correspont with a property, it is no headerline
			}
		}
		return true; // All properties are equal to the headers
	}

	#endregion Check if the first line contains the header

	#endregion Remove header from list if available

	#region Get strings from language.xaml

	public static string GetResourceString( string key )
	{
		// Controleer of de resource bestaat
		return Application.Current.Resources.Contains( key )
			? Application.Current.Resources [ key ]?.ToString() ?? $"Resource '{key}' is leeg."
			: $"Resource '{key}' niet gevonden.";
	}

	#endregion Get strings from language.xaml

	#region Get rich text from flow document to convert a RTF (memo) field to a string
	public static string GetRichTextFromFlowDocument( FlowDocument fDoc )
	{
		string result = string.Empty;

		//convert to string
		if ( fDoc != null )
		{
			TextRange tr = new(fDoc.ContentStart, fDoc.ContentEnd);

			using MemoryStream ms = new();
			tr.Save( ms, System.Windows.DataFormats.Rtf );
			result = System.Text.Encoding.UTF8.GetString( ms.ToArray() );
		}
		return result;
	}
	#endregion
}