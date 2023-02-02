using SharpEnd.ORM;
using System.Collections;
using System.Reflection;

namespace SharpEnd.Model
{
    internal static class ModelUtils
    {
        public static List<string> GetPropertyStrings(ObjectRelationMapper map)
        {
            List<string> props = new List<string>();
            foreach (var property in map.GetType().GetProperties())
            {
                props.Add(property.Name);
            }
            return props;
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

        public static IList? EmptyTypeList(Type type) 
        {
            return (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(type));
        }
    }
}
