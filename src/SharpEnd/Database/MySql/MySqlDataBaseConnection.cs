using MySql.Data.MySqlClient;
using System.Data;

namespace SharpEnd.MySQL
{
    public class MySqlDataBaseConnection
    {
        private string _connectionString;
        private MySqlConnection _baseConnection;

        public MySqlDataBaseConnection(MySqlConfig config)
        {
            _connectionString = config.ConnectionString;
            _baseConnection = new MySqlConnection(_connectionString);
        }

        public void OpenConnection()
        {
            if (_baseConnection.State == ConnectionState.Closed)
                _baseConnection.Open();
        }
        public void CloseConnection()
        {
            if (_baseConnection.State == ConnectionState.Open)
                _baseConnection.Close();
        }
        public MySqlConnection GetConnection()
        {
            return _baseConnection;
        }
    }
}
