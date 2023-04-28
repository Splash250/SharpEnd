using SharpEnd.MySQL;

namespace SharpEnd.Database
{
    public abstract class Database
    {
        public string Address { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DatabaseName { get; set; }
        private MySqlDataBaseConnection _connection;
        public MySqlDataBaseConnection Connection
        {
            get
            {
                _connection ??= new MySqlDataBaseConnection(_getConfig);
                return _connection;
            }
        }
        private MySqlConfig _getConfig 
        {
            get 
            {
                return new(Address, Port.ToString(), UserName, Password, DatabaseName);
            }
        }
        public void CreateSchema(Blueprint schemaBlueprint) 
        {
            Migration migration = new(Connection);
            migration.Create(schemaBlueprint);
        }
        public void InsertRow(string tableName,Dictionary<string, string> rowData)
        {
            Seeder seeder = new(Connection);
            seeder.Run(tableName, rowData);
        }
    }
}
