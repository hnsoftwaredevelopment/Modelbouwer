﻿namespace Modelbouwer.Helper;

public class DBNames
{
	#region Sql commands

	public static readonly string SqlAnd = " AND ";
	public static readonly string SqlAsc = " ASC ";
	public static readonly string SqlAs = " AS ";
	public static readonly string SqlBetween = " BETWEEN ";
	public static readonly string SqlCast = "CAST( ";
	public static readonly string SqlConcat = "CONCAT( ";
	public static readonly string SqlCount = " COUNT( ";
	public static readonly string SqlCountAll = " COUNT(*) ";
	public static readonly string SqlCountUnique = " COUNT(DISTINCT ";
	public static readonly string SqlDelete = "DELETE ";
	public static readonly string SqlDeleteFrom = "DELETE FROM ";
	public static readonly string SqlDesc = " DESC ";
	public static readonly string SqlFrom = " FROM ";
	public static readonly string SqlIn = "IN ";
	public static readonly string SqlInnerJoin = "INNER JOIN ";
	public static readonly string SqlInsert = "INSERT INTO ";
	public static readonly string SqlIsNull = " IS NULL ";
	public static readonly string SqlLimit = " LIMIT ";
	public static readonly string SqlLimit1 = " LIMIT 1 ";
	public static readonly string SqlLike = " LIKE ";
	public static readonly string SqlLower = " LOWER ( ";
	public static readonly string SqlMax = "MAX( ";
	public static readonly string SqlMin = "MIN( ";
	public static readonly string SqlOn = " ON ";
	public static readonly string SqlOr = " OR ";
	public static readonly string SqlOrder = " ORDER BY ";
	public static readonly string SqlOrderBy = " ORDER BY ";
	public static readonly string SqlSelect = "SELECT ";
	public static readonly string SqlSelectAll = "SELECT *";
	public static readonly string SqlSelectDistinct = "SELECT DISTINCT ";
	public static readonly string SqlSet = " SET ";
	public static readonly string SqlSum = " SUM( ";
	public static readonly string SqlSubString = " SUBSTRING( ";
	public static readonly string SqlUnsigned = " as UNSIGNED) ";
	public static readonly string SqlUnionAll = "UNION ALL";
	public static readonly string SqlUpdate = "UPDATE ";
	public static readonly string SqlValues = " VALUES ";
	public static readonly string SqlWhere = " WHERE ";
	public static readonly string SqlWithRecursive = "WITH RECURSIVE ";

	#endregion Sql commands

	#region Database

	public static readonly string Database = "modelbuilder";

	#endregion Database

	#region Settings table

	public static readonly string SettingsTable = "settings";
	public static readonly string SettingsFieldNameCulture = "Culture";
	public static readonly string SettingsFieldNameLanguage = "Language";
	public static readonly string SettingsFieldNameHourRate = "HourRate";

	public static readonly string SettingsFieldTypeCulture = "string";
	public static readonly string SettingsFieldTypeLanguage = "string";
	public static readonly string SettingsFieldTypeHourRate = "double";

	#endregion Settings table

	#region Brand table

	public static readonly string BrandTable = "brand";
	public static readonly string BrandFieldNameId = "Id";
	public static readonly string BrandFieldNameName = "Name";

	public static readonly string BrandFieldTypeId = "int";
	public static readonly string BrandFieldTypeName = "string";

	#endregion Brand table

	#region Category table

	public static readonly string CategoryTable = "category";
	public static readonly string CategoryFieldNameId = "Id";
	public static readonly string CategoryFieldNameParentId = "ParentId";
	public static readonly string CategoryFieldNameName = "Name";
	public static readonly string CategoryFieldNameFullpath = "Fullpath";
	public static readonly string CategoryFieldTypeId = "int";
	public static readonly string CategoryFieldTypeParentId = "int";
	public static readonly string CategoryFieldTypeName = "string";
	public static readonly string CategoryFieldTypeFullpath = "string";

	#endregion Category table

	#region Contacttype table

	public static readonly string ContactTypeTable = "contacttype";
	public static readonly string ContactTypeFieldNameId = "Id";
	public static readonly string ContactTypeFieldTypeId = "Int";
	public static readonly string ContactTypeFieldNameName = "Name";
	public static readonly string ContactTypeFieldTypeName = "string";

