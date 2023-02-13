namespace SharpEnd.MySQL
{
    public class Migration
    {
        public MySqlDataBaseConnection Connection { get; private set; }
        public string TableName { get; private set; }

        public Migration(MySqlDataBaseConnection connection)
        {
            Connection = connection;
            TableName = String.Empty;
        }

        public void Create(Blueprint blueprint)
        {
            TableName = blueprint.TableName;
            MySqlActions.CreateTable(Connection, blueprint.ToString());
        }

        public void Down() 
        {
            MySqlActions.DropTable(Connection, TableName);
        }
    }
}
