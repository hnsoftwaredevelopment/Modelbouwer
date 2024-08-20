namespace Modelbouwer.Helper;
/// <summary>
/// Used Error codes
///  1 => Product already exists
///  2 => Worktype already exists
///  3 => Brand already excists
///  4 => Unit Already Excists
///  5 => Category already excists
///  6 => Storage already exists
///  7 => Project already exists
///  8 => Contacttype already excists
///  9 => Supplier already excists
/// 10 => Currency already excists
/// 11 => Country already excists
/// 12 => Category Parent Id already excists
/// 13 => Worktype Parent Id already excists
/// 14 => Product per supplier already exists
/// 15 => Supplier Contact already excists
/// 16 => Time Entry already excists
/// 21 => Not existing ProductCode
/// 22 => Not existing WorktypeName
/// 23 => Not existing BrandId
/// 24 => Not existing UnitId
/// 25 => Not existing Category
/// 26 => Not existing StorageId
/// 27 => Not existing ProjectId
/// 28 => Not existing Contacttype
/// 29 => Not existing Supplier
/// 30 => Not existing CurrencyId
/// 31 => Not existing CountryId
/// 32 => Not existing Category ParentId
/// 33 => Not existing Worktype ParentId
/// 40 => Endtime bigger or equal then Starttime
/// 41 => Incorrect number of fields in CSV file
/// </summary>

/// <summary Errorlevels>
/// 1 => Already excists
/// 2 => Not existing
/// </summary>

internal class ErrorClass
{
	public ErrorClass()
	{
		// Nothing to do her yet
	}

	public string GetSingleErrorMessage( int Error )
	{
		// Use default error in case no matching error is found
		var ErrorMessage = (string)System.Windows.Application.Current.FindResource( "Import.Messagebox.Error.Default.Long");

		switch ( Error )
		{
			case 40:
				// Endtime bigger or equal then Starttime
				ErrorMessage = ( string ) System.Windows.Application.Current.FindResource( "Import.Messagebox.Error.StartEndTimeError" );
				break;
			case 41:
				// Incorrect number of fields in CSV file
				ErrorMessage = ( string ) System.Windows.Application.Current.FindResource( "Import.Messagebox.Error.HeaderError" );
				break;
		}
		return ErrorMessage;
	}
	public (string Label, string ErrorMessageShort, string ErrorMessageLong) GetErrorMessages( int Error )
	{
		// Use default error in case no matching error is found
		var _metadataType = "";
		var _errorLevel = 0;
		var Label=(string)System.Windows.Application.Current.FindResource( "Import.Messagebox.Error.Unknown");
		var ErrorMessageShort = (string)System.Windows.Application.Current.FindResource( "Import.Messagebox.Error.Unknown");
		var ErrorMessageLong = (string)System.Windows.Application.Current.FindResource( "Import.Messagebox.Error.Unknown.Long");
		switch ( Error )
		{
			case 1:
				// Product already exists
				_metadataType = "Product";
				_errorLevel = 1;
				break;
			case 2:
				// Worktype already exists
				_metadataType = "WorkType";
				_errorLevel = 1;
				break;
			case 3:
				// Brand already excists
				_metadataType = "Brands";
				_errorLevel = 1;
				break;
			case 4:
				// Unit Already Excists
				_metadataType = "Unit";
				_errorLevel = 1;
				break;
			case 5:
				// Category already excists
				_metadataType = "Category";
				_errorLevel = 1;
				break;
			case 6:
				// Storage already exists
				_metadataType = "Storage";
				_errorLevel = 1;
				break;
			case 7:
				// Project already exists
				_metadataType = "Project";
				_errorLevel = 1;
				break;
			case 8:
				// Contacttype already excists
				_metadataType = "ContactType";
				_errorLevel = 1;
				break;
			case 9:
				// Supplier already excists
				_metadataType = "Supplier";
				_errorLevel = 1;
				break;
			case 10:
				// Currency already excists
				_metadataType = "Currency";
				_errorLevel = 1;
				break;
			case 11:
				// Country already excists
				_metadataType = "Country";
				_errorLevel = 1;
				break;
			case 12:
				// Category Parent Id already excists
				_metadataType = "CategoryParentId";
				_errorLevel = 1;
				break;
			case 13:
				// Worktype Parent Id already excists
				_metadataType = "WorktypeParentId";
				_errorLevel = 1;
				break;
			case 14:
				// Product already exists for this supplier
				_metadataType = "ProductSupplier";
				_errorLevel = 1;
				break;
			case 15:
				// Contact already exists for this supplier
				_metadataType = "SupplierContact";
				_errorLevel = 1;
				break;
			case 16:
				// Time entry already exists
				_metadataType = "Time";
				_errorLevel = 1;
				break;
			case 21:
				// Not existing ProductCode
				_metadataType = "ProductCode";
				_errorLevel = 2;
				break;
			case 22:
				// Not existing WorktypeName
				_metadataType = "WorktypeName";
				_errorLevel = 2;
				break;
			case 23:
				// Not existing BrandId
				_metadataType = "BrandId";
				_errorLevel = 2;
				break;
			case 24:
				// Not existing UnitId
				_metadataType = "UnitId";
				_errorLevel = 2;
				break;
			case 25:
				// Not existing Category
				_metadataType = "Category";
				_errorLevel = 2;
				break;
			case 26:
				// Not existing StorageId
				_metadataType = "StorageId";
				_errorLevel = 2;
				break;
			case 27:
				// Not existing ProjectId
				_metadataType = "ProjectId";
				_errorLevel = 2;
				break;
			case 28:
				// Not existing Contacttype
				_metadataType = "Contacttype";
				_errorLevel = 2;
				break;
			case 29:
				// Not existing Supplier
				_metadataType = "Supplier";
				_errorLevel = 2;
				break;
			case 30:
				// Not existing Currency
				_metadataType = "Currency";
				_errorLevel = 2;
				break;
			case 31:
				// Not existing Country
				_metadataType = "Country";
				_errorLevel = 2;
				break;
			case 32:
				// Not existing Category Parent Id
				_metadataType = "CategoryParentId";
				_errorLevel = 2;
				break;
			case 33:
				// Not existing Worktype Parent Id
				_metadataType = "WorktypeParentId";
				_errorLevel = 2;
				break;
		}

		Label = ( string ) System.Windows.Application.Current.FindResource( $"Import.{_metadataType}.Button.Label" );

		switch ( _errorLevel )
		{
			case 1:
				ErrorMessageShort = ( string ) System.Windows.Application.Current.FindResource( $"Import.Messagebox.Error.Existing.Short" );
				ErrorMessageLong = ( string ) System.Windows.Application.Current.FindResource( $"Import.Messagebox.Error.Existing.Long" );
				break;
			case 2:
				ErrorMessageShort = ( string ) System.Windows.Application.Current.FindResource( $"Import.{_metadataType}.Messagebox.Error.NonExiststing.Short" );
				ErrorMessageLong = ( string ) System.Windows.Application.Current.FindResource( $"Import.{_metadataType}.Messagebox.Error.NonExiststing.Long" );
				break;
			default:
				ErrorMessageShort = "Unknown";
				ErrorMessageLong = "Unknow error ocured";
				break;
		}

		return (Label, ErrorMessageShort, ErrorMessageLong);
	}
}