	#endregion Contacttype table

	#region Currency table

	public static readonly string CurrencyTable = "currency";
	public static readonly string CurrencyFieldNameId = "Id";
	public static readonly string CurrencyFieldTypeId = "int";
	public static readonly string CurrencyFieldNameCode = "Code";
	public static readonly string CurrencyFieldTypeCode = "string";
	public static readonly string CurrencyFieldNameSymbol = "Symbol";
	public static readonly string CurrencyFieldTypeSymbol = "string";
	public static readonly string CurrencyFieldNameName = "Name";
	public static readonly string CurrencyFieldTypeName = "string";
	public static readonly string CurrencyFieldNameRate = "ConversionRate";
	public static readonly string CurrencyFieldTypeRate = "double";

	#endregion Currency table

	#region Country table

	public static readonly string CountryTable = "country";
	public static readonly string CountryView = "view_country";
	public static readonly string CountryFieldNameId = "Id";
	public static readonly string CountryFieldTypeId = "int";
	public static readonly string CountryFieldNameCode = "Code";
	public static readonly string CountryFieldTypeCode = "string";
	public static readonly string CountryFieldNameCurrencySymbol = "Symbol";
	public static readonly string CountryFieldTypeCurrencySymbol = "string";
	public static readonly string CountryFieldNameName = "Name";
	public static readonly string CountryFieldTypeName = "string";
	public static readonly string CountryFieldNameCurrencyId = "Defaultcurrency_Id";
	public static readonly string CountryFieldTypeCurrencyId = "int";

	#endregion Country table

	#region Product table

	public static readonly string ProductTable = "product";
	public static readonly string ProductView = "view_product";
	public static readonly string ProductFieldNameId = "Id";
	public static readonly string ProductFieldTypeId = "int";
	public static readonly string ProductFieldNameCode = "Code";
	public static readonly string ProductFieldTypeCode = "string";
	public static readonly string ProductFieldNameName = "Name";
	public static readonly string ProductFieldTypeName = "string";
	public static readonly string ProductFieldNameDimensions = "Dimensions";
	public static readonly string ProductFieldTypeDimensions = "string";
	public static readonly string ProductFieldNamePrice = "Price";
	public static readonly string ProductFieldTypePrice = "double";
	public static readonly string ProductFieldNameMinimalStock = "MinimalStock";
	public static readonly string ProductFieldTypeMinimalStock = "double";
	public static readonly string ProductFieldNameStandardOrderQuantity = "StandardOrderQuantity";
	public static readonly string ProductFieldTypeStandardOrderQuantity = "double";
	public static readonly string ProductFieldNameProjectCosts = "ProjectCosts";
	public static readonly string ProductFieldTypeProjectCosts = "int";
	public static readonly string ProductFieldNameUnitId = "Unit_Id";
	public static readonly string ProductFieldTypeUnitId = "int";
	public static readonly string ProductFieldNameImageRotationAngle = "ImageRotationAngle";
	public static readonly string ProductFieldTypeImageRotationAngle = "string";
	public static readonly string ProductFieldNameImage = "Image";
	public static readonly string ProductFieldTypeImage = "blob";
	public static readonly string ProductFieldNameBrandId = "Brand_Id";
	public static readonly string ProductFieldTypeBrandId = "int";
	public static readonly string ProductFieldNameCategoryId = "Category_Id";
	public static readonly string ProductFieldTypeCategoryId = "int";
	public static readonly string ProductFieldNameStorageId = "Storage_Id";
	public static readonly string ProductFieldTypeStorageId = "int";
	public static readonly string ProductFieldNameMemo = "Memo";
	public static readonly string ProductFieldTypeMemo = "longtext";

	#endregion Product table

	#region ProductSupplier table

