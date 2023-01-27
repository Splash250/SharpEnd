namespace SharpEnd.MySQL
{
    public class MySqlQuery
    {
        private string MySqlString;

        public MySqlQuery()
        {
            MySqlString = "";
        }
        public MySqlQuery(string StarterQuery)
        {
            MySqlString = StarterQuery;
        }
        public MySqlQuery InsertInto(string TableName, params string[] ColumnNames)
        {
            string joinedColumns = FormatArray(ColumnNames);
            ProcessMySqlString("INSERT INTO", $"`{TableName}` ({joinedColumns})");
            return this;
        }
        public MySqlQuery Values(params string[] Values)
        {
            string joinedValues = FormatArray(Values, true);
            ProcessMySqlString("VALUES", $"({joinedValues})");
            return this;
        }

        private static string FormatArray(string[] Array, bool IsValueString = false)
        {
            string[] thisArray = new string[Array.Length];
            for (int i = 0; i < thisArray.Length; i++)
            {
                if (IsValueString)
                    thisArray[i] = $"@{Array[i]}";
                else
                    thisArray[i] = $"`{Array[i]}`";
            }
            return String.Join(", ", thisArray);
        }

        public MySqlQuery Select(params string[] FieldNames)
        {
            ProcessMySqlString("SELECT", String.Join(", ", FieldNames));
            return this;
        }
        public MySqlQuery SelectDistinct(params string[] FieldNames)
        {
            ProcessMySqlString("SELECT DISTINCT", String.Join(", ", FieldNames));
            return this;
        }
        public MySqlQuery From(params string[] TableNames)
        {
            string joinedTables = FormatArray(TableNames);
            ProcessMySqlString("FROM", joinedTables);
            return this;
        }
        public MySqlQuery InnerJoin(string ForeignTableName)
        {
            ProcessMySqlString("INNER JOIN", ForeignTableName);
            return this;
        }
        public MySqlQuery On(string LocalFieldName, string ForeignFieldName)
        {
            ProcessMySqlString("ON", $"{LocalFieldName} = {ForeignFieldName}");
            return this;
        }
        public MySqlQuery Where(string WhereClause)
        {
            ProcessMySqlString("WHERE", WhereClause);
            return this;
        }
        public MySqlQuery GroupBy(string FieldName)
        {
            ProcessMySqlString("GROUP BY", FieldName);
            return this;
        }
        public MySqlQuery Having(string HavingClause)
        {
            ProcessMySqlString("HAVING", HavingClause);
            return this;
        }
        public MySqlQuery OrderBy(string OrderClause)
        {
            ProcessMySqlString("ORDER BY", OrderClause);
            return this;
        }
        public MySqlQuery Take(int TakeNumber)
        {
            ProcessMySqlString("TAKE", TakeNumber.ToString());

            return this;
        }
        public MySqlQuery Limit(int LimitNumber)
        {
            ProcessMySqlString("LIMIT", LimitNumber.ToString());
            return this;
        }
        private void ProcessMySqlString(string action, string clause)
        {
            if (MySqlString.Length > 0)
            {
                MySqlString += " ";
            }
            MySqlString += String.Join(" ", new string[2] { action, clause });
        }
        public override string ToString()
        {
            return MySqlString;
        }
    }
}