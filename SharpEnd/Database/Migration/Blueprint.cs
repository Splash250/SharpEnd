namespace SharpEnd.MySQL
{

    public class Blueprint
    {
        private string BlueprintString;
        public string TableName { get; private set; }
        public Blueprint(string tableName)
        {
            BlueprintString = String.Empty;
            TableName = tableName;
            AddTableString();
        }

        private void AddTableString()
        {
            //add to blueprint string
            BlueprintString += "CREATE TABLE IF NOT EXISTS `" + TableName + "` (";
        }
        private void AddEndString()
        {

            RemoveLastComma();
            BlueprintString += ") ENGINE=InnoDB DEFAULT CHARSET=utf8;";
        }
        private void RemoveLastComma()
        {
            BlueprintString = BlueprintString.Remove(BlueprintString.Length - 1);
        
        }
        public void PrimaryKey(string name)
        {
            BlueprintString += "PRIMARY KEY (`" + name + "`),";
        }
        public void UniqueKey(string name)
        {
            BlueprintString += "UNIQUE KEY `" + name + "` (`" + name + "`),";
        }
        public void Index(string name)
        {
            BlueprintString += "KEY `" + name + "` (`" + name + "`),";
        }
        public void FullTextIndex(string name)
        {
            BlueprintString += "FULLTEXT KEY `" + name + "` (`" + name + "`),";
        }
        public static string FormatVariable(string variableName, MySqlDataType dataType, int length, int decimals)
        {
            //convert dataTypes to string
            string variableType = dataType.ToString();
            return $"`{variableName}` {variableType}({length},{decimals}) ";

        }

        private static string FormatVariable(string variableName, MySqlDataType dataType, int length)
        {
            //convert dataTypes to string
            string variableType = dataType.ToString();
            return $"`{variableName}` {variableType}({length}) ";

        }
        private static string FormatVariable(string variableName, MySqlDataType dataType)
        {
            //convert dataTypes to string
            string variableType = dataType.ToString();
            return $"`{variableName}` {variableType} ";

        }
        public void Char(string name, int length, bool nullable = false, string? defaultValue = null)
        {
            BlueprintString += FormatVariable(name, MySqlDataType.CHAR, length);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }

        public void VarChar(string name, int length, bool nullable = false, string? defaultValue = null)
        {
            //convert this line to string interpolation

            BlueprintString += FormatVariable(name, MySqlDataType.VARCHAR, length);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }

        public void Binary(string name, int length, bool nullable = false, string? defaultValue = null)
        {
            BlueprintString += FormatVariable(name, MySqlDataType.BINARY, length);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }

        public void VarBinary(string name, int length, bool nullable = false, string? defaultValue = null)
        {
            BlueprintString += FormatVariable(name, MySqlDataType.VARBINARY, length);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }
        public void TinyBlob (string name, bool nullable = false, string? defaultValue = null)
        {
            BlueprintString += FormatVariable(name, MySqlDataType.TINYBLOB);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }

        public void TinyText(string name, bool nullable = false, string? defaultValue = null)
        {
            BlueprintString += FormatVariable(name, MySqlDataType.TINYTEXT);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }

        public void Text(string name, int length, bool nullable = false, string? defaultValue = null)
        {
            BlueprintString += FormatVariable(name, MySqlDataType.TEXT, length);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }

        public void Blob(string name, int length, bool nullable = false, string? defaultValue = null)
        {
            BlueprintString += FormatVariable(name, MySqlDataType.BLOB, length);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }

        public void MediumText(string name, bool nullable = false, string? defaultValue = null)
        {
            BlueprintString += FormatVariable(name, MySqlDataType.MEDIUMTEXT);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }

        public void MediumBlob(string name, bool nullable = false, string? defaultValue = null)
        {
            BlueprintString += FormatVariable(name, MySqlDataType.MEDIUMBLOB);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }

        public void LongText(string name, bool nullable = false, string? defaultValue = null)
        {
            BlueprintString += FormatVariable(name, MySqlDataType.LONGTEXT);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }

        public void LongBlob(string name, bool nullable = false, string? defaultValue = null)
        {
            BlueprintString += FormatVariable(name, MySqlDataType.LONGBLOB);
            AddNullableAndDefaultValue(nullable, defaultValue);

        }
        public void Enum(string name, string[] values, bool nullable = false, string? defaultValue = null)
        {
            BlueprintString += "`" + name + "` ENUM(";
            AddValuesToBlueprint(values);
            BlueprintString += ") ";
            AddNullableAndDefaultValue(nullable, defaultValue);

        }

        public void Set(string name, string[] values, bool nullable = false, string? defaultValue = null)
        {
            BlueprintString += "`" + name + "` SET(";
            AddValuesToBlueprint(values);
            BlueprintString += ") ";
            AddNullableAndDefaultValue(nullable, defaultValue);

        }

        private void AddValuesToBlueprint(string[] values) 
        {
            for (int i = 0; i < values.Length; i++)
            {
                if (i == values.Length - 1)
                    BlueprintString += values[i];
                else
                    BlueprintString += values[i] + ",";
            }
        }

        public void Bit(string name, int size, bool nullable = false, string? defaultValue = null)
        {
            BlueprintString += FormatVariable(name, MySqlDataType.BIT, size);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }

        public void TinyInt(string name, int size, bool nullable = false, string? defaultValue = null)
        {
            BlueprintString += FormatVariable(name, MySqlDataType.TINYINT, size);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }

        public void Bool(string name, bool nullable = false, string? defaultValue = null)
        {
            BlueprintString += FormatVariable(name, MySqlDataType.BOOL);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }

        public void Boolean(string name, bool nullable = false, string? defaultValue = null)
        {
            BlueprintString += FormatVariable(name, MySqlDataType.BOOL);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }

        public void SmallInt(string name, int size, bool nullable = false, string? defaultValue = null)
        {
            BlueprintString += FormatVariable(name, MySqlDataType.SMALLINT, size);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }

        public void MediumInt(string name, int size, bool nullable = false, string? defaultValue = null)
        {
            BlueprintString += FormatVariable(name, MySqlDataType.MEDIUMINT, size);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }

        public void Int(string name, int size, bool nullable = false, string? defaultValue = null)
        {
            BlueprintString += FormatVariable(name, MySqlDataType.INT, size);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }

        public void Integer(string name, int size, bool nullable = false, string? defaultValue = null)
        {
            BlueprintString += FormatVariable(name, MySqlDataType.INT, size);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }

        public void BigInt(string name, int size, bool nullable = false, string? defaultValue = null)
        {
            BlueprintString += FormatVariable(name, MySqlDataType.BIGINT, size);
            AddNullableAndDefaultValue(nullable, defaultValue);

        }

        public void Float(string name, int size, int d, bool nullable = false, string? defaultValue = null)
        {
            BlueprintString += FormatVariable(name, MySqlDataType.FLOAT, size, d);
            AddNullableAndDefaultValue(nullable, defaultValue);

        }

        public void Float(string name, int p, bool nullable = false, string? defaultValue = null)
        {
            BlueprintString += FormatVariable(name, MySqlDataType.FLOAT, p);
            AddNullableAndDefaultValue(nullable, defaultValue);

        }

        public void Double(string name, int size, int d, bool nullable = false, string? defaultValue = null)
        {
            BlueprintString += FormatVariable(name, MySqlDataType.DOUBLE, size, d);
            AddNullableAndDefaultValue(nullable, defaultValue);

        }

        public void DoublePrecision(string name, int size, int d, bool nullable = false, string? defaultValue = null)
        {
            BlueprintString += FormatVariable(name, MySqlDataType.DOUBLE, size, d);
            AddNullableAndDefaultValue(nullable, defaultValue);

        }

        public void Decimal(string name, int size, int d, bool nullable = false, string? defaultValue = null)
        {
            BlueprintString += FormatVariable(name, MySqlDataType.DECIMAL, size, d);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }

        public void Dec(string name, int size, int d, bool nullable = false, string? defaultValue = null)
        {
            BlueprintString += FormatVariable(name, MySqlDataType.DECIMAL, size, d);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }

        public void Date(string name, bool nullable = false, string? defaultValue = null)
        {
            BlueprintString += FormatVariable(name, MySqlDataType.DATE);
            AddNullableAndDefaultValue(nullable, defaultValue);

        }

        public void DateTime(string name, int fsp, bool nullable = false, string? defaultValue = null)
        {
            BlueprintString += FormatVariable(name, MySqlDataType.DATETIME, fsp);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }

        public void TimeStamp(string name, int fsp, bool nullable = false, string? defaultValue = null)
        {
            BlueprintString += FormatVariable(name, MySqlDataType.TIMESTAMP, fsp);
            AddNullableAndDefaultValue(nullable, defaultValue);

        }
        public void Time(string name, int fsp, bool nullable = false, string? defaultValue = null)
        {
            BlueprintString += FormatVariable(name, MySqlDataType.TIME, fsp);
            AddNullableAndDefaultValue(nullable, defaultValue);

        }

        public void Year(string name, bool nullable = false, string? defaultValue = null)
        {
            BlueprintString += FormatVariable(name, MySqlDataType.YEAR);
            AddNullableAndDefaultValue(nullable, defaultValue);
        }

        public void BigIncrements(string name, int size = 8)
        {
            BlueprintString += MakeIncrementer(name, "BIGINT", size);
        }

        public void Increments(string name, int size = 4)
        {
            BlueprintString += MakeIncrementer(name, "INT", size);
        }

        public void MediumIncrements(string name, int size = 3)
        {
            BlueprintString += MakeIncrementer(name, "MEDIUMINT", size);
        }

        public void SmallIncrements(string name, int size = 2)
        {
            BlueprintString += MakeIncrementer(name, "SMALLINT", size);
        }

        public void TinyIncrements(string name, int size = 1)
        {
            BlueprintString += MakeIncrementer(name, "TINYINT", size);
        }

        private static string MakeIncrementer(string name, string type ,int size)
        {
            return "`" + name + "` " + type + "(" + size + ") UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY,";
        }

        private void AddNullableAndDefaultValue(bool nullable, string? defaultValue) 
        {
            if (nullable)
                BlueprintString += "NULL ";
            else
                BlueprintString += "NOT NULL ";

            if (defaultValue != null)
                BlueprintString += "DEFAULT " + defaultValue + " ";
            else if (defaultValue == null && nullable)
                BlueprintString += "DEFAULT NULL ";

            BlueprintString += ",";
        }

        public override string ToString()
        {
            string previous = BlueprintString;
            AddEndString();
            string completeBlueprint = BlueprintString;
            BlueprintString = previous;
            return completeBlueprint;
        }
    }
}