	public static readonly string ProductSupplierTable = "productsupplier";
	public static readonly string ProductSupplierView = "view_productsupplier";
	public static readonly string ProductSupplierFieldNameId = "Id";
	public static readonly string ProductSupplierFieldTypeId = "int";
	public static readonly string ProductSupplierFieldNameProductId = "Product_Id";
	public static readonly string ProductSupplierFieldTypeProductId = "int";
	public static readonly string ProductSupplierFieldNameProductCode = "Code";          // Not in table only needed for searching the Product table on import
	public static readonly string ProductSupplierFieldTypeProductCode = "string";        // Not in table only needed for searching the Product table on import
	public static readonly string ProductSupplierFieldNameSupplierId = "Supplier_Id";
	public static readonly string ProductSupplierFieldTypeSupplierId = "int";
	public static readonly string ProductSupplierFieldNameCurrencyId = "Currency_Id";
	public static readonly string ProductSupplierFieldTypeCurrencyId = "int";
	public static readonly string ProductSupplierFieldNameProductNumber = "ProductNumber";
	public static readonly string ProductSupplierFieldTypeProductNumber = "string";
	public static readonly string ProductSupplierFieldNameProductName = "ProductName";
	public static readonly string ProductSupplierFieldTypeProductName = "string";
	public static readonly string ProductSupplierFieldNamePrice = "Price";
	public static readonly string ProductSupplierFieldTypePrice = "double";
	public static readonly string ProductSupplierFieldNameProductUrl = "Url";
	public static readonly string ProductSupplierFieldTypeProductUrl = "string";
	public static readonly string ProductSupplierFieldNameDefaultSupplier = "DefaultSupplier";
	public static readonly string ProductSupplierFieldTypeDefaultSupplier = "string";

	#endregion ProductSupplier table

	#region Project table

	public static readonly string ProjectTable = "project";
	public static readonly string ProjectFieldNameId = "Id";
	public static readonly string ProjectFieldTypeId = "int";
	public static readonly string ProjectFieldNameName = "Name";
	public static readonly string ProjectFieldTypeName = "string";
	public static readonly string ProjectFieldNameCode = "Code";
	public static readonly string ProjectFieldTypeCode = "string";
	public static readonly string ProjectFieldNameStartDate = "StartDate";
	public static readonly string ProjectFieldTypeStartDate = "date";
	public static readonly string ProjectFieldNameEndDate = "EndDate";
	public static readonly string ProjectFieldTypeEndDate = "date";
	public static readonly string ProjectFieldNameClosed = "Closed";
	public static readonly string ProjectFieldTypeClosed = "int";
	public static readonly string ProjectFieldNameExpectedTime = "ExpectedTime";
	public static readonly string ProjectFieldTypeExpectedTime = "int";
	public static readonly string ProjectFieldNameMemo = "Memo";
	public static readonly string ProjectFieldTypeMemo = "longtext";
	public static readonly string ProjectFieldNameImageRotationAngle = "ImageRotationAngle";
	public static readonly string ProjectFieldTypeImageRotationAngle = "string";
	public static readonly string ProjectFieldNameImage = "Image";
	public static readonly string ProjectFieldTypeImage = "blob";

	public static readonly string ProjectCostsView = "view_projectcosts";
	public static readonly string ProjectCostsViewFieldNameProjectName = "ProjectName";
	public static readonly string ProjectCostsViewFieldTypeProjectName = "string";
	public static readonly string ProjectCostsViewFieldNameCategoryName = "Category";
	public static readonly string ProjectCostsViewFieldTypeCategoryName = "string";
	public static readonly string ProjectCostsViewFieldNameProductName = "ProductName";
	public static readonly string ProjectCostsViewFieldTypeProductName = "string";
	public static readonly string ProjectCostsViewFieldNameAmountUsed = "AmountUsed";
	public static readonly string ProjectCostsViewFieldTypeAmountUsed = "int";
	public static readonly string ProjectCostsViewFieldNamePrice = "Price";
	public static readonly string ProjectCostsViewFieldTypePrice = "double";
	public static readonly string ProjectCostsViewFieldNameTotal = "Total";
	public static readonly string ProjectCostsViewFieldTypeTotal = "double";

	public static readonly string ProjectExportView = "view_projectexport";

	#endregion Project table

	#region Stock table

	public static readonly string StockTable = "stock";
	public static readonly string StockView = "view_stock";
	public static readonly string StockFieldNameId = "Id";
	public static readonly string StockFieldTypeId = "int";
	public static readonly string StockFieldNameProductId = "product_Id";
	public static readonly string StockFieldTypeProductId = "int";
	public static readonly string StockFieldNameAmount = "Amount";
	public static readonly string StockFieldTypeAmount = "double";
	public static readonly string StockViewFieldNameId = "Id";
	public static readonly string StockViewFieldTypeId = "int";
	public static readonly string StockViewFieldNameProductId = "product_Id";
	public static readonly string StockViewFieldTypeProductId = "int";
	public static readonly string StockViewFieldNameAmount = "Amount";
	public static readonly string StockViewFieldTypeAmount = "double";

