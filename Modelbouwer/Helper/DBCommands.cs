namespace Modelbouwer.Helper;
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
				BrandId = ( int ) _dt.Rows [ i ] [ 0 ],
				BrandName = _dt.Rows [ i ] [ 1 ].ToString(),
			} );
		}
		return brandList;
	}
	#endregion BrandList

	#region CategoryList
	public ObservableCollection<CategoryModel> GetCategoryList( ObservableCollection<CategoryModel>? categoryList = null )
	{
		categoryList ??= [ ];
		DataTable? _dt = GetData( DBNames.CategoryTable, DBNames.CategoryFieldNameName );

		for ( int i = 0; i < _dt.Rows.Count; i++ )
		{
			int? _parent = null;
			if ( _dt.Rows [ i ] [ 1 ] != DBNull.Value ) { _parent = ( int ) _dt.Rows [ i ] [ 1 ]; }

			if ( _parent != null )
			{
				categoryList.Add( new CategoryModel
				{
					CategoryId = ( int ) _dt.Rows [ i ] [ 0 ],
					CategoryParentId = ( int ) _parent,
					CategoryName = _dt.Rows [ i ] [ 2 ].ToString()
				} );
			}
			else
			{
				categoryList.Add( new CategoryModel
				{
					CategoryId = ( int ) _dt.Rows [ i ] [ 0 ],
					CategoryName = _dt.Rows [ i ] [ 2 ].ToString()
				} );
			}

		}
		return GetCategoryHierarchy( categoryList );
	}

	private ObservableCollection<CategoryModel> GetCategoryHierarchy( ObservableCollection<CategoryModel>? categoryList )
	{
		ILookup<int?, CategoryModel> lookup =      categoryList.ToLookup( c => c.CategoryParentId )  ;
		foreach ( CategoryModel category in categoryList )
		{
			category.SubCategories = lookup [ category.CategoryId ].ToObservableCollection();
		}
		return lookup [ null ].ToObservableCollection();
	}
	#endregion CategoryList

	#region ContactTypeList
	public static ObservableCollection<SupplierContactTypeModel> GetContactTypeList( ObservableCollection<SupplierContactTypeModel>? _list = null )
	{
		_list ??= [ ];
		DataTable? _dt = GetData( DBNames.ContactTypeTable, DBNames.ContactTypeFieldNameName );

		for ( int i = 0; i < _dt.Rows.Count; i++ )
		{
			_list.Add( new SupplierContactTypeModel
			{
				ContactTypeId = ( int ) _dt.Rows [ i ] [ 0 ],
				ContactTypeName = _dt.Rows [ i ] [ 1 ].ToString(),
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
				CountryId = ( int ) _dt.Rows [ i ] [ 0 ],
				CountryCode = _dt.Rows [ i ] [ 1 ].ToString(),
				CountryName = _dt.Rows [ i ] [ 2 ].ToString(),
				CountryCurrencyId = ( int ) _dt.Rows [ i ] [ 3 ],
				CountryCurrencySymbol = _dt.Rows [ i ] [ 4 ].ToString()
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
				CountryId = ( int ) _dt.Rows [ i ] [ 0 ],
				CountryCode = _dt.Rows [ i ] [ 1 ].ToString(),
				CountryName = _dt.Rows [ i ] [ 2 ].ToString(),
				CountryCurrencyId = ( int ) _dt.Rows [ i ] [ 3 ],
				CountryCurrencySymbol = _dt.Rows [ i ] [ 4 ].ToString()
			} );
		}
		return countryList;
	}
	#endregion CountryList

	#region CurrencyList
	public ObservableCollection<CurrencyModel> GetCurrencyList( ObservableCollection<CurrencyModel>? currencyList = null )
	{
		currencyList ??= [ ];
		DataTable? _dt = GetData( DBNames.CurrencyTable, DBNames.CurrencyFieldNameName );

		for ( int i = 0; i < _dt.Rows.Count; i++ )
		{
			currencyList.Add( new CurrencyModel
			{
				CurrencyId = ( int ) _dt.Rows [ i ] [ 0 ],
				CurrencyCode = _dt.Rows [ i ] [ 1 ].ToString(),
				CurrencySymbol = _dt.Rows [ i ] [ 2 ].ToString(),
				CurrencyName = _dt.Rows [ i ] [ 3 ].ToString(),
				CurrencyConversionRate = ( double ) _dt.Rows [ i ] [ 4 ]
			} );
		}
		return currencyList;
	}
	#endregion CurrencyList

	#region StorageLocationList
	public static ObservableCollection<StorageModel> GetStorageList1( ObservableCollection<StorageModel>? storagelocationList = null )
	{
		storagelocationList ??= [ ];
		DataTable? _dt = GetData( DBNames.StorageTable, DBNames.StorageFieldNameFullpath );

		for ( int i = 0; i < _dt.Rows.Count; i++ )
		{
			int _parent = 0;
			if ( _dt.Rows [ i ] [ 1 ] != DBNull.Value ) { _parent = ( int ) _dt.Rows [ i ] [ 1 ]; }
			storagelocationList.Add( new StorageModel
			{
				StorageId = int.Parse( _dt.Rows [ i ] [ 0 ].ToString() ),
				StorageParentId = _parent,
				StorageName = _dt.Rows [ i ] [ 2 ].ToString()
			} );
		}
		return storagelocationList;
	}

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
					StorageId = ( int ) _dt.Rows [ i ] [ 0 ],
					StorageParentId = ( int ) _parent,
					StorageName = _dt.Rows [ i ] [ 3 ].ToString()
				} );
			}
			else
			{
				storageList.Add( new StorageModel
				{
					StorageId = ( int ) _dt.Rows [ i ] [ 0 ],
					StorageName = _dt.Rows [ i ] [ 3 ].ToString()
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
	#endregion StorageLocationList

	#region UnitdList
	public static ObservableCollection<UnitModel> GetUnitList( ObservableCollection<UnitModel>? unitList = null )
	{
		unitList ??= [ ];
		DataTable? _dt = GetData( DBNames.UnitTable, DBNames.UnitFieldNameUnitName );

		for ( int i = 0; i < _dt.Rows.Count; i++ )
		{
			unitList.Add( new UnitModel
			{
				UnitId = ( int ) _dt.Rows [ i ] [ 0 ],
				UnitName = _dt.Rows [ i ] [ 1 ].ToString(),
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
					WorktypeId = ( int ) _dt.Rows [ i ] [ 0 ],
					WorktypeParentId = ( int ) _parent,
					WorktypeName = _dt.Rows [ i ] [ 2 ].ToString()
				} );
			}
			else
			{
				worktypeList.Add( new WorktypeModel
				{
					WorktypeId = ( int ) _dt.Rows [ i ] [ 0 ],
					WorktypeName = _dt.Rows [ i ] [ 2 ].ToString()
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
	#endregion Fill lists
	#endregion GetData

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
	public static string InsertInTable( string _table, string [ , ] _fields, byte [ ] _image, string _imageName )
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

		sqlFields = string.Concat( $"{sqlFields}, {_imageName})" );
		sqlValues = string.Concat( $"{sqlValues}, @{_imageName})" );

		sqlQuery = $"{sqlQuery}{sqlFields} {DBNames.SqlValues} {sqlValues};";

		string result;
		try
		{
			int rowsAffected = ExecuteNonQueryTable(sqlQuery, _fields, _image, _imageName);

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
						string[] _tempDates = _fields[i, 2].Split("-");

						// Add leading zero's to date and month
						GeneralHelper helper = new();
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
	private static int ExecuteNonQueryTable( string _sqlQuery, string [ , ] _fields, byte [ ] _image, string _imageName )
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
			cmd.Parameters.Add( "@" + _imageName, MySqlDbType.Blob ).Value = _image;
			rowsAffected = cmd.ExecuteNonQuery();
		}
		return rowsAffected;
	}
	#endregion Execute Non Query: SqlText + Array with Fields + Image
	#endregion Execute Non Query Handlers
}