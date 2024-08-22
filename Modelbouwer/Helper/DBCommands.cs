using System.Collections.ObjectModel;
using System.Diagnostics;

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
	#endregion

	#region GetData Sorted
	public static DataTable GetData( string _table, string _orderByFieldName )
	{
		string selectQuery;
		if ( _orderByFieldName.Equals( "nosort", StringComparison.CurrentCultureIgnoreCase ) )
		{
			selectQuery = $"" +
				$"{DBNames.SqlSelectAll}{DBNames.SqlFrom}{DBNames.Database}.{_table}";
		}
		else
		{
			selectQuery = $"" +
				$"{DBNames.SqlSelectAll}{DBNames.SqlFrom}{DBNames.Database}.{_table}" +
				$"{DBNames.SqlOrder}{_orderByFieldName}";
		}
		return GetTable( selectQuery );
	}
	#endregion

	#region GetData Sorted and filtered
	public static DataTable GetData( string _table, string _orderByFieldName, string _whereFieldName, string _whereFieldValue )
	{
		string selectQuery;
		if ( _orderByFieldName.Equals( "nosort", StringComparison.CurrentCultureIgnoreCase ) )
		{
			selectQuery = $"" +
				$"{DBNames.SqlSelectAll}{DBNames.SqlFrom}{DBNames.Database}.{_table}" +
				$"{DBNames.SqlWhere}{_whereFieldName} = '{_whereFieldValue}';";
		}
		else
		{
			selectQuery = $"" +
				$"{DBNames.SqlSelectAll}{DBNames.SqlFrom}{DBNames.Database}.{_table}" +
				$"{DBNames.SqlWhere}{_whereFieldName} = '{_whereFieldValue}'" +
				$"{DBNames.SqlOrder}{_orderByFieldName};";
		}
		return GetTable( selectQuery );
	}
	#endregion

	#region Get data sorted, and filtered on two criteria
	public static DataTable GetData( string _table, string _orderByFieldName, string _whereFieldName, string _whereFieldValue, string _andWhereFieldName, string _andWhereFieldValue )
	{
		string selectQuery;
		if ( _orderByFieldName.Equals( "nosort", StringComparison.CurrentCultureIgnoreCase ) )
		{
			selectQuery = $"" +
				$"{DBNames.SqlSelectAll}{DBNames.SqlFrom}{DBNames.Database}.{_table}" +
				$"{DBNames.SqlWhere}{_whereFieldName} = '{_whereFieldValue}'" +
				$"{DBNames.SqlAnd}{_andWhereFieldName} = '{_andWhereFieldValue}';";
		}
		else
		{
			selectQuery = $"" +
				$"{DBNames.SqlSelectAll}{DBNames.SqlFrom}{DBNames.Database}.{_table}" +
				$"{DBNames.SqlWhere}{_whereFieldName} = '{_whereFieldValue}'" +
				$"{DBNames.SqlAnd}{_andWhereFieldName} = '{_andWhereFieldValue}'" +
				$"{DBNames.SqlOrder}{_orderByFieldName};";
		}

		MySqlConnection connection = new(DBConnect.ConnectionString);
		connection.Open();
		DataTable table = new();
		MySqlDataAdapter adapter = new(selectQuery, connection);
		adapter.Fill( table );
		connection.Close();
		//return GetTable( selectQuery );
		return table;
	}
	#endregion

	#region Get Field(s) from table
	public static string GetData( string _table, string [ , ] _whereFields, string [ , ] _fields )
	{
		// There is an Id or String available for each condition, so one of them has a value the other one is 0 or ""
		StringBuilder sqlQuery = new();
		sqlQuery.Append( DBNames.SqlSelect );
		string prefix = "";

		for ( int i = 0; i < _fields.GetLength( 0 ); i++ )
		{
			if ( i != 0 )
			{ prefix = ", "; }
			sqlQuery.Append( $"{prefix}{_fields [ i, 0 ]}" );
		}

		sqlQuery.Append( $"{DBNames.SqlFrom}{_table.ToLower()} " );
		prefix = "";

		if ( _whereFields.GetLength( 0 ) > 0 )
		{
			sqlQuery.Append( DBNames.SqlWhere );

			for ( int i = 0; i < _whereFields.GetLength( 0 ); i++ )
			{
				if ( i != 0 )
				{ prefix = DBNames.SqlAnd; }
				sqlQuery.Append( $"{prefix}{_whereFields [ i, 0 ]} = @{_whereFields [ i, 0 ]}" );
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
					String[] _tempDates = _whereFields[i, 2].Split("-");

					// Add leading zero's to date and month
					GeneralHelper helper = new();
					var _tempDate = _tempDates[2] + "-" + GeneralHelper.AddZeros(_tempDates[1], 2) + "-" + GeneralHelper.AddZeros(_tempDates[0], 2);
					cmd.Parameters.Add( $"@{_whereFields [ i, 0 ]}", MySqlDbType.String ).Value = _tempDate;
					break;
			}
		}

		string resultString = "";
		int resultInt;
		double resultDouble;
		float resultFloat;

		if ( _fields [ 0, 1 ].ToLower() == "string" || _fields [ 0, 1 ].ToLower() == "date" || _fields [ 0, 1 ].ToLower() == "time" )
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
		adapter.Fill( table );
		connection.Close();

		return table;
	}
	#endregion

	#region Fill lists
	#region BrandList
	public static ObservableCollection<BrandModel> GetBrandList( ObservableCollection<BrandModel>? brandList = null )
	{
		DataTable? _dt = null;
		_dt = GetData( DBNames.BrandTable, DBNames.BrandFieldNameName );

		for ( int i = 0; i < _dt.Rows.Count; i++ )
		{
			if ( _dt.Rows [ i ] [ 2 ] == DBNull.Value )
			{
				brandList.Add( new BrandModel
				{
					BrandId = int.Parse( _dt.Rows [ i ] [ 0 ].ToString() ),
					BrandName = _dt.Rows [ i ] [ 1 ].ToString(),
				} );
			}
		}
		return brandList;
	}
	#endregion

	#region CategoryList
	public static ObservableCollection<CategoryModel> GetCategoryList( ObservableCollection<CategoryModel>? categoryList = null )
	{
		if ( categoryList == null )
		{
			categoryList = new ObservableCollection<CategoryModel>();
		}

		DataTable? _dt = null;
		_dt = GetData( DBNames.CategoryTable, DBNames.CategoryFieldNameFullpath );

		for ( int i = 0; i < _dt.Rows.Count; i++ )
		{
			//int _id = (int) _dt.Rows [ i ] [ 0 ];
			int _parent = 0;
			if ( _dt.Rows [ i ] [ 1 ] != DBNull.Value ) { _parent = ( int ) _dt.Rows [ i ] [ 1 ]; }

			categoryList.Add( new CategoryModel
			{
				CategoryId = ( int ) _dt.Rows [ i ] [ 0 ],
				CategoryParentId = _parent,
				CategoryName = _dt.Rows [ i ] [ 2 ].ToString(),
				CategoryFullpath = _dt.Rows [ i ] [ 3 ].ToString()
			} );
		}
		return categoryList;
	}
	#endregion

	#region StorageLocationList
	public static ObservableCollection<StorageLocationModel> GetStorageLocationList( ObservableCollection<StorageLocationModel>? storagelocationList = null )
	{
		DataTable? _dt = null;
		_dt = GetData( DBNames.StorageTable, DBNames.StorageFieldNameFullpath );

		for ( int i = 0; i < _dt.Rows.Count; i++ )
		{
			if ( _dt.Rows [ i ] [ 2 ] == DBNull.Value )
			{
				storagelocationList.Add( new StorageLocationModel
				{
					StorageLocationId = int.Parse( _dt.Rows [ i ] [ 0 ].ToString() ),
					StorageLocationParentId = int.Parse( _dt.Rows [ i ] [ 1 ].ToString() ),
					StorageLocationName = _dt.Rows [ i ] [ 2 ].ToString(),
					StorageLocationFullpath = _dt.Rows [ i ] [ 3 ].ToString()
				} );
			}
		}
		return storagelocationList;
	}
	#endregion

	#region WorktypeList
	public static ObservableCollection<WorktypeModel> GetWorktypeList( ObservableCollection<WorktypeModel>? worktypeList = null )
	{
		DataTable? _dt = null;
		_dt = GetData( DBNames.WorktypeTable, DBNames.WorktypeFieldNameFullpath );

		for ( int i = 0; i < _dt.Rows.Count; i++ )
		{
			if ( _dt.Rows [ i ] [ 2 ] == DBNull.Value )
			{
				worktypeList.Add( new WorktypeModel
				{
					WorktypeId = int.Parse( _dt.Rows [ i ] [ 0 ].ToString() ),
					WorktypeParentId = int.Parse( _dt.Rows [ i ] [ 1 ].ToString() ),
					WorktypeName = _dt.Rows [ i ] [ 2 ].ToString(),
					WorktypeFullpath = _dt.Rows [ i ] [ 3 ].ToString()
				} );
			}
		}
		return worktypeList;
	}
	#endregion
	#endregion
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
			List<string> rowValues = new();

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
	#endregion

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
		string result = string.Empty;
		string sqlQuery = $"{DBNames.SqlInsert}{DBNames.Database}.{_table} ";

		string sqlFields = "( ";
		string sqlValues = "( ";
		string prefix = "";

		for ( int i = 0; i < _fields.GetLength( 0 ); i++ )
		{
			if ( i != 0 )
			{ prefix = ", "; }
			sqlFields = string.Concat( $"{sqlFields}{prefix}{_fields [ i, 0 ]}" );
			sqlValues = string.Concat( $"{sqlValues}{prefix}@{_fields [ i, 0 ]}" );
		}

		sqlFields += " )";
		sqlValues += " )";

		sqlQuery = string.Concat( $"{sqlQuery}{sqlFields}{DBNames.SqlValues}{sqlValues};" );

		try
		{
			int rowsAffected = ExecuteNonQueryTable(sqlQuery, _fields);

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
		foreach ( var item in itemsToInsert )
		{
			var columns = new List<string>();
			var values = new List<string>();

			foreach ( var header in headers )
			{
				string propertyName = headerToPropertyMap.ContainsKey(header) ? headerToPropertyMap[header] : header;
				var propertyInfo = typeof(T).GetProperty(propertyName);

				if ( propertyInfo != null )
				{
					var value = propertyInfo.GetValue(item);

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

	#endregion

	#region Insert Fields and Image in table
	public static string InsertInTable( string _table, string [ , ] _fields, byte [ ] _image, string _imageName )
	{
		var result = string.Empty;
		var sqlQuery = $"{DBNames.SqlInsert}{_table} ";

		var sqlFields = "(";
		var sqlValues = "(";
		var prefix = "";

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

		try
		{
			int rowsAffected = ExecuteNonQueryTable(sqlQuery, _fields, _image, _imageName);

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
	#endregion

	#region Check if there is a record in the table based (returns no of records)
	public static int CheckForRecords( string _table, string [ , ] _whereFields )
	{
		int result = 0;
		StringBuilder sqlQuery = new();
		sqlQuery.Append( $"{DBNames.SqlSelect}{DBNames.SqlCount}*) " +
			$"{DBNames.SqlFrom}{_table.ToLower()}" +
			$"{DBNames.SqlWhere}" );

		string prefix = "";

		for ( int i = 0; i < _whereFields.GetLength( 0 ); i++ )
		{
			if ( i != 0 )
			{ prefix = DBNames.SqlAnd; }
			sqlQuery.Append( $"{prefix}`{_whereFields [ i, 0 ]}` = @{_whereFields [ i, 0 ]}" );
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
							var _tempDate = $"{_tempDates[2]}-{GeneralHelper.AddZeros(_tempDates[1], 2)}-{GeneralHelper.AddZeros(_tempDates[0], 2)}";
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
	#endregion

	#region Execute Non Query Handlers
	#region Execute Non Query
	private static void ExecuteNonQuery( string _sqlQuery )
	{
		using MySqlConnection connection = new(DBConnect.ConnectionString);
		connection.Open();

		using MySqlCommand cmd = new(_sqlQuery, connection);
		cmd.ExecuteNonQuery();
	}
	#endregion Execute NonQuery

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
						String[] _tempDates = _fields[i, 2].Split("-");

						// Add leading zero's to date and month
						var _tempDate = _tempDates[2] + "-" + GeneralHelper.AddZeros(_tempDates[1], 2) + "-" + GeneralHelper.AddZeros(_tempDates[0], 2);
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
	#endregion SqlText + Array with Fields

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
						String[] _tempDates = _whereFields[i, 2].Split("-");

						// Add leading zero's to date and month
						var _tempDate = _tempDates[2] + "-" + GeneralHelper.AddZeros(_tempDates[1], 2) + "-" + GeneralHelper.AddZeros(_tempDates[0], 2);
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
						String[] _tempDates = _fields[i, 2].Split("-");

						// Add leading zero's to date and month
						GeneralHelper helper = new();
						var _tempDate = _tempDates[2] + "-" + GeneralHelper.AddZeros(_tempDates[1], 2) + "-" + GeneralHelper.AddZeros(_tempDates[0], 2);
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
	#endregion

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
						String[] _tempDates = _whereFields[i, 2].Split("-");

						// Add leading zero's to date and month
						var _tempDate = _tempDates[2] + "-" + GeneralHelper.AddZeros(_tempDates[1], 2) + "-" + GeneralHelper.AddZeros(_tempDates[0], 2);
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
	#endregion

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
						String[] _tempDates = _fields[i, 2].Split("-");

						// Add leading zero's to date and month
						var _tempDate = _tempDates[2] + "-" + GeneralHelper.AddZeros(_tempDates[1], 2) + "-" + GeneralHelper.AddZeros(_tempDates[0], 2);
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
	#endregion
	#endregion

}
