using MySql.Data.MySqlClient;
using SharpEnd.Utils;

namespace SharpEnd.MySQL
{
    internal static class MySqlActions
    {
        public static void DropTable(MySqlDataBaseConnection connection, string tableName)
        {
            connection.OpenConnection();
            string sqlActionString = $"DROP TABLE IF EXISTS {tableName};";
            MySqlCommand command = new(sqlActionString, connection.GetConnection());
            command.Prepare();
            command.ExecuteNonQuery();
            connection.CloseConnection();
        }
        public static void CreateTable(MySqlDataBaseConnection connection, string sqlString) 
        {
            connection.OpenConnection();
            MySqlCommand command = new(sqlString, connection.GetConnection());
            command.Prepare();
            command.ExecuteNonQuery();
            connection.CloseConnection();
        }

        public static void InsertData(MySqlDataBaseConnection connection, string tableName, Dictionary<string, string> data)
        {
            connection.OpenConnection();
            MySqlQuery mySQLQuery = new();
            
            string[] keys = BasicUtility.GetKeysFromDictionary(data);
            mySQLQuery.InsertInto(tableName, keys).Values(keys);
            string sqlActionString = mySQLQuery.ToString();

            MySqlCommand command = new(sqlActionString, connection.GetConnection());
            AddValuesToInsertCommand(ref command, data);
            command.Prepare();
            command.ExecuteNonQuery();
            connection.CloseConnection();
        }

        private static void AddValuesToInsertCommand(ref MySqlCommand mySqlCommand, Dictionary<string, string> data) 
        {
            foreach (KeyValuePair<string, string> pair in data)
                mySqlCommand.Parameters.AddWithValue($"@{pair.Key}", pair.Value);
        }
    }
}
