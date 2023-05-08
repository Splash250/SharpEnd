using SharpEnd.ORM;
using System.Collections;
using System.Reflection;

namespace SharpEnd.Model
{
    internal static class ModelUtils
    {
        public static List<string> GetPropertyStrings(ObjectRelationMapper map)
        {
            List<string> props = new();
            foreach (var property in map.GetType().GetProperties())
            {
                props.Add(property.Name);
            }
            return props;
        }
        public static List<string> GetRowValueStrings(object? rowObject) 
        {
            List<string> values = new();
            foreach (var property in GetPropertyInfos(rowObject))
            {
                values.Add(property.GetValue(rowObject).ToString());
            }
            return values;
        }

        public static PropertyInfo[] GetPropertyInfos(object? fromObject)
        {
            return fromObject.GetType().GetProperties();
        }

        public static IList? EmptyTypeList(Type type) 
        {
            return (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(type));
        }


    }
}