	#endregion Stock table

	#region Stocklog table

	public static readonly string StocklogTable = "stocklog";
	public static readonly string StocklogFieldNameId = "Id";
	public static readonly string StocklogFieldTypeId = "int";
	public static readonly string StocklogFieldNameProductId = "product_Id";
	public static readonly string StocklogFieldTypeProductId = "int";
	public static readonly string StocklogFieldNameStorageId = "storage_Id";
	public static readonly string StocklogFieldTypeStorageId = "int";
	public static readonly string StocklogFieldNameSupplyOrderId = "supplyorder_Id";
	public static readonly string StocklogFieldTypeSupplyOrderId = "int";
	public static readonly string StocklogFieldNameProductUsageId = "productusage_Id";
	public static readonly string StocklogFieldTypeProductUsageId = "int";
	public static readonly string StocklogFieldNameSupplyOrderlineId = "supplyorderline_Id";
	public static readonly string StocklogFieldTypeSupplyOrderlineId = "int";
	public static readonly string StocklogFieldNameAmountReceived = "AmountReceived";
	public static readonly string StocklogFieldTypeAmountReceived = "double";
	public static readonly string StocklogFieldNameAmountUsed = "AmountUsed";
	public static readonly string StocklogFieldTypeAmountUsed = "double";
	public static readonly string StocklogFieldNameDate = "LogDate";
	public static readonly string StocklogFieldTypeDate = "date";

	#endregion Stocklog table

	#region Storage table

	public static readonly string StorageTable = "storage";
	public static readonly string StorageFieldNameId = "Id";
	public static readonly string StorageFieldTypeId = "int";
	public static readonly string StorageFieldNameParentId = "ParentId";
	public static readonly string StorageFieldTypeParentId = "int";
	public static readonly string StorageFieldNameName = "Name";
	public static readonly string StorageFieldTypeName = "string";
	public static readonly string StorageFieldNameFullpath = "Fullpath";
	public static readonly string StorageFieldTypeFullpath = "string";

	#endregion Storage table

	#region Supplier tables

	#region Supplier table

	public static readonly string SupplierTable = "supplier";
	public static readonly string SupplierView = "view_supplier";
	public static readonly string SupplierFieldNameId = "Id";
	public static readonly string SupplierFieldTypeId = "int";
	public static readonly string SupplierFieldNameCode = "Code";
	public static readonly string SupplierFieldTypeCode = "string";
	public static readonly string SupplierFieldNameName = "Name";
	public static readonly string SupplierFieldTypeName = "string";
	public static readonly string SupplierFieldNameAddress1 = "Address1";
	public static readonly string SupplierFieldTypeAddress1 = "string";
	public static readonly string SupplierFieldNameAddress2 = "Address2";
	public static readonly string SupplierFieldTypeAddress2 = "string";
	public static readonly string SupplierFieldNameZip = "Zip";
	public static readonly string SupplierFieldTypeZip = "string";
	public static readonly string SupplierFieldNameCity = "City";
	public static readonly string SupplierFieldTypeCity = "string";
	public static readonly string SupplierFieldNameUrl = "Url";
	public static readonly string SupplierFieldTypeUrl = "string";
	public static readonly string SupplierFieldNameMemo = "Memo";
	public static readonly string SupplierFieldTypeMemo = "longtext";
	public static readonly string SupplierFieldNameCountryId = "Country_Id";
	public static readonly string SupplierFieldTypeCountryId = "int";
	public static readonly string SupplierFieldNameCurrencyId = "Currency_Id";
	public static readonly string SupplierFieldTypeCurrencyId = "int";
	public static readonly string SupplierFieldNameShippingCosts = "ShippingCosts";
	public static readonly string SupplierFieldTypeShippingCosts = "double";
	public static readonly string SupplierFieldNameMinShippingCosts = "MinShippingCosts";
	public static readonly string SupplierFieldTypeMinShippingCosts = "double";
	public static readonly string SupplierFieldNameOrderCosts = "OrderCosts";
	public static readonly string SupplierFieldTypeOrderCosts = "double";

