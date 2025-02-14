using Application = System.Windows.Application;

namespace Modelbouwer.Helper;
/// <summary>
/// Te Database commands helper.
/// This helper contains function and method that perform Database actions
/// </summary>
public class DBCommands
{
	#region GetData
	#region GetData unsorted
	public static DataTable GetData( string _table )
	{
		string selectQuery = $"" +
			$"{DBNames.SqlSelectAll}{DBNames.SqlFrom}{DBNames.Database}.{_table}";

		return GetTable( selectQuery );
	}
	#endregion GetData unsorted

	#region GetData Sorted
	public static DataTable GetData( string _table, string _orderByFieldName )
	{
		string selectQuery =  _orderByFieldName.Equals( "nosort", StringComparison.CurrentCultureIgnoreCase )
			?  $"" +
				$"{DBNames.SqlSelectAll}{DBNames.SqlFrom}{DBNames.Database}.{_table}"
			:  $"" +
				$"{DBNames.SqlSelectAll}{DBNames.SqlFrom}{DBNames.Database}.{_table}" +
				$"{DBNames.SqlOrder}{_orderByFieldName}" ;
		return GetTable( selectQuery );
	}
	#endregion GetData Sorted

	#region GetData Sorted and filtered
	public static DataTable GetData( string _table, string _orderByFieldName, string _whereFieldName, string _whereFieldValue )
	{
		string selectQuery =  _orderByFieldName.Equals( "nosort", StringComparison.CurrentCultureIgnoreCase )
			?  $"" +
				$"{DBNames.SqlSelectAll}{DBNames.SqlFrom}{DBNames.Database}.{_table}" +
				$"{DBNames.SqlWhere}{_whereFieldName} = '{_whereFieldValue}';"
			:  $"" +
				$"{DBNames.SqlSelectAll}{DBNames.SqlFrom}{DBNames.Database}.{_table}" +
				$"{DBNames.SqlWhere}{_whereFieldName} = '{_whereFieldValue}'" +
				$"{DBNames.SqlOrder}{_orderByFieldName};" ;
		return GetTable( selectQuery );
	}
	#endregion GetData Sorted and filtered

	#region Get data sorted, and filtered on two criteria
	public static DataTable GetData( string _table, string _orderByFieldName, string _whereFieldName, string _whereFieldValue, string _andWhereFieldName, string _andWhereFieldValue )
	{
		string selectQuery =  _orderByFieldName.Equals( "nosort", StringComparison.CurrentCultureIgnoreCase )
			?  $"" +
				$"{DBNames.SqlSelectAll}{DBNames.SqlFrom}{DBNames.Database}.{_table}" +
				$"{DBNames.SqlWhere}{_whereFieldName} = '{_whereFieldValue}'" +
				$"{DBNames.SqlAnd}{_andWhereFieldName} = '{_andWhereFieldValue}';"
			:  $"" +
				$"{DBNames.SqlSelectAll}{DBNames.SqlFrom}{DBNames.Database}.{_table}" +
				$"{DBNames.SqlWhere}{_whereFieldName} = '{_whereFieldValue}'" +
				$"{DBNames.SqlAnd}{_andWhereFieldName} = '{_andWhereFieldValue}'" +
				$"{DBNames.SqlOrder}{_orderByFieldName};" ;
		MySqlConnection connection = new(DBConnect.ConnectionString);
		connection.Open();
		DataTable table = new();
		MySqlDataAdapter adapter = new(selectQuery, connection);
		_ = adapter.Fill( table );
		connection.Close();
		//return GetTable( selectQuery );
		return table;
	}
	#endregion Get data sorted, and filtered on two criteria

	#region Get Field(s) from table
	public static string GetData( string _table, string [ , ] _whereFields, string [ , ] _fields )
	{
		// There is an Id or String available for each condition, so one of them has a value the other one is 0 or ""
		StringBuilder sqlQuery = new();
		_ = sqlQuery.Append( DBNames.SqlSelect );
		string prefix = "";

		for ( int i = 0; i < _fields.GetLength( 0 ); i++ )
		{
			if ( i != 0 )
			{ prefix = ", "; }
			_ = sqlQuery.Append( $"{prefix}{_fields [ i, 0 ]}" );
		}

		_ = sqlQuery.Append( $"{DBNames.SqlFrom}{_table.ToLower()} " );
		prefix = "";

		if ( _whereFields.GetLength( 0 ) > 0 )
		{
			_ = sqlQuery.Append( DBNames.SqlWhere );

			for ( int i = 0; i < _whereFields.GetLength( 0 ); i++ )
			{
				if ( i != 0 )
				{ prefix = DBNames.SqlAnd; }
				_ = sqlQuery.Append( $"{prefix}{_whereFields [ i, 0 ]} = @{_whereFields [ i, 0 ]}" );
			}
		}

		MySqlConnection connection = new(DBConnect.ConnectionString);

		connection.Open();

		MySqlCommand cmd = new (sqlQuery.ToString(), connection);

		for ( int i = 0; i < _whereFields.GetLength( 0 ); i++ )
		{
			switch ( _whereFields [ i, 1 ].ToLower() )
			{
				case "string":
					cmd.Parameters.Add( $"@{_whereFields [ i, 0 ]}", MySqlDbType.String ).Value = _whereFields [ i, 2 ];
					break;

				case "int":
					cmd.Parameters.Add( $"@{_whereFields [ i, 0 ]}", MySqlDbType.Int32 ).Value = int.Parse( _whereFields [ i, 2 ] );
					break;

				case "double":
					cmd.Parameters.Add( $"@{_whereFields [ i, 0 ]}", MySqlDbType.Double ).Value = double.Parse( _whereFields [ i, 2 ] );
					break;

				case "float":
					cmd.Parameters.Add( $"@{_whereFields [ i, 0 ]}", MySqlDbType.Float ).Value = float.Parse( _whereFields [ i, 2 ] );
					break;

				case "longtext":
					cmd.Parameters.Add( $"@{_whereFields [ i, 0 ]}", MySqlDbType.LongText ).Value = DBNull.Value;
					break;

				case "longblob":
					cmd.Parameters.Add( $"@{_whereFields [ i, 0 ]}", MySqlDbType.Blob ).Value = DBNull.Value;
					break;

				case "date":
					string[] _tempDates = _whereFields[i, 2].Split("-");

					// Add leading zero's to date and month
					_ = new
					// Add leading zero's to date and month
					GeneralHelper();
					string _tempDate = _tempDates[2] + "-" + GeneralHelper.AddZeros(_tempDates[1], 2) + "-" + GeneralHelper.AddZeros(_tempDates[0], 2);
					cmd.Parameters.Add( $"@{_whereFields [ i, 0 ]}", MySqlDbType.String ).Value = _tempDate;
					break;
			}
		}

		string resultString = "";
		int resultInt;
		double resultDouble;
		float resultFloat;

		if ( _fields [ 0, 1 ].ToLower() is "string" or "date" or "time" )
		{ resultString = ( string ) cmd.ExecuteScalar(); }
		if ( _fields [ 0, 1 ].ToLower() == "int" )
		{ resultInt = ( int ) cmd.ExecuteScalar(); resultString = resultInt.ToString(); }
		if ( _fields [ 0, 1 ].ToLower() == "double" )
		{ resultDouble = ( double ) cmd.ExecuteScalar(); resultString = resultDouble.ToString(); }
		if ( _fields [ 0, 1 ].ToLower() == "float" )
		{ resultFloat = ( float ) cmd.ExecuteScalar(); resultString = resultFloat.ToString(); }

		return resultString;
	}
	#endregion Get Field(s) from table

	#region Get the data table based on the select query
	private static DataTable GetTable( string _sqlQuery )
	{
		MySqlConnection connection = new(DBConnect.ConnectionString);
		connection.Open();
		DataTable table = new();
		MySqlDataAdapter adapter = new(_sqlQuery, connection);
		_ = adapter.Fill( table );
		connection.Close();

		return table;
	}
	#endregion Get the data table based on the select query

	#region Get the latet inserted record Id from a specified table
	public static string GetLatestIdFromTable( string _table )
	{
		// There is an Id or String available for each condition, so one of them has a value the other one is 0 or ""
		string sqlQuery = $"" +
			$"{DBNames.SqlSelect}{DBNames.SqlMax}Id)" +
			$"{DBNames.SqlFrom}{DBNames.Database}.{_table.ToLower()}";

		MySqlConnection connection = new(DBConnect.ConnectionString);

		connection.Open();

		MySqlCommand cmd = new(sqlQuery, connection);

		string resultString = ((int)cmd.ExecuteScalar()).ToString();

		return resultString;
	}
	#endregion
	#endregion

