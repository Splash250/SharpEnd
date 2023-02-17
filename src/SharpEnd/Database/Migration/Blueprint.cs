namespace SharpEnd.MySQL
{

    public class Blueprint
    {
        private string _blueprintString;
        public string TableName { get; private set; }
        public Blueprint(string tableName)
        {
            _blueprintString = String.Empty;
            TableName = tableName;
            AddTableString();
        }

        private void AddTableString()
        {
            _blueprintString += "CREATE TABLE IF NOT EXISTS `" + TableName + "` (";
        }
        private void AddEndString()
        {
            RemoveLastComma();
            _blueprintString += ") ENGINE=InnoDB DEFAULT CHARSET=utf8;";
        }
        private void RemoveLastComma()
        {
            _blueprintString = _blueprintString.Remove(_blueprintString.Length - 1);
        }
        public void PrimaryKey(string name)
        {
            _blueprintString += "PRIMARY KEY (`" + name + "`),";
        }
        public void UniqueKey(string name)
        {
            _blueprintString += "UNIQUE KEY `" + name + "` (`" + name + "`),";
        }
        public void Index(string name)
        {
            _blueprintString += "KEY `" + name + "` (`" + name + "`),";
        }
        public void FullTextIndex(string name)
        {
            _blueprintString += "FULLTEXT KEY `" + name + "` (`" + name + "`),";
        }
        private static string FormatVariable(string variableName, MySqlDataType dataType, int length, int decimals)
        {
            string variableType = dataType.ToString();
            return $"`{variableName}` {variableType}({length},{decimals}) ";
        }
        private static string FormatVariable(string variableName, MySqlDataType dataType, int length)
        {
            string variableType = dataType.ToString();
            return $"`{variableName}` {variableType}({length}) ";

        }
        private static string FormatVariable(string variableName, MySqlDataType dataType)
        {
            string variableType = dataType.ToString();
            return $"`{variableName}` {variableType} ";
        }
        public void Char(string name, int length, bool nullable = false, string? defaultValue = null)
        {
            _blueprintString += FormatVariable(name, MySqlDataType.CHAR, length);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }
        public void VarChar(string name, int length, bool nullable = false, string? defaultValue = null)
        {
            _blueprintString += FormatVariable(name, MySqlDataType.VARCHAR, length);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }
        public void Binary(string name, int length, bool nullable = false, string? defaultValue = null)
        {
            _blueprintString += FormatVariable(name, MySqlDataType.BINARY, length);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }
        public void VarBinary(string name, int length, bool nullable = false, string? defaultValue = null)
        {
            _blueprintString += FormatVariable(name, MySqlDataType.VARBINARY, length);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }
        public void TinyBlob (string name, bool nullable = false, string? defaultValue = null)
        {
            _blueprintString += FormatVariable(name, MySqlDataType.TINYBLOB);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }
        public void TinyText(string name, bool nullable = false, string? defaultValue = null)
        {
            _blueprintString += FormatVariable(name, MySqlDataType.TINYTEXT);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }
        public void Text(string name, int length, bool nullable = false, string? defaultValue = null)
        {
            _blueprintString += FormatVariable(name, MySqlDataType.TEXT, length);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }
        public void Blob(string name, int length, bool nullable = false, string? defaultValue = null)
        {
            _blueprintString += FormatVariable(name, MySqlDataType.BLOB, length);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }
        public void MediumText(string name, bool nullable = false, string? defaultValue = null)
        {
            _blueprintString += FormatVariable(name, MySqlDataType.MEDIUMTEXT);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }
        public void MediumBlob(string name, bool nullable = false, string? defaultValue = null)
        {
            _blueprintString += FormatVariable(name, MySqlDataType.MEDIUMBLOB);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }
        public void LongText(string name, bool nullable = false, string? defaultValue = null)
        {
            _blueprintString += FormatVariable(name, MySqlDataType.LONGTEXT);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }
        public void LongBlob(string name, bool nullable = false, string? defaultValue = null)
        {
            _blueprintString += FormatVariable(name, MySqlDataType.LONGBLOB);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }
        public void Enum(string name, string[] values, bool nullable = false, string? defaultValue = null)
        {
            _blueprintString += "`" + name + "` ENUM(";
            AddValuesToBlueprint(values);
            _blueprintString += ") ";
            AddNullableAndDefaultValue(nullable, defaultValue);
        }
        public void Set(string name, string[] values, bool nullable = false, string? defaultValue = null)
        {
            _blueprintString += "`" + name + "` SET(";
            AddValuesToBlueprint(values);
            _blueprintString += ") ";
            AddNullableAndDefaultValue(nullable, defaultValue);
        }
        private void AddValuesToBlueprint(string[] values) 
        {
            for (int i = 0; i < values.Length; i++)
            {
                if (i == values.Length - 1)
                    _blueprintString += values[i];
                else
                    _blueprintString += values[i] + ",";
            }
        }
        public void Bit(string name, int size, bool nullable = false, string? defaultValue = null)
        {
            _blueprintString += FormatVariable(name, MySqlDataType.BIT, size);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }
        public void TinyInt(string name, int size, bool nullable = false, string? defaultValue = null)
        {
            _blueprintString += FormatVariable(name, MySqlDataType.TINYINT, size);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }
        public void Bool(string name, bool nullable = false, string? defaultValue = null)
        {
            _blueprintString += FormatVariable(name, MySqlDataType.BOOL);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }
        public void Boolean(string name, bool nullable = false, string? defaultValue = null)
        {
            _blueprintString += FormatVariable(name, MySqlDataType.BOOL);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }
        public void SmallInt(string name, int size, bool nullable = false, string? defaultValue = null)
        {
            _blueprintString += FormatVariable(name, MySqlDataType.SMALLINT, size);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }
        public void MediumInt(string name, int size, bool nullable = false, string? defaultValue = null)
        {
            _blueprintString += FormatVariable(name, MySqlDataType.MEDIUMINT, size);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }
        public void Int(string name, int size, bool nullable = false, string? defaultValue = null)
        {
            _blueprintString += FormatVariable(name, MySqlDataType.INT, size);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }
        public void Integer(string name, int size, bool nullable = false, string? defaultValue = null)
        {
            _blueprintString += FormatVariable(name, MySqlDataType.INT, size);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }
        public void BigInt(string name, int size, bool nullable = false, string? defaultValue = null)
        {
            _blueprintString += FormatVariable(name, MySqlDataType.BIGINT, size);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }
        public void Float(string name, int size, int d, bool nullable = false, string? defaultValue = null)
        {
            _blueprintString += FormatVariable(name, MySqlDataType.FLOAT, size, d);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }
        public void Float(string name, int p, bool nullable = false, string? defaultValue = null)
        {
            _blueprintString += FormatVariable(name, MySqlDataType.FLOAT, p);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }
        public void Double(string name, int size, int d, bool nullable = false, string? defaultValue = null)
        {
            _blueprintString += FormatVariable(name, MySqlDataType.DOUBLE, size, d);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }
        public void DoublePrecision(string name, int size, int d, bool nullable = false, string? defaultValue = null)
        {
            _blueprintString += FormatVariable(name, MySqlDataType.DOUBLE, size, d);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }
        public void Decimal(string name, int size, int d, bool nullable = false, string? defaultValue = null)
        {
            _blueprintString += FormatVariable(name, MySqlDataType.DECIMAL, size, d);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }
        public void Dec(string name, int size, int d, bool nullable = false, string? defaultValue = null)
        {
            _blueprintString += FormatVariable(name, MySqlDataType.DECIMAL, size, d);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }
        public void Date(string name, bool nullable = false, string? defaultValue = null)
        {
            _blueprintString += FormatVariable(name, MySqlDataType.DATE);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }
        public void DateTime(string name, int fsp, bool nullable = false, string? defaultValue = null)
        {
            _blueprintString += FormatVariable(name, MySqlDataType.DATETIME, fsp);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }
        public void TimeStamp(string name, int fsp, bool nullable = false, string? defaultValue = null)
        {
            _blueprintString += FormatVariable(name, MySqlDataType.TIMESTAMP, fsp);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }
        public void Time(string name, int fsp, bool nullable = false, string? defaultValue = null)
        {
            _blueprintString += FormatVariable(name, MySqlDataType.TIME, fsp);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }
        public void Year(string name, bool nullable = false, string? defaultValue = null)
        {
            _blueprintString += FormatVariable(name, MySqlDataType.YEAR);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }
        public void BigIncrements(string name, int size = 8)
        {
            _blueprintString += MakeIncrementer(name, "BIGINT", size);
        }
        public void Increments(string name, int size = 4)
        {
            _blueprintString += MakeIncrementer(name, "INT", size);
        }
        public void MediumIncrements(string name, int size = 3)
        {
            _blueprintString += MakeIncrementer(name, "MEDIUMINT", size);
        }
        public void SmallIncrements(string name, int size = 2)
        {
            _blueprintString += MakeIncrementer(name, "SMALLINT", size);
        }
        public void TinyIncrements(string name, int size = 1)
        {
            _blueprintString += MakeIncrementer(name, "TINYINT", size);
        }
        private static string MakeIncrementer(string name, string type ,int size)
        {
            return "`" + name + "` " + type + "(" + size + ") UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY,";
        }
        private void AddNullableAndDefaultValue(bool nullable, string? defaultValue) 
        {
            if (nullable)
                _blueprintString += "NULL ";
            else
                _blueprintString += "NOT NULL ";

            if (defaultValue != null)
                _blueprintString += "DEFAULT " + defaultValue + " ";
            else if (defaultValue == null && nullable)
                _blueprintString += "DEFAULT NULL ";

            _blueprintString += ",";
        }
        public override string ToString()
        {
            string previous = _blueprintString;
            AddEndString();
            string completeBlueprint = _blueprintString;
            _blueprintString = previous;
            return completeBlueprint;
        }
    }
}