	#endregion Supplier table

	#region SupplierContact Table

	public static readonly string SupplierContactTable = "suppliercontact";
	public static readonly string SupplierContactView = "view_suppliercontact";
	public static readonly string SupplierContactFieldNameId = "Id";
	public static readonly string SupplierContactFieldTypeId = "int";
	public static readonly string SupplierContactFieldNameSupplierId = "Supplier_Id";
	public static readonly string SupplierContactFieldTypeSupplierId = "int";
	public static readonly string SupplierContactFieldNameName = "Name";
	public static readonly string SupplierContactFieldTypeName = "string";
	public static readonly string SupplierContactFieldNameTypeId = "Contacttype_Id";
	public static readonly string SupplierContactFieldTypeTypeId = "int";
	public static readonly string SupplierContactFieldNameTypeName = "ContactType";
	public static readonly string SupplierContactFieldTypeTypeName = "string";
	public static readonly string SupplierContactFieldNamePhone = "Phone";
	public static readonly string SupplierContactFieldTypePhone = "string";
	public static readonly string SupplierContactFieldNameMail = "Mail";
	public static readonly string SupplierContactFieldTypeMail = "string";

	#endregion SupplierContact Table

	#endregion Supplier tables

	#region Time

	public static readonly string TimeTable = "time";
	public static readonly string TimeFieldNameId = "Id";
	public static readonly string TimeFieldTypeId = "int";
	public static readonly string TimeFieldNameProjectId = "project_Id";
	public static readonly string TimeFieldTypeProjectId = "int";
	public static readonly string TimeFieldNameWorktypeId = "Worktype_Id";
	public static readonly string TimeFieldTypeWorktypeId = "int";
	public static readonly string TimeFieldNameWorkDate = "WorkDate";
	public static readonly string TimeFieldTypeWorkDate = "date";
	public static readonly string TimeFieldNameStartTime = "StartTime";
	public static readonly string TimeFieldTypeStartTime = "time";
	public static readonly string TimeFieldNameEndTime = "EndTime";
	public static readonly string TimeFieldTypeEndTime = "time";
	public static readonly string TimeFieldNameComment = "Comment";
	public static readonly string TimeFieldTypeComment = "string";

	public static readonly string TimeView = "view_time";
	public static readonly string TimeViewFieldNameId = "Id";
	public static readonly string TimeViewFieldTypeId = "int";
	public static readonly string TimeViewFieldNameProjectId = "ProjectId";
	public static readonly string TimeViewFieldTypeProjectId = "int";
	public static readonly string TimeViewFieldNameWorktypeId = "WorktypeId";
	public static readonly string TimeViewFieldTypeWorktypeId = "int";
	public static readonly string TimeViewFieldNameProjectName = "ProjectName";
	public static readonly string TimeViewFieldTypeProjectName = "string";
	public static readonly string TimeViewFieldNameWorktypeName = "WorktypeName";
	public static readonly string TimeViewFieldTypeWorktypeName = "string";
	public static readonly string TimeViewFieldNameDate = "`Date`";
	public static readonly string TimeViewFieldTypeDate = "date";
	public static readonly string TimeViewFieldNameWorkDate = "WorkDate";
	public static readonly string TimeViewFieldTypeWorkDate = "date";
	public static readonly string TimeViewFieldNameWorkTime = "WorkTime";
	public static readonly string TimeViewFieldTypeWorkTime = "string";
	public static readonly string TimeViewFieldNameStartTime = "StartTime";
	public static readonly string TimeViewFieldTypeStartTime = "string";
	public static readonly string TimeViewFieldNameEndTime = "EndTime";
	public static readonly string TimeViewFieldTypeEndTime = "string";
	public static readonly string TimeViewFieldNameElapsedTime = "ELapsedTime";
	public static readonly string TimeViewFieldTypeElapsedTime = "string";
	public static readonly string TimeViewFieldNameElapsedMinutes = "ELapsedMinutes";
	public static readonly string TimeViewFieldTypeElapsedMinutes = "string";
	public static readonly string TimeViewFieldNameComment = "Comment";
	public static readonly string TimeViewFieldTypeComment = "string";

