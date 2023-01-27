namespace SharpEnd.MySQL
{
    public class Seeder
    {
        private MySqlDataBaseConnection Connection;
        public Seeder(MySqlDataBaseConnection Connection ) 
        {
            this.Connection = Connection;
        }

        public void Run(string TableName, Dictionary<string, string> Data)
        {
            MySqlActions.InsertData(Connection, TableName, Data);
        }

    }
}
