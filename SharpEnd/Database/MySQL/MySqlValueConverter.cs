namespace SharpEnd.MySQL
{
    internal class MySqlValueConverter
    {
        public static string ConvertValue(object value)
        {
            Type type = value.GetType();

            if (type == typeof(string))
                return $"{value}";
            else if (type == typeof(DateTime))
                return $"{((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss")}";
            else if (type == typeof(bool))
                return ((bool)value ? 1 : 0).ToString();
            else if (type == typeof(int) || type == typeof(long) || type == typeof(float) || type == typeof(double) || type == typeof(decimal))
                return value.ToString();
            else
                throw new Exception("Unsupported data type.");
        }
    }
}