	public static readonly string TimeReportView = "view_timereport";
	public static readonly string TimeViewFieldNameWorkedMinutes = "WorkedMinutes";
	public static readonly string TimeViewFieldTypeWorkedMinutes = "int";

	public static readonly string TimeExportView = "view_timeexport";

	#endregion Time

	#region ProductUsage

	public static readonly string ProductUsageTable = "productusage";
	public static readonly string ProductUsageFieldNameId = "Id";
	public static readonly string ProductUsageFieldTypeId = "int";
	public static readonly string ProductUsageFieldNameProjectId = "project_Id";
	public static readonly string ProductUsageFieldTypeProjectId = "int";
	public static readonly string ProductUsageFieldNameProductId = "product_Id";
	public static readonly string ProductUsageFieldTypeProductId = "int";
	public static readonly string ProductUsageFieldNameStorageId = "storage_Id";
	public static readonly string ProductUsageFieldTypeStorageId = "int";
	public static readonly string ProductUsageFieldNameAmountUsed = "AmountUsed";
	public static readonly string ProductUsageFieldTypeAmountUsed = "double";
	public static readonly string ProductUsageFieldNameUsageDate = "UsageDate";
	public static readonly string ProductUsageFieldTypeUsageDate = "date";
	public static readonly string ProductUsageFieldNameComment = "Comment";
	public static readonly string ProductUsageFieldTypeComment = "string";

	public static readonly string ProductUsageView = "view_productusage";
	public static readonly string ProductUsageViewFieldNameId = "Id";
	public static readonly string ProductUsageViewFieldTypeId = "int";
	public static readonly string ProductUsageViewFieldNameProjectId = "ProjectId";
	public static readonly string ProductUsageViewFieldTypeProjectId = "int";
	public static readonly string ProductUsageViewFieldNameProductId = "ProductId";
	public static readonly string ProductUsageViewFieldTypeProductId = "int";
	public static readonly string ProductUsageViewFieldNameStorageId = "StorageId";
	public static readonly string ProductUsageViewFieldTypeStorageId = "int";
	public static readonly string ProductUsageViewFieldNameCategoryId = "CategoryId";
	public static readonly string ProductUsageViewFieldTypeCategoryId = "int";
	public static readonly string ProductUsageViewFieldNameProjectName = "ProjectName";
	public static readonly string ProductUsageViewFieldTypeProjectName = "string";
	public static readonly string ProductUsageViewFieldNameProductName = "ProductName";
	public static readonly string ProductUsageViewFieldTypeProductName = "string";
	public static readonly string ProductUsageViewFieldNameStorageName = "StorageName";
	public static readonly string ProductUsageViewFieldTypeStorageName = "string";
	public static readonly string ProductUsageViewFieldNameCategoryName = "CategoryName";
	public static readonly string ProductUsageViewFieldTypeCategoryName = "string";
	public static readonly string ProductUsageViewFieldNameAmountUsed = "AmountUsed";
	public static readonly string ProductUsageViewFieldTypeAmountUsed = "double";
	public static readonly string ProductUsageViewFieldNameUsageDate = "UsageDate";
	public static readonly string ProductUsageViewFieldTypeUsageDate = "date";

	#endregion ProductUsage

	#region Order table and view

