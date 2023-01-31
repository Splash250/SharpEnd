using System.Reflection;

namespace SharpEnd.Model
{
    internal static class ModelUtils
    {
        public static List<string> GetPropertyStrings(Model model)
        {
            List<string> props = new List<string>();
            foreach (var property in model.GetType().GetProperties())
            {
                props.Add(property.Name);
            }
            return props;
        }
        public static PropertyInfo[] GetProperties(Model model)
        {
            return model.GetType().GetProperties();
        }

        public static List<string>[] GetRowDataMatrix(Model model) 
        {
            List<string>[] matrix = new List<string>[model.Rows.Count];
            for (int i = 0; i < model.Rows.Count; i++)
            {
                matrix[i] = GetRowValueStrings(model.Rows[i]);
            }
            return matrix;
        }
        public static List<string> GetRowValueStrings(object? RowObject) 
        {
            List<string> values = new List<string>();
            foreach (var property in GetPropertyInfos(RowObject))
            {
                values.Add(property.GetValue(RowObject).ToString());
            }
            return values;
        }

        private static PropertyInfo[] GetPropertyInfos(object? Object)
        {
            return Object.GetType().GetProperties();
        }
    }
}
