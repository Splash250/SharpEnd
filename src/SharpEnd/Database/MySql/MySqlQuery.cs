using MySql.Data.MySqlClient;
using Org.BouncyCastle.Bcpg;

namespace SharpEnd.MySQL
{
    public class MySqlQuery
    {
        private string _mySqlString;

        public MySqlQuery()
        {
            _mySqlString = "";
        }
        public MySqlQuery(string StarterQuery)
        {
            _mySqlString = StarterQuery;
        }
        internal string TrimAnchor() 
        {
            //remove the select and from anchors from the front of the query
            //if they exist
            string trimmed = _mySqlString;
            if (trimmed.StartsWith("SELECT"))
                //remove characters to the second occurance of space
                trimmed = trimmed.Remove(0, trimmed.IndexOf(' ', trimmed.IndexOf(' ') + 1));
            if (trimmed.StartsWith("FROM"))
                //remove characters to the second occurance of space
                trimmed = trimmed.Remove(0, trimmed.IndexOf(' ', trimmed.IndexOf(' ') + 1));
            return trimmed;
        }
        public MySqlQuery InsertInto(string tableName, params string[] columnNames)
        {
            string joinedColumns = FormatArray(columnNames);
            ProcessMySqlString("INSERT INTO", $"`{tableName}` ({joinedColumns})");
            return this;
        }
        public MySqlQuery Values(params string[] values)
        {
            string joinedValues = FormatArray(values, true);
            ProcessMySqlString("VALUES", $"({joinedValues})");
            return this;
        }

        private static string FormatArray(string[] array, bool isValueString = false)
        {
            string[] thisArray = new string[array.Length];
            for (int i = 0; i < thisArray.Length; i++)
            {
                if (isValueString)
                    thisArray[i] = $"@{array[i]}";
                else
                    thisArray[i] = $"`{array[i]}`";
            }
            return String.Join(", ", thisArray);
        }

        public MySqlQuery Select(params string[] fieldNames)
        {
            ProcessMySqlString("SELECT", String.Join(", ", fieldNames));
            return this;
        }
        public MySqlQuery SelectDistinct(params string[] fieldNames)
        {
            ProcessMySqlString("SELECT DISTINCT", String.Join(", ", fieldNames));
            return this;
        }
        public MySqlQuery From(params string[] tableNames)
        {
            string joinedTables = FormatArray(tableNames);
            ProcessMySqlString("FROM", joinedTables);
            return this;
        }
        public MySqlQuery InnerJoin(string foreignTableName)
        {
            ProcessMySqlString("INNER JOIN", foreignTableName);
            return this;
        }
        public MySqlQuery On(string localFieldName, string foreignFieldName)
        {
            ProcessMySqlString("ON", $"{localFieldName} = {foreignFieldName}");
            return this;
        }
        public MySqlQuery Where(string whereClause)
        {
            ProcessMySqlString("WHERE", whereClause);
            return this;
        }
        public MySqlQuery GroupBy(string fieldName)
        {
            ProcessMySqlString("GROUP BY", fieldName);
            return this;
        }
        public MySqlQuery Having(string havingClause)
        {
            ProcessMySqlString("HAVING", havingClause);
            return this;
        }
        public MySqlQuery OrderBy(string orderClause)
        {
            ProcessMySqlString("ORDER BY", orderClause);
            return this;
        }
        public MySqlQuery Take(int takeNumber)
        {
            ProcessMySqlString("TAKE", takeNumber.ToString());

            return this;
        }
        public MySqlQuery Limit(int limitNumber)
        {
            ProcessMySqlString("LIMIT", limitNumber.ToString());
            return this;
        }
        private void ProcessMySqlString(string action, string clause)
        {
            if (_mySqlString.Length > 0)
            {
                _mySqlString += " ";
            }
            _mySqlString += String.Join(" ", new string[2] { action, clause });
        }

        public string Merge(MySqlQuery other) 
        {
            return ToString() + other.ToString();
        }
        public MySqlCommand GetCommand(MySqlConnection connection) 
        {
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = ToString();
            return command;
        }
        public override string ToString()
        {
            return _mySqlString;
        }
    }
}