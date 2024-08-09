using System.Data;
using System.Text;

using MySql.Data.MySqlClient;

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
	#endregion



}
