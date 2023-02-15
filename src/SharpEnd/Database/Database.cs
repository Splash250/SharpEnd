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
                if (_connection == null)
                {
                    _connection = new MySqlDataBaseConnection(Config);
                }
                return _connection;
            }
        }
        private MySqlConfig Config 
        {
            get 
            {
                return new(Address, Port.ToString(), UserName, Password, DatabaseName);
            }
        }
    }
}