	public static readonly string OrderTable = "supplyorder";
	public static readonly string OrderView = "view_supplyorder";
	public static readonly string OpenOrderView = "view_supplyopenorder";
	public static readonly string OrderFieldNameId = "Id";
	public static readonly string OrderFieldTypeId = "int";
	public static readonly string OrderFieldNameClosed = "Closed";
	public static readonly string OrderFieldTypeClosed = "int";
	public static readonly string OrderFieldNameClosedDate = "ClosedDate";
	public static readonly string OrderFieldTypeClosedDate = "date";
	public static readonly string OrderFieldNameSupplierId = "Supplier_Id";
	public static readonly string OrderFieldTypeSupplierId = "int";
	public static readonly string OrderFieldNameCurrencyId = "Currency_Id";
	public static readonly string OrderFieldTypeCurrencyId = "int";
	public static readonly string OrderFieldNameOrderNumber = "OrderNumber";
	public static readonly string OrderFieldTypeOrderNumber = "string";
	public static readonly string OrderFieldNameOrderDate = "OrderDate";
	public static readonly string OrderFieldTypeOrderDate = "date";
	public static readonly string OrderFieldNameCurrencySymbol = "CurrencySymbol";
	public static readonly string OrderFieldTypeCurrencySymbol = "string";
	public static readonly string OrderFieldNameConversionRate = "CurrencyConversionRate";
	public static readonly string OrderFieldTypeConversionRate = "double";
	public static readonly string OrderFieldNameShippingCosts = "ShippingCosts";
	public static readonly string OrderFieldTypeShippingCosts = "double";
	public static readonly string OrderFieldNameOrderCosts = "OrderCosts";
	public static readonly string OrderFieldTypeOrderCosts = "double";
	public static readonly string OrderFieldNameOrderMemo = "Memo";
	public static readonly string OrderFieldTypeOrderMemo = "longtext";

	public static readonly string OrderLineTable = "supplyorderline";
	public static readonly string OrderLineView = "view_supplyorderline";
	public static readonly string OpenOrderLineView = "view_supplyopenorderline";
	public static readonly string OrderLineFieldNameId = "Id";
	public static readonly string OrderLineFieldTypeId = "int";
	public static readonly string OrderLineFieldNameSupplierId = "Supplier_Id";
	public static readonly string OrderLineFieldTypeSupplierId = "int";
	public static readonly string OrderLineFieldNameProductId = "Product_Id";
	public static readonly string OrderLineFieldTypeProductId = "int";
	public static readonly string OrderLineFieldNameProjectId = "Project_Id";
	public static readonly string OrderLineFieldTypeProjectId = "int";
	public static readonly string OrderLineFieldNameCategoryId = "Category_Id";
	public static readonly string OrderLineFieldTypeCategoryId = "int";
	public static readonly string OrderLineFieldNameSupplierProductName = "SupplierProductName";
	public static readonly string OrderLineFieldTypeSupplierProductName = "string";
	public static readonly string OrderLineFieldNameOrderId = "supplyorder_Id";
	public static readonly string OrderLineFieldTypeOrderId = "int";
	public static readonly string OrderLineFieldNameAmount = "Amount";
	public static readonly string OrderLineFieldTypeAmount = "double";
	public static readonly string OrderLineFieldNameOpenAmount = "OpenAmount";
	public static readonly string OrderLineFieldTypeOpenAmount = "double";
	public static readonly string OrderLineFieldNamePrice = "Price";
	public static readonly string OrderLineFieldTypePrice = "double";
	public static readonly string OrderLineFieldNameRealRowTotal = "RealRowTotal";
	public static readonly string OrderLineFieldTypeRealRowTotal = "double";
	public static readonly string OrderLineFieldNameClosed = "Closed";
	public static readonly string OrderLineFieldTypeClosed = "int";
	public static readonly string OrderLineFieldNameClosedDate = "ClosedDate";
	public static readonly string OrderLineFieldTypeClosedDate = "date";

	public static readonly string OpenOrderLineFieldNameSupplyOrderId = "Supplyorder_Id";
	public static readonly string OpenOrderLineFieldTypeSupplyOrderId = "int";

	#endregion Order table and view

	#region Unit Table

	public static readonly string UnitTable = "unit";
	public static readonly string UnitFieldNameUnitId = "Id";
	public static readonly string UnitFieldTypeUnitId = "int";
	public static readonly string UnitFieldNameUnitName = "Name";
	public static readonly string UnitFieldTypeUnitName = "string";

	#endregion Unit Table

	#region Worktype Table

	public static readonly string WorktypeTable = "worktype";
	public static readonly string WorktypeFieldNameId = "Id";
	public static readonly string WorktypeFieldTypeId = "int";
	public static readonly string WorktypeFieldNameParentId = "ParentId";
	public static readonly string WorktypeFieldTypeParentId = "int";
	public static readonly string WorktypeFieldNameName = "Name";
	public static readonly string WorktypeFieldTypeName = "string";
	public static readonly string WorktypeFieldNameFullpath = "FullPath";
	public static readonly string WorktypeFieldTypeFullpath = "string";

	#endregion Worktype Table
}