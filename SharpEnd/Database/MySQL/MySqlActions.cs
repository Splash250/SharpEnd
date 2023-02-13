using MySql.Data.MySqlClient;

namespace SharpEnd.MySQL
{
    internal static class MySqlActions
    {
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
