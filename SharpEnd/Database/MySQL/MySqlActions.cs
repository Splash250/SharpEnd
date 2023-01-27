using MySql.Data.MySqlClient;
using SharpEnd.Packet;

namespace SharpEnd.MySQL
{
    internal static class MySqlActions
    {
        //public static bool Login(string Email, string Password, MySQLDataBaseConnection DataBaseConnection)
        //{
        //    string hashedPassword = MySQLSecurity.ComputeHash(Password);
        //    DataTable table = new();
        //    MySqlDataAdapter adapter = new();
        //    MySqlCommand command = new(MySQLQueryStrings.LoginMySQLString, DataBaseConnection.GetConnection());
        //    command.Parameters.Add("@email", MySqlDbType.VarChar).Value = Email;
        //    command.Parameters.Add("@password", MySqlDbType.VarChar).Value = hashedPassword;
        //    adapter.SelectCommand = command;
        //    adapter.Fill(table);
        //    return table.Rows.Count > 0;
        //}
        //public static bool TryRegisterAccount(string Email, string Password, MySQLDataBaseConnection DataBaseConnection)
        //{
        //    string hashedPassword = MySQLSecurity.ComputeHash(Password);
        //    if (!IsEmailInUse(Email, DataBaseConnection))
        //    {
        //        MySqlCommand command = new(MySQLQueryStrings.InsertAccountSQLString, DataBaseConnection.GetConnection());
        //        command.Parameters.Add("@email", MySqlDbType.VarChar).Value = Email;
        //        command.Parameters.Add("@password", MySqlDbType.VarChar).Value = hashedPassword;
        //        DataBaseConnection.OpenConnection();
        //        return command.ExecuteNonQuery() == 1;
        //    }
        //    return false;
        //}
        //static bool IsEmailInUse(string Email, MySQLDataBaseConnection DataBaseConnection)
        //{
        //    DataTable table = new DataTable();
        //    MySqlDataAdapter adapter = new();
        //    MySqlCommand command = new(MySQLQueryStrings.CheckEmailMySQLString, DataBaseConnection.GetConnection());
        //    command.Parameters.Add("@email", MySqlDbType.VarChar).Value = Email;
        //    adapter.SelectCommand = command;
        //    adapter.Fill(table);
        //    return table.Rows.Count > 0;
        //}
        //public static Dictionary<int, string> GetCitiesDictionary(MySQLDataBaseConnection DataBaseConnection) 
        //{
        //    Dictionary<int, string> cities = new Dictionary<int, string>();
        //    using (MySqlCommand command = new MySqlCommand(MySQLQueryStrings.GetCityNamesSQLString, DataBaseConnection.GetConnection()))
        //    {
        //        using MySqlDataReader reader = command.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            cities.Add(reader.GetInt32(0), reader.GetString(1));
        //        }
        //    }
        //    return cities;
        //}
        //public static List<int[]> GetRailwaysList(MySQLDataBaseConnection DataBaseConnection) 
        //{
        //    List<int[]> railways = new List<int[]>();
        //    using (MySqlCommand command = new(MySQLQueryStrings.GetRailwaysSQLString, DataBaseConnection.GetConnection()))
        //    {
        //        using (MySqlDataReader reader = command.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                railways.Add(new int[3] { reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2) });
        //            }
        //        }
        //    }
        //    return railways;
        //}
        public static void DropTable(MySqlDataBaseConnection Connection, string tableName)
        {
            Connection.OpenConnection();
            string sqlActionString = $"DROP TABLE IF EXISTS {tableName};";
            MySqlCommand command = new(sqlActionString, Connection.GetConnection());
            command.Prepare();
            command.ExecuteNonQuery();
            Connection.CloseConnection();
        }
        public static void CreateTable(MySqlDataBaseConnection Connection, string sqlString) 
        {
            Connection.OpenConnection();
            MySqlCommand command = new(sqlString, Connection.GetConnection());
            command.Prepare();
            command.ExecuteNonQuery();
            Connection.CloseConnection();
        }

        public static void InsertData(MySqlDataBaseConnection Connection, string tableName, Dictionary<string, string> data)
        {
            Connection.OpenConnection();
            MySqlQuery mySQLQuery = new();
            
            string[] keys = Utility.GetKeysFromDictionary(data);
            mySQLQuery.InsertInto(tableName, keys).Values(keys);
            string sqlActionString = mySQLQuery.ToString();

            MySqlCommand command = new(sqlActionString, Connection.GetConnection());
            ParameterizeInsertDataCommand(ref command, data);
            command.Prepare();
            command.ExecuteNonQuery();
            Connection.CloseConnection();
        }

        private static void ParameterizeInsertDataCommand(ref MySqlCommand mySqlCommand, Dictionary<string, string> data) 
        {
            foreach (KeyValuePair<string, string> pair in data)
                mySqlCommand.Parameters.AddWithValue($"@{pair.Key}", pair.Value);
        }
    }
}
