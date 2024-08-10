using MySql.Data.MySqlClient;

namespace Modelbouwer.Helper;

/// <summary>
/// Create connectionstring to connect to the MySql Database.
/// Use GetIp to retrieve IP Address of the MySql Server from the configfile.
/// With the additional parameter GetIp("local") is forced the local MySql Server is used
/// </summary>
public class DBConnect
{
	public MySqlConnection? connection;

	//public static readonly string server = IpHelper.GetIP();
	public static readonly string server = IpHelper.GetIP();
	public static readonly string database = DBNames.Database;
	public static readonly string port = "3306";
	public static readonly string uid = "root";
	public static readonly string password = "OefenenKHMK24!";
	public static readonly string ConnectionString = $"" +
		$"SERVER = {server}; PORT = {port}; " + $"DATABASE = {database}; UID = {uid}; PASSWORD = {password};";
}