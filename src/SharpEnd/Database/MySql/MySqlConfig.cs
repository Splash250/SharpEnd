namespace SharpEnd.MySQL
{
    public class MySqlConfig
    {
        public string DatabaseAddress { get; set;}
        public string DatabasePort { get; set;}
        public string DatabaseUsername { get; set;}
        public string DatabasePassword { get; set;}
        public string Database  { get; set;}
        public string ConnectionString
        {
            get
            {
                return
                    "server=" + DatabaseAddress +
                    ";port=" + DatabasePort +
                    ";username=" + DatabaseUsername +
                    ";password=" + DatabasePassword +
                    ";database=" + Database;
            }
        }

        public MySqlConfig()
        {
            DatabaseAddress = "";
            DatabasePort = "";
            DatabaseUsername = "";
            DatabasePassword = "";
            Database = "";
        }

        public MySqlConfig(string databaseAddress,
                           string databasePort,
                           string databaseUsername,
                           string databasePassword,
                           string database)
        {
            DatabaseAddress = databaseAddress;
            DatabasePort = databasePort;
            DatabaseUsername = databaseUsername;
            DatabasePassword = databasePassword;
            Database = database;
        }
    }
}
