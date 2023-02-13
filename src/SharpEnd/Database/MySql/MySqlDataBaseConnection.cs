using MySql.Data.MySqlClient;
using System.Data;

namespace SharpEnd.MySQL
{
    public class MySqlDataBaseConnection
    {
        private string connectionString;
        private MySqlConnection baseConnection;

        public MySqlDataBaseConnection(MySqlConfig config)
        {
            connectionString = config.ConnectionString;
            baseConnection = new MySqlConnection(connectionString);
        }

        public void OpenConnection()
        {
            if (baseConnection.State == ConnectionState.Closed)
                baseConnection.Open();
        }
        public void CloseConnection()
        {
            if (baseConnection.State == ConnectionState.Open)
                baseConnection.Close();
        }
        public MySqlConnection GetConnection()
        {
            return baseConnection;
        }
    }
}
