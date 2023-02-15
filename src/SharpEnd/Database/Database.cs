using SharpEnd.MySQL;

namespace SharpEnd.Database
{
    public abstract class Database
    {
        public MySqlDataBaseConnection Connection { get; private set; }
        public string Address { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 3306;
        public string UserName { get; set; } = "root";
        public string Password { get; set; } = String.Empty;
        public string DatabaseName { get; set; }
        private MySqlConfig Config 
        {
            get 
            {
                return new(Address, Port.ToString(), UserName, Password, DatabaseName);
            }
        }




    }
}
