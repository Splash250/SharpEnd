namespace SharpEnd.MySQL
{
    public class Seeder
    {
        private MySqlDataBaseConnection _connection;
        public Seeder(MySqlDataBaseConnection connection ) 
        {
            _connection = connection;
        }

        public void Run(string tableName, Dictionary<string, string> data)
        {
            MySqlActions.InsertData(_connection, tableName, data);
        }

    }
}