	#region Fill lists
	#region BrandList
	public static ObservableCollection<BrandModel> GetBrandList( ObservableCollection<BrandModel>? brandList = null )
	{
		brandList ??= [ ];
		DataTable? _dt = GetData( DBNames.BrandTable, DBNames.BrandFieldNameName );

		for ( int i = 0; i < _dt.Rows.Count; i++ )
		{
			brandList.Add( new BrandModel
			{
				BrandId = DatabaseValueConverter.GetInt( _dt.Rows [ i ] [ 0 ] ),
				BrandName = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 1 ] ),
			} );
		}
		return brandList;
	}
	#endregion

	#region CategoryList
	public ObservableCollection<CategoryModel> GetCategoryList( ObservableCollection<CategoryModel>? categoryList = null )
	{
		categoryList ??= [ ];
		DataTable? _dt = GetData( DBNames.CategoryTable, DBNames.CategoryFieldNameName );

		for ( int i = 0; i < _dt.Rows.Count; i++ )
		{
			int? _parent = null;
			if ( _dt.Rows [ i ] [ 1 ] != DBNull.Value ) { _parent = DatabaseValueConverter.GetInt( _dt.Rows [ i ] [ 1 ] ); }

			if ( _parent != null )
			{
				categoryList.Add( new CategoryModel
				{
					CategoryId = DatabaseValueConverter.GetInt( _dt.Rows [ i ] [ 0 ] ),
					CategoryParentId = ( int ) _parent,
					CategoryName = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 2 ] )
				} );
			}
			else
			{
				categoryList.Add( new CategoryModel
				{
					CategoryId = DatabaseValueConverter.GetInt( _dt.Rows [ i ] [ 0 ] ),
					CategoryName = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 2 ] )
				} );
			}

		}
		return GetCategoryHierarchy( categoryList );
	}

	private ObservableCollection<CategoryModel> GetCategoryHierarchy( ObservableCollection<CategoryModel>? categoryList )
	{
		ILookup<int?, CategoryModel> lookup = categoryList.ToLookup( c => c.CategoryParentId )  ;

		foreach ( CategoryModel category in categoryList )
		{
			category.SubCategories = lookup [ category.CategoryId ].ToObservableCollection();
		}

		return lookup [ null ].ToObservableCollection();
	}


	public List<CategoryModel> GetFlatCategoryList( List<CategoryModel>? categoryList = null )
	{
		categoryList ??= [ ];
		DataTable? _dt = GetData( DBNames.CategoryTable, DBNames.CategoryFieldNameName );

		for ( int i = 0; i < _dt.Rows.Count; i++ )
		{
			int? _parent = null;
			if ( _dt.Rows [ i ] [ 1 ] != DBNull.Value ) { _parent = DatabaseValueConverter.GetInt( _dt.Rows [ i ] [ 1 ] ); }

			if ( _parent != null )
			{
				categoryList.Add( new CategoryModel
				{
					CategoryId = DatabaseValueConverter.GetInt( _dt.Rows [ i ] [ 0 ] ),
					CategoryParentId = ( int ) _parent,
					CategoryName = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 2 ] )
				} );
			}
			else
			{
				categoryList.Add( new CategoryModel
				{
					CategoryId = DatabaseValueConverter.GetInt( _dt.Rows [ i ] [ 0 ] ),
					CategoryName = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 2 ] )
				} );
			}
		}
		return categoryList;
	}
	#endregion CategoryList

	#region ContactList
	public static ObservableCollection<SupplierContactModel> GetContactList( ObservableCollection<SupplierContactModel>? contactList = null )
	{
		contactList ??= [ ];
		DataTable? _dt = GetData( DBNames.SupplierContactView, DBNames.SupplierContactFieldNameName );

		for ( int i = 0; i < _dt.Rows.Count; i++ )
		{
			contactList.Add( new SupplierContactModel
			{
				SupplierContactId = DatabaseValueConverter.GetInt( _dt.Rows [ i ] [ 0 ] ),
				SupplierContactSuppplierId = DatabaseValueConverter.GetInt( _dt.Rows [ i ] [ 1 ] ),
				SupplierContactName = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 2 ] ),
				SupplierContactContactTypeId = DatabaseValueConverter.GetInt( _dt.Rows [ i ] [ 3 ] ),
				SupplierContactContactType = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 4 ] ),
				SupplierContactMail = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 5 ] ),
				SupplierContactPhone = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 6 ] )
			} );
		}
		return contactList;
	}
	#endregion

	#region ContactTypeList
	public static ObservableCollection<SupplierContactTypeModel> GetContactTypeList( ObservableCollection<SupplierContactTypeModel>? _list = null )
	{
		_list ??= [ ];
		DataTable? _dt = GetData( DBNames.ContactTypeTable, DBNames.ContactTypeFieldNameName );

		for ( int i = 0; i < _dt.Rows.Count; i++ )
		{
			_list.Add( new SupplierContactTypeModel
			{
				ContactTypeId = DatabaseValueConverter.GetInt( _dt.Rows [ i ] [ 0 ] ),
				ContactTypeName = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 1 ] ),
			} );
		}
		return _list;
	}
	#endregion Brand

	#region CountryList
	public static ObservableCollection<CountryModel> GetCountryList( ObservableCollection<CountryModel>? countryList = null )
	{
		countryList ??= [ ];
		DataTable? _dt = GetData( DBNames.CountryView, DBNames.CountryFieldNameName );

		for ( int i = 0; i < _dt.Rows.Count; i++ )
		{
			countryList.Add( new CountryModel
			{
				CountryId = DatabaseValueConverter.GetInt( _dt.Rows [ i ] [ 0 ] ),
				CountryCode = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 1 ] ),
				CountryName = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 2 ] ),
				CountryCurrencyId = DatabaseValueConverter.GetInt( _dt.Rows [ i ] [ 3 ] ),
				CountryCurrencySymbol = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 4 ] )
			} );
		}
		return countryList;
	}
	public static ObservableCollection<CountryViewModel> GetCountryViewList( ObservableCollection<CountryViewModel>? countryList = null )
	{
		countryList ??= [ ];
		DataTable? _dt = GetData( DBNames.CountryView, DBNames.CountryFieldNameName );

		for ( int i = 0; i < _dt.Rows.Count; i++ )
		{
			countryList.Add( new CountryViewModel
			{
				CountryId = DatabaseValueConverter.GetInt( _dt.Rows [ i ] [ 0 ] ),
				CountryCode = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 1 ] ),
				CountryName = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 2 ] ),
				CountryCurrencyId = DatabaseValueConverter.GetInt( _dt.Rows [ i ] [ 3 ] ),
				CountryCurrencySymbol = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 4 ] )
			} );
		}
		return countryList;
	}
	#endregion CountryList

	#region CurrencyList
	public ObservableCollection<CurrencyModel> GetCurrencyList( ObservableCollection<CurrencyModel>? _list = null )
	{
		_list ??= [ ];
		DataTable? _dt = GetData( DBNames.CurrencyTable, DBNames.CurrencyFieldNameName );

		for ( int i = 0; i < _dt.Rows.Count; i++ )
		{
			_list.Add( new CurrencyModel
			{
				CurrencyId = DatabaseValueConverter.GetInt( _dt.Rows [ i ] [ 0 ] ),
				CurrencyCode = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 1 ] ),
				CurrencySymbol = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 2 ] ),
				CurrencyName = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 3 ] ),
				CurrencyConversionRate = DatabaseValueConverter.GetDouble( _dt.Rows [ i ] [ 4 ] )
			} );
		}
		return _list;
	}
	#endregion CurrencyList

	#region Inventory
	#region Get inventory list
	public static ObservableCollection<InventoryModel> GetInventory( ObservableCollection<InventoryModel>? _list = null )
	{
		_list ??= [ ];
		DataTable? _dt = GetData( DBNames.ProductInventoryView, DBNames.ProductInventoryViewFieldNameProductCode );

		for ( int i = 0; i < _dt.Rows.Count; i++ )
		{
			var _price = double.Parse(_dt.Rows [ i ] [ 3 ].ToString());
			var _value = double.Parse(_dt.Rows [ i ] [ 8 ].ToString());
			var _virtualValue = double.Parse(_dt.Rows [ i ] [ 11 ].ToString());
			_list.Add( new InventoryModel
			{
				ProductId = DatabaseValueConverter.GetInt( _dt.Rows [ i ] [ 0 ] ),
				ProductCode = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 1 ] ),
				ProductName = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 2 ] ),
				ProductPrice = _price,
				ProductMinimalStock = DatabaseValueConverter.GetDouble( _dt.Rows [ i ] [ 4 ] ),
				ProductCategory = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 5 ] ),
				ProductStorageLocation = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 6 ] ),
				ProductInventory = DatabaseValueConverter.GetDouble( _dt.Rows [ i ] [ 7 ] ),
				ProductInventoryValue = _value,
				ProductInOrder = DatabaseValueConverter.GetDouble( _dt.Rows [ i ] [ 9 ] ),
				ProductVirtualInventory = DatabaseValueConverter.GetDouble( _dt.Rows [ i ] [ 10 ] ),
				ProductVirtualInventoryValue = _virtualValue,
				ProductShortInventory = DatabaseValueConverter.GetDouble( _dt.Rows [ i ] [ 12 ] ),
				ProductTempShortInventory = DatabaseValueConverter.GetDouble( _dt.Rows [ i ] [ 13 ] )
			} );
		}
		return _list;
	}
	#endregion

	#region get Inventory list for orders
	public static ObservableCollection<InventoryOrderModel> GetInventoryOrder( int _supplierId = 0, ObservableCollection<InventoryOrderModel>? _list = null )
	{
		_list ??= [ ];
		var sqlQuery = $"{DBNames.SqlCall}{DBNames.Database}.{DBNames.ProductInventoryOrderProcedure} ({_supplierId})";
		DataTable? _dt = GetTable( sqlQuery );

		for ( int i = 0; i < _dt.Rows.Count; i++ )
		{
			if ( double.TryParse( _dt.Rows [ i ] [ 4 ].ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out double _price ) &&
	double.TryParse( _dt.Rows [ i ] [ 13 ].ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out double _supplierPrice ) )

				//var _price = double.Parse(_dt.Rows [ i ] [ 4 ].ToString(), CultureInfo.InvariantCulture);
				//var _supplierPrice = double.Parse(_dt.Rows [ i ] [ 13 ].ToString(), CultureInfo.InvariantCulture);

				_list.Add( new InventoryOrderModel
				{
					ProductId = DatabaseValueConverter.GetInt( _dt.Rows [ i ] [ 0 ] ),
					ProductCode = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 1 ] ),
					ProductName = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 2 ] ),
					SupplierProductName = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 3 ] ),
					ProductPrice = _price,
					ProductMinimalStock = DatabaseValueConverter.GetDouble( _dt.Rows [ i ] [ 5 ] ),
					ProductOrderPer = DatabaseValueConverter.GetDouble( _dt.Rows [ i ] [ 6 ] ),
					ProductCategory = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 7 ] ),
					ProductInventory = DatabaseValueConverter.GetDouble( _dt.Rows [ i ] [ 8 ] ),
					ProductInOrder = DatabaseValueConverter.GetDouble( _dt.Rows [ i ] [ 9 ] ),
					ProductShortInventory = DatabaseValueConverter.GetDouble( _dt.Rows [ i ] [ 10 ] ),
					ProductToOrder = DatabaseValueConverter.GetDouble( _dt.Rows [ i ] [ 11 ] ),
					SupplierProductNumber = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 12 ] ),
					SupplierPrice = _supplierPrice,
					SupplierCurrencyId = Convert.ToInt32( _dt.Rows [ i ] [ 14 ] ),
					SupplierCurrencySymbol = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 15 ] ),
					ProductFromSupplier = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 16 ] )
				} );
		}
		return _list;
	}
	#endregion
	#endregion

	#region StorageLocationList
	public ObservableCollection<StorageModel> GetStorageList( ObservableCollection<StorageModel>? storageList = null )
	{
		storageList ??= [ ];
		DataTable? _dt = GetData( DBNames.StorageTable, DBNames.StorageFieldNameName );

		for ( int i = 0; i < _dt.Rows.Count; i++ )
		{
			int? _parent = null;
			if ( _dt.Rows [ i ] [ 1 ] != DBNull.Value ) { _parent = ( int? ) _dt.Rows [ i ] [ 1 ]; }

			if ( _parent != null )
			{
				storageList.Add( new StorageModel
				{
					StorageId = DatabaseValueConverter.GetInt( _dt.Rows [ i ] [ 0 ] ),
					StorageParentId = ( int ) _parent,
					StorageName = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 3 ] )
				} );
			}
			else
			{
				storageList.Add( new StorageModel
				{
					StorageId = DatabaseValueConverter.GetInt( _dt.Rows [ i ] [ 0 ] ),
					StorageName = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 3 ] )
				} );
			}

		}
		return GetStorageHierarchy( storageList );
	}
	private ObservableCollection<StorageModel> GetStorageHierarchy( ObservableCollection<StorageModel>? storagelocationList )
	{
		ILookup<int?, StorageModel> lookup =      storagelocationList.ToLookup(c => c.StorageParentId )      ;
		foreach ( StorageModel storagelocation in storagelocationList )
		{
			storagelocation.SubStorage = lookup [ storagelocation.StorageId ].ToObservableCollection();
		}
		return lookup [ null ].ToObservableCollection();
	}

	public List<StorageModel> GetFlatStorageList( List<StorageModel>? storageList = null )
	{
		storageList ??= [ ];
		DataTable? _dt = GetData( DBNames.StorageTable, DBNames.StorageFieldNameName );

		for ( int i = 0; i < _dt.Rows.Count; i++ )
		{
			int? _parent = null;
			if ( _dt.Rows [ i ] [ 1 ] != DBNull.Value ) { _parent = DatabaseValueConverter.GetInt( _dt.Rows [ i ] [ 1 ] ); }

			if ( _parent != null )
			{
				storageList.Add( new StorageModel
				{
					StorageId = DatabaseValueConverter.GetInt( _dt.Rows [ i ] [ 0 ] ),
					StorageParentId = ( int ) _parent,
					StorageName = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 3 ] )
				} );
			}
			else
			{
				storageList.Add( new StorageModel
				{
					StorageId = DatabaseValueConverter.GetInt( _dt.Rows [ i ] [ 0 ] ),
					StorageName = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 3 ] )
				} );
			}
		}
		return storageList;
	}
	#endregion StorageLocationList

	#region SupplierList
	public static ObservableCollection<SupplierModel> GetSupplierList( ObservableCollection<SupplierModel>? supplierList = null )
	{
		supplierList ??= [ ];
		DataTable? _dt = GetData( DBNames.SupplierView, DBNames.SupplierFieldNameCode );

		for ( int i = 0; i < _dt.Rows.Count; i++ )
		{
			supplierList.Add( new SupplierModel
			{
				SupplierId = DatabaseValueConverter.GetInt( _dt.Rows [ i ] [ 0 ] ),
				SupplierCode = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 1 ] ),
				SupplierName = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 2 ] ),
				SupplierAddress1 = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 3 ] ),
				SupplierAddress2 = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 4 ] ),
				SupplierZip = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 5 ] ),
				SupplierCity = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 6 ] ),
				SupplierUrl = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 7 ] ),
				SupplierCountryId = DatabaseValueConverter.GetInt( _dt.Rows [ i ] [ 8 ] ),
				SupplierCountry = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 9 ] ),
				SupplierCurrencyId = DatabaseValueConverter.GetInt( _dt.Rows [ i ] [ 10 ] ),
				SupplierCurrency = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 11 ] ),
				SupplierShippingCosts = DatabaseValueConverter.GetDouble( _dt.Rows [ i ] [ 12 ] ),
				SupplierMinShippingCosts = DatabaseValueConverter.GetDouble( _dt.Rows [ i ] [ 13 ] ),
				SupplierOrderCosts = DatabaseValueConverter.GetDouble( _dt.Rows [ i ] [ 14 ] ),
				SupplierMemo = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 15 ] ),
			} );
		}
		return supplierList;
	}

	public static ObservableCollection<ProductSupplierModel> GetProductSupplierList( ObservableCollection<ProductSupplierModel>? supplierList = null )
	{
		supplierList ??= [ ];
		DataTable? _dt = GetData( DBNames.ProductSupplierView, DBNames.ProductSupplierFieldNameProductId );

		for ( int i = 0; i < _dt.Rows.Count; i++ )
		{
			var _tempCheck = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 4 ] ) == "*";
			var parsedPrice =  double.Parse(_dt.Rows [ i ] [ 10 ].ToString(), CultureInfo.InvariantCulture);

			supplierList.Add( new ProductSupplierModel
			{
				ProductSupplierId = DatabaseValueConverter.GetInt( _dt.Rows [ i ] [ 0 ] ),
				ProductSupplierProductId = DatabaseValueConverter.GetInt( _dt.Rows [ i ] [ 1 ] ),
				ProductSupplierSupplierId = DatabaseValueConverter.GetInt( _dt.Rows [ i ] [ 2 ] ),
				ProductSupplierCurrencyId = DatabaseValueConverter.GetInt( _dt.Rows [ i ] [ 3 ] ),
				ProductSupplierDefaultSupplier = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 4 ] ),
				ProductSupplierProductName = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 5 ] ),
				ProductSupplierSupplierName = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 6 ] ),
				ProductSupplierProductNumber = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 7 ] ),
				ProductSupplierURL = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 9 ] ),
				ProductSupplierPrice = parsedPrice,
				ProductSupplierCurrencySymbol = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 11 ] ),
				ProductSupplierDefaultSupplierCheck = _tempCheck
			} );
		}
		return supplierList;
	}
	#endregion

	#region SupplierOrderList
	public static ObservableCollection<SupplyOrderModel> GetSupplierOrderList( ObservableCollection<SupplyOrderModel>? orderList = null )
	{
		orderList ??= [ ];
		DataTable _dt = GetData(DBNames.OrderTable, DBNames.OrderFieldNameOrderNumber);

		for ( int i = 0; i < _dt.Rows.Count; i++ )
		{
			orderList.Add( new SupplyOrderModel
			{
				SupplyOrderId = DatabaseValueConverter.GetInt( _dt.Rows [ i ] [ 0 ] ),
				SupplyOrderSupplierId = DatabaseValueConverter.GetInt( _dt.Rows [ i ] [ 1 ] ),
				SupplyOrderCurrencyId = DatabaseValueConverter.GetInt( _dt.Rows [ i ] [ 2 ] ),
				SupplyOrderNumber = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 3 ] ),
				SupplyOrderDate = DatabaseValueConverter.GetDateOnly( _dt.Rows [ i ] [ 4 ] ),
				SupplyOrderCurrencySymbol = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 5 ] ),
				SupplyOrderCurrencyRate = DatabaseValueConverter.GetDouble( _dt.Rows [ i ] [ 6 ] ),
				SupplyOrderShippingCosts = DatabaseValueConverter.GetDouble( _dt.Rows [ i ] [ 7 ] ),
				SupplyOrderOrderCosts = DatabaseValueConverter.GetDouble( _dt.Rows [ i ] [ 8 ] ),
				SupplyOrderMemo = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 9 ] ),
				SupplyOrderClosed = DatabaseValueConverter.GetSByte( _dt.Rows [ i ] [ 10 ] ),
				SupplyOrderClosedDate = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 11 ] ),
			} );
		}
		return orderList;
	}
	#endregion

	#region SupplierOrderLineShortList
	public static ObservableCollection<SupplyOrderLineModel> GetSupplierOrderLineShortList( ObservableCollection<SupplyOrderLineModel>? orderLineList = null )
	{
		orderLineList ??= [ ];
		DataTable _dt = GetData(DBNames.OrderLineShortView, DBNames.OrderLineShortFieldNameOrderId);

		for ( int i = 0; i < _dt.Rows.Count; i++ )
		{
			orderLineList.Add( new SupplyOrderLineModel
			{
				SupplyOrderlineShortId = DatabaseValueConverter.GetInt( _dt.Rows [ i ] [ 0 ] ),
				SupplyOrderlineShortOrderId = DatabaseValueConverter.GetInt( _dt.Rows [ i ] [ 1 ] ),
				SupplyOrderlineShortProductId = DatabaseValueConverter.GetInt( _dt.Rows [ i ] [ 2 ] ),
				SupplyOrderlineShortAmount = DatabaseValueConverter.GetDouble( _dt.Rows [ i ] [ 3 ] ),
				SupplyOrderlineShortPrice = DatabaseValueConverter.GetDouble( _dt.Rows [ i ] [ 4 ] )
			} );
		}
		return orderLineList;
	}
	#endregion

	#region ProductList
	public static ObservableCollection<ProductModel> GetProductList( ObservableCollection<ProductModel>? productList = null )
	{
		productList ??= [ ];
		DataTable? _dt = GetData(DBNames.ProductTable, DBNames.ProductFieldNameCode, DBNames.ProductFieldNameHide, "0");

		for ( int i = 0; i < _dt.Rows.Count; i++ )
		{
			productList.Add( new ProductModel
			{
				ProductId = DatabaseValueConverter.GetInt( _dt.Rows [ i ] [ 0 ] ),
				ProductCode = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 1 ] ),
				ProductName = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 2 ] ),
				ProductDimensions = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 3 ] ),
				ProductPrice = DatabaseValueConverter.GetDouble( _dt.Rows [ i ] [ 4 ] ),
				ProductMinimalStock = DatabaseValueConverter.GetDouble( _dt.Rows [ i ] [ 5 ] ),
				ProductStandardQuantity = DatabaseValueConverter.GetDouble( _dt.Rows [ i ] [ 6 ] ),
				ProductProjectCosts = DatabaseValueConverter.GetInt( _dt.Rows [ i ] [ 7 ] ),
				ProductUnitId = DatabaseValueConverter.GetInt( _dt.Rows [ i ] [ 8 ] ),
				ProductImageRotationAngle = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 9 ] ),
				ProductImage = _dt.Rows [ i ] [ 10 ] != DBNull.Value ? ( byte [ ] ) _dt.Rows [ i ] [ 10 ] : GetDefaultImage(),
				ProductBrandId = DatabaseValueConverter.GetInt( _dt.Rows [ i ] [ 11 ] ),
				ProductCategoryId = DatabaseValueConverter.GetInt( _dt.Rows [ i ] [ 12 ] ),
				ProductStorageId = DatabaseValueConverter.GetInt( _dt.Rows [ i ] [ 13 ] ),
				ProductMemo = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 14 ] )
			} );
		}
		return productList;
	}
	#endregion

	#region Costs
	public static ObservableCollection<ProductUsageModel> GetCostList( ObservableCollection<ProductUsageModel>? costList = null )
	{
		costList ??= [ ];
		DataTable? _dt = GetData(DBNames.ProductUsageView, DBNames.ProductUsageFieldNameUsageDate);
		for ( int i = 0; i < _dt.Rows.Count; i++ )
		{
			costList.Add( new ProductUsageModel
			{
				ProductUsageId = DatabaseValueConverter.GetInt( _dt.Rows [ i ] [ 0 ] ),
				ProductUsageProjectId = DatabaseValueConverter.GetInt( _dt.Rows [ i ] [ 1 ] ),
				ProductUsageProductId = DatabaseValueConverter.GetInt( _dt.Rows [ i ] [ 2 ] ),
				ProductUsageUsageDate = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 3 ] ),
				ProductUsageProjectName = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 4 ] ),
				ProductUsageProductName = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 5 ] ),
				ProductUsageCategoryName = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 6 ] ),
				ProductUsageCategoryId = DatabaseValueConverter.GetInt( _dt.Rows [ i ] [ 7 ] ),
				ProductUsageAmount = DatabaseValueConverter.GetDouble( _dt.Rows [ i ] [ 8 ] ),
				ProductUsageProductPrice = DatabaseValueConverter.GetDouble( _dt.Rows [ i ] [ 9 ] ),
				ProductUsageCosts = DatabaseValueConverter.GetDouble( _dt.Rows [ i ] [ 10 ] ),
				ProductUsageComment = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 11 ] )
			} );
		}
		return costList;
	}
	#endregion

	#region Time
	public static ObservableCollection<TimeModel> GetTimeList( ObservableCollection<TimeModel>? timeList = null )
	{
		timeList ??= [ ];
		DataTable? _dt = GetData(DBNames.TimeView, DBNames.TimeViewFieldNameSortIndex);
		var _workdayName = "";

		for ( int i = 0; i < _dt.Rows.Count; i++ )
		{
			#region Deterrmine the day of the week
			switch ( ( int.Parse( _dt.Rows [ i ] [ 13 ].ToString() ) ) )
			{
				case 0:
					_workdayName = ( string ) Application.Current.Resources [ "Region.Day.Name.01" ];
					break;
				case 1:
					_workdayName = ( string ) Application.Current.Resources [ "Region.Day.Name.02" ];
					break;
				case 2:
					_workdayName = ( string ) Application.Current.Resources [ "Region.Day.Name.03" ];
					break;
				case 3:
					_workdayName = ( string ) Application.Current.Resources [ "Region.Day.Name.04" ];
					break;
				case 4:
					_workdayName = ( string ) Application.Current.Resources [ "Region.Day.Name.05" ];
					break;
				case 5:
					_workdayName = ( string ) Application.Current.Resources [ "Region.Day.Name.06" ];
					break;
				case 6:
					_workdayName = ( string ) Application.Current.Resources [ "Region.Day.Name.07" ];
					break;
				default:
					_workdayName = ( string ) Application.Current.Resources [ "Region.Day.Error" ];
					break;
			}
			#endregion

			#region Determin the ElepsedHours
			var _workedHours = TimeSpan.FromMinutes(DatabaseValueConverter.GetDouble( _dt.Rows [ i ] [ 8 ] ));
			#endregion

			timeList.Add( new TimeModel
			{
				TimeId = DatabaseValueConverter.GetInt( _dt.Rows [ i ] [ 0 ] ),
				TimeProjectId = DatabaseValueConverter.GetInt( _dt.Rows [ i ] [ 1 ] ),
				TimeProjectName = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 2 ] ),
				TimeWorktypeId = DatabaseValueConverter.GetInt( _dt.Rows [ i ] [ 3 ] ),
				TimeWorktypeName = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 4 ] ),
				TimeWorkDate = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 5 ] ),
				TimeStartTime = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 6 ] ),
				TimeEndTime = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 7 ] ),
				TimeElapsedMinutes = DatabaseValueConverter.GetDouble( _dt.Rows [ i ] [ 8 ] ),
				TimeElapsedTime = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 9 ] ),
				TimeWorkedHours = _workedHours,
				TimeComment = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 10 ] ),
				TimeYear = int.Parse( _dt.Rows [ i ] [ 11 ].ToString() ),
				TimeMonth = int.Parse( _dt.Rows [ i ] [ 12 ].ToString() ),
				TimeWorkday = int.Parse( _dt.Rows [ i ] [ 13 ].ToString() ),
				TimeWorkdayName = _workdayName,
				TimeYearMonth = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 14 ] ),
				TimeYearWorkday = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 15 ] ),
				TimeSortIndex = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 16 ] )
			} );
		}
		return timeList;
	}
	#endregion

	#region ProjectList
	public static ObservableCollection<ProjectModel> GetProjectList( ObservableCollection<ProjectModel>? projectList = null )
	{
		var HourRate = GetHourRate();
		projectList ??= [ ];
		DataTable? _dt = GetData(DBNames.ProjectTotalsView, DBNames.ProjectFieldNameCode);


		for ( int i = 0; i < _dt.Rows.Count; i++ )
		{
			bool _projectClosed = false;
			if ( _dt.Rows [ i ] [ 15 ].ToString().Equals( "true", StringComparison.CurrentCultureIgnoreCase ) ) { _projectClosed = true; }
			var workdaysToGo = double.Parse( _dt.Rows [ i ] [ 5 ].ToString() ) / 8;
			var timeToGo = double.Parse( _dt.Rows [ i ] [ 5 ].ToString() ) - double.Parse( _dt.Rows [ i ] [ 9 ].ToString() );
			var materialCosts = GetProjectMaterialCosts(DatabaseValueConverter.GetInt( _dt.Rows [ i ] [ 0 ] ));
			var timeCosts = double.Parse(_dt.Rows [ i ] [ 9 ].ToString()) * HourRate;
			var totalCosts = materialCosts + timeCosts;
			var expectedTime = int.Parse(_dt.Rows [ i ] [ 5 ].ToString());

			projectList.Add( new ProjectModel
			{
				ProjectId = DatabaseValueConverter.GetInt( _dt.Rows [ i ] [ 0 ] ),
				ProjectCode = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 1 ] ),
				ProjectName = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 2 ] ),
				ProjectStartDate = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 3 ] ),
				ProjectEndDate = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 4 ] ),
				ProjectExpectedTime = expectedTime.ToString(),
				ProjectImage = _dt.Rows [ i ] [ 6 ] != DBNull.Value ? ( byte [ ] ) _dt.Rows [ i ] [ 6 ] : GetDefaultImage(),
				ProjectImageRotationAngle = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 7 ] ),
				ProjectMemo = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 8 ] ),
				ProjectTotalTimeInHours = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 9 ] ),
				ProjectShortestWorkday = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 10 ] ),
				ProjectShortestWorkdayHours = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 11 ] ),
				ProjectLongestWorkday = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 12 ] ),
				ProjectLongestWorkdayHours = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 13 ] ),
				ProjectBuildDays = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 14 ] ),
				ProjectClosed = _projectClosed,
				ProjectAverageHoursPerDay = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 16 ] ),
				ProjectTodoTime = timeToGo.ToString(),
				ProjectExpectedWorkdays = workdaysToGo.ToString(),
				ProjectMaterialCosts = materialCosts.ToString(),
				ProjectTimeCosts = timeCosts.ToString(),
				ProjectTotalCosts = totalCosts.ToString(),
			} );
		}
		return projectList;
	}
	#endregion

	#region Get default image
	private static byte [ ] GetDefaultImage()
	{
		// Retrieve your 'NoImage' DrawingImage from the resource dictionary
		var drawingImage = (DrawingImage)System.Windows.Application.Current.Resources["noimage"];

		// Render the DrawingImage to a bitmap
		var drawingVisual = new DrawingVisual();
		using ( var context = drawingVisual.RenderOpen() )
		{
			context.DrawImage( drawingImage, new Rect( 0, 0, drawingImage.Width, drawingImage.Height ) );
		}

		// Render to bitmap (set width and height)
		var width = (int)drawingImage.Width;
		var height = (int)drawingImage.Height;
		var renderBitmap = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);
		renderBitmap.Render( drawingVisual );

		// Convert bitmap to byte[]
		var pngEncoder = new PngBitmapEncoder();
		pngEncoder.Frames.Add( BitmapFrame.Create( renderBitmap ) );

		using ( var ms = new MemoryStream() )
		{
			pngEncoder.Save( ms );
			return ms.ToArray();  // Return byte array of the PNG
		}    //return new byte[] { }; // of laad je 'NoImage' als een byte[]
	}
	#endregion

	#region UnitdList
	public static ObservableCollection<UnitModel> GetUnitList( ObservableCollection<UnitModel>? unitList = null )
	{
		unitList ??= [ ];
		DataTable? _dt = GetData( DBNames.UnitTable, DBNames.UnitFieldNameUnitName );

		for ( int i = 0; i < _dt.Rows.Count; i++ )
		{
			unitList.Add( new UnitModel
			{
				UnitId = DatabaseValueConverter.GetInt( _dt.Rows [ i ] [ 0 ] ),
				UnitName = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 1 ] ),
			} );
		}
		return unitList;
	}
	#endregion Brand

	#region WorktypeList
	public ObservableCollection<WorktypeModel> GetWorktypeList( ObservableCollection<WorktypeModel>? worktypeList = null )
	{
		worktypeList ??= [ ];
		DataTable? _dt = GetData( DBNames.WorktypeTable, DBNames.WorktypeFieldNameName );

		for ( int i = 0; i < _dt.Rows.Count; i++ )
		{
			int? _parent = null;
			if ( _dt.Rows [ i ] [ 1 ] != DBNull.Value ) { _parent = ( int? ) _dt.Rows [ i ] [ 1 ]; }

			if ( _parent != null )
			{
				worktypeList.Add( new WorktypeModel
				{
					WorktypeId = DatabaseValueConverter.GetInt( _dt.Rows [ i ] [ 0 ] ),
					WorktypeParentId = ( int ) _parent,
					WorktypeName = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 2 ] )
				} );
			}
			else
			{
				worktypeList.Add( new WorktypeModel
				{
					WorktypeId = DatabaseValueConverter.GetInt( _dt.Rows [ i ] [ 0 ] ),
					WorktypeName = DatabaseValueConverter.GetString( _dt.Rows [ i ] [ 2 ] )
				} );
			}

		}
		return GetWorktypeHierarchy( worktypeList );
	}
	private ObservableCollection<WorktypeModel> GetWorktypeHierarchy( ObservableCollection<WorktypeModel>? worktypeList )
	{
		ILookup<int?, WorktypeModel> lookup = worktypeList.ToLookup(c => c.WorktypeParentId )    ;
		foreach ( WorktypeModel worktype in worktypeList )
		{
			worktype.SubWorktypes = lookup [ worktype.WorktypeId ].ToObservableCollection();
		}
		return lookup [ null ].ToObservableCollection();
	}
	#endregion
	#endregion

	#region Get the prognosed end date of the selected project
	public static string GetProjectEndDate( int projectId )
	{
		string result = "Onbekend";

		using var connection = new MySqlConnection( DBConnect.ConnectionString );
		connection.Open();

		using var command = new MySqlCommand( DBNames.SPGeProjectEndDate, connection );
		command.CommandType = CommandType.StoredProcedure;
		command.Parameters.AddWithValue( DBNames.SPProjectEndDateInputParameter, projectId );

		var sqlResult = command.ExecuteScalar().ToString();
		if ( sqlResult != null && sqlResult != "" )
		{
			var _date = sqlResult.Split(" ");
			var _temp = _date[0].Split("-");
			result = $"{_temp [ 0 ]} {GeneralHelper.MonthName( int.Parse( _temp [ 1 ] ) )} {_temp [ 2 ]}";
		}

		return result;
	}
	#endregion

	#region Get the totalmaterial costs for a project
	public static double GetProjectMaterialCosts( int projectId )
	{
		using var connection = new MySqlConnection( DBConnect.ConnectionString );
		connection.Open();

		using var command = new MySqlCommand( DBNames.SPGeProjectMaterialCosts, connection );
		command.CommandType = CommandType.StoredProcedure;
		command.Parameters.AddWithValue( DBNames.SPProjectMaterialCostsInputParameter, projectId );

		var sqlResult = command.ExecuteScalar().ToString();

		if ( sqlResult == "" )
		{
			return 0.00;
		}
		else
		{ return double.Parse( sqlResult ); }
	}
	#endregion

	#region Get the HourRate
	public static double GetHourRate()
	{
		var result = 0.00;

		var sqlQuery = $"{DBNames.SqlSelect}{DBNames.SettingsFieldNameHourRate}{DBNames.SqlFrom}{DBNames.Database}.{DBNames.SettingsTable}";

		using var connection = new MySqlConnection( DBConnect.ConnectionString );
		connection.Open();

		using var command = new MySqlCommand( sqlQuery, connection );

		result = ( double ) command.ExecuteScalar();

		return result;
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
		using StreamWriter sw = new(_filename, false);

		if ( _needsHeader.ToLower() == "header" )
		{
			sw.WriteLine( string.Join( ";", _header ) );
		}

		foreach ( DataRow dr in _dt.Rows )
		{
			List<string> rowValues = [];

			foreach ( string header in _header )
			{
				if ( _dt.Columns.Contains( header ) )
				{
					string value = dr[header]?.ToString() ?? string.Empty;

					// When a value contains a ; the value will be surrounded by quotes
					if ( value.Contains( ';' ) )
					{
						value = $"\"{value}\"";
					}

					rowValues.Add( value );
				}
			}

			sw.WriteLine( string.Join( ";", rowValues ) );
		}
	}
	#endregion Export data to CSV file

	#region Insert data in the database
	#region Insert new record in Table
	public static string InsertInTable( string _sqlQuery )
	{
		string result = string.Empty;

		try
		{
			ExecuteNonQuery( _sqlQuery );
		}
		catch ( MySqlException ex )
		{
			Debug.WriteLine( "Error (Insert in _table - MySqlException): " + ex.Message );
			throw;
		}
		catch ( Exception ex )
		{
			Debug.WriteLine( "Error (Insert in _table): " + ex.Message );
			throw;
		}
		return result;
	}

	public static string InsertInTable( string _table, string [ , ] _fields )
	{
		string sqlQuery = $"{DBNames.SqlInsert}{DBNames.Database}.{_table} ";

		string sqlFields = "( ";
		string sqlValues = "( ";
		string prefix = "";

		for ( int i = 0; i < _fields.GetLength( 0 ); i++ )
		{
			if ( i != 0 )
			{ prefix = ", "; }
			sqlFields = string.Concat( $"{sqlFields}{prefix}`{_fields [ i, 0 ]}`" );
			sqlValues = string.Concat( $"{sqlValues}{prefix}@{_fields [ i, 0 ]}" );
		}

		sqlFields += " )";
		sqlValues += " )";

		sqlQuery = string.Concat( $"{sqlQuery}{sqlFields}{DBNames.SqlValues}{sqlValues};" );

		string result;
		try
		{
			int rowsAffected = ExecuteNonQueryTable(sqlQuery, _fields);

			result = rowsAffected > 0 ? "Rij toegevoegd." : "Rij niet toegevoegd.";
		}
		catch ( MySqlException ex )
		{
			Debug.WriteLine( "Error (Insert in _table - MySqlException): " + ex.Message );
			throw;
		}
		catch ( Exception ex )
		{
			Debug.WriteLine( "Error (Insert in _table): " + ex.Message );
			throw;
		}
		return result;
	}

	// used for Generic imput
	public static void InsertInTable<T>( List<T> itemsToInsert, string tableName, string [ ] headers, Dictionary<string, string> headerToPropertyMap ) where T : INameable, new()
	{
		foreach ( T item in itemsToInsert )
		{
			List<string> columns = [];
			List<string> values = [];

			foreach ( string header in headers )
			{
				string propertyName = headerToPropertyMap.ContainsKey(header) ? headerToPropertyMap[header] : header;
				System.Reflection.PropertyInfo? propertyInfo = typeof( T ).GetProperty( propertyName );

				if ( propertyInfo != null )
				{
					object? value = propertyInfo.GetValue( item );

					// Als het gaat om de ParentId (of vergelijkbare eigenschap) en de waarde is 0, voeg NULL toe
					if ( propertyName.ToLower().Contains( "parentid" ) && value is int intValue && intValue == 0 )
					{
						values.Add( "NULL" );
					}
					else if ( value is string strValue )
					{
						// Voeg aanhalingstekens toe om stringwaarden te omringen
						values.Add( $"'{strValue}'" );
					}
					else
					{
						// Voor andere waarden gewoon direct toevoegen
						values.Add( value?.ToString() ?? "NULL" );
					}

					columns.Add( header ); // Voeg kolomnamen toe
				}
			}

			// Build the SQL query string
			string columnsJoined = string.Join(", ", columns);
			string valuesJoined = string.Join(", ", values);
			string sqlQuery = $"INSERT INTO {DBNames.Database}.{tableName} ({columnsJoined}) VALUES ({valuesJoined});";

			ExecuteNonQuery( StringHelper.EscapeBackslashes( sqlQuery ) );
		}
	}

	#endregion Insert new record in Table

	#region Insert Fields and Image in table
	public static string InsertInTable( string _table, string [ , ] _fields, byte [ ] _image, string _imageFieldName )
	{
		string sqlQuery = $"{DBNames.SqlInsert}{_table} ";

		string sqlFields = "(";
		string sqlValues = "(";
		string prefix = "";

		for ( int i = 0; i < _fields.GetLength( 0 ); i++ )
		{
			if ( i != 0 )
			{ prefix = ", "; }
			sqlFields = string.Concat( sqlFields, prefix, _fields [ i, 0 ] );
			sqlValues = string.Concat( sqlValues, prefix, "@", _fields [ i, 0 ] );
		}

		sqlFields = string.Concat( $"{sqlFields}, {_imageFieldName})" );
		sqlValues = string.Concat( $"{sqlValues}, @{_imageFieldName})" );

		sqlQuery = $"{sqlQuery}{sqlFields} {DBNames.SqlValues} {sqlValues};";

		string result;
		try
		{
			int rowsAffected = ExecuteNonQueryTable(sqlQuery, _fields, _image, _imageFieldName);

			result = rowsAffected > 0 ? "Rij toegevoegd." : "Rij niet toegevoegd.";
		}
		catch ( MySqlException ex )
		{
			Debug.WriteLine( "Error (Insert in _table - MySqlException): " + ex.Message );
			throw;
		}
		catch ( Exception ex )
		{
			Debug.WriteLine( "Error (Insert in _table): " + ex.Message );
			throw;
		}
		return result;
	}
	#endregion Insert Fields and Image in table
	#endregion Insert data in the database

	#region Update record in database
	public static string UpdateInTable( string _table, string [ , ] _fieldsToUpdate, string [ , ] _whereFields )
	{
		string sqlQuery = $"{DBNames.SqlUpdate}{DBNames.Database}.{_table} {DBNames.SqlSet}";
		string sqlUpdateFields = "", sqlWhereFields = $"{DBNames.SqlWhere}", prefix = "";

		// Concatenate the fields to change
		for ( int i = 0; i < _fieldsToUpdate.GetLength( 0 ); i++ )
		{
			if ( i != 0 )
			{ prefix = ", "; }
			sqlUpdateFields = string.Concat( $"{sqlUpdateFields}{prefix}`{_fieldsToUpdate [ i, 0 ]}` = @{_fieldsToUpdate [ i, 0 ]}" );
		}

		prefix = "";

		// Concatenate the condition fields
		for ( int i = 0; i < _whereFields.GetLength( 0 ); i++ )
		{
			if ( i != 0 )
			{ prefix = ", "; }
			sqlWhereFields = string.Concat( $"{sqlWhereFields}{prefix}`{_whereFields [ i, 0 ]}` = @{_whereFields [ i, 0 ]}" );
		}

		sqlQuery = string.Concat( $"{sqlQuery}{sqlUpdateFields}{sqlWhereFields};" );

		string result;
		try
		{
			int rowsAffected = ExecuteNonQueryTable(sqlQuery, _whereFields, _fieldsToUpdate);

			result = rowsAffected > 0 ? $"{GeneralHelper.GetResourceString( "Maintanance.Statusline.Updated" )}." : $"{GeneralHelper.GetResourceString( "Maintanance.Statusline.NotUpdated" )}.";
		}
		catch ( MySqlException ex )
		{
			Debug.WriteLine( "Error (Insert in _table - MySqlException): " + ex.Message );
			throw;
		}
		catch ( Exception ex )
		{
			Debug.WriteLine( "Error (Insert in _table): " + ex.Message );
			throw;
		}
		return result;
	}
	#endregion Update record in database

	#region Update Image in table
	public static string UpdateImageInTable( string _table, string [ , ] _whereFields, byte [ ] _imageContent, string _imageFieldName )
	{
		string result = string.Empty;
		StringBuilder sqlQuery = new();

		sqlQuery.Append( $"{DBNames.SqlUpdate}{_table.ToLower()}{DBNames.SqlSet}{_imageFieldName} = @{_imageFieldName}{DBNames.SqlWhere}" );
		string prefix = "";

		for ( int i = 0; i < _whereFields.GetLength( 0 ); i++ )
		{
			if ( i != 0 )
			{ prefix = ", "; }
			sqlQuery.Append( $"{prefix}{_whereFields [ i, 0 ]} = @{_whereFields [ i, 0 ]}" );
		}

		try
		{
			int rowsAffected = ExecuteNonQueryTable(sqlQuery.ToString(), _whereFields, _imageContent, _imageFieldName);

			result = rowsAffected > 0 ? "Rij toegevoegd." : "Rij niet toegevoegd.";
		}
		catch ( MySqlException ex )
		{
			Debug.WriteLine( "Error (Insert in _table - MySqlException): " + ex.Message );
			throw;
		}
		catch ( Exception ex )
		{
			Debug.WriteLine( "Error (Insert in _table): " + ex.Message );
			throw;
		}

		return result;
	}
	#endregion

	#region Update Memo Field in Table
	public static string UpdateMemoFieldInTable( string _table, string [ , ] _whereFields, string _memoFieldName, string _memoFieldContent )
	{
		string result = string.Empty;
		StringBuilder sqlQuery = new();
		sqlQuery.Append( $"{DBNames.SqlUpdate}{_table.ToLower()}{DBNames.SqlSet}{_memoFieldName} = @{_memoFieldName}" );

		sqlQuery.Append( DBNames.SqlWhere );
		string prefix = "";

		for ( int i = 0; i < _whereFields.GetLength( 0 ); i++ )
		{
			if ( i != 0 )
			{ prefix = ", "; }
			sqlQuery.Append( $"{prefix}{_whereFields [ i, 0 ]} = @{_whereFields [ i, 0 ]}" );
		}

		try
		{
			int rowsAffected = ExecuteNonQueryTable(sqlQuery.ToString(), _whereFields, _memoField: _memoFieldName, _memoContent: _memoFieldContent);

			if ( rowsAffected > 0 )
			{

				result = "Rij toegevoegd.";
			}
			else
			{
				result = "Rij niet toegevoegd.";
			}
		}
		catch ( MySqlException ex )
		{
			Debug.WriteLine( "Error (Update _table - MySqlException): " + ex.Message );
			throw;
		}
		catch ( Exception ex )
		{
			Debug.WriteLine( "Error (Update _table): " + ex.Message );
			throw;
		}
		return result;
	}
	#endregion

	#region Replace start of FullPath with changed FullPath
	public static void ChangeFullPath( string _table, string _fullPathFieldName, string _oldPath, string _newPath )
	{
		///UPDATE Category SET CategoryFullPath = CONCAT('Item A', SUBSTRING(CategoryFullPath, 7)) WHERE CategoryFullPath LIKE 'Item 1%';
		string sqlQuery = $"" +
			$"{DBNames.SqlUpdate}{_table}" +
			$"{DBNames.SqlSet}{_fullPathFieldName} = " +
			$"{DBNames.SqlConcat}'{_newPath}',{DBNames.SqlSubString}{_fullPathFieldName}, {_oldPath.Length + 1} ) )" +
			$"{DBNames.SqlWhere}{_fullPathFieldName}{DBNames.SqlLike}'{_oldPath}%';";

		try
		{
			ExecuteNonQuery( sqlQuery );
		}
		catch ( MySqlException ex )
		{
			Debug.WriteLine( "Error (Insert in _table - MySqlException): " + ex.Message );
			throw;
		}
		catch ( Exception ex )
		{
			Debug.WriteLine( "Error (Insert in _table): " + ex.Message );
			throw;
		}
	}
	#endregion Replace start of FullPath with changed FullPath

	#region Delete records from database
	public static string DeleteRecord( string _table, string [ , ] _whereFields )
	{
		string sqlQuery = $"{DBNames.SqlDeleteFrom}{DBNames.Database}.{_table}{DBNames.SqlWhere}", prefix = "";

		for ( int i = 0; i < _whereFields.GetLength( 0 ); i++ )
		{
			if ( i != 0 ) { prefix = $"{DBNames.SqlAnd}"; }

			sqlQuery = string.Concat( $"{sqlQuery}{prefix}`{_whereFields [ i, 0 ]}` = @{_whereFields [ i, 0 ]}" );
		}

		string result;
		try
		{
			int rowsAffected = ExecuteNonQueryTable(sqlQuery, _whereFields);

			result = rowsAffected > 0 ? $"{GeneralHelper.GetResourceString( "Maintanance.Statusline.Deleted" )}." : $"{GeneralHelper.GetResourceString( "Maintanance.Statusline.NotDeleted" )}.";
		}
		catch ( MySqlException ex )
		{
			Debug.WriteLine( "Error (Insert in _table - MySqlException): " + ex.Message );
			throw;
		}
		catch ( Exception ex )
		{
			Debug.WriteLine( "Error (Insert in _table): " + ex.Message );
			throw;
		}
		return result;
	}
	#endregion Delete records from database

	#region Delete recordTree from table based on (Parent) Id
	public static void DeleteRecordTree( string _table, string _idField, string _parentIdField, int _id = 0 )
	{
		if ( _id != 0 )
		{
			string sqlQuery = $"" +
				$"{DBNames.SqlWithRecursive}RecordTree{DBNames.SqlAs}( " +
				$"{DBNames.SqlSelect}{_idField}" +
				$"{DBNames.SqlFrom}{_table}" +
				$"{DBNames.SqlWhere}{_idField} = {_id} " +
				$"{DBNames.SqlUnionAll} " +
				$"{DBNames.SqlSelect}c.{_idField}" +
				$"{DBNames.SqlFrom}{_table} c " +
				$"{DBNames.SqlInnerJoin}RecordTree rt{DBNames.SqlOn}c.{_parentIdField} = rt.{_idField} ) " +
				$"{DBNames.SqlDeleteFrom}{_table}" +
				$"{DBNames.SqlWhere}{_idField} " +
				$"{DBNames.SqlIn}( {DBNames.SqlSelect}{_idField}{DBNames.SqlFrom}RecordTree );";

			try
			{
				ExecuteNonQuery( sqlQuery );
			}
			catch ( MySqlException ex )
			{
				Debug.WriteLine( "Error (Insert in _table - MySqlException): " + ex.Message );
				throw;
			}
			catch ( Exception ex )
			{
				Debug.WriteLine( "Error (Insert in _table): " + ex.Message );
				throw;
			}
		}
	}
	#endregion Delete recordTree from table based on (Parent) Id

	#region Check if there is a record in the table based (returns no of records)
	public static int CheckForRecords( string _table, string [ , ] _whereFields )
	{
		int result = 0;
		StringBuilder sqlQuery = new();
		_ = sqlQuery.Append( $"{DBNames.SqlSelect}{DBNames.SqlCount}*) " +
			$"{DBNames.SqlFrom}{_table.ToLower()}" +
			$"{DBNames.SqlWhere}" );

		string prefix = "";

		for ( int i = 0; i < _whereFields.GetLength( 0 ); i++ )
		{
			if ( i != 0 )
			{ prefix = DBNames.SqlAnd; }
			_ = sqlQuery.Append( $"{prefix}{DBNames.SqlLower}`{_whereFields [ i, 0 ]}` ) = @{_whereFields [ i, 0 ]}" );
		}

		using ( MySqlConnection connection = new( DBConnect.ConnectionString ) )
		{
			connection.Open();

			using ( MySqlCommand cmd = new( sqlQuery.ToString(), connection ) )
			{
				for ( int i = 0; i < _whereFields.GetLength( 0 ); i++ )
				{
					switch ( _whereFields [ i, 1 ].ToLower() )
					{
						case "string":
							cmd.Parameters.Add( $"@{_whereFields [ i, 0 ]}", MySqlDbType.String ).Value = _whereFields [ i, 2 ];
							break;

						case "int":
							cmd.Parameters.Add( $"@{_whereFields [ i, 0 ]}", MySqlDbType.Int32 ).Value = int.Parse( _whereFields [ i, 2 ] );
							break;

						case "double":
							cmd.Parameters.Add( $"@{_whereFields [ i, 0 ]}", MySqlDbType.Double ).Value = double.Parse( _whereFields [ i, 2 ] );
							break;

						case "float":
							cmd.Parameters.Add( $"@{_whereFields [ i, 0 ]}", MySqlDbType.Float ).Value = float.Parse( _whereFields [ i, 2 ] );
							break;

						case "longtext":
							cmd.Parameters.Add( $"@{_whereFields [ i, 0 ]}", MySqlDbType.LongText ).Value = DBNull.Value;
							break;

						case "longblob":
							cmd.Parameters.Add( $"@{_whereFields [ i, 0 ]}", MySqlDbType.Blob ).Value = DBNull.Value;
							break;

						case "date":
							string [] _tempDates = _whereFields[i, 2].Split("-");

							// Add leading zero's to date and month
							string _tempDate = $"{_tempDates[2]}-{GeneralHelper.AddZeros(_tempDates[1], 2)}-{GeneralHelper.AddZeros(_tempDates[0], 2)}";
							cmd.Parameters.Add( $"@{_whereFields [ i, 0 ]}", MySqlDbType.String ).Value = _tempDate;
							break;
					}
				}
				result = ( int ) ( long ) cmd.ExecuteScalar();
			}
			connection.Close();
		}
		return result;
	}
	#endregion Check if there is a record in the table based (returns no of records)

	#region Delete or Hide a product from the product table
	/// <summary>
	/// Delete or Hide a product from the product table, if it is used in history in is set to Hidden. If deleted all related records in the productsuppliertable will also be deleted
	/// </summary>
	/// <param name="productId">The id of the selected product.</param>
	/// <returns>An int</returns> (1 = hidden, 0 = deleted)
	public static int DeleteProduct( int productId )
	{
		using MySqlConnection connection = new(DBConnect.ConnectionString);
		connection.Open();

		using MySqlCommand cmd = new MySqlCommand(DBNames.SPDeleteProductId, connection);
		cmd.CommandType = CommandType.StoredProcedure;

		//Provide input parameter to the stored procedure
		cmd.Parameters.AddWithValue( DBNames.SPDeleteProductIdInputParameter, productId );

		//handle result from the stored procedure
		MySqlParameter resultParam = new (DBNames.SPDeleteProductIdOutputParameter, MySqlDbType.Int32);
		resultParam.Direction = ParameterDirection.Output;
		cmd.Parameters.Add( resultParam );

		// Execute stored procedure
		cmd.ExecuteNonQuery();

		// Get the result (1 = hidden, 0 = deleted)
		int result = Convert.ToInt32(resultParam.Value);

		return result;
	}
	#endregion

	#region Set the  default Supplier for a specific product
	public static void SetDefaultSupplier( string productId, string supplierId, string action )
	{
		string sqlQuery = "";

		switch ( action.ToLower() )
		{
			case "set":
				sqlQuery = $"{DBNames.SqlCall}{DBNames.Database}.SetDefaultSupplier( {int.Parse( productId )}, {int.Parse( supplierId )};";
				break;
			case "reset":
				sqlQuery = $"{DBNames.SqlCall}{DBNames.Database}.ResetDefaultSupplier( {int.Parse( productId )}, {int.Parse( supplierId )};";
				break;
			default:
				break;
		}

		if ( sqlQuery != "" )
		{
			using MySqlConnection connection = new( DBConnect.ConnectionString );
			connection.Open();

			sqlQuery = $"{DBNames.SqlCall}{DBNames.Database}.SetDefaultSupplier( {int.Parse( productId )}, {int.Parse( supplierId )};";

			using MySqlCommand cmd = new( sqlQuery, connection );
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.AddWithValue( "@p_ProductId", productId );
			cmd.Parameters.AddWithValue( "@p_SupplierId", supplierId );

			cmd.ExecuteNonQuery();
		}
	}
	#endregion

	#region Reset the  default Supplier for a specific product
	public static void ResetDefaultSupplier( string productId, string supplierId )
	{
		using MySqlConnection connection = new( DBConnect.ConnectionString );
		connection.Open();

		var sqlQuery = $"{DBNames.SqlCall}{DBNames.Database}.ResetDefaultSupplier( {int.Parse(productId)}, {int.Parse(supplierId)};";

		using MySqlCommand cmd = new( sqlQuery, connection );
		cmd.CommandType = CommandType.StoredProcedure;

		cmd.Parameters.AddWithValue( "@p_ProductId", productId );
		cmd.Parameters.AddWithValue( "@p_SupplierId", supplierId );

		cmd.ExecuteNonQuery();
	}
	#endregion

	#region Execute Non Query Handlers
	#region Execute Non Query
	private static void ExecuteNonQuery( string _sqlQuery )
	{
		using MySqlConnection connection = new(DBConnect.ConnectionString);
		connection.Open();

		using MySqlCommand cmd = new(_sqlQuery, connection);
		_ = cmd.ExecuteNonQuery();
	}
	#endregion Execute Non Query

	#region Execute Non Query SqlText + Array with Fields
	public static int ExecuteNonQueryTable( string _sqlQuery, string [ , ] _fields )
	{
		int rowsAffected = 0;

		using ( MySqlConnection connection = new( DBConnect.ConnectionString ) )
		{
			connection.Open();

			using MySqlCommand cmd = new(_sqlQuery, connection);
			for ( int i = 0; i < _fields.GetLength( 0 ); i++ )
			{
				switch ( _fields [ i, 1 ].ToLower() )
				{
					case "string":
						cmd.Parameters.Add( "@" + _fields [ i, 0 ], MySqlDbType.String ).Value = _fields [ i, 2 ];
						break;

					case "int":
					{ cmd.Parameters.Add( "@" + _fields [ i, 0 ], MySqlDbType.Int32 ).Value = _fields [ i, 2 ] is "0" or "" ? null : int.Parse( _fields [ i, 2 ] ); }
					break;

					case "double":
						cmd.Parameters.Add( "@" + _fields [ i, 0 ], MySqlDbType.Double ).Value = Math.Round( double.Parse( _fields [ i, 2 ] ), 2 );
						break;

					case "float":
						cmd.Parameters.Add( "@" + _fields [ i, 0 ], MySqlDbType.Float ).Value = float.Parse( _fields [ i, 2 ] );
						break;

					case "longtext":
						cmd.Parameters.Add( "@" + _fields [ i, 0 ], MySqlDbType.LongText ).Value = _fields [ i, 2 ];
						break;

					case "date":
						string[] _tempDates = _fields[i, 2].Split("-");

						// Add leading zero's to date and month
						string _tempDate = _tempDates[2] + "-" + GeneralHelper.AddZeros(_tempDates[1], 2) + "-" + GeneralHelper.AddZeros(_tempDates[0], 2);
						cmd.Parameters.Add( "@" + _fields [ i, 0 ], MySqlDbType.String ).Value = _tempDate;
						break;

					case "time":
						cmd.Parameters.Add( "@" + _fields [ i, 0 ], MySqlDbType.String ).Value = _fields [ i, 2 ];
						break;
				}
			}
			rowsAffected = cmd.ExecuteNonQuery();
		}
		return rowsAffected;
	}
	#endregion Execute Non Query SqlText + Array with Fields

	#region Execute Non Query SqlText + Array with WhereFields + Array with Fields
	public static int ExecuteNonQueryTable( string sqlQuery, string [ , ] _whereFields, string [ , ] _fields )
	{
		int rowsAffected = 0;

		using ( MySqlConnection connection = new( DBConnect.ConnectionString ) )
		{
			connection.Open();

			using MySqlCommand cmd = new(sqlQuery, connection);
			for ( int i = 0; i < _whereFields.GetLength( 0 ); i++ )
			{
				switch ( _whereFields [ i, 1 ].ToLower() )
				{
					case "string":
						cmd.Parameters.Add( "@" + _whereFields [ i, 0 ], MySqlDbType.String ).Value = _whereFields [ i, 2 ];
						break;

					case "int":
						cmd.Parameters.Add( "@" + _whereFields [ i, 0 ], MySqlDbType.Int32 ).Value = int.Parse( _whereFields [ i, 2 ] );
						break;

					case "double":
						cmd.Parameters.Add( "@" + _whereFields [ i, 0 ], MySqlDbType.Double ).Value = double.Parse( _whereFields [ i, 2 ] );
						break;

					case "float":
						cmd.Parameters.Add( "@" + _whereFields [ i, 0 ], MySqlDbType.Float ).Value = float.Parse( _whereFields [ i, 2 ] );
						break;

					case "longtext":
						cmd.Parameters.Add( "@" + _whereFields [ i, 0 ], MySqlDbType.LongText ).Value = _whereFields [ i, 2 ];
						break;

					case "date":
						string[] _tempDates = _whereFields[i, 2].Split("-");

						// Add leading zero's to date and month
						string _tempDate = _tempDates[2] + "-" + GeneralHelper.AddZeros(_tempDates[1], 2) + "-" + GeneralHelper.AddZeros(_tempDates[0], 2);
						cmd.Parameters.Add( "@" + _whereFields [ i, 0 ], MySqlDbType.String ).Value = _tempDate;
						break;

					case "time":
						cmd.Parameters.Add( "@" + _whereFields [ i, 0 ], MySqlDbType.String ).Value = _fields [ i, 2 ];
						break;
				}
			}

			for ( int i = 0; i < _fields.GetLength( 0 ); i++ )
			{
				switch ( _fields [ i, 1 ].ToLower() )
				{
					case "string":
						cmd.Parameters.Add( "@" + _fields [ i, 0 ], MySqlDbType.String ).Value = _fields [ i, 2 ];
						break;

					case "int":
						cmd.Parameters.Add( "@" + _fields [ i, 0 ], MySqlDbType.Int32 ).Value = int.Parse( _fields [ i, 2 ] );
						break;

					case "double":
						cmd.Parameters.Add( "@" + _fields [ i, 0 ], MySqlDbType.Double ).Value = double.Parse( _fields [ i, 2 ] );
						break;

					case "float":
						cmd.Parameters.Add( "@" + _fields [ i, 0 ], MySqlDbType.Float ).Value = float.Parse( _fields [ i, 2 ] );
						break;

					case "longtext":
						cmd.Parameters.Add( "@" + _whereFields [ i, 0 ], MySqlDbType.LongText ).Value = _fields [ i, 2 ];
						break;

					case "date":
						if ( _fields [ i, 2 ] != "" )
						{
							string[] _tempDates = _fields[i, 2].Split("-");

							// Add leading zero's to date and month
							GeneralHelper helper = new();
							string _tempDate = _tempDates[2] + "-" + GeneralHelper.AddZeros(_tempDates[1], 2) + "-" + GeneralHelper.AddZeros(_tempDates[0], 2);
							cmd.Parameters.Add( "@" + _fields [ i, 0 ], MySqlDbType.String ).Value = _tempDate;
						}
						else
						{ cmd.Parameters.Add( "@" + _fields [ i, 0 ], MySqlDbType.String ).Value = null; }
						break;

					case "time":
						cmd.Parameters.Add( "@" + _fields [ i, 0 ], MySqlDbType.String ).Value = _fields [ i, 2 ];
						break;
				}
			}
			rowsAffected = cmd.ExecuteNonQuery();
		}
		return rowsAffected;
	}
	#endregion Execute Non Query SqlText + Array with WhereFields + Array with Fields

	#region SqlText + Array with WhereFields + Memo
	public static int ExecuteNonQueryTable( string sqlQuery, string [ , ] _whereFields, string _memoField, string _memoContent )
	{
		int rowsAffected = 0;

		using ( MySqlConnection connection = new( DBConnect.ConnectionString ) )
		{
			connection.Open();

			using MySqlCommand cmd = new(sqlQuery, connection);
			for ( int i = 0; i < _whereFields.GetLength( 0 ); i++ )
			{
				switch ( _whereFields [ i, 1 ].ToLower() )
				{
					case "string":
						cmd.Parameters.Add( $"@{_whereFields [ i, 0 ]}", MySqlDbType.String ).Value = _whereFields [ i, 2 ];
						break;

					case "int":
						cmd.Parameters.Add( $"@{_whereFields [ i, 0 ]}", MySqlDbType.Int32 ).Value = int.Parse( _whereFields [ i, 2 ] );
						break;

					case "double":
						cmd.Parameters.Add( $"@{_whereFields [ i, 0 ]}", MySqlDbType.Double ).Value = double.Parse( _whereFields [ i, 2 ] );
						break;

					case "float":
						cmd.Parameters.Add( $"@{_whereFields [ i, 0 ]}", MySqlDbType.Float ).Value = float.Parse( _whereFields [ i, 2 ] );
						break;

					case "date":
						string[] _tempDates = _whereFields[i, 2].Split("-");

						// Add leading zero's to date and month
						string _tempDate = _tempDates[2] + "-" + GeneralHelper.AddZeros(_tempDates[1], 2) + "-" + GeneralHelper.AddZeros(_tempDates[0], 2);
						cmd.Parameters.Add( $"@{_whereFields [ i, 0 ]}", MySqlDbType.String ).Value = _tempDate;
						break;

					case "time":
						cmd.Parameters.Add( $"@{_whereFields [ i, 0 ]}", MySqlDbType.String ).Value = _whereFields [ i, 2 ];
						break;
				}
			}

			cmd.Parameters.Add( $"@{_memoField}", MySqlDbType.LongText ).Value = _memoContent;

			rowsAffected = cmd.ExecuteNonQuery();
		}
		return rowsAffected;
	}
	#endregion SqlText + Array with WhereFields + Memo

	#region Execute Non Query: SqlText + Array with Fields + Image
	private static int ExecuteNonQueryTable( string _sqlQuery, string [ , ] _fields, byte [ ] _image, string _imageFieldName )
	{
		int rowsAffected = 0;

		using ( MySqlConnection connection = new( DBConnect.ConnectionString ) )
		{
			connection.Open();

			using MySqlCommand cmd = new(_sqlQuery, connection);
			for ( int i = 0; i < _fields.GetLength( 0 ); i++ )
			{
				switch ( _fields [ i, 1 ].ToLower() )
				{
					case "string":
						cmd.Parameters.Add( "@" + _fields [ i, 0 ], MySqlDbType.String ).Value = _fields [ i, 2 ];
						break;

					case "int":
						cmd.Parameters.Add( "@" + _fields [ i, 0 ], MySqlDbType.Int32 ).Value = int.Parse( _fields [ i, 2 ] );
						break;

					case "double":
						cmd.Parameters.Add( "@" + _fields [ i, 0 ], MySqlDbType.Double ).Value = double.Parse( _fields [ i, 2 ] );
						break;

					case "float":
						cmd.Parameters.Add( "@" + _fields [ i, 0 ], MySqlDbType.Float ).Value = float.Parse( _fields [ i, 2 ] );
						break;

					case "longtext":
						cmd.Parameters.Add( "@" + _fields [ i, 0 ], MySqlDbType.LongText ).Value = _fields [ i, 2 ];
						break;

					case "date":
						string[] _tempDates = _fields[i, 2].Split("-");

						// Add leading zero's to date and month
						string _tempDate = _tempDates[2] + "-" + GeneralHelper.AddZeros(_tempDates[1], 2) + "-" + GeneralHelper.AddZeros(_tempDates[0], 2);
						cmd.Parameters.Add( "@" + _fields [ i, 0 ], MySqlDbType.String ).Value = _tempDate;
						break;

					case "time":
						cmd.Parameters.Add( "@" + _fields [ i, 0 ], MySqlDbType.String ).Value = _fields [ i, 2 ];
						break;
				}
			}
			// Add _image to the commandstring
			cmd.Parameters.Add( "@" + _imageFieldName, MySqlDbType.Blob ).Value = _image;
			rowsAffected = cmd.ExecuteNonQuery();
		}
		return rowsAffected;
	}
	#endregion Execute Non Query: SqlText + Array with Fields + Image
	#endregion Execute Non Query Handlers
}