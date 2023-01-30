using MySql.Data.MySqlClient;
using SharpEnd.MySQL;
using SharpEnd.ORM;
using System.Collections;

namespace SharpEnd.Model
{
    public class Model
    {

        public Type ModelType { get; set; }
        public string TableName { get; set; }
        public IList? rows { get; set; }
        public Model(Type modelType, string tableName)
        {
            ModelType = modelType;
            TableName = tableName;
            rows = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(ModelType));

        }

        public Model(MySqlDataBaseConnection connection, string tableName)
        {
            DynamicRelationClassBuilder builder = new(connection, tableName);
            ModelType = builder.GetType();
            TableName = tableName;
            rows = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(ModelType));

        }

        public Model(MySqlDataBaseConnection connection, string tableName, MySqlQuery query)
        {
            DynamicRelationClassBuilder builder = new(connection, tableName);
            ModelType = builder.GetType();
            TableName = tableName;
            rows = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(ModelType));
            SelectItems(connection, query);
        }

        public void SelectItems(MySqlDataBaseConnection mySqlConnection, MySqlQuery query)
        {
            using MySqlConnection connection = mySqlConnection.GetConnection();
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = query.ToString();
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                var tableRow = Activator.CreateInstance(ModelType);
                foreach (var property in ModelType.GetProperties())
                {
                    property.SetValue(tableRow, Convert.ChangeType(reader[property.Name], property.PropertyType));
                }
                rows.Add(tableRow);
            }
        }


    }
}
