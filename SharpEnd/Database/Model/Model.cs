using MySql.Data.MySqlClient;
using SharpEnd.MySQL;
using SharpEnd.ORM;
using System.Collections;
using System.Reflection;

namespace SharpEnd.Model
{
    public class Model
    {

        public Type ModelType { get; set; }
        public string TableName { get; set; }
        public IList? Rows { get; set; }


        public Model(Type modelType, string tableName)
        {
            ModelType = modelType;
            TableName = tableName;
            Rows = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(ModelType));

        }

        public Model(MySqlDataBaseConnection connection, string tableName)
        {
            DynamicRelationClassBuilder builder = new(connection, tableName);
            ModelType = builder.GetType();
            TableName = tableName;
            Rows = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(ModelType));

        }

        public Model(MySqlDataBaseConnection connection, string tableName, MySqlQuery query)
        {
            DynamicRelationClassBuilder builder = new(connection, tableName);
            ModelType = builder.GetType();
            TableName = tableName;
            Rows = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(ModelType));
            Query(connection, query);
        }

        public PropertyInfo[] Properties { get => GetProperties(); }

        public void Query(MySqlDataBaseConnection mySqlConnection, MySqlQuery query)
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
                Rows.Add(tableRow);
            }
        }

        public List<string> GetPropertyStrings()
        {
            return ModelUtils.GetPropertyStrings(this);
        }

        public PropertyInfo[] GetProperties()
        {
            return ModelUtils.GetProperties(this);
        }

        public List<string>[] GetRowDataMatrix()
        {
            return ModelUtils.GetRowDataMatrix(this);
        }

        public static List<string> GetRowValueStrings(object? RowObject)
        {
            return ModelUtils.GetRowValueStrings(RowObject);
        }

        public override string ToString()
        {
            return TableName;
        }

        public new Type GetType()
        {
            return ModelType;
        }
    }
}